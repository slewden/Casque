// l'application angular
var app = angular.module('casqueApp', ['ngRoute', 'ngSanitize', 'angularModalService', 'pascalprecht.translate', 'webStorageModule', '$noConst', 'no.paging']);
var userKey = 'CasqueUserKey';
var returnKey = 'returnUrl';

//-- routes -----------------------------------------------------------------------------------------------------------
app.config(['$routeProvider', function ($routeProvider) {
  $routeProvider
    .when('/', { templateUrl: '/Partials/Home.html', controller: 'homeController' })
    .when('/login', { templateUrl: '/Partials/Login.html', controller: 'loginController' })
    .when('/configuration', { templateUrl: '/Partials/Configuration.html', controller: 'configurationController' })
    .when('/administration', { templateUrl: '/Partials/Administration.html', controller: 'administrationController' })
    //-- menu 1 : commandes
    .when('/commande', { templateUrl: '/Partials/CommandeResume.html', controller: 'commandeResumeController' })
    .when('/commandeList', { templateUrl: '/Partials/CommandeList.html', controller: 'commandeListController' })
    .when('/commandeEdit', { templateUrl: '/Partials/CommandeEdit.html', controller: 'commandeEditController' })
    .when('/commandePrint', { templateUrl: '/Partials/CommandePrint.html', controller: 'commandePrintController' })
    .when('/reception', { templateUrl: '/Partials/ReceptionEdit.html', controller: 'receptionController', resolve: { parameters: function () { return { mode: 'reception' }; } } })
    //-- menu 3
    .when('/assemblage', { templateUrl: '/Partials/AssemblageResume.html', controller: 'assemblageResumeController' })
    .when('/assemblageList', { templateUrl: '/Partials/AssemblageList.html', controller: 'assemblageListController' })
    .when('/assemblageEdit', { templateUrl: '/Partials/AssemblageEdit.html', controller: 'receptionController', resolve: { parameters: function () { return { mode: 'assemblage' }; } } })
    .when('/assemblageDetail', { templateUrl: '/Partials/AssemblageDetail.html', controller: 'assemblageDetailController' })
    //-- menu 4
    .when('/livraison', { templateUrl: '/Partials/LivraisonResume.html', controller: 'livraisonResumeController' })
    .when('/livraisonList', { templateUrl: '/Partials/LivraisonList.html', controller: 'livraisonListController' })
    .when('/livraisonEdit', { templateUrl: '/Partials/LivraisonEdit.html', controller: 'receptionController', resolve: { parameters: function () { return { mode: 'livraison' }; } } })
    .when('/livraisonDetail', { templateUrl: '/Partials/LivraisonDetail.html', controller: 'livraisonDetailController' })
    .when('/consultation', { templateUrl: '/Partials/Consultation.html', controller: 'consultationController', resolve: { parameters: function () { return { mode: 'consultation' }; } } })

    //--- menu 6
    .when('/client', { templateUrl: '/Partials/ClientList.html', controller: 'clientListController' })
    .when('/clientEdit', { templateUrl: '/Partials/ClientEdit.html', controller: 'clientEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/clientRead', { templateUrl: '/Partials/ClientRead.html', controller: 'clientEditController', resolve: { parameters: function () { return { modeRead: true }; } } })

    .when('/fournisseur', { templateUrl: '/Partials/FournisseurList.html', controller: 'fournisseurListController' })
    .when('/fournisseurEdit', { templateUrl: '/Partials/FournisseurEdit.html', controller: 'fournisseurEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/fournisseurRead', { templateUrl: '/Partials/FournisseurRead.html', controller: 'fournisseurEditController', resolve: { parameters: function () { return { modeRead: true }; } } })
    .when('/fournisseurPiece', { templateUrl: '/Partials/FournisseurPiece.html', controller: 'fournisseurPieceController' })

    .when('/typePiece', { templateUrl: '/Partials/TypePieceList.html', controller: 'typePieceListController' })
    .when('/typePieceEdit', { templateUrl: '/Partials/TypePieceEdit.html', controller: 'typePieceEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/typePieceRead', { templateUrl: '/Partials/TypePieceRead.html', controller: 'typePieceEditController', resolve: { parameters: function () { return { modeRead: true }; } } })

    .when('/casque', { templateUrl: '/Partials/CasqueList.html', controller: 'casqueListController' })
    .when('/casqueEdit', { templateUrl: '/Partials/CasqueEdit.html', controller: 'casqueEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/casqueRead', { templateUrl: '/Partials/CasqueRead.html', controller: 'casqueEditController', resolve: { parameters: function () { return { modeRead: true }; } } })
    .when('/casqueConstitue', { templateUrl: '/Partials/CasqueConstitue.html', controller: 'casqueConstitueController' })

    .when('/carton', { templateUrl: '/Partials/CartonList.html', controller: 'cartonListController' })
    .when('/cartonEdit', { templateUrl: '/Partials/CartonEdit.html', controller: 'cartonEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/cartonRead', { templateUrl: '/Partials/CartonRead.html', controller: 'cartonEditController', resolve: { parameters: function () { return { modeRead: true }; } } })

    .when('/couleur', { templateUrl: '/Partials/CouleurList.html', controller: 'couleurListController' })
    .when('/couleurEdit', { templateUrl: '/Partials/CouleurEdit.html', controller: 'couleurEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/couleurRead', { templateUrl: '/Partials/CouleurRead.html', controller: 'couleurEditController', resolve: { parameters: function () { return { modeRead: true }; } } })

    .when('/taille', { templateUrl: '/Partials/TailleList.html', controller: 'tailleListController' })
    .when('/tailleEdit', { templateUrl: '/Partials/TailleEdit.html', controller: 'tailleEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/tailleRead', { templateUrl: '/Partials/TailleRead.html', controller: 'tailleEditController', resolve: { parameters: function () { return { modeRead: true }; } } })

    //--- menu 7
    .when('/utilisateur', { templateUrl: '/Partials/UtilisateurList.html', controller: 'utilisateurListController' })
    .when('/utilisateurEdit', { templateUrl: '/Partials/UtilisateurEdit.html', controller: 'utilisateurEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/utilisateurRead', { templateUrl: '/Partials/UtilisateurRead.html', controller: 'utilisateurEditController', resolve: { parameters: function () { return { modeRead: true }; } } })
    .when('/utilisateurDroit', { templateUrl: '/Partials/UtilisateurDroit.html', controller: 'utilisateurDroitController' })

    .when('/poste', { templateUrl: '/Partials/PosteList.html', controller: 'posteListController' })
    .when('/posteEdit', { templateUrl: '/Partials/PosteEdit.html', controller: 'posteEditController', resolve: { parameters: function () { return { modeRead: false }; } } })
    .when('/posteRead', { templateUrl: '/Partials/PosteRead.html', controller: 'posteEditController', resolve: { parameters: function () { return { modeRead: true }; } } })
    .when('/configurations', { templateUrl: '/Partials/Configurations.html', controller: 'configurationsController', resolve: { parameters: function () { return { modeRead: true }; } } })

    .when('/mailconfig', { templateUrl: '/Partials/MailConfig.html', controller: 'mailconfigController'})

    .otherwise({ redirectTo: '/' });
}]);

//-- Application Run Verification que le user est authentifié ---------------------------------------------------------
app.run(['$rootScope', '$location', 'webStorage', '$sce', '$timeout', function ($rootScope, $location, webStorage, $sce, $timeout) {
  // Hook sur le changement de page pour authentification
  $rootScope.$on("$locationChangeStart", function (event, next, current) {
    if ($location.path().indexOf("login") >= 0) {
      return;
    }

    var checkLogin = function () {
      var key = new utilisateur(webStorage.get(userKey));
      return (key && key.apiKey && key.apiKey.length == 36);
    };

    if (!checkLogin()) { // pas ou mal logué
      var cpath = $location.path();
      if (cpath != "" && cpath != "/" && cpath.indexOf("Login") < 0) {
        window.location = "/Default.html";
      }
      else {
        $location.path('/login');
      }
    }

    // $rootScope.date = new Date();
    $rootScope.queryLeHub();
  });

  //--- méthodes et variables utiles à toutes les pages
  $rootScope.error = null;
  $rootScope.clearError = function () {
    $rootScope.error = null;
  }
  //-- waiting ring
  $rootScope.processing = false;
  //-- encodange en HTML
  $rootScope.encode = function (x) {
    return $sce.trustAsHtml(x);
  }
  //-- gestion du hub de communication avec le matériel
  $rootScope.queryLeHub = function () {
    $rootScope.roothub = {
      open: false,              // hub ouvert
      message: '',              // dernier message
      connectionId: '?',        // id de connexion
      newTag: null,             // remplit quand un tag est arrivé
      newMessage: null,         // remplit quand un message d'un lecteur arrive
      newReport: null,          // remplit quand un message d'un encodeur arrive
      progression: null,        // informations de progression d'encodage
      progressionFinie: null,   // d'encodage fini
      actifLecteur: 0,          // y a t il un lecteur utilisable  0=non / 1=oui / 2=y en a au moins un d'occupé
      actifEncodeur: 0,         // y a t il un encodeur utilisable 0=non / 1=oui / 2=y en a au moins un d'occupé
    };
    $rootScope.rootCasqueHub = $.connection.casqueHub; // nom de la classe hub c#
    //-- A la réception d'un numéro de tag (nom de la méthode utilisée dans le hub c#)
    $rootScope.rootCasqueHub.client.tag = function (tag) {
      $timeout(function () {
        $rootScope.roothub.newTag = tag.trim();
      });
    };
    //-- A la réception d'un message de retour d'un lecteur
    $rootScope.rootCasqueHub.client.message = function (error, action, msg) {
      $timeout(function () {
        $rootScope.roothub.newMessage = {
        error: error,
        action: action,
        msg: msg
      };
      });
    }
    //-- compte rendu d'un encodage
    $rootScope.rootCasqueHub.client.report = function (error, action, cle, msg) {
      $timeout(function () {
        $rootScope.roothub.newReport = {
          error: error, 
          action: action, 
          cle : cle, 
          msg: msg
        };

        if (error) { // y a un erreur de traitement ==> on annule toute progression en cours
          $rootScope.roothub.progression = {
            message: msg,
            action: action,
            cle: cle,
            index: 0,
            total: 0
          };
        } else { // réception d'une fin d'impression ==> on annule toute progression en cours
          $rootScope.roothub.progression = null;
          $rootScope.roothub.progressionFinie = {
            action: action,
            cle: cle,
          };
        }
      });
    }
    //-- A la réception compte rendu d'un encodage
    $rootScope.rootCasqueHub.client.progress = function (action, cle, index, total) {
      $timeout(function () {
        $rootScope.roothub.progression = {
          action: action,
          cle: cle,
          index: index,
          total: total
        };
      });
    }
    //-- A la réception d'un hello changement des variables d'état des lecteurs
    $rootScope.rootCasqueHub.client.sayHello = function (values) {
      $timeout(function () {
        $rootScope.roothub.actifLecteur = (values & 64) == 64 ? 2 : ((values & 2) == 2 ? 1 : 0);
        $rootScope.roothub.actifEncodeur = (values & 32) == 32 ? 2 : ((values & 1) == 1 ? 1 : 0);
        $rootScope.roothub.date = new Date();
      });
    }

    $.connection.hub.start()
        .done(function () { // hub initialisé
          $rootScope.roothub.connectionId = $.connection.hub.id;
          $rootScope.roothub.open = true;
          $rootScope.rootCasqueHub.server.hello(0) // 0 car on n'est pas un pilote de lecteur juste consommateur
             .done(function (values) { // Recue valeur
               $rootScope.roothub.actifLecteur = (values & 2) == 2;
               $rootScope.roothub.actifEncodeur = (values & 1) == 1;
               $rootScope.roothub.date = new Date();
               $rootScope.$apply();
             });
        });
  }
}]);
//---------------------------------------------------------------------------------------------------------------------
//-- module noHttp = http authentifié ---------------------------------------------------------------------------------
app.factory('$noHttp', ['$http', 'webStorage', '$rootScope', '$translate', function ($http, webStorage, $rootScope, $translate) {
  var key;
  var Resource = function (data) {
    angular.extend(this, data);
  };
  Resource.get = function (url, queryJson, successcb, errorcb) {
    $rootScope.processing = true;
    var httpPromise = $http.get(serviceUrl(url) + prepareQueryString(queryJson, true));
    return promiseThen(httpPromise, successcb, errorcb, resourceRespTransform, 'Common.ErrorGet');
  };
  Resource.post = function (url, param, successcb, errorcb) {
    $rootScope.processing = true;
    var httpPromise = $http.post(serviceUrl(url), param);
    return promiseThen(httpPromise, successcb, errorcb, resourceRespTransform, 'Common.ErrorPost');
  };
  Resource.upload = function (url, param, successcb, errorcb) {
    $rootScope.processing = true;
    var httpPromise = $http.post(serviceUrl(url), param, {
      withCredentials: false,
      headers: { 'Content-type': undefined },
      transformRequest: angular.identity
    });
    return promiseThen(httpPromise, successcb, errorcb, resourceRespTransform, 'Common.ErrorUpload');
  };
  Resource.put = function (url, param, successcb, errorcb) {
    $rootScope.processing = true;
    var httpPromise = $http.put(serviceUrl(url), param);
    return promiseThen(httpPromise, successcb, errorcb, resourceRespTransform, 'Common.ErrorPut');
  };
  Resource.delete = function (url, queryJson, successcb, errorcb) {
    $rootScope.processing = true;
    var httpPromise = $http.delete(serviceUrl(url) + prepareQueryString(queryJson, true));
    return promiseThen(httpPromise, successcb, errorcb, resourceRespTransform, 'Common.ErrorDelete');
  };
  // traite l'appel
  var promiseThen = function (httpPromise, successcb, errorcb, transformFn, errorKey) {
    return httpPromise.then(function (response) {
      $rootScope.processing = false;
      $rootScope.error = null;
      var result = transformFn(response.data);
      (successcb || angular.noop)(result, response.status, response.headers, response.config);
      return result;
    }, function (response) {
      $rootScope.processing = false;
      // pas logué ?
      if (response.status == 403) {
        webStorage.clear(true);
        webStorage.add('loginPb', 1);
        webStorage.add(returnKey, window.location.hash);
        window.location = "/Default.html"; // force le rafraichissement de la page et donc des factories
      }
      // propager l'erreur
      $translate(errorKey, { code: response.status, text: response.statusText }).then(function (result) {
        $rootScope.error = result;
      });
      (errorcb || angular.noop)(undefined, response.status, response.headers, response.config, response.statusText);
      return undefined;
    });
  };
  // transforme le résultat
  var resourceRespTransform = function (data) {
    return new Resource(data);
  };
  // Ajout l'APIkey à l'appel
  var serviceUrl = function (url) {
    if (url.indexOf('/', url.length - 1) !== -1) {
      key = new utilisateur(webStorage.get(userKey));
      var fullUrl = url;
      if (key && key.apiKey && key.apiKey.length == 36) {
        fullUrl += key.apiKey;
      }
      return fullUrl;
    }
    else {
      return url;
    }
  };
  // objet json en querystring
  var prepareQueryString = function (queryJson, firstUse) {
    var s = '';
    for (var k in queryJson) {
      if (queryJson[k] != null) {
        s += (firstUse ? '?' : '&') + k + '=' + encodeURIComponent(queryJson[k]);
        firstUse = false;
      }
    }
    return s;
  };
  return Resource;
}]);

//---------------------------------------------------------------------------------------------------------------------
//-- Hearder 
app.directive('headerMenu', ['$noHttp', '$location', '$route', 'webStorage', function ($noHttp, $location, $route, webStorage) {
  return {
    priority: 0,
    templateUrl: '/Partials/HeaderMenu.html',
    replace: true,
    restrict: 'E',
    scope: {
      titre: '@',
      niveauMenu: '@',
      roothub: '='
    },
    link: function (scope, elem, attrs) {
      //things happen here
      scope.path = $location.path();
      scope.date = new Date();

      // Close the dropdown if the user clicks outside of it
      window.onclick = function (event) {
        if (event.target.id != 'burgerMenu') {
          var dropdown = document.getElementById("menuPop");
          if (dropdown && dropdown.classList && dropdown.classList.contains('show')) {
            dropdown.classList.remove('show');
          }
        }
      }
    },
    controller: function ($scope) {
      var user = new utilisateur(webStorage.get(userKey));
      $scope.user = user;
      //$scope.date = new Date();
      if (!$scope.niveauMenu) {
        $scope.niveauMenu = 1;
      }

      $scope.logOff = function () {
        var key = new utilisateur(webStorage.get(userKey));
        if (key && key.apiKey && key.apiKey.length == 36) {
          webStorage.clear(true);
          $noHttp.delete('/api/login', { apiKey: key.apiKey });
          window.location = "/Default.html"; // force le rafraichissement de la page
        }
      };

      $scope.showMenuPop = function() {
        document.getElementById("menuPop").classList.toggle("show");
      }
    }
  };
}]);
//-- Pied de page 
app.directive('footer', ['$window', function ($window) {
  return {
    priority: 0,
    templateUrl: '/Partials/Footer.html',
    replace: true,
    restrict: 'E',
    scope: {
      alwaysBottom: '@'
    },
    link: function (scope, element, attr, ctrl) {
      scope.footer = $('#footer');
      scope.compute = function () {
        if (scope.alwaysBottom) {
          scope.footer.css({ 'position': 'fixed', 'bottom': 0 });
        } else {
          var scrollposition = $(window).scrollTop();
          var wh = $(window).height();
          var dh = $(document).height();
          var pg = $('bg-page').height();
          var fixed = dh == wh && scrollposition == 0;
          if (fixed) { //-- on set les attributs
            scope.footer.css({ 'position': 'fixed', 'bottom': 0 });
          }
          else { //-- on efface les attributs
            scope.footer.css({ 'position': '', 'bottom': '' });
          }
        }
      };
      // au resize de la page on repositionne le footer
      angular.element($window).bind("resize", function () {
        scope.compute();
      });
      // au scroll de la page on repositionne le footer
      angular.element($window).bind("scroll", function () {
        scope.compute();
      });

      // au premier dessin de la page on repositionne le footer
      scope.compute();
      angular.element(document).ready(function () {
        scope.compute();
      });
    }
  };
}]);
//-- Tips de prix
app.directive('sorter', [function () {
  return {
    restrict: 'E',
    replace: true,
    scope: {
      tri: '=',
      triAsc: '=',
      column: '@',
      changeMethod: '&'
    },
    template: '<i data-ng-click="change()" class="fa sort {{ icon }}" title="{{ \'Common.TipsSort\' | translate }}"></i>',
    link: function (scope, element, attr, ctrl) {

      scope.$watchGroup(['tri', 'triAsc'], function () {
        scope.updateIcon();
      });

      scope.updateIcon = function(){
        scope.icon = 'fa-sort';
        if (scope.tri == scope.column && scope.triAsc) {
          scope.icon = 'fa-sort-asc';
        } else if (scope.tri == scope.column && !scope.triAsc) {
          scope.icon = 'fa-sort-desc';
        }
      }

      scope.change = function () {
        scope.changeMethod();
      }
      scope.updateIcon();
    },
  };
}]);

app.directive('displayError', [function () {
  return {
    restrict: 'E',
    replace: true,
    template: '<div class="error-message-bloc" data-ng-show="error">'
               +' {{ error }} '
               + ' <span class="btn pull-right" title="{{ \'Common.TipsHide\' | translate }}" data-ng-click="clearError()"><i class="fa fa-times"></i></span>'
               + '</div>'
  }
}]);
