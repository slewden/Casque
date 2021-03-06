﻿//-------------------------------------------------------------------------------------------------------
//--- pour simplifier le HTML : Waiting ring et bouton retour
app.directive('waitingRingAndRetour', [function () {
  return {
    restrict: 'E',
    replace: true,
    template: '<div>'
            + ' <i class="fa fa-circle-o-notch fa-spin gray" data-ng-show="processing"></i>'
            + ' <a href="#/livraison">{{ \'Common.BoutonRetour\' | translate }}</a>'
            + '</div>',
  };
}]);

app.directive('showMessage', [function () {
  return {
    restrict: 'E',
    replace: true,
    scope: {
      message: '@',
    },
    template: '<span><i class="fa fa-times red"></i>&nbsp;{{ message | translate }}</span>',
    link: function (scope, element, attr, ctrl) {
      scope.message = attr.message;
    }
  };
}]);
//-------------------------------------------------------------------------------------------------------
//--- Controlleur Consultation : nouvelle version plus automatisée
//-------------------------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------------------------
app.controller('consultationController', ['$scope', '$rootScope', 'webStorage', '$noHttp', 'NOCONFIG', '$translate', '$timeout', 'parameters',
  function ($scope, $rootScope, webStorage, $noHttp, NOCONFIG, $translate, $timeout, parameters) {
    $scope.pageCode = parameters.mode;                        // Type d'action attendue
    $scope.user = new utilisateur(webStorage.get(userKey));   // le user qui as le jeton "utilisation poste"
    $scope.messageBloquant = null;                            // bloque la page si pas utilisable (ex : y a pas de tag en bdd !)
    $scope.manualStop = null;                                 // l'utilisateur a stoppé manuellement le lecteur
    $scope.postes = [];                                       // liste des postes de lecture possibles
    $scope.poste = null;                                      // poste sélectionné
    $scope.utilisationPosteCle = null;                        // la clé de l'utilisation poste
    $scope.tagManuel = null;                                  // N° demandé à la main par le user

    //--------------------------------------------------------------------- Méthodes privées : utilitaires -
    //-- défini un message Ok
    var setMessageOk = function (msg) {
      $rootScope.error = null;
      $scope.reader.messageOk = msg;
      $scope.reader.messageKo = null;
    }
    //-- défini un message Ko
    var setMessageKo = function (msg) {
      $rootScope.error = msg;
      $scope.reader.messageOk = null;
      $scope.reader.messageKo = msg;
    }
    //---------------------------------------------------------------- Bdd : Les postes / utilisationPoste -
    //-- Load la liste des postes possibles 
    var getPostes = function () {
      loadFromSession();
      $noHttp.get('/api/lecteursGet/', {
        pageCode: $scope.pageCode,
        UtilisationPosteCle: $scope.utilisationPosteCle,
      }, function (data) {
        $scope.messageBloquant = data.messageBloquant;
        $scope.postes = data.postes;
        if (!$scope.messageBloquant) { // pas de message : on a pas le droit de continuer
          if ($scope.utilisationPosteCle) { // on a un n° d'utilisation c'est que le lecteur est peut être en route ?
            $translate('Consultation.QueryStatutReader').then(function (result) {
              setMessageOk(result);
            });
          }
          if ($scope.postes && $scope.postes.length == 1) { // préselection et démarrage (si possible)
            $scope.poste = $scope.postes[0];
            if (!$scope.manualStop) {
              $scope.startReception();
            }
          }
        }
      }, function (data, statusCode, headers, config, statusText) {
        $scope.postes = null;
        setMessageKo(statusText);
      });
    };
    //-- annule l'utilisation poste en cours == Delete de la bdd
    var annuleUtilisationPoste = function (withRestart) {
      if ($scope.utilisationPosteCle) { // y a une !
        $noHttp.delete('/api/utilisationPosteDelete/', {
          pageCode: $scope.pageCode,
          utilisationPosteCle: $scope.utilisationPosteCle
        }, function (data) {
          $scope.utilisationPosteCle = null;
          $scope.postes = data.postes;
          if ($scope.postes && $scope.postes.length == 1) {
            $scope.poste = $scope.postes[0];
            // ici on ne relance pas le lecteur : on vient de s'arreter !
            if (withRestart) {
              $scope.startReception();
            }
          }
          termine();
        }, function (data, statusCode, headers, config, statusText) {
          $translate('HubLecteur.ErrorWhenCanceling', { code: statusCode, text: statusText }).then(function (result) {
            setMessageKo(result);
          });
        });
      } else { // y en avait pas !
        termine();
      }
    }
    //-------------------------------------------------------------------- Initialisations / Finalisations -
    //-- initialise les infos du lecteur
    var readerInit = function () {
      $scope.reader = {
        statut: NOCONFIG.READER_STOPPED,                        // statut du lecteur stop / starting / started / stopping / ??
        stopIsCancel: false,                                    // pour savoir qui demande l'arrêt (le user ou un problème)
        messageOk: null,                                        // le dernier message ok du lecteur
        messageKo: null,                                        // le dernier message ko du lecteur
      };
    }
    //-- initialise les infos de l'analyseur de tag lus
    var analyseurInit = function (dontSave) {
      $scope.tagInconnus = [];
      $scope.tagConnus = [];
      $scope.analyseur = {
        lectures: [],
        nbDemandeEnCours: 0,
      };
      if (!dontSave) {
        saveToSession();
      }
    }
    //-- Après la fin d'une action : remet tout en ordre
    var termine = function () {
      $scope.reader.statut = NOCONFIG.READER_STOPPED;
      if ($scope.reader.stopIsCancel) {
        $translate('HubLecteur.LecteurCanceled').then(function (result) {
          setMessageOk(result);
        });
      } else {
        $translate('HubLecteur.LecteurSaved').then(function (result) {
          setMessageOk(result);
        });
      }
      saveToSession();
    }
    //---------------------------------------------------------------- Méthodes de l'interface pour le hub -
    //-- demarre une consultation
    $scope.startReception = function () {
      if ($scope.poste && $scope.poste.cle && $rootScope.roothub && $rootScope.roothub.actifLecteur) {
        // on a un poste (valide), le hub est ok et ya un lecteur, le user à pas tout stoppé
        if (!$scope.utilisationPosteCle) {
          // Y a un lecteur et le user a pas tout stoppé ==> on peut démarrer
          $scope.manualStop = null;
          $rootScope.roothub.message = '';
          $noHttp.put('/api/utilisationPosteStart/', {
            pageCode: $scope.pageCode,
            posteCle: $scope.poste.cle,
            utilisateurCle: $scope.user.cle,
          }, function (data) { // on a l'autorisation
            $scope.utilisationPosteCle = data.utilisationPosteCle;
            analyseurInit();
            readerStart();
          }, function (data, statusCode, headers, config, statusText) { // Fail
            $scope.utilisationPosteCle = null;
            setMessageKo(statusText);
          });
        } else { // on a déjà un utilisation poste ==> demande de l'état du lecteur
          analyseurInit();
          readerQueryStatut();
        }
      }
    }
    //-- Anule le démarrage
    $scope.cancelStarting = function () {
      if ($scope.reader.statut == NOCONFIG.READER_STATING) {
        $scope.manualStop = true;
        $translate('HubLecteur.Statut5').then(function (result) {
          setMessageOk(result);
        });
        readerStop(true);
      }
    }
    //--- traite un reset
    $scope.readerReset = function () {
      if ($rootScope.roothub.open && $scope.poste && $scope.reader.statut == NOCONFIG.READER_STARTED) {
        $rootScope.rootCasqueHub.server.piloteReader($rootScope.roothub.connectionId, NOCONFIG.ACTIONLECTEURRESETLECTURES, $scope.poste.configurationTxt)
        .done(function () { // nothing to do le lecteur répondra par un message de statut
          analyseurInit();
        })
        .fail(function (error) { // erreur lors de l'envoie
          $translate('HubLecteur.ResetFail', { msg: error }).then(function (result) {
            setMessageKo(result);
          });
        });
      } else {
        analyseurInit();
      }
    }
    //-- termine une réception == sauve le tout en bdd
    $scope.stopReception = function () {
      if ($scope.reader.statut == NOCONFIG.READER_STARTED) {
        $scope.manualStop = true;
        $translate('HubLecteur.Statut555').then(function (result) {
          setMessageOk(result);
        });
        readerStop(false);
      }
    }
    //-- essaie de nouveau de se connecter
    $scope.retry = function () {
      if ($scope.utilisationPosteCle) {
        annuleUtilisationPoste(true);
      } else {
        $scope.startReception();
      }
    }
    //-- Voir quels processus sont en cours
    $scope.checkOtherProcess = function () {
      $noHttp.get('/api/lecteursEnCours/', {
        pageCode: $scope.pageCode,
      }, function (data) {
        $scope.postes = data.postes;
        if ($scope.postes && $scope.postes.length == 1) {
          $scope.poste = $scope.postes[0];
          if (!$scope.manualStop) {
            $scope.startReception();
          }
          return;
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
              if (!$scope.manualStop) {
                $scope.startReception();
              }
              return;
            }
            $scope.lecteursEnCours = null;
            $scope.noReaderFound = false;
          }, function (data, statusCode, headers, config, statusText) {
            $scope.lecteursEnCours = null;
            $scope.noReaderFound = true;
            $translate('HubLecteur.ErrorCancelSession', { code: statusCode, text: statusText }).then(function (result) {
              setMessageKo(result)
            });
          });
        }
      });
    }
    //-------------------------------------------------------------------------------------------- Session -
    //-- sauve en session les paramètres
    var saveToSession = function () {
      webStorage.add($scope.pageCode + 'Controller', {
        poste: $scope.poste,
        utilisationPosteCle: $scope.utilisationPosteCle,
      });
    }
    //-- Recupère de la session les paramètres
    var loadFromSession = function () {
      var key = webStorage.get($scope.pageCode + 'Controller');
      if (key) {
        if (key.poste) {
          $scope.poste = key.poste;
        }
        if (key.utilisationPosteCle) {
          $scope.utilisationPosteCle = key.utilisationPosteCle;
        }
      }
    }
    //----------------------------------------------------------------------------------- Parle au lecteur -
    //-- Démarrer la lecture
    var readerStart = function () {
      if ($rootScope.roothub.open) { // hub ouvert
        if ($scope.reader.statut == NOCONFIG.READER_STOPPED) { // le lecteur est bien déja stoppé
          $scope.reader.statut = NOCONFIG.READER_STATING;
          saveToSession();
          $rootScope.rootCasqueHub.server.piloteReader($rootScope.roothub.connectionId, NOCONFIG.ACTIONLECTEURSTART, $scope.poste.configurationTxt)
          .done(function () {
            $translate('HubLecteur.LecteurStarting').then(function (result) {
              setMessageOk(result);
            });
          })
          .fail(function (error) { // erreur lors de l'envoie
            $translate('HubLecteur.LecteurStartFail', { msg: error }).then(function (result) {
              setMessageKo(result);
            });
          });
        }
      } else {
        $translate('HubLecteur.ErrorHubClosed').then(function (result) {
          setMessageKo(result);
        });
      }
    }
    //-- arret la lecture pour valider ou annuler
    var readerStop = function (cancel) {
      if ($rootScope.roothub.open && $scope.poste && $scope.reader.statut == NOCONFIG.READER_STARTED) {
        // le lecteur est démarré :on le stoppe
        $scope.reader.stopIsCancel = cancel;
        $scope.reader.statut = NOCONFIG.READER_STOPPING;
        saveToSession();
        $rootScope.rootCasqueHub.server.piloteReader($rootScope.roothub.connectionId, NOCONFIG.ACTIONLECTEURSTOP, $scope.poste.configurationTxt)
        .done(function () {
          annuleUtilisationPoste();
        })
        .fail(function (error) { // erreur lors de l'envoie
          $translate('HubLecteur.LecteurStopFail', { msg: error }).then(function (result) {
            setMessageKo(result);
          });
        });
      } else { // au cas ou ?
        annuleUtilisationPoste();
      }
    }
    //--- demande au lecteur son état
    var readerQueryStatut = function (withAction) {
      if ($rootScope.roothub.open) {
        $scope.reader.statut = NOCONFIG.READER_UNKNOW;
        saveToSession();
        $rootScope.rootCasqueHub.server.piloteReader($rootScope.roothub.connectionId, NOCONFIG.ACTIONLECTEURQUERYSTATUT, $scope.poste.configurationTxt)
        .done(function (erreur, action, message) { // nothing to do le lecteur répondra par un message de statut
          setMessageOk('err ' + erreur + ' Act ' + action + ' : ' + message );
        })
        .fail(function (error) { // erreur lors de l'envoie
          $translate('HubLecteur.LecteurStopFail', { msg: error }).then(function (result) {
            setMessageKo(result);
          });
        });
      }
    }
    //-------------------------------------------------------------------- Analyse des tags lus (ou saisi) -
    //--- s'assure que les tag inconnus sont mis à part
    var checkTagInconnus = function (ti) {
      if (!$scope.tagInconnus) {
        $scope.tagInconnus = [];
      }
      if (!ti || !ti.length) { // pas de tag pas de travail 
        return;
      }
      for (var i = 0; i < ti.length; i++) {
        $scope.tagInconnus.push(ti[i]);
        removeTagInconnu(ti[i]);
      }
    }
    //--- Retire de la liste des lecture les tag inconnus
    var removeTagInconnu = function (tag) {
      if ($scope.analyseur && $scope.analyseur.lectures && $scope.analyseur.lectures.length && tag) {
        tag = tag.trim();
        var i = 0
        while (i < $scope.analyseur.lectures.length) {
          if ($scope.analyseur.lectures[i] == tag) { // le tag trouvé
            $scope.analyseur.lectures.splice(i, 1); // on retire le tag de la listes des tags inconnus
            return;
          } else {
            i++;
          }
        }
      }
    }
    //---------------------------------------------------------------------------- Spécifique consultation -
    //-- envoie une demande d'analyse des tags lus
    $scope.faitUneDemande = function () {
      if ($scope.analyseur.nbDemandeEnCours == 0) { // pas déjà une demande on peut y aller
        saveToSession();
        $scope.analyseur.nbDemandeEnCours++;
        $noHttp.get('/api/analyseTag/', {
          pageCode: $scope.pageCode,
          utilisationPosteCle: $scope.utilisationPosteCle,
          tags: $scope.analyseur.lectures,
        }, function (data) {
          $scope.analyseur.nbDemandeEnCours--;
          checkTagInconnus(data.tagInconnus);
          if (data.tagConnus) { // on a trouvé des tags connus
            if (!$scope.tagConnus) {
              $scope.tagConnus = [];
            }
            for (var i = 0; i < data.tagConnus.length; i++) { // ajout des tags connus
              $scope.tagConnus.unshift(data.tagConnus[i]);
              removeTagInconnu(data.tagConnus[i].numero.trim());
            }
          }
        }, function (data, statusCode, headers, config, statusText) {
          $scope.analyseur.nbDemandeEnCours--;
        });
      }
    }
    //-- Raz de la liste des tags inconnus
    $scope.razInconnus = function () {
      $scope.tagInconnus = [];
      saveToSession();
    }
    //-- efface une ligne de résultat
    $scope.removeTagConnu = function (t) {
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
    //-- simule l'arrivée d'un tag
    $scope.queryManuel = function () {
      if ($scope.tagManuel) { // nouveau tag arrivé
        if ($scope.analyseur.lectures.indexOf($scope.tagManuel) == -1) {
          $scope.queryTag($scope.tagManuel.trim());
          $scope.tagManuel = null;
        }
      }
    }
    //-- interroge pour avoir le detail de tagNum
    $scope.queryTag = function (tagNum) {
      $scope.analyseur.lectures.unshift(tagNum.trim());
      $scope.faitUneDemande();
    }
    //-- indique s'il y a plus d'une lecture fait (pour le bouton reset all)
    $scope.canResetAll = function () {
      var nt = $scope.tagInconnus ? $scope.tagInconnus.length : 0;
      var lt = $scope.analyseur && $scope.analyseur.lectures ? $scope.analyseur.lectures.length : 0;
      var tc = $scope.tagConnus ? $scope.tagConnus.length : 0;
      return nt + lt + tc > 1;
    }
    //-- Demane le détail d'un assemblage
    $scope.showDetailAssemblage = function (taglu) {
      if (taglu != null && taglu.assemblage && taglu.assemblage.cle) {
        $noHttp.get('/api/analyseAssemblage/', {
          cle: taglu.assemblage.cle,
        }, function (data) {
          if (data && data.pieces && data.pieces.length) {
            taglu.assemblage.pieces = data.pieces;
          }
        }, function (data, statusCode, headers, config, statusText) {
        });
      }
    }
    //-- efface le détail d'un assemblage 
    $scope.removeDetailAssemblage = function (taglu) {
      if (taglu != null && taglu.assemblage && taglu.assemblage.pieces) {
        taglu.assemblage.pieces = null;
      }
    }
    //-------------------------------------------------------------------------- Le events Hub et lecteurs -
    //-- surveille l'activation du hub
    $scope.$watch('roothub.actifLecteur', function (value) {
      // peut prendre 0 = aucun lecteur, 1 = dispo, 2 = occupé
      // pas de time out necessaire ici l'app a déjà fait le boulot
      if (value == 1) { // ca passe à dispo on relance le lecteur
        if ($scope.reader.statut == NOCONFIG.READER_STOPPED && $scope.poste && !$scope.manualStop) {
          // le lecteur est arrété, on a un poste et l'utilisateur n'a pas stoppé le lecteur
          // ==> on peut démarrer automatiquement
          $timeout(function () {
            $scope.startReception();
          }, 500); // attente de quelques ms pour que le drivers lecteur soit bien Ok !
        }
      } else if (value == 0) { // passage à 0 ==> init
        if ($scope.reader.statut == NOCONFIG.READER_STARTED) {
          annuleUtilisationPoste();
        }
      }
    });
    //-- surveille l'arrivée de TAGs
    $scope.$watch('roothub.newTag', function (value) {
      $timeout(function () {
        if (value) { // nouveau tag arrivé
          $scope.analyseur.lectures.unshift(value);
          saveToSession();
          $scope.faitUneDemande();
        }
      });
    });
    //-- surveille l'arrivée de message en provenance du lecteur
    $scope.$watch('roothub.newMessage', function (value) {
      $timeout(function () {
        if (value) { // nouveau rapport du lecteur : on le traite
          switch (value.action) {
            case 1: // Démarrer
              if (value.error) { // Erreur de démarrage : donc on stoppe le lecteur
                //readerStop(true);
                $scope.reader.statut = NOCONFIG.READER_STOPPED;
                setMessageKo(value.msg);
              } else { // on memorise que le lecteur a correctement démarré
                $scope.reader.statut = NOCONFIG.READER_STARTED;
                setMessageOk(value.msg);
              }
              saveToSession();
              break;
            case 2: // Arreter
              if (value.error) { // Erreur lors du stop : on a pas la main sur le lecteur (c'est quelqu'un d'autre
                $scope.reader.statut = NOCONFIG.READER_STOPPED;
                setMessageKo(value.msg);
              } else { // pas d'erreur le lecteur est bien stoppé
                $scope.reader.statut = NOCONFIG.READER_STOPPED;
                setMessageOk(value.msg);
                annuleUtilisationPoste();
              }
              saveToSession();
              break;
            case 3: // Reset des lectures
              if (value.error) { // erreur lors du reset des lectures
                setMessageKo(value.msg);
              } else { // Ok reset fini
                setMessageOk(value.msg);
                // TODO : vider les lectures
              }
              break;
            case 4: // query statut
              if (value.error) {
                setMessageKo(value.msg);
              } else {
                if (value.action == NOCONFIG.ACTIONLECTEURSTART) { // le lecteur est en route
                  $scope.reader.statut = NOCONFIG.READER_STARTED;
                  setMessageOk(value.msg);
                } else if (value.action == NOCONFIG.ACTIONLECTEURSTOP) { // le lecteur est stoppé
                  $scope.reader.statut = NOCONFIG.READER_STOPPED;
                  setMessageOk(value.msg);
                }
                saveToSession();
              }
              break;
          }
          // on efface le message on l'a traité
          $rootScope.roothub.newMessage = null;
        }
      });
    });
    //----------------------------------------------------------------------------------------------- Page -
    //-- debut de la page
    analyseurInit(true);
    readerInit();
    getPostes();
  }
]);
