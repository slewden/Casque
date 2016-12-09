var app = angular.module('testApp', []);

app.controller('testController', ['$scope',
  function ($scope) {
    //-- initialisation des variables du hub (à appeller une seule fois)
    $scope.initHub = function () {
      $scope.casqueHub = $.connection.casqueHub; // nom de la classe hub c#
      var cnn = $.hubConnection();
      //cnn.closed(function () { $scope.tags.push("receive closed"); }); // not working ????
      cnn.connectionSlow(function () { $scope.tags.push("receive connection slow"); });
      cnn.error(function (ex) { $scope.tags.push("receive error : " + ex); });
      cnn.received(function (obj) { $scope.tags.push("receive receive : " + obj); });
      cnn.reconnected(function () { $scope.tags.push("receive reconnected"); });
      cnn.reconnecting(function () { $scope.tags.push("receive reconnecting..."); });
      cnn.stateChanged(function (state) { $scope.tags.push("receive state change from " + state.oldState + " to " + state.newState); });

      //-- réception d'un numéro de tag : nom de la méthode utilisée pour poussé hors du hub c#
      $scope.casqueHub.client.tag = function (tag) {
        // filtre la donnée pour éviter les injections de scripts
        var encodedTag = $('<div />').text(tag).html();
        var dt = new Date();
        $scope.tags.push('Reçu Tag (' + encodedTag + ')');
        $scope.$apply();
      };
      //-- réception d'un message de retour d'un lecteur
      $scope.casqueHub.client.message = function (error, action, msg) {
        var act = 'N°' + action;
        if (action == 1) {
          act = 'Démarre';
        } else if (action == 2) {
          act = 'Stoppe';
        } else if (action == 3) {
          act = 'Reset Lecture';
        } else if (action == 4) {
          act = 'Etat lecteur';
        }
        $scope.tags.push('Reçu Lecteur (' + (error ? 'Erreur' : 'Message') + ', ' + act + ', "' + msg + '")');
        $scope.$apply();
      };
      //-- compte rendu d'un encodage
      $scope.casqueHub.client.report = function (error, action, cle, msg) {
        var act = 'N°' + action;
        if (action == 1) {
          act = 'Commande';
        } else if (action == 2) {
          act = 'Assemblage';
        } else if (action == 3) {
          act = 'Cancel';
        }
        $scope.tags.push('Reçu Encodeur (' + (error ? 'Erreur' : 'Message') + ', ' + act + '[id ' + cle + '], "' + msg + '")');
        $scope.$apply();
      }
      //-- réception des commandes de lecteur (pour débug)
      $scope.casqueHub.client.fireAction = function (demande) {
        var act = 'N°' + demande.Action;
        if (demande.Action == 1) {
          act = 'Démarre';
        } else if (demande.Action == 2) {
          act = 'Stoppe';
        } else if (demande.Action == 3) {
          act = 'Reset Lecture';
        } else if (demande.Action == 4) {
          act = 'Etat lecteur';
        }
        $scope.tags.push('Reçu action de lecture ( ' + demande.ClientId + ', ' + act + ', xml ' + demande.XmlParameter + ')');
        $scope.$apply();
      }
      //-- réception des commandes d'un encodeur (pour débug)
      $scope.casqueHub.client.processCommande = function (demande) {
        var act = 'N°' + demande.Action;
        if (demande.Action == 1) {
          act = 'Encoder et imprimer les tags d\'une commande';
        } else if (demande.Action == 2) {
          act = 'Encoder le tag d\'un assemblage';
        } else if (demande.Action == 3) {
          act = 'Annuler l\'opération en cours';
        } else if (demande.Action == 4) {
          act = 'Etat encodeur';
        }
        $scope.tags.push('Reçu action d\'encodage ( ' + demande.ClientId + ', ' + act + ', cle ' + demande.Cle + ', xml ' + demande.XmlParameter + ')');
        $scope.$apply();
      }
      //-- reception d'un message de progression
      $scope.casqueHub.client.progress = function (action, cle, index, total) {
        if (action == 0 && cle == 0) {
          $scope.tags.push('Reçu Encodeur ( Attente commande )');
        } else {

          var act = 'N°' + action;
          if (action == 1) {
            act = 'Encode Commande';
          } else if (action == 2) {
            act = 'Encode Casque';
          }
          $scope.tags.push('Reçu Encodeur ( Progression ' + act + '[id ' + cle + '] ' + index + ' sur ' + total + ')');
        }
        $scope.$apply();
      };
    };
    //-- Se connecter au hub
    $scope.Connect = function () {
      if (!$scope.hubOpen) {
        ////$.connection.hub.logging = true; // vraiment très verbeux pour debug only !!
        $.connection.hub.start().done(function () { // hub initialisé
          $scope.connectionId = $.connection.hub.id;
          $scope.modeConnexion = $.connection.hub.transport.name;
          $scope.simulCommande.clientId = $scope.connectionId;
          $scope.simulEncode.clientId = $scope.connectionId;
          $scope.hubOpen = true;
          $scope.$apply();
        }).fail(function () { // hub KO
          $scope.hubOpen = false;
          $scope.message = 'Impossible de se connecter au hub !';
          $scope.connectionId = null;
          $scope.modeConnexion = null;
          $scope.$apply();
        });
      }
    }
    //-- déconnexion du hub
    $scope.Disconnect = function () {
      if ($scope.hubOpen) {
        $.connection.hub.stop();
        $scope.message = null;
        $scope.connectionId = null;
        $scope.modeConnexion = null;
        $scope.simulCommande.clientId = null;
        $scope.simulEncode.clientId = null;
        $scope.hubOpen = false;
      }
    }
    //-- Appel à ResetHub
    $scope.processResetHub = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.resetHub($scope.typeReset.total)
          .done(function (values) { // Recue valeur
            $scope.typeLecteurs.done = true;
            $scope.typeLecteurs.actifLecteur = (values & 2) == 2;
            $scope.typeLecteurs.actifEncodeur = (values & 1) == 1;
            $scope.typeLecteurs.actifRien = !$scope.typeLecteurs.actifLecteur && !$scope.typeLecteurs.actifEncodeur;
            $scope.$apply();
          })
          .fail(function (error) { // erreur lors de l'envoie
            $scope.tags.push("Fail to send Reset Hub : " + error);
          });
      }
    }
    //-- Appel à Hello
    $scope.processHello = function (sansLecteur) {
      if ($scope.hubOpen) {
        var n = sansLecteur ? 0 : $scope.typeLecteurs.total;
        $scope.casqueHub.server.hello(n)
          .done(function(values){ // Recue valeur
            $scope.typeLecteurs.done = true;
            $scope.typeLecteurs.actifLecteur = (values & 2) == 2;
            $scope.typeLecteurs.actifEncodeur = (values & 1) == 1;
            $scope.typeLecteurs.actifRien = !$scope.typeLecteurs.actifLecteur && !$scope.typeLecteurs.actifEncodeur;
            $scope.$apply();
          })
          .fail(function (error) { // erreur lors de l'envoie
            $scope.typeLecteurs.done = false;
            $scope.tags.push("Fail to send hello : " + error);
          });
      }
    }
    //-- Appel à Bye
    $scope.processBye = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.bye($scope.typeLecteurs.total)
          .done(function (values) { // Recue valeur
            $scope.typeLecteurs.done = true;
            $scope.typeLecteurs.actifLecteur = (values & 2) == 2;
            $scope.typeLecteurs.actifEncodeur = (values & 1) == 1;
            $scope.typeLecteurs.actifRien = !$scope.typeLecteurs.actifLecteur && !$scope.typeLecteurs.actifEncodeur;
            $scope.$apply();
          }).fail(function (error) { // erreur lors de l'envoie
            $scope.tags.push("Fail to send bye : " + error);
          });
      }
    }
    //-- simule l'envoie d'un n° de tag
    $scope.processSimulTag = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.newTag($scope.simulTag.clientId, $scope.simulTag.tag)
          .fail(function (error) { // erreur lors de l'envoie
            $scope.tags.push("Fail to send new tag : " + error);
          });
      }
    }
    //-- simule l'envoie d'une notification d'un lecteur
    $scope.processSimulMessage = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.notity($scope.simulMessage.clientId, $scope.simulMessage.erreur, $scope.simulMessage.action, $scope.simulMessage.message)
          .fail(function (error) { // erreur lors de l'envoie
            $scope.tags.push("Fail to send new notify : " + error);
          });
      }
    }
    //-- Démarrer la lecture
    $scope.start = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.piloteReader($scope.simulCommande.clientId, 1, $scope.simulCommande.paramXml)  // 1 = start
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to piloteReader ON : " + error);
        });
      }
    }
    //-- Stopper la lecture
    $scope.stop = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.piloteReader($scope.simulCommande.clientId, 2, $scope.simulCommande.paramXml) // 2 = stop
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to piloteReader OFF : " + error);
        });
      }
    }
    //-- Reinit des lectures dans le lecteur
    $scope.resetLectures = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.piloteReader($scope.simulCommande.clientId, 3, $scope.simulCommande.paramXml) // 3 = resetlectures
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to reset reads : " + error);
        });
      }
    }
    //-- Query statut
    $scope.queryStatut = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.piloteReader($scope.simulCommande.clientId, 4, $scope.simulCommande.paramXml) // 4 = query statut
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to reset reads : " + error);
        });
      }
    }
    //-- Encodage Tag
    $scope.encodeTag = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.piloteWriter($scope.simulEncode.clientId, $scope.simulEncode.action, $scope.simulEncode.cle, $scope.simulEncode.paramXml) // 1 == Commande, 2 == assemblage, 3 = cancel, 4 == Query statut
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to send encode : " + error);
        });
      }
    }
    //-- Encode Cancel
    $scope.encodeCancel = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.piloteWriter($scope.simulEncode.clientId, 3, $scope.simulEncode.cle, $scope.simulEncode.paramXml) // 1 == Commande, 2 == assemblage, 3 = cancel, 4 == Query statut
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to send encode : " + error);
        });
      }
    }
    //-- Encode Query statut
    $scope.encodeQuery = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.piloteWriter($scope.simulEncode.clientId, 4, $scope.simulEncode.cle, $scope.simulEncode.paramXml) // 1 == Commande, 2 == assemblage, 3 = cancel, 4 == Query statut
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to send encode : " + error);
        });
      }
    }
    //-- process Rapport
    $scope.processRapport = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.rapporte($scope.simulRapport.clientId, $scope.simulRapport.erreur, $scope.simulRapport.action, $scope.simulRapport.cle, $scope.simulRapport.message)
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to send progresse : " + error);
        });
      }
    }
    //-- message de progression
    $scope.processProgresse = function () {
      if ($scope.hubOpen) {
        $scope.casqueHub.server.progresse($scope.simulprogresse.clientId, $scope.simulprogresse.action, $scope.simulprogresse.cle, $scope.simulprogresse.index, $scope.simulprogresse.total)
        .fail(function (error) { // erreur lors de l'envoie
          console.log("fail to send progresse : " + error);
        });
      }
    }
    //-- clear logs
    $scope.clearTags = function () {
      $scope.tags = [];
    }
    //-- change d'onglet
    $scope.changeOnglet = function (x) {
      $scope.onglet = x;
      $scope.typeLecteurs.done = false;
    }
    //-------------------------------------------------------------------------------
    //-- debut de la page
    $scope.onglet = 'client';

    $scope.simulTag = {
      clientId: '008',
      tag: 'FF FF FF 00',
    };
    $scope.simulMessage = {
      clientId: '007',
      erreur: true,
      action: 1,
      message: 'Test erreur démarre',
    };
    $scope.simulprogresse = {
      action: 1,
      cle:43,
      clientId: '00A',
      index: 1,
      total: 10,
    };
    $scope.simulCommande = {
      clientId: '007',
      paramXml: '<config>\n  <adresseIp>192.168.1.20</adresseIp>\n  <seuil>5</seuil>\n  <antennes>\n    <antenne index="0" gainDb="22" position="1" />\n  </antennes>\n</config>',
    };
    $scope.simulEncode = {
      action: 1,  // 1 : commande
      clientId: '008',
      cle: 36,
      paramXml: '<config>\n  <adresseIp>192.168.1.20</adresseIp>\n  <seuil>5</seuil>\n  <antennes>\n    <antenne index="0" gainDb="22" position="1" />\n  </antennes>\n</config>',
    };
    $scope.simulRapport = {
      clientId: '007',
      erreur:false,
      action:1,
      cle:4,
      message:'Ok ca cruise',
    };
    $scope.typeLecteurs = {
      done: false,
      lecteurRead: false,
      lecteurEncode: false,
      total: 0,
    };
    $scope.$watchGroup(['typeLecteurs.lecteurRead', 'typeLecteurs.lecteurEncode'], function () {
      var n = 0;
      if ($scope.typeLecteurs.lecteurRead) { n += 2; }
      if ($scope.typeLecteurs.lecteurEncode) { n += 1; }
      $scope.typeLecteurs.total = n;
    });

    $scope.typeReset = {
      done: false,
      lecteurRead: true,
      lecteurEncode: true,
      total: 0,
    };
    $scope.$watchGroup(['typeReset.lecteurRead', 'typeReset.lecteurEncode'], function () {
      var n = 0;
      if ($scope.typeReset.lecteurRead) { n += 2; }
      if ($scope.typeReset.lecteurEncode) { n += 1; }
      $scope.typeReset.total = n;
    });


    $scope.message = null;
    $scope.hubOpen = false;
    $scope.tags = [];
    $scope.initHub();
    $scope.Connect();
  }]);
