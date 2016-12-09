var app = angular.module('casquePrintApp', ['ngRoute', 'ngSanitize', 'pascalprecht.translate', 'webStorageModule']);
var userKey = 'CasqueUserKey';
var returnKey = 'returnUrl';

//-- routes -----------------------------------------------------------------------------------------------------------
app.config(['$routeProvider', function ($routeProvider) {
  $routeProvider
    .when('/livraison', { templateUrl: '/PartialsPrint/Livraison.html', controller: 'livraisonController' })
    .when('/commande', { templateUrl: '/PartialsPrint/Commande.html', controller: 'commandeController' })
  //.otherwise({ redirectTo: '/' });
}]);

//-- Application Run Verification que le user est authentifié ---------------------------------------------------------
app.run(['$rootScope', '$location', 'webStorage', '$sce', function ($rootScope, $location, webStorage, $sce) {
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
      window.location = "/Default.html";
    }
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
      $translate(errorKey, { code: response.status, text: response.statusText }).then(function (result) { $rootScope.error = result; });
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
//-- Controlleur Commande ---------------------------------------------------------------------------------------------
app.controller('commandeController', ['$scope', '$noHttp', '$route', '$location', '$sce',
  function ($scope, $noHttp, $route, $location, $sce) {
    var redirection = function () {
      window.location = "/Default.html";
    }
    //-- recupère les infos de la commande
    $scope.getInfos = function () {
      $noHttp.get('/api/commandeGet/', { cle: $scope.cle }, function (data) {
        $scope.commande = data.commande;
        if (data && data.commande && data.commande.cle && data.commande.impressionFinie) {
          setTimeout(function () { window.print(); }, 500);
        } else {
          redirection();
        }
      }, function (data, statusCode, headers, config, statusText) {
        redirection();
      });
    };
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if (!$scope.cle || $scope.cle < 0) {
      redirection();
    }
    $scope.getInfos();
  }]);
//---------------------------------------------------------------------------------------------------------------------
//-- Controlleur Livraison --------------------------------------------------------------------------------------------
app.controller('livraisonController', ['$scope', '$noHttp', '$route', '$location', '$sce',
  function ($scope, $noHttp, $route, $location, $sce) {
    var redirection = function () {
      window.location = "/Default.html";
    }
    //-- recupére les infos de la livraison
    $scope.getInfos = function () {
      $noHttp.get('/api/livraisonDetail/', { cle: $scope.cle }, function (data) {
        $scope.livraison = data.livraison;
        $scope.clients = data.clients
        $scope.cartons = data.cartons;
        if (data  && data.livraison  && data.livraison.cle && data.livraison.clientNom) {
          setTimeout(function () { window.print(); }, 500);
        } else {
          redirection();
        }
      }, function (data, statusCode, headers, config, statusText) {
        redirection();
      });
    };
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if (!$scope.cle || $scope.cle < 0) {
      redirection();
    }
    $scope.getInfos();
  }]);
