var app = angular.module('casqueExcelApp', ['ngRoute', 'ngSanitize', 'pascalprecht.translate', 'webStorageModule']);
var userKey = 'CasqueUserKey';
var returnKey = 'returnUrl';

//-- routes -----------------------------------------------------------------------------------------------------------
app.config(['$routeProvider', function ($routeProvider) {
  $routeProvider
    .when('/livraison', { templateUrl: '/PartialsExcel/Livraison.html', controller: 'livraisonController' })
    .when('/commande', { controller: 'commandeController' })
  //.otherwise({ redirectTo: '/' });
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
      $noHttp.get('/api/commandeGet/', { cle: $scope.cle, Excel:1 }, function (data) {
        $scope.commande = data.commande;
        if (data && data.commande && data.commande.cle && data.commande.impressionFinie) {
          //setTimeout(function () { window.print(); }, 500);
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
