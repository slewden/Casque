//-------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------
//-- Controlleur Pages : reception / assemblage / livraison : pages avec lecteur quoi (sauf consultation)
//-------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------
app.controller('receptionController', ['$scope', '$rootScope', '$timeout', '$noHttp', 'NOCONFIG', 'webStorage', '$sce', '$translate', 'parameters',
  function ($scope, $rootScope, $timeout, $noHttp, NOCONFIG, webStorage, $sce, $translate, parameters) {
    $scope.pageCode = parameters.mode;    // Type d'action attendue
    $scope.waitTagStatut = $scope.pageCode == 'livraison' ? 1 : $scope.pageCode == 'assemblage' ? 2 : $scope.pageCode == 'reception' ? 3 : 0;             // Etat des tags attendus
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.progression = 0;
    $scope.poste = null;
    $scope.postes = [];
    $scope.tagInconnus = [];
    $scope.utilisationPosteCle = 0;
    $scope.reader = {
      statut: NOCONFIG.READER_STOPPED,
      stopIsCancel: false,
    };
    $scope.hub = {
      open: false,
      message: '',
      connectionId: '?'
    };

    $scope.assemblageCle = 0; // Spécifique au pageCode "assemblage"
    $scope.livraison = null;  // Spécifique au pageCode "livraison"
    $scope.cartonIndex = 0;   // Spécifique au pageCode "livraison"
    $scope.tagConnus = null;  // Spécifique au pageCode "consultation"
    //------------------------------------------------------------------------------------------------------
    //-- initialise l'analyseur de lectures
    $scope.analyseurInit = function () {
      $scope.tagInconnus = [];
      $scope.tagConnus = [];
      $scope.analyseur = {
        lectures: [],
        nbDemandeEnCours: 0,
        commandes: [],
        casques: [],
      };
    }
    //-- indique à la page s'il y a des lectures non sauvées
    $scope.isLecturePending = function () {
      return $scope.analyseur && (($scope.analyseur.lectures && $scope.analyseur.lectures.length)
                                 || ($scope.analyseur.commandes && $scope.analyseur.commandes.length)
                                 );
    }
    //------------------------------------------------------------------------------------------------------
    //-- initialisation des variables du hub (à appeller une seule fois)
    $scope.hubInit = function () {
      $scope.progression = 1;
      $scope.casqueHub = $.connection.casqueHub; // nom de la classe hub c#
      //-- A la réception d'un numéro de tag (nom de la méthode utilisée dans le hub c#)
      $scope.casqueHub.client.tag = function (tag) {
        tag = tag.trim();
        if (!$scope.addTagToCommandes(tag)) { // on a pas trouvé la "commande" qui contient le tag
          $scope.analyseur.lectures.push(tag);
          $scope.saveToSession();
          $scope.faitUneDemande();
        }
        $scope.$apply();
      };
      //-- A la réception d'un message de retour d'un lecteur
      $scope.casqueHub.client.message = function (error, action, msg) {
        switch (action) {
          case 1: // Démarrer
            if (!error) { // on memorise que le lecteur est correctement démarré
              $scope.reader.statut = NOCONFIG.READER_STARTED;
              $translate('HubLecteur.HubNotify', { msg: msg }).then(function (result) { $scope.hub.message = result; });
              $scope.saveToSession();
              if ($scope.progression == 6) { // Query lecteur statut + Started ==> démarré
                $scope.progression = 8;
              }
            } else { // Erreur de démarrage : donc on stoppe le lecteur
              $scope.readerStop(true);
              $translate('HubLecteur.HubError', { msg: msg }).then(function (result) { $rootScope.error = result; });
              $scope.poste = null;
              $scope.progression = 5;
            }
            break;
          case 2: // Arreter
            if (!error) { // pas d'erreur le lecteur est bien stoppé
              $translate('HubLecteur.HubNotify', { msg: msg }).then(function (result) { $scope.hub.message = result; });
              if ($scope.reader.stopIsCancel) {
                $translate('HubLecteur.LecteurCanceled').then(function (result) { $scope.hub.message = result; });
              } else {
                $translate('HubLecteur.LecteurStopped').then(function (result) { $scope.hub.message = result; });
              }
              $scope.annuleUtilisationPoste();
              if ($scope.progression == 6) { // Query lecteur statut + Stoped ==> prêt à démarré
                $scope.progression = 5;
              }
            } else {
              $scope.poste = null;
              $scope.progression = 5;
              $translate('HubLecteur.HubError', { msg: msg }).then(function (result) { $rootScope.error = result; });
            }
            break;
          case 3: // Reset des lectures
            if (error) {
              $translate('HubLecteur.HubError', { msg: msg }).then(function (result) { $rootScope.error = result; });
            } else {
              $translate('HubLecteur.HubNotify', { msg: msg }).then(function (result) { $scope.hub.message = result; });
            }
            break;
        }
        $scope.$apply();
      }
      //-- A la réception compte rendu d'un encodage
      $scope.casqueHub.client.report = function (error, action, cle, msg) {
        if (error) {
          $translate('HubLecteur.HubError', { msg: msg }).then(function (result) { $rootScope.error = result; });
          $scope.progression = 10;
        } else {
          $translate('HubLecteur.HubNotify', { msg: msg }).then(function (result) { $scope.hub.message = result; });
          $scope.progression = 13;
        }
        $scope.$apply();
      }
      //-- maintenant se connecter au hub
      $scope.hubConnect();
    };
    //-- Enclenche la connexion au hub
    $scope.hubConnect = function () {
      if (!$scope.hub.open) {
        $scope.hub.message = '';
        $.connection.hub.stop();
        $.connection.hub.start()
        .done(function () { // hub initialisé
          $scope.hub.connectionId = $.connection.hub.id;
          $scope.hub.open = true;
          $scope.$apply();
          $scope.sayHello();
        })
        .fail(function () { // hub KO
          $scope.progression = 0;
          $translate('HubLecteur.StatutConnectionFail').then(function (result) { $rootScope.error = result; });
          $scope.hub.message = '';
          $scope.hub.connectionId = 'x';
          $scope.hub.open = false;
          $scope.$apply();
        });
      }
    }
    //-- Say Hello
    $scope.sayHello = function () {
      $scope.progression = 2;
      $scope.casqueHub.server.hello(0) // 0 car on n'est pas un pilote de lecteur juste consommateur
       .done(function (values) { // Recue valeur
         $scope.hub.actifLecteur = (values & 2) == 2;
         $scope.hub.actifEncodeur = (values & 1) == 1;
         // Fonction du PageCode on a besoin d'une ou des deux fonctionnalité
         if (!$scope.hub.actifLecteur) {
           $translate('HubLecteur.HubNoDriverForReader').then(function (result) { $rootScope.error = result; });
           $scope.progression = 3;
           $scope.$apply();
         }
         else if ($scope.pageCode == 'assemblage' && !$scope.hub.actifEncodeur) {
           $translate('HubLecteur.HubNoDriverForWriter').then(function (result) { $rootScope.error = result; });
           $scope.progression = 4;
           $scope.$apply();
         }
         else { // OK y a le ou les lecteurs attendus
           $rootScope.error = '';
           $scope.progression = 5;
           if ($scope.utilisationPosteCle) {
             if ($scope.poste) { // on ne sait pas dans quel état est le lecteur : on lui demande
               $scope.readerQueryStatut();
             } else { // probleme config flinguée 
               $scope.annuleUtilisationPoste();
             }
           }
           $scope.reader.statut = NOCONFIG.READER_STOPPED;
           if ($scope.postes && $scope.postes.length == 1) {
             $scope.poste = $scope.postes[0];
           } else {
             $scope.poste = null;
           }
           $scope.$apply();
         }
       }).fail(function (error) { // erreur lors de l'envoie
         $translate('HubLecteur.HubErrorReaderStatuUnknow').then(function (result) { $rootScope.error = result; });
         $scope.$apply();
       });
    }
    //------------------------------------------------------------------------------------------------------
    //-- ajoute le tag à la "commande" qui l'attend 
    $scope.addTagToCommandes = function (tag) {
      if ($scope.analyseur && $scope.analyseur.commandes && $scope.analyseur.commandes.length) {
        for (var i = 0; i < $scope.analyseur.commandes.length; i++) {
          if ($scope.addTagToCommande($scope.analyseur.commandes[i], tag)) {
            return true;
          }
        }
      }
      return false;
    }
    //-- ajoute le tag à la commande qui l'attend 
    $scope.addTagToCommande = function (cmd, tag) {
      if (cmd.nombreLu == null) {
        cmd.nombreLu = 0;
      }
      if (cmd && cmd.pieces && cmd.pieces.length) {
        var res;
        for (var i = 0; i < cmd.pieces.length; i++) {
          res = $scope.addTagToCommandePiece(cmd.pieces[i], tag);
          if (res == 1) { // tag reconnu et attendu 
            cmd.nombreLu++;
            return true;
          } else if (res == 2) { // tag reconnu mais pas attendu
            return true;
          }
        }
      }
      return false;
    }
    //-- ajoute le tag à la pièce de commande qui l'attend 
    $scope.addTagToCommandePiece = function (piece, tag) {
      if (piece && piece.tags && piece.tags.length) {
        for (var i = 0; i < piece.tags.length; i++) {
          if (piece.tags[i] && piece.tags[i].numero && piece.tags[i].numero == tag) { // trouvé !
            if (piece.tags[i].statutInt == $scope.waitTagStatut) { // le tag est bien un tag attendu 
              if (!piece.tagLus) {
                piece.tagLus = [];
              }
              piece.tagLus.push(tag);
              return 1; //-- Tag reconnu et attendu 
            } else { // le tag ne devrait pas être lu !
              if (!piece.tagNonAttendu) {
                piece.tagNonAttendu = [];
              }
              piece.tagNonAttendu.push(tag);
              return 2; //-- Tag reconnu mais pas attendu 
            }

          }
        }
      }
      return 0; //-- tag non reconnu
    }
    //-- avec la commande fournie : trouve les tags qui la contiennent
    $scope.analyseNouvelleCommande = function (cmd) {
      if (cmd && $scope.analyseur.lectures && $scope.analyseur.lectures.length) {
        if ($scope.pageCode == 'assemblage') {
          for (var i = 0; i < $scope.analyseur.lectures.length; i++) {
            $scope.addTagToCommande(cmd, $scope.analyseur.lectures[i]);
          }
        }
        else { //if ($scope.pageCode == 'reception') {
          var i = 0
          while (i < $scope.analyseur.lectures.length) {
            if ($scope.addTagToCommande(cmd, $scope.analyseur.lectures[i])) { // le tag à trouvé sa commande
              $scope.analyseur.lectures.splice(i, 1); // on retire le tag de la listes des tags inconnus
            } else {
              i++;
            }
          }
        }
        $scope.saveToSession();
      }
    }
    //-- envoie une demande d'analyse des tags lus
    $scope.faitUneDemande = function () {
      if ($scope.analyseur.nbDemandeEnCours == 0) {
        $scope.saveToSession();
        $scope.analyseur.nbDemandeEnCours++;
        $noHttp.get('/api/analyseTag/', {
          pageCode: $scope.pageCode,
          utilisationPosteCle: $scope.utilisationPosteCle,
          tags: $scope.analyseur.lectures,
        }, function (data) {
          $scope.analyseur.nbDemandeEnCours--;
          $scope.checkTagInconnus(data.tagInconnus);
          if (data.commandes != null) { // on a trouvé des commandes qui matchent avec les tags envoyés
            $scope.waitTagStatut = 1;
            for (var i = 0; i < data.commandes.length; i++) {
              $scope.analyseur.commandes.push(data.commandes[i]);
              $scope.analyseNouvelleCommande(data.commandes[i]);
            }
          }
          else if (data.casques) { // on a trouvé des casques à constituer qui matchent avec les tags envoyés
            $scope.waitTagStatut = 2;
            $scope.analyseur.commandes = []; // Dans ce mode la dernière requette atoujours la liste complète
            $scope.analyseur.nombreCasqueComplet = data.nombreCasqueComplet;
            $scope.analyseur.nombreCasqueEligible = data.nombreCasqueEligible;
            for (var i = 0; i < data.casques.length; i++) {
              $scope.analyseur.commandes.push(data.casques[i]);
              $scope.analyseNouvelleCommande(data.casques[i]);
            }
          }
          else if (data.assemblages) { // on a trouvé des assemblages à livrer qui matchent avec les tags envoyés
            $scope.waitTagStatut = 3;
            $scope.cartons = data.cartons;
            if ($scope.cartons && $scope.cartons.length == 1) {
              $scope.carton = $scope.cartons[0];
            }
            for (var i = 0; i < data.assemblages.length; i++) {
              $scope.analyseur.commandes.push(data.assemblages[i]);
              $scope.analyseNouvelleCommande(data.assemblages[i]);
              $scope.addCasque(data.assemblages[i]);
            }
          }
          else if (data.tagConnus) { // on a trouvé des tags connus
            if (!$scope.tagConnus) {
              $scope.tagConnus = [];
            }
            for (var i = 0; i < data.tagConnus.length; i++) { // ajout des tags connus
              $scope.tagConnus.push(data.tagConnus[i]);
            }
            if ($scope.tagConnus && $scope.tagConnus.length) {
              for (var i = 0; i < $scope.tagConnus.length; i++) {
                if ($scope.tagConnus[i] && $scope.tagConnus[i].numero) {
                  $scope.removeTagInconnu($scope.tagConnus[i].numero);
                }
              }
            }

            $scope.removeTagInconnu($scope.tagConnus);
          }
        }, function (data, statusCode, headers, config, statusText) {
          $scope.analyseur.nbDemandeEnCours--;
        });
      }
    }
    //--- s'assure que les tag inconnus sont mis à part
    $scope.checkTagInconnus = function (ti) {
      if (!$scope.tagInconnus) {
        $scope.tagInconnus = [];
      }
      if (!ti || !ti.length) { // pas de tag pas de travail 
        return;
      }
      for (var i = 0; i < ti.length; i++) {
        $scope.tagInconnus.push(ti[i]);
        $scope.removeTagInconnu(ti[i]);
      }
    }
    //--- Retire de la liste des lecture les tag inconnus
    $scope.removeTagInconnu = function (tag) {
      if ($scope.analyseur && $scope.analyseur.lectures && $scope.analyseur.lectures.length) {
        var i = 0
        while (i < $scope.analyseur.lectures.length) {
          if ($scope.analyseur.lectures[i] == tag) { // le tag trouvé
            $scope.analyseur.lectures.splice(i, 1); // on retire le tag de la listes des tags inconnus
          } else {
            i++;
          }
        }
      }
    }
    //------------------------------------------------------------------------------------------------------
    //-- Load la liste des postes possibles pour ce pageCode 
    $scope.getPostes = function () {
      $noHttp.get('/api/lecteursGet/', {
        pageCode: $scope.pageCode,
      }, function (data) {
        $scope.messageBloquant = data.messageBloquant;
        $scope.postes = data.postes;
        if ($scope.postes && $scope.postes.length == 1) { // préselection
          $scope.poste = $scope.postes[0];
        }
        if (data.cartons) { // on fournis aussi les cartons
          $scope.cartons = data.cartons;
          if ($scope.cartons && $scope.cartons.length == 1) {
            $scope.carton = $scope.cartons[0];
          }
        }
      }, function (data, statusCode, headers, config, statusText) {
        $scope.postes = null;
      });
    };
    //------------------------------------------------------------------------------------------------------
    //-- Démarrer la lecture
    $scope.readerStart = function () {
      if ($scope.hub.open) { // hub ouvert
        $scope.reader.statut = NOCONFIG.READER_STATING;
        $scope.saveToSession();
        $scope.casqueHub.server.piloteReader($scope.hub.connectionId, NOCONFIG.ACTIONLECTEURSTART, $scope.poste.configurationTxt)
        .done(function () {
          $translate('HubLecteur.LecteurStarting').then(function (result) { $scope.hub.message = result; });
        })
        .fail(function (error) { // erreur lors de l'envoie
          $translate('HubLecteur.LecteurStartFail', { msg: error }).then(function (result) { $rootScope.error = result; });
        });
      } else {
        $translate('HubLecteur.ErrorHubClosed').then(function (result) { $rootScope.error = result; });
      }
    }
    //-- arret la lecture pour valider ou annuler
    $scope.readerStop = function (cancel) {
      if ($scope.hub.open && $scope.poste) {
        $scope.reader.stopIsCancel = cancel;
        $scope.reader.statut = NOCONFIG.READER_STOPPING;
        $scope.saveToSession();
        $scope.casqueHub.server.piloteReader($scope.hub.connectionId, NOCONFIG.ACTIONLECTEURSTOP, $scope.poste.configurationTxt)
        .done(function () {
          $scope.annuleUtilisationPoste();
        })
        .fail(function (error) { // erreur lors de l'envoie
          $translate('HubLecteur.LecteurStopFail', { msg: error }).then(function (result) { $rootScope.error = result; });
        });
      } else { // au cas ou ?
        $scope.annuleUtilisationPoste();
      }
    }
    //--- demande au lecteur son état
    $scope.readerQueryStatut = function () {
      if ($scope.hub.open) {
        $scope.progression = 6;
        $scope.reader.statut = NOCONFIG.READER_UNKNOW;
        $scope.saveToSession();
        $scope.casqueHub.server.piloteReader($scope.hub.connectionId, NOCONFIG.ACTIONLECTEURQUERYSTATUT, $scope.poste.configurationTxt)
        .done(function () { // nothing to do le lecteur répondra par un message de statut
        })
        .fail(function (error) { // erreur lors de l'envoie
          $translate('HubLecteur.LecteurStopFail', { msg: error }).then(function (result) { $rootScope.error = result; });
        });
      }
    }
    //------------------------------------------------------------------------------------------------------
    //-- annule l'utilisation poste en cours == Delete de la bdd
    $scope.annuleUtilisationPoste = function () {
      if ($scope.utilisationPosteCle) {
        $noHttp.delete('/api/utilisationPosteDelete/', {
          pageCode: $scope.pageCode,
          utilisationPosteCle: $scope.utilisationPosteCle
        }, function (data) {
          $scope.postes = data.postes;
          if ($scope.postes && $scope.postes.length == 1) {
            $scope.poste = $scope.postes[0];
          }
          $scope.termine();
        }, function (data, statusCode, headers, config, statusText) {
          $translate('HubLecteur.ErrorWhenCanceling', { code: statusCode, text: statusText }).then(function (result) {
            $rootScope.error = result;
          });
        });
      } else {
        $scope.termine();
      }
    }
    //-- Après la fin d'une action : remet tout en ordre
    $scope.termine = function () {
      $scope.progression = 5;
      $scope.utilisationPosteCle = 0;
      $scope.reader.statut = NOCONFIG.READER_STOPPED;
      $scope.saveToSession();
      if ($scope.reader.stopIsCancel) {
        $translate('HubLecteur.LecteurCanceled').then(function (result) { $scope.hub.message = result; });
      } else {
        $translate('HubLecteur.LecteurSaved').then(function (result) { $scope.hub.message = result; });
      }
      $scope.saveToSession();
    }
    //-- sauve en session les paramètres
    $scope.saveToSession = function () {
      webStorage.add($scope.pageCode + 'Controller',
        {
          poste: $scope.poste,
          utilisationPosteCle: $scope.utilisationPosteCle,
          reader: { statut: $scope.reader.statut },
          analyseur: $scope.analyseur,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.loadFromSession = function () {
      var key = webStorage.get($scope.pageCode + 'Controller');
      if (key) {
        if (key.poste) {
          $scope.poste = key.poste;
        }
        if (key.utilisationPosteCle) {
          $scope.utilisationPosteCle = key.utilisationPosteCle;
        }
        if (key.reader && key.reader.statut != null) {
          $scope.reader.statut = key.reader.statut;
        }
        if (key.analyseur) {
          $scope.analyseur = key.analyseur;
        }
      }
    }
    //-------------- fonctions dans l'interface ------------------------------------------------------------
    //-- demarre une réception
    $scope.startReception = function () {
      if ($scope.poste && $scope.poste.cle) {
        $scope.progression = 8;
        $scope.hub.message = '';
        $noHttp.put('/api/utilisationPosteStart/', {
          pageCode: $scope.pageCode,
          posteCle: $scope.poste.cle,
          utilisateurCle: $scope.user.cle,
        }, function (data) {
          $scope.utilisationPosteCle = data.utilisationPosteCle;
          $scope.analyseurInit();
          $scope.saveToSession();
          $scope.readerStart();
        }, function (data, statusCode, headers, config, statusText) {
          $scope.utilisationPosteCle = 0;
        });
      }
    }
    //-- Anule le démarrage
    $scope.cancelStarting = function () {
      $scope.poste = null;
      $scope.progression = 5;
      $rootScope.error = '';
      $translate('HubLecteur.Statut5').then(function (result) { $scope.hub.message = result; });
      $scope.readerStop(true);
    }
    //-- Annule une réception == la supprimer
    $scope.cancelReception = function () {
      if (!$scope.utilisationPosteCle) {
        return;
      }
      var key;
      if ($scope.pageCode == 'reception') {
        key = 'Reception.ConfirmCancel';
      } else if ($scope.pageCode == 'assemblage') {
        key = 'Assemblage.ConfirmCancel';
      } else if ($scope.pageCode == 'livraison') {
        key = 'Livraison.ConfirmCancel';
      } else if ($scope.pageCode == 'consultation') {
        key = 'Consultation.ConfirmCancel';
      }
      $translate(key).then(function (result) {
        if (confirm(result)) {
          $rootScope.error = '';
          $translate('HubLecteur.Statut5').then(function (result) { $scope.hub.message = result; });
          $scope.readerStop(true);
        }
      });
    }
    //-- termine une réception == sauve le tout en bdd
    $scope.stopReception = function () {
      if (!$scope.utilisationPosteCle) {
        return;
      }
      $rootScope.error = '';
      $translate('HubLecteur.Statut555').then(function (result) { $scope.hub.message = result; });
      $scope.readerStop(false);
    }
    //-- valide la réception d'une commande, la fin d'un assemblage
    $scope.valideCommande = function (cmd) {
      var tgs = [];
      if (cmd && cmd.pieces && cmd.pieces.length) {
        for (var i = 0; i < cmd.pieces.length; i++) {
          if (cmd.pieces[i] && cmd.pieces[i].tagLus && cmd.pieces[i].tagLus.length) {
            for (var j = 0; j < cmd.pieces[i].tagLus.length; j++) {
              tgs.push(cmd.pieces[i].tagLus[j]);
            }
          }
        }
      }
      $noHttp.post('/api/analyseCloture/', {
        pageCode: $scope.pageCode,
        posteCle: $scope.poste.cle,
        utilisationPosteCle: $scope.utilisationPosteCle,
        utilisateurCle: $scope.user.cle,
        cle: cmd.cle,
        lectures: tgs,
      }, function (data) {
        $scope.utilisationPosteCle = data.utilisationPosteCle;
        if ($scope.pageCode == 'assemblage') {
          $scope.assemblageCle = data.assemblageCle;
          $scope.postesPrint = data.postes;
          if ($scope.postesPrint && $scope.postesPrint.length == 1) {
            $scope.postePrint = $scope.postesPrint[0];
          }
          $scope.assemblageSaved = cmd;
          $scope.analyseurInit();
          $scope.progression = 10; //--- démarre une impression de l'étiquette d'assemblage
        } else {
          if ($scope.analyseur.commandes && $scope.analyseur.commandes.length) {
            var idx = $scope.analyseur.commandes.indexOf(cmd);
            if (idx >= 0) {
              $scope.analyseur.commandes.splice(idx, 1);
            }
            else {
              $translate('HubLecteur.ErrorWhenRemoveCommande').then(function (result) { $rootScope.error = result; });
            }
          }
        }
      }, function (data, statusCode, headers, config, statusText) {
        // en cas d'erreur
      });
    }
    //-- retire la commande des infos lues
    $scope.retireCommande = function (cmd) {
      var idx = $scope.analyseur.commandes.indexOf(cmd);
      if (idx >= 0) {
        $scope.analyseur.commandes.splice(idx, 1);
      }
      else {
        $translate('HubLecteur.ErrorWhenRemoveCommande').then(function (result) { $rootScope.error = result; });
      }
    }
    //-- initialise la liste des lectures
    $scope.resetReception = function () {
      if (!$scope.utilisationPosteCle) {
        return;
      }
      $translate('HubLecteur.ConfirmReset').then(function (result) {
        if (confirm(result)) {
          $scope.processReset();
        }
      });
    }
    //--- traite un reset
    $scope.processReset = function () {
      if ($scope.hub.open) {
        $rootScope.error = '';
        $scope.hub.message = '';
        if ($scope.poste) {
          $scope.casqueHub.server.piloteReader($scope.hub.connectionId, NOCONFIG.ACTIONLECTEURRESETLECTURES, $scope.poste.configurationTxt)
          .done(function () {
            $scope.analyseurInit();
            $translate('HubLecteur.LecturesReseting').then(function (result) {
              $scope.hub.message = result;
              // comment car erreur JS ! $scope.$apply();
            });
          })
          .fail(function (error) { // erreur lors de l'envoie
            $translate('HubLecteur.ResetFail', { msg: error }).then(function (result) { $rootScope.error = result; });
          });
        } else {
          $scope.analyseurInit();
        }
      }
    }
    //-- raz de la liste des tags attendus
    $scope.razWaiting = function () {
      $scope.analyseur.lectures = [];
    }
    //-- Raz de la liste des tags inconnus
    $scope.razInconnus = function () {
      $scope.tagInconnus = [];
    }
    //-- Clear la notifiaction
    $scope.clearNotification = function () {
      $scope.hub.message = '';
    }
    //-- Voir quels processus sont en cours
    $scope.checkOtherProcess = function () {
      $noHttp.get('/api/lecteursEnCours/', {
        pageCode: $scope.pageCode,
      }, function (data) {
        $scope.postes = data.postes;
        if ($scope.postes && $scope.postes.length == 1) {
          $scope.poste = $scope.postes[0];
        }
        $scope.lecteursEnCours = data.lecteursEnCours;
        $scope.noReaderFound = data.noReaderFound;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.lecteursEnCours = null;
        $scope.noReaderFound = true;
      });
    }
    //-- supprime les sessions fantomes
    $scope.deleteOtherProcess = function (cle) {
      $translate('HubLecteur.ConfirmCancelSession').then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/utilisationPosteDelete/', {
            pageCode: $scope.pageCode,
            utilisationPosteCle: cle,
          }, function (data) {
            $scope.postes = data.postes;
            if ($scope.postes && $scope.postes.length == 1) {
              $scope.poste = $scope.postes[0];
            }
            $scope.lecteursEnCours = null;
            $scope.noReaderFound = false;
          }, function (data, statusCode, headers, config, statusText) {
            $scope.lecteursEnCours = null;
            $scope.noReaderFound = true;
            $translate('HubLecteur.ErrorCancelSession', { code: statusCode, text: statusText }).then(function (result) {
              $rootScope.error = result;
            });
          });
        }
      });
    }

    //------- Spécifique assemblage ------------------------------------------------------------------------
    $scope.startPrint = function () {
      if ($scope.postePrint && $scope.hub.open) {
        $scope.progression = 11;
        $scope.casqueHub.server.piloteWriter($scope.hub.connectionId, 2, $scope.assemblageCle, $scope.postePrint.configurationTxt) // 2 == print etiquette assemblage
          .done(function () {
            $scope.progression = 12;
            $scope.$apply();
          })
          .fail(function (error) { // erreur lors de l'envoie
            $scope.progression = 10;
            $scope.postePrint = null;
            $translate('Assemblage.ErrorWhenCanceling', { text: error }).then(function (result) {
              $rootScope.error = result;
              $scope.$apply();
            });
          });
      }
    }
    //-- fin d'un assemblage passe au suivant
    $scope.resumeAssemblage = function () {
      $scope.analyseurInit();
      $rootScope.error = '';
      $scope.progression = 8;
      $scope.saveToSession();
    }
    //-- détruit l'assemblage en cours
    $scope.deleteAssemblage = function () {
      if ($scope.assemblageCle) {
        $translate('Assemblage.ConfirmCancelAssemblage').then(function (result) {
          if (confirm(result)) {
            if ($scope.utilisationPosteCle) {
              $noHttp.delete('/api/assemblageDelete/', {
                pageCode: $scope.pageCode,
                cle: $scope.assemblageCle
              }, function (data) {
                $scope.assemblageCle = null;
                $scope.resumeAssemblage();
              }, function (data, statusCode, headers, config, statusText) {
                $translate('Assemblage.ErrorCancelAssemblage', { code: statusCode, text: statusText }).then(function (result) {
                  $rootScope.error = result;
                  $scope.assemblageCle = null;
                  $scope.resumeAssemblage();
                });
              });
            } else {
              $scope.termine();
            }
          }
        });
      }
    }
    //------- Spécifique livraison -------------------------------------------------------------------------
    $scope.isComplet = function (cmd) {
      if (cmd.pieces && cmd.pieces.length) {
        for (var i = 0; i < cmd.pieces.length; i++) {
          if (!(cmd.pieces[i] && cmd.pieces[i].tagLus && cmd.pieces[i].tagLus.length == 1)) {
            return false;
          }
        } // ici toutes les pièces ont une lecture
        return true;
      }
      return false;
    }
    $scope.addCasque = function (cmd) {
      if ($scope.analyseur) {
        if ($scope.analyseur.casques && $scope.analyseur.casques.length) {
          for (var i = 0; i < $scope.analyseur.casques.length; i++) {
            if ($scope.analyseur.casques[i] && $scope.analyseur.casques[i].cle == cmd.casqueCle) {
              $scope.analyseur.casques[i].nombre++;
              return;
            }
          }
        }
        $scope.analyseur.casques.push({
          cle: cmd.casqueCle,
          code: cmd.casqueCode,
          nom: cmd.casqueNom,
          photoUrl: cmd.casquePhotoUrl,
          nombre: 1,
        });
      }
    }
    //-- valide la fin d'un carton
    $scope.valideCarton = function (fini) {
      var tgs = [];
      if ($scope.analyseur.commandes && $scope.analyseur.commandes.length) { // on veut tous les asse_ids
        for (var i = 0; i < $scope.analyseur.commandes.length; i++) {
          tgs.push($scope.analyseur.commandes[i].cle);
        }
      }
      $noHttp.post('/api/analyseCloture/', {
        pageCode: $scope.pageCode,
        posteCle: $scope.poste.cle,
        utilisationPosteCle: $scope.utilisationPosteCle,
        utilisateurCle: $scope.user.cle,
        livraisonCle: ($scope.livraison && $scope.livraison.cle) ? $scope.livraison.cle : 0,
        cle: $scope.carton.cle,
        cartonIndex: $scope.cartonIndex,
        lectures: tgs,
      }, function (data) {
        $scope.utilisationPosteCle = data.utilisationPosteCle;
        if ($scope.cartons.length > 1) {
          $scope.carton = null;
        }
        $scope.livraison = data.livraison;
        if (fini) {
          $scope.cartonIndex = 0;
          $scope.clients = data.clients;
          $scope.configEmailOk = data.configEmailOk;
          $scope.printTheBL = true;
          $scope.processEnvoie = true;
          $scope.progression = 20; // saisie du client
        } else {
          $scope.cartonIndex++;
          $scope.processReset();
        }
      }, function (data, statusCode, headers, config, statusText) {
        $translate('Livraison.ErrorSaveCarton', { code: statusCode, text: statusText }).then(function (result) {
          $rootScope.error = result;
        });
      });
    }
    //-- Supprime toute la livraison en cours de constitution
    $scope.deleteLivraison = function () {
      if ($scope.livraison && $scope.livraison.cle) {
        $translate('Livraison.ConfirmCancelLivraison').then(function (result) {
          if (confirm(result)) {
            if ($scope.utilisationPosteCle) {
              $noHttp.delete('/api/assemblageDelete/', {
                pageCode: $scope.pageCode,
                Cle: $scope.livraison.cle,
              }, function (data) {
                $scope.resumeLivraison();
              }, function (data, statusCode, headers, config, statusText) {
                $translate('HubLecteur.ErrorCancelLivraison', { code: statusCode, text: statusText }).then(function (result) {
                  $rootScope.error = result;
                });
                $scope.resumeLivraison();
              });
            } else {
              $scope.termine();
            }
          }
        });
      }
    }
    //-- termine une livraison
    $scope.finaliseLivraison = function () {
      $noHttp.post('/api/finaliseLivraison/', {
        livraisonCle: $scope.livraison.cle,
        processEnvoie: $scope.processEnvoie && $scope.configEmailOk && ($scope.client.email != null),
        clientCle: $scope.client.cle,
      }, function (data) {
        if ($scope.printTheBL) { // impression du BL
          $scope.printBl($scope.livraison.cle);
        }
        $scope.resumeLivraison();
      }, function (data, statusCode, headers, config, statusText) {
        $translate('HubLecteur.ErrorSaveLivraison', { code: statusCode, text: statusText }).then(function (result) {
          $rootScope.error = result;
          $scope.resumeLivraison();
        });
      });
    }
    //--- imprimer le bon d'expédition
    $scope.printBl = function (cle) {
      var w = window.open('/Print.html#/livraison?cle=' + cle);
      angular.element(w).bind('load', function () {
        setTimeout(function () {
          w.close();
        }, 1000);
      });
    }    //--- réinit après la fin d'une livraison
    $scope.resumeLivraison = function () {
      $scope.livraison = null;
      $scope.carton = null;
      $scope.client = null;
      $scope.cartonIndex = 0;
      $scope.progression = 8; // lecture
      $scope.processReset();
    }
    //------- Spécifique consultation -------------------------------------------------------------------------
    $scope.RazTagConnus = function () {
      $scope.processReset();
      $scope.tagConnus = [];
    }
    $scope.RemoveTagConnu = function (t) {
      if (!t || !t.numero || !$scope.tagConnus || !$scope.tagConnus.length) {
        return;
      }
      var i = 0;
      while (i < $scope.tagConnus.length) {
        if ($scope.tagConnus[i].numero == t.numero) { // le tag trouvé 
          $scope.tagConnus.splice(i, 1); // on retire le tag de la listes
          return;
        }
        i++;
      } // not found !
    }
    //-------------- Debut de la page ----------------------------------------------------------------------
    $scope.analyseurInit();
    $scope.getPostes();
    $scope.loadFromSession();
    $scope.hubInit();
  }]);

//-- Impression des étiquettes d'une commande
app.controller('commandePrintController', ['$scope', '$rootScope', '$noHttp', '$route', '$location', 'webStorage', '$timeout', '$translate',
  function ($scope, $rootScope, $noHttp, $route, $location, webStorage, $timeout, $translate) {
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.progression = 0;
    //-- recupère les infos de la commande
    $scope.getInfos = function () {
      $noHttp.get('/api/commandeGet/', { cle: $scope.cle }, function (data) {
        $scope.commande = data.commande;
        if ($scope.commande == null || !$scope.commande.cle || !$scope.commande.validation || $scope.commande.impressionFinie) {
          //-- problème avec la commande ==> redirection
          $scope.goRead();
        }
        $scope.postes = data.postes; // La liste des lecteurs pour encoder
        if ($scope.postes && $scope.postes.length == 1) {
          $scope.poste = $scope.postes[0];
        } else {
          $scope.poste = null;
        }
        $scope.initHubAndSendCommande();
      }, function (data, statusCode, headers, config, statusText) {
        $scope.commande = null;
      });
    };
    //--- redirige vers la page edit commande
    $scope.goRead = function () {
      $location.path('/commandeEdit').search({ cle: $route.current.params.cle });
    }
    //--- redirige vers la page liste des commandes
    $scope.goList = function () {
      $location.path('/commandeList').search();
    }
    //--- initialise le hub et envoie la commande
    $scope.initHubAndSendCommande = function () {
      $scope.progression = 1;
      $scope.casqueHub = $.connection.casqueHub; // nom de la classe hub c#
      //-- compte rendu d'un encodage
      $scope.casqueHub.client.report = function (error, action, cle, msg) {
        if (error) { // erreur de traitement
          $timeout(function () {
            $rootScope.error = msg;
            $scope.poste = null;
            $scope.progression = 4;
          }, 5);
        } else { // réception d'une fin d'impression
          $timeout(function () {
            $scope.progression = 8;
            $scope.goRead();
          }, 50);
        }
      }
      $scope.casqueHub.client.progress = function (action, cle, index, total) {
        // TODO : géere action et cle
        $scope.progression = 7;
        $scope.progressIndex = index;
        $scope.progressTotal = total;
        $scope.$apply();
      }
      //--- démarre la connexion au hub
      $.connection.hub.stop();
      $.connection.hub.start()
        .done(function () { // hub initialisé
          $scope.connectionId = $.connection.hub.id;
          $scope.hubOpen = true;
          $scope.sayHello();
        })
        .fail(function () { // hub KO
          $scope.progression = 0;
          $translate('HubLecteur.ErrorHubClosed').then(function (result) {
            $rootScope.error = result;
            $scope.connectionId = null;
            $scope.hubOpen = false;
            $scope.$apply();
          });
        });
    }
    //-- cherche si le drivers est On
    $scope.sayHello = function () {
      $scope.progression = 2;
      $scope.casqueHub.server.hello(0)
          .done(function (values) { // Recue valeur
            if ((values & 1) == 0) { // aucun encodeur disponible
              $scope.progression = 3;
              $translate('HubLecteur.HubNoDriverForWriter').then(function (result) {
                $rootScope.error = result;
              });
            } else { // c'est Ok
              $scope.progression = 4;
              $rootScope.error = '';
              if ($scope.postes && $scope.postes.length == 1) {
                $scope.poste = $scope.postes[0];
              }
            }
            $scope.$apply();
          })
          .fail(function (error) { // erreur lors de l'envoie
            $translate('HubLecteur.ErrorWriter', { text: error }).then(function (result) {
              $rootScope.error = result;
            });
            $scope.progression = 3;
            $scope.$apply();
          });
    }
    //--- dès qu'on a un poste pour imprimer les tag on le fait !
    $scope.startPrint = function () {
      if ($scope.poste && $scope.hubOpen) {
        $scope.progression = 5;
        $rootScope.error = '';
        $scope.casqueHub.server.piloteWriter($scope.connectionId, 1, $scope.commande.cle, $scope.poste.configurationTxt) // 1 == Commande
          .done(function () {
            $scope.progression = 6;
            $scope.$apply();
          })
          .fail(function (error) { // erreur lors de l'envoie
            $scope.progression = 4;
            $scope.postePrint = null;
            $translate('Commande.ErrorPrinting', { text: error }).then(function (result) {
              $rootScope.error = result;
              $scope.$apply();
            });
          });
      }
    }
    //-- début de la page
    $scope.poste = null;
    $scope.hubOpen = false;
    $scope.cle = $route.current.params.cle;
    $scope.getInfos(); // on fait tous le temps pour avoir les données satellites
  }]);
