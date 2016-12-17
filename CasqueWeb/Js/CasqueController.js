//-------------------------------------------------------------------------------------------------------
//-- Controlleur de la page d'accueil -------------------------------------------------------------------
app.controller('homeController', ['$scope', '$noHttp', 'webStorage', '$translate',
  function ($scope, $noHttp, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $noHttp.get('/api/homeInfos/', {
        pageCode: 'home'
      }, function (data) {
        $scope.statistiques = data.statistiques;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.statistiques = null;
      });
    };
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('homeController');
      if (key) {
        if (key.hideMessage) { $scope.hideMessage = key.hideMessage; }
      }
    }
    //---- debut de la page
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.bienvenu = null;
    $scope.LoadFromSession();
    if (!$scope.hideMessage) {
      $translate('Home.Bienvenu', { user: $scope.user.nom }).then(function (result) {
        $scope.bienvenu = result;
        $scope.$watch('bienvenu', function () {
          if (!$scope.bienvenu) {
            webStorage.add('homeController', { hideMessage: true });
          }
        });
      });
    }
    $scope.getInfos();
  }]);

//-- Controlleur de la page login -----------------------------------------------------------------------
app.controller('loginController', ['$scope', '$rootScope', '$noHttp', '$translate', '$location', 'webStorage', '$timeout',
  function ($scope, $rootScope, $noHttp, $translate, $location, webStorage, $timeout) {
    $scope.login = '';
    $scope.pass = '';
    $scope.count = 0;
    $scope.logFocus = false;
    //-- Verifie login / pass
    $scope.logMe = function () {
      var param = {
        login: $scope.login,
        password: $scope.pass
      }
      webStorage.remove(userKey); // on s'assure que l'appel ne se fera pas avec une apiKey
      $noHttp.post('/api/login', param, function (data) {
        if (!data || !data.apiKey) {
          $scope.pass = '';
          $scope.logFocus = true;
          $scope.saisie.$setPristine();
          $translate('Login.ErrorLogin1').then(function (result) { $rootScope.error = result; });
          return;
        }
        $scope.login = '';
        $scope.pass = '';
        var user = { apiKey: data.apiKey, cle: data.cle, nom: data.nom, menus: data.menus };
        webStorage.add(userKey, user);
        var retUrl = webStorage.get(returnKey);
        if (retUrl) { // ancienne url vers laquelle retourner
          webStorage.remove(returnKey);
          var url = retUrl.split('?')[0].replace('#', '');
          var params = retUrl.split('?')[1];
          if (params) {
            $location.path(url).search(params);
          }
          else {
            $location.path(url);
          }
        } else {
          $location.path('/');
        }
      }, function (data) {
        $scope.pass = '';
        $scope.logFocus = true;
        $scope.saisie.$setPristine();
        $translate('Login.ErrorLogin2').then(function (result) { $rootScope.error = result; });
      });
    }
    var pb = webStorage.get('loginPb');
    if (pb === 1) {
      $translate('Login.ErrorLogin2').then(function (result) { $rootScope.error = result; });
    }

    $timeout(function () {
      $scope.logFocus = false;
    }, 50);
  }]);

//-------------------------------------------------------------------------------------------------------
//-- Controlle de la page intermediaire de configuration ------------------------------------------------
app.controller('configurationController', ['$scope', '$noHttp', 'webStorage',
  function ($scope, $noHttp, webStorage) {
    //-- Load les données
    $scope.getInfos = function () {
      $noHttp.get('/api/homeInfos/', {
        pageCode: 'configuration'
      }, function (data) {
        $scope.compteurMenu = data.compteurMenu;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.compteurMenu = null;
      });
    };
    //-- compte le nombre d'éléments
    $scope.getCount = function (pageCode) {
      if ($scope.compteurMenu && $scope.compteurMenu.length) {
        for (var i = 0; i < $scope.compteurMenu.length; i++) {
          if ($scope.compteurMenu[i].nom == pageCode) {
            if ($scope.compteurMenu[i].cle) {
              return 'x ' + $scope.compteurMenu[i].cle;
            } else {
              return null;
            }
          }
        }
      }
      return null;
    }
    //---- debut de la page
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.getInfos();
  }]);

//-- Controlleur Carton List ----------------------------------------------------------------------------
app.controller('cartonListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/cartonList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.cartons = data.cartons;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.cartons = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime un carton
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/cartonDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('cartonListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('cartonListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('CartonList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);

//-- Controlleur Carton Edit ----------------------------------------------------------------------------
app.controller('cartonEditController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    $scope.getInfos = function () {
      $noHttp.get('/api/cartonGet/', { cle: $scope.cle }, function (data) {
        $scope.carton = data.carton;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.carton = null;
      });
    };
    //-- Enregistre les modifications sur le carton
    $scope.saveModif = function () {
      $noHttp.post('/api/cartonEdit/',
        {
          cle: $scope.cle,
          carton: {
            nom: $scope.carton.nom,
            code: $scope.carton.code,
            description: $scope.carton.description,
          }
        }, function (data) { // c'est ok redirection
          $location.path('/carton');
        }, function (data, statusCode, headers, config, statusText) {
          // En cas d'erreur
        });
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // new
      $scope.carton = {
        nom: '',
        code: '',
        description: '',
      }
    }

  }]);

//-- Controlleur Couleur List ---------------------------------------------------------------------------
app.controller('couleurListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/couleurList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.couleurs = data.couleurs;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.couleurs = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime une couleur
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/couleurDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('couleurListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('couleurListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('CouleurList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);

//-- Controlleur Couleur Edit ---------------------------------------------------------------------------
app.controller('couleurEditController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    $scope.getInfos = function () {
      $noHttp.get('/api/couleurGet/', { cle: $scope.cle }, function (data) {
        $scope.couleur = data.couleur;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.couleur = null;
      });
    };
    //-- Enregistre les modifications sur la couleur
    $scope.saveModif = function () {
      $noHttp.post('/api/couleurEdit/',
        {
          cle: $scope.cle,
          couleur: {
            nom: $scope.couleur.nom,
            code: $scope.couleur.code,
            description: $scope.couleur.description,
          }
        }, function (data) { // c'est ok redirection
          $location.path('/couleur');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // new
      $scope.couleur = {
        nom: '',
        code: '',
        description: '',
      }
    }
  }]);

//-- Controlleur Taille List ----------------------------------------------------------------------------
app.controller('tailleListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/tailleList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.tailles = data.tailles;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.tailles = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime une couleur
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/tailleDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('tailleListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('tailleListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('TailleList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);

//-- Controlleur Taille Edit ----------------------------------------------------------------------------
app.controller('tailleEditController', ['$scope', '$noHttp', '$route', '$location', 'webStorage',
  function ($scope, $noHttp, $route, $location, webStorage) {
    $scope.getInfos = function () {
      $noHttp.get('/api/tailleGet/', { cle: $scope.cle }, function (data) {
        $scope.taille = data.taille;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.taille = null;
      });
    };
    //-- Enregistre les modifications sur la taille
    $scope.saveModif = function () {
      $noHttp.post('/api/tailleEdit/',
        {
          cle: $scope.cle,
          taille: {
            nom: $scope.taille.nom,
            code: $scope.taille.code,
            ordre: $scope.taille.ordre,
            description: $scope.taille.description,
          }
        }, function (data) { // c'est ok redirection
          $location.path('/taille');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('ClientList') == 3;
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // new
      $scope.taille = {
        nom: '',
        code: '',
        description: '',
      }
    }
  }]);

//-- Controlleur Client List ----------------------------------------------------------------------------
app.controller('clientListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$sce', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $sce, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/clientList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.clients = data.clients;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.clients = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime un carton
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/clientDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('clientListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('clientListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('ClientList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);

//-- Controlleur Client Edit ----------------------------------------------------------------------------
app.controller('clientEditController', ['$scope', '$noHttp', '$route', '$location', '$sce',
  function ($scope, $noHttp, $route, $location, $sce) {
    $scope.getInfos = function () {
      $noHttp.get('/api/clientGet/', { cle: $scope.cle }, function (data) {
        $scope.client = data.client;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.client = null;
      });
    };
    //-- Enregistre les modifications sur le client
    $scope.saveModif = function () {
      $noHttp.post('/api/clientEdit/',
        {
          cle: $scope.cle,
          client: $scope.client,
        }, function (data) { // c'est ok redirection
          $location.path('/client');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- debut de la page
    $scope.subjectClientFocus = false;
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // new
      $scope.client = {
        nom: '',
        adresseCommande: '',
        adresseLivraison: '',
        email: '',
      }
    }
  }]);

//-- Controlleur Fournisseur List -----------------------------------------------------------------------
app.controller('fournisseurListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$sce', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $sce, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/fournisseurList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.fournisseurs = data.fournisseurs;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.fournisseurs = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime un carton
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/fournisseurDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('fournisseurListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('fournisseurListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('FournisseurList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);

//-- Controlleur Fournisseur Edit -----------------------------------------------------------------------
app.controller('fournisseurEditController', ['$scope', '$noHttp', '$route', '$location', '$sce', 'parameters',
  function ($scope, $noHttp, $route, $location, $sce, parameters) {
    $scope.modeRead = parameters.modeRead;
    $scope.getInfos = function () {
      $noHttp.get('/api/fournisseurGet/', { cle: $scope.cle, modeRead: $scope.modeRead }, function (data) {
        $scope.fournisseur = data.fournisseur;
        $scope.pieces = data.pieces; // utilisé pour le mode read only
      }, function (data, statusCode, headers, config, statusText) {
        $scope.fournisseur = null;
        $scope.pieces = null;
      });
    };
    //-- Enregistre les modifications sur le fournisseur
    $scope.saveModif = function () {
      $noHttp.post('/api/fournisseurEdit/',
        {
          cle: $scope.cle,
          fournisseur: $scope.fournisseur,
        }, function (data) { // c'est ok redirection
          if (!$scope.cle && data && data.fournisseur && data.fournisseur.cle) {
            $location.path('/fournisseurPiece').search({ cle: data.fournisseur.cle })
          }
          else { // redirect vers la liste
            $location.path('/fournisseur');
          }
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // new
      $scope.fournisseur = {
        nom: '',
        adresseCommande: '',
        adresseLivraison: '',
        email: '',
      }
    }
  }]);

//-- Controlleur Fournisseur Piece ----------------------------------------------------------------------
app.controller('fournisseurPieceController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    $scope.getInfos = function () {
      $noHttp.get('/api/fournisseurPieceGet/', { cle: $scope.cle }, function (data) {
        $scope.fournisseur = data.fournisseur;
        $scope.pieces = data.pieces;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.fournisseur = null;
        $scope.pieces = null;
      });
    };
    //-- Enregistre les modifications sur la liste des pièces
    $scope.saveModif = function () {
      $noHttp.post('/api/fournisseurPieceEdit/',
        {
          cle: $scope.cle,
          pieces: $scope.pieces,
        }, function (data) { // c'est ok redirection
          $location.path('/fournisseur');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- choisi une pièce
    $scope.togglePiece = function (piece) {
      if (piece.fournisseurCle) {
        piece.fournisseurCle = null;
      } else {
        piece.fournisseurCle = $scope.fournisseur.cle;
      }
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // pas de clé pas d'écran
      $location.path('/fournisseur');
    }
  }]);

//-- Controlleur Type de pièce List ---------------------------------------------------------------------
app.controller('typePieceListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/typePieceList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.typePieces = data.typePieces;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.typePieces = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime un carton
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/typePieceDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('typePieceListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('typePieceListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('TypePieceList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);

//-- Controlleur Type de pièce Edit ---------------------------------------------------------------------
app.controller('typePieceEditController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    $scope.getInfos = function () {
      $noHttp.get('/api/typePieceGet/', { cle: $scope.cle }, function (data) {
        $scope.typePiece = data.typePiece;
        $scope.tailles = data.tailles;
        $scope.couleurs = data.couleurs;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.typePiece = null;
        $scope.tailles = null;
        $scope.couleurs = null;
      });
    };
    //-- Enregistre les modifications sur le type de pièce
    $scope.saveModif = function () {
      if ($scope.leFichier) { // y a une photo on l'upload
        var fd = new FormData();
        fd.append("file", $scope.leFichier)
        $noHttp.upload('/api/typePiecePhoto/', fd, function (data) {
          $scope.photo = data.photoNom;
          $scope.postModif();
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur on continue quand même
          $scope.postModif();
        });
      } else {
        $scope.photo = $scope.typePiece.photo;
        $scope.postModif();
      }
    }
    //-- Poste les modifications sur l'exposition
    $scope.postModif = function () {
      $noHttp.post('/api/typePieceEdit/',
        {
          cle: $scope.cle,
          typePiece: {
            nom: $scope.typePiece.nom,
            code: $scope.typePiece.code,
            avecTag: $scope.typePiece.avecTag,
            description: $scope.typePiece.description,
            photo: $scope.photo,
            cleTailles: $scope.typePiece.cleTailles,
            cleCouleurs: $scope.typePiece.cleCouleurs,
          }
        }, function (data) { // c'est ok redirection
          $location.path('/typePiece');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- choix d'ajouter ou retirer une taille
    $scope.toggleTaille = function (cle) {
      var idx = $scope.typePiece.cleTailles.indexOf(cle);
      if (idx > -1) { // Déjà sel on remove
        $scope.typePiece.cleTailles.splice(idx, 1);
      } else { // nouvel sélection
        $scope.typePiece.cleTailles.push(cle);
      }
    }
    //-- choix d'ajouter ou retirer une couleur
    $scope.toggleCouleur = function (cle) {
      var idx = $scope.typePiece.cleCouleurs.indexOf(cle);
      if (idx > -1) { // Déjà sel on remove
        $scope.typePiece.cleCouleurs.splice(idx, 1);
      } else { // nouvel sélection
        $scope.typePiece.cleCouleurs.push(cle);
      }
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    $scope.getInfos(); // on appelle tous le temp pour avoir les données des listes de valeurs possibles
  }]);

//-- Controlleur Casque List ----------------------------------------------------------------------------
app.controller('casqueListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/casqueList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.casques = data.casques;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.casques = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime un carton
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/casqueDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('casqueListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('casqueListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('CasqueList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);

//-- Controlleur Casque Edit ----------------------------------------------------------------------------
app.controller('casqueEditController', ['$scope', '$noHttp', '$route', '$location', 'parameters',
  function ($scope, $noHttp, $route, $location, parameters) {
    $scope.modeRead = parameters.modeRead;
    $scope.getInfos = function () {
      $noHttp.get('/api/casqueGet/', { cle: $scope.cle, modeRead: $scope.modeRead, }, function (data) {
        $scope.casque = data.casque;
        $scope.pieces = data.pieces;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.casque = null;
        $scope.pieces = null;
      });
    };
    //-- Enregistre les modifications sur le casque
    $scope.saveModif = function () {
      if ($scope.leFichier) { // y a une photo on l'upload
        var fd = new FormData();
        fd.append("file", $scope.leFichier)
        $noHttp.upload('/api/casquePhoto/', fd, function (data) {
          $scope.photo = data.photoNom;
          $scope.postModif();
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur on continue quand même
          $scope.postModif();
        });
      } else {
        $scope.photo = $scope.casque.photo;
        $scope.postModif();
      }
    }
    //-- Poste les modifications sur l'exposition
    $scope.postModif = function () {
      $noHttp.post('/api/casqueEdit/',
        {
          cle: $scope.cle,
          casque: {
            nom: $scope.casque.nom,
            code: $scope.casque.code,
            description: $scope.casque.description,
            photo: $scope.photo,
          }
        }, function (data) { // c'est ok redirection
          if (! $scope.cle  && data && data.casque && data.casque.cle) {
            $location.path('/casqueConstitue').search({ cle: data.casque.cle })
          }
          else { // redirect vers la liste
            $location.path('/casque');
          }
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // new
      $scope.casque = {
        nom: '',
        code: '',
        description: '',
        photo: '',
      }
    }
  }]);

//-- Controlleur Casque Constitue -----------------------------------------------------------------------
app.controller('casqueConstitueController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    $scope.getInfos = function () {
      $noHttp.get('/api/casqueConstitueGet/', { cle: $scope.cle }, function (data) {
        $scope.casque = data.casque;
        $scope.nom = data.casque.nom;
        $scope.pieces = data.pieces;
        $scope.canEdit = data.nombreAssemblage == 0;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.casque = null;
        $scope.nom = null;
        $scope.pieces = null;
        $scope.canEdit = false;
      });
    };
    //-- Enregistre les modifications sur le casque
    $scope.saveModif = function () {
      $noHttp.post('/api/casqueConstitueEdit/',
        {
          cle: $scope.cle,
          nom: $scope.nom,
          pieces: $scope.pieces,
        }, function (data) { // c'est ok redirection
          $location.path('/casque');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- Récupère les infos sur le type de pièce choisi
    $scope.getTypePieceInfos = function (piece) {
      if (piece && piece.cle && (piece.nombreTaille || piece.nombreCouleur)) {
        // la pièce existe, elle a des tailles ou des couleurs ==> chercher la liste
        $noHttp.get('/api/casqueConstitueInfoGet/', { cle: piece.cle }, function (data) {
          piece.tailles = data.tailles;
          piece.couleurs = data.couleurs;
        }, function (data, statusCode, headers, config, statusText) {
          piece.tailles = null;
          piece.couleurs = null;
        });
      }
    };
    //-- Ajout d'une pièce au casque
    $scope.addPiece = function (piece) {
      if (!piece.casqueCle) {
        piece.casqueCle = $scope.casque.cle;
        if (piece.cle && (piece.nombreTaille || piece.nombreCouleur) && (!piece.tailles || !piece.Couleurs)) {
          // faut remplir les listes
          $scope.getTypePieceInfos(piece);
        }
      }
    }
    //-- retire une pièce du casque
    $scope.removePiece = function (piece) {
      if (piece.casqueCle) {
        piece.casqueCle = null;
      }
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // new
      $scope.casque = {
        nom: '',
        code: '',
        description: '',
        photo: '',
      }
    }
  }]);

//-------------------------------------------------------------------------------------------------------
//-- Controlleur de la page intermédiaire d'administration ----------------------------------------------
app.controller('administrationController', ['$scope', '$noHttp', 'webStorage',
  function ($scope, $noHttp, webStorage) {
    //-- Load les données
    $scope.getInfos = function () {
      $noHttp.get('/api/homeInfos/', {
        pageCode: 'administration'
      }, function (data) {
        $scope.compteurMenu = data.compteurMenu;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.compteurMenu = null;
      });
    };
    //-- compte le nombre d'éléments
    $scope.getCount = function (pageCode) {
      if ($scope.compteurMenu && $scope.compteurMenu.length) {
        for (var i = 0; i < $scope.compteurMenu.length; i++) {
          if ($scope.compteurMenu[i].nom == pageCode) {
            if ($scope.compteurMenu[i].cle) {
              return 'x ' + $scope.compteurMenu[i].cle;
            } else {
              return null;
            }
          }
        }
      }
      return null;
    }
    //---- debut de la page
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.getInfos();
  }]);

//-- Controlleur Configurations
app.controller('configurationsController', ['$scope', '$noHttp', '$location',
   function ($scope, $noHttp, $location) {
     $scope.getInfos = function () {
       $noHttp.get('/api/configurationsGet/', { }, function (data) {
         $scope.configurations = data.configurations;
       }, function (data, statusCode, headers, config, statusText) {
         $scope.configurations = null;
       });
     };
     //-- Enregistre les modifications sur l'utilisateur
     $scope.saveModif = function () {
       $noHttp.post('/api/configurationsGet/',
       {
         configurations: $scope.configurations
       }, function (data) { // c'est ok redirection
         $location.path('/administration');
       }, function (data, statusCode, headers, config, statusText) {
         // en cas d'erreur
       });
     }
     //-- debut de la page
     $scope.getInfos();
   }]);
//-- Controlleur Utilisateur List -----------------------------------------------------------------------
app.controller('utilisateurListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/utilisateurList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.utilisateurs = data.utilisateurs;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.utilisateurs = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime un utilisateur
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/utilisateurDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('utilisateurListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('utilisateurListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('UtilisateurList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);
//-- Controlleur Utilisateur Edit -----------------------------------------------------------------------
app.controller('utilisateurEditController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    $scope.cpassword = '';
    $scope.getInfos = function () {
      $noHttp.get('/api/utilisateurGet/', { cle: $scope.cle }, function (data) {
        $scope.utilisateur = data.utilisateur;
        $scope.cpassword = data.utilisateur.password;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.utilisateur = null;
        $scope.cpassword = '';
      });
    };
    //-- Enregistre les modifications sur l'utilisateur
    $scope.saveModif = function () {
      $noHttp.post('/api/utilisateurEdit/',
        {
          cle: $scope.cle,
          utilisateur: {
            nom: $scope.utilisateur.nom,
            login: $scope.utilisateur.login,
            password: $scope.utilisateur.password,
            actif: $scope.utilisateur.actif,
            email: $scope.utilisateur.email,
          }
        }, function (data) { // c'est ok redirection
          $location.path('/utilisateur');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // new
      $scope.utilisateur = {
        nom: '',
        login: '',
        password: '',
        actif: true,
        email: ''
      }
    }
  }]);
//-- Controlleur Utilisateur Droit ----------------------------------------------------------------------
app.controller('utilisateurDroitController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    $scope.getInfos = function () {
      $noHttp.get('/api/utilisateurDroitGet/', { cle: $scope.cle }, function (data) {
        $scope.utilisateur = data.utilisateur;
        $scope.droits = data.droits;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.utilisateur = null;
        $scope.droits = null;
      });
    };
    //-- Enregistre les modifications sur les droits de l'utilisateur
    $scope.saveModif = function () {
      $noHttp.post('/api/utilisateurDroitEdit/',
        {
          cle: $scope.cle,
          droits: $scope.droits,
        }, function (data) { // c'est ok redirection
          $location.path('/utilisateur'); //Edit?cle=' + $scope.cle);
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- change le niveau d'un menu
    $scope.changeAcces = function (d, acces, parent) {
      d.accesInt = acces;
      d.acces = null;
      if (parent != null && $scope.droits && $scope.droits.length) {
        for (var i = 0; i < $scope.droits.length; i++) {
          if ($scope.droits && $scope.droits[i].cle == parent) {
            if (acces > $scope.droits[i].accesInt) {
              $scope.droits[i].accesInt = acces;
              $scope.droits[i].acces = null;
            }
          }
        }
      }
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if ($scope.cle > 0) { // edit
      $scope.getInfos();
    }
    else { // pas de clé pas d'écran
      $location.path('/utilisateur');
    }
  }]);

//-- Controlleur Poste List -----------------------------------------------------------------------------
app.controller('posteListController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/posteList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText
      }, function (data) {
        $scope.postes = data.postes;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.postes = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime un utilisateur
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Common.ConfirmDelete', { nom: x.nom }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/posteDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('posteListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('posteListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'nom';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('PosteList') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);
//-- Controlleur Poste Edit -----------------------------------------------------------------------------
app.controller('posteEditController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    //-- recupére les infos du poste
    $scope.getInfos = function () {
      $noHttp.get('/api/posteGet/', { cle: $scope.cle }, function (data) {
        $scope.poste = data.poste;
        $scope.config = data.config;
        $scope.posteTypes = data.posteTypes;
        $scope.affectations = data.affectations;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.poste = null;
        $scope.config = null;
        $scope.posteTypes = null;
        $scope.affectations = null;
      });
    };
    //-- Enregistre les modifications sur le poste
    $scope.saveModif = function () {
      for (var i = 0; i < $scope.config.antennes.length; i++) {
        if ($scope.config.antennes[i].active) {
          $scope.config.antennes[i].antenneIndex = i;
          $scope.config.antennes[i].position = 1 + i;
        }
        else {
          $scope.config.antennes[i] = null;
        }
      }
      $noHttp.post('/api/posteEdit/',
        {
          cle: $scope.cle,
          poste: {
            posteTypeInt: $scope.poste.posteTypeNomCle.cle,
            nom: $scope.poste.nom,
            description: $scope.poste.description,
            pageCode: $scope.poste.affectation.code,
            configurationTxt: $scope.poste.configurationTxt,
          },
          config: $scope.config,
        }, function (data) { // c'est ok redirection
          $location.path('/poste');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- indique si la config est Ok
    $scope.configOneAntenneActive = function () {
      if ($scope.config && $scope.config.antennes && $scope.config.antennes.length) {
        for (var i = 0; i < $scope.config.antennes.length; i++) {
          if ($scope.config.antennes[i] && $scope.config.antennes[i].active) {
            // une antenne active c'est Ok
            return true;
          }
        }
      }
      return false;
    }
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    $scope.getInfos(); // on fait tous le temps pour avoir les données satellites
  }]);

//-- Controlleur livraison Resume -----------------------------------------------------------------------
app.controller('livraisonResumeController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage', '$sce',
  function ($scope, $noHttp, NOCONFIG, webStorage, $sce) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      $noHttp.get('/api/livraisonResume/', {
        mode: $scope.mode,
      }, function (data) {
        $scope.livraisons = data.livraisons;
        $scope.total = data.total;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.livraisons = null;
        $scope.total = 0;
      });
    };
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('livraisonResumeController',
        {
          mode: $scope.mode,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('livraisonResumeController');
      if (key) {
        if (key.mode) { $scope.mode = key.mode; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.mode = "0";
    }
    //--- Debut de la page
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = $scope.user.isInRole('Livraison') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    $scope.getInfos();
  }]);
//-- Controlleur livraison List -------------------------------------------------------------------------
app.controller('livraisonListController', ['$scope', '$noHttp', 'NOCONFIG', '$route', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, $route, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/livraisonList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText,
        statut: $scope.statut
      }, function (data) {
        $scope.livraisons = data.livraisons;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.livraisons = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }

    //-- supprime une livraison
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Livraison.ConfirmCancelLivraison', { nom: x.reference }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/livraisonDelete/', { cle: x.cle, PageCode: 'livraison' }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('livraisonListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
          statut: $scope.statut,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('livraisonListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
        if (key.statut) { $scope.statut = key.statut; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'cle';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('Livraison') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    if ($route.current.params.statut) {
      $scope.statut = $route.current.params.statut;
    }
    $scope.getInfos();
  }]);
//-- Controlleur Livraison Detail ----------------------------------------------------------------------
app.controller('livraisonDetailController', ['$scope', '$noHttp', '$route', '$location', '$sce', '$window', '$translate', 'webStorage',
  function ($scope, $noHttp, $route, $location, $sce, $window, $translate, webStorage) {
    //-- recupére les infos du poste
    $scope.getInfos = function () {
      $noHttp.get('/api/livraisonDetail/', { cle: $scope.cle }, function (data) {
        $scope.livraison = data.livraison;
        $scope.clients = data.clients
        $scope.cartons = data.cartons;
        if (!data || !data.livraison || !data.livraison.cle) {
          $location.path('/livraisonList').search();
        }
      }, function (data, statusCode, headers, config, statusText) {
        $scope.livraison = null;
        $scope.clients = null;
        $scope.cartons = null;
      });
    };
    //-- finalise une livraison
    $scope.saveModif = function () {
      $noHttp.post('/api/finaliseLivraison/', {
        livraisonCle: $scope.livraison.cle,
        clientCle: $scope.client.cle,
      }, function (data) {
        $location.path('/livraisonList');
      }, function (data, statusCode, headers, config, statusText) {
        $translate('HubLecteur.ErrorSaveLivraison', { code: statusCode, text: statusText }).then(function (result) {
          $rootScope.error = result;
        });
      });
    }
    //--- imprimer le bon d'expédition
    $scope.printBl = function () {
      var w = window.open('/Print.html#/livraison?cle=' + $scope.cle);
      angular.element(w).bind('load', function () {
        setTimeout(function () { w.close(); }, 1000);
      });
    }
    //-- demande la génération du email
    $scope.sendEmail = function () {
      $noHttp.post('/api/livraisonEmail/',
        {
          cle: $scope.cle,
          emailSuplementaire: $scope.emailSuplementaire,
        }, function (data) {
        $translate('Livraison.ConfirmMailSend').then(function (result) {
          $scope.showEmailplus = false;
          alert(result);
        });
        }, function (data, statusCode, headers, config, statusText) {
          $scope.showEmailplus = false;
      });
    }
    //-- debut de la page
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.readOnly = $scope.user.isInRole('Livraison') < 3;
    $scope.cle = $route.current.params.cle;
    if (!$scope.cle || $scope.cle < 0) {
      $location.path('/livraisonList').search();
    }
    $scope.showEmailplus = false;
    $scope.getInfos();
  }]);

//-- Controlleur Assemblage Resume ----------------------------------------------------------------------
app.controller('assemblageResumeController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage',
  function ($scope, $noHttp, NOCONFIG, webStorage) {
    //-- Load les données
    $scope.getInfos = function () {
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/assemblageResume/', {
      }, function (data) {
        $scope.assemblages = data.assemblages;
        $scope.total = data.total;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.assemblages = null;
        $scope.total = 0;
      });
    };
    //--- Debut de la page
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = $scope.user.isInRole('Assemblage') == 3;
    $scope.getInfos();
  }]);
//-- Controlleur Assemblage List ------------------------------------------------------------------------
app.controller('assemblageListController', ['$scope', '$noHttp', 'NOCONFIG', '$route', 'webStorage',
  function ($scope, $noHttp, NOCONFIG, $route, webStorage) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/assemblageList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText,
        statut: $scope.statut,
        casqueCle: $scope.casque ? $scope.casque.cle : $scope.casqueCleUlr ? $scope.casqueCleUlr : 0,
      }, function (data) {
        $scope.assemblages = data.assemblages;
        $scope.casques = data.casques;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
        if ($scope.casqueCleUlr && !$scope.casque) { // recherche du casque à préaffecter
          for (var i = 0; i < $scope.casques.length; i++) {
            if ($scope.casques[i] && $scope.casques[i].cle == $scope.casqueCleUlr) {
              $scope.casque = $scope.casques[i];
              $scope.casqueCleUlr = null;
              break;
            }
          }
        }
      }, function (data, statusCode, headers, config, statusText) {
        $scope.assemblages = null;
        $scope.casques = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('assemblageListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
          statut: $scope.statut,
          casque: $scope.casque,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('assemblageListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
        if (key.statut) { $scope.statut = key.statut; }
        if (key.casque) { $scope.casque = key.casque; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'cle';
      $scope.triAsc = true;
      $scope.searchText = '';
      $scope.casque = null;
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('Assemblage') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    if ($route.current.params.statut) {
      $scope.statut = $route.current.params.statut;
    }
    if ($route.current.params.casque) {
      $scope.casqueCleUlr = $route.current.params.casque;
    }
    $scope.getInfos();
  }]);
//-- Controlleur assemblage Detail ----------------------------------------------------------------------
app.controller('assemblageDetailController', ['$scope', '$noHttp', '$route', '$location',
  function ($scope, $noHttp, $route, $location) {
    //-- recupére les infos du poste
    $scope.getInfos = function () {
      $noHttp.get('/api/assemblageDetail/', { cle: $scope.cle }, function (data) {
        $scope.assemblage = data.assemblage;
        $scope.etiquettes = data.etiquettes;
        if (!data || !data.assemblage || !data.assemblage.cle) {
          $location.path('/assemblage').search();
        }
      }, function (data, statusCode, headers, config, statusText) {
        $scope.assemblage = null;
        $scope.etiquettes = null;
      });
    };
    //-- debut de la page
    $scope.cle = $route.current.params.cle;
    if (!$scope.cle || $scope.cle < 0) {
      $location.path('/assemblageList').search();
    }
    $scope.getInfos();
  }]);

//-- Controlleur Commande Resume ------------------------------------------------------------------------
app.controller('commandeResumeController', ['$scope', '$noHttp', 'NOCONFIG', 'webStorage',
  function ($scope, $noHttp, NOCONFIG, webStorage) {
    //-- Load les données
    $scope.getInfos = function () {
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/commandeResume/', {
      }, function (data) {
        $scope.commandes = data.commandes;
        $scope.total = data.total;
        $scope.totalPiece = data.totalPiece;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.commandes = null;
        $scope.total = 0;
      });
    };
    //--- Debut de la page
    $scope.user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = $scope.user.isInRole('Commande') == 3;
    $scope.getInfos();

    $scope.$watch('roothub.progressionFinie', function (value) {
      if (value) {
        $scope.getInfos();
        value == null;
      }
    });
  }]);
//-- Controlleur Commande List --------------------------------------------------------------------------
app.controller('commandeListController', ['$scope', '$noHttp', 'NOCONFIG', '$route', 'webStorage', '$translate',
  function ($scope, $noHttp, NOCONFIG, $route, webStorage, $translate) {
    //-- Load les données
    $scope.getInfos = function () {
      $scope.SaveToSession();
      var letri = $scope.tri + ($scope.triAsc ? '' : ':DESC');
      $noHttp.get('/api/comamndeList/', {
        page: $scope.page - 1,
        pageSize: $scope.pageSize,
        tri: letri,
        searchText: $scope.searchText,
        statut: $scope.statut
      }, function (data) {
        $scope.commandes = data.commandes;
        $scope.nombre = data.nombre;
        $scope.page = data.page + 1;
        $scope.tri = data.tri;
        $scope.triAsc = data.triAsc;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.commandes = null;
        $scope.nombre = 0;
        $scope.page = 0;
      });
    };
    //-- met à jour la page courante et charge les données
    $scope.changePage = function (page) {
      $scope.page = page;
      $scope.getInfos();
    };
    //-- change le tri
    $scope.changeTri = function (tri) {
      if (tri == $scope.tri) {
        $scope.triAsc = !$scope.triAsc;
      } else {
        $scope.triAsc = true;
      }
      $scope.tri = tri;
      $scope.getInfos();
    }
    //-- supprime une commande
    $scope.confirmDelete = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Commande.ConfirmDelete', { nom: x.numero }).then(function (result) {
        if (confirm(result)) {
          $noHttp.delete('/api/commandeDelete/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- acquitte la commande
    $scope.confirmAcquitte = function (x) {
      if (!x || x.cle <= 0) {
        return;
      }
      $translate('Commande.ConfirmAcquitte', { nom: x.numero }).then(function (result) {
        if (confirm(result)) {
          $noHttp.post('/api/commandeAcquitte/', { cle: x.cle }, function (data) {
            $scope.getInfos();
          }, function (data, statusCode, headers, config, statusText) {
            // en cas d'erreur
          });
        }
      });
    }
    //-- sauve en session les paramètres
    $scope.SaveToSession = function () {
      webStorage.add('commandeListController',
        {
          page: $scope.page,
          tri: $scope.tri,
          triAsc: $scope.triAsc ? 'asc' : 'desc',
          searchText: $scope.searchText,
          statut: $scope.statut,
        });
    }
    //-- Recupère de la session les paramètres
    $scope.LoadFromSession = function () {
      var key = webStorage.get('commandeListController');
      if (key) {
        if (key.page) { $scope.page = key.page; }
        if (key.tri) { $scope.tri = key.tri; }
        if (key.triAsc) { $scope.triAsc = key.triAsc == 'asc'; }
        if (key.searchText) { $scope.searchText = key.searchText; }
        if (key.statut) { $scope.statut = key.statut; }
      }
    }
    //-- Vide le formulaire
    $scope.defaultParam = function () {
      $scope.setDefaultParam();
      $scope.getInfos();
    }
    //-- Place les valeurs par défaut
    $scope.setDefaultParam = function () {
      $scope.page = 1;
      $scope.pageSize = NOCONFIG.PAGESIZE;
      $scope.tri = 'numero';
      $scope.triAsc = true;
      $scope.searchText = '';
    }
    //--- Debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('Commande') == 3;
    $scope.setDefaultParam();
    $scope.LoadFromSession();
    if ($route.current.params.statut) {
      $scope.statut = $route.current.params.statut;
    }
    $scope.getInfos();

    $scope.$watch('roothub.progressionFinie', function (value) {
      if (value) {
        $scope.getInfos();
        value == null;
      }
    });
  }]);
//-- Controlleur Commande Edit --------------------------------------------------------------------------
app.controller('commandeEditController', ['$scope', '$noHttp', '$route', '$location', 'webStorage', '$translate',
  function ($scope, $noHttp, $route, $location, webStorage, $translate) {
    $scope.user = new utilisateur(webStorage.get(userKey));
    //-- recupère les infos de la commande
    $scope.getInfos = function () {
      $noHttp.get('/api/commandeGet/', { cle: $scope.cle, modeRead: $scope.readOnly }, function (data) {
        getOk(data);
      }, function (data, statusCode, headers, config, statusText) {
        getKo(data, statusCode, headers, config, statusText);
      });
    };
    //--- traite le retour d'un Get OK
    var getOk = function (data) {
      $scope.commande = data.commande;
      $scope.fournisseurs = data.fournisseurs;
      $scope.acquittee = data.commande.acquittee != null;
      $scope.configEmailOk = data.configEmailOk;
      $scope.doPrintBC = !$scope.commande.pieceRecues;
      $scope.qteMax = data.commandeLigneQuantiteMax;
    }
    //--- traite le retour d'un Get KO
    var getKo = function (data, statusCode, headers, config, statusText) {
      $scope.commande = null;
      $scope.fournisseurs = null;
      $scope.acquittee = false;
    }
    //-- insert une commande en Bdd
    $scope.saveCommande = function () {
      $noHttp.put('/api/commandeInsert/',
        {
          cle: 0,
          commande: {
            numero: $scope.commande.numero,
            fournisseurCle: $scope.commande.fournisseurNomCle.cle,
            utilisateurCle: $scope.user.cle,
          }
        }, function (data) { // c'est ok 
          $scope.commande = data.commande;
          $scope.cle = data.commande.cle;
          $scope.fournisseurs = data.fournisseurs;
        }, function (data, statusCode, headers, config, statusText) {
          $scope.commande = null;
          $scope.cle = 0;
          $scope.fournisseurs = null;
        });
    }
    //-- Actualise les totaux de la commande (dynamique : utilisé lors de l'edition des pièces)
    $scope.computeTotaux = function () {
      var ttl = 0;
      var ttlEtq = 0;
      var sum = 0
      if ($scope.commande && $scope.commande.pieces && $scope.commande.pieces.length) {
        var f, p;
        for (var i = 0; i < $scope.commande.pieces.length; i++) {
          if ($scope.commande.pieces[i] && $scope.commande.pieces[i].quantite) {
            p = $scope.commande.pieces[i].prixUnitaire || 0;
            f = $scope.commande.pieces[i].frais || 0;
            sum += $scope.commande.pieces[i].quantite * p + f;
            ttl += $scope.commande.pieces[i].quantite;
            if ($scope.commande.pieces[i].typePieceAvecTag) {
              ttlEtq += $scope.commande.pieces[i].quantite;
            }
          }
        }
      }

      $scope.commande.nombreProduit = ttl;
      $scope.commande.nombreProduitEtiquette = ttlEtq;
      $scope.commande.montantCommande = sum;
    }
    //-- Ajoute une seconde pièce quand on a des tailles ou couleurs possibles
    $scope.addPiece = function (piece) {
      if (!piece) {
        return;
      }
      var clone = JSON.parse(JSON.stringify(piece));
      clone.guid = 'g' + $scope.commande.pieces.length;
      var idx = $scope.commande.pieces.indexOf(piece);
      $scope.commande.pieces.splice(idx + 1, 0, clone);
      $scope.computeTotaux();
    }
    //-- Enregistre les modifications sur la commande
    $scope.saveModif = function () {
      for (var i = 0; i < $scope.commande.pieces.length; i++) {
        if ($scope.commande.pieces[i].couleur && $scope.commande.pieces[i].couleur.cle) {
          $scope.commande.pieces[i].couleurCle = $scope.commande.pieces[i].couleur.cle;
        }
        if ($scope.commande.pieces[i].taille && $scope.commande.pieces[i].taille.cle) {
          $scope.commande.pieces[i].tailleCle = $scope.commande.pieces[i].taille.cle;
        }
      }
      $noHttp.post('/api/commandeEdit/',
        {
          cle: $scope.cle,
          validation: $scope.validation,
          envoieEmail: $scope.envoieEmail,
          processEnvoie: $scope.processEnvoie && $scope.configEmailOk && ($scope.commande.fournisseurEmail != null),
          emailSuplementaire: $scope.emailSuplementaire,
          acquittee: $scope.acquittee,
          commande: $scope.commande,
        }, function (data) { // c'est ok 
          $scope.commande = data.commande;
          if ($scope.commande.validation && !$scope.commande.debutImpression) {
            $scope.goPrint();
          } else if ($scope.commande.envoieEmail) {
            if ($scope.doPrintBC) { // le user a validé la demande
              $scope.printBC(true);
            }
            $location.path('/commandeList');
          } else {
            $location.path('/commandeList');
          }
        }, function (data, statusCode, headers, config, statusText) {
        });
    }
    //-- redirection vers l'impression des étiquettes de la commande
    $scope.goPrint = function () {
      $location.path('/commandePrint').search({ cle: $scope.commande.cle });
    }
    //-- imprimme le BL
    $scope.printBC = function () {
      var w = window.open('/Print.html#/commande?cle=' + $scope.cle);
      angular.element(w).bind('load', function () {
          setTimeout(function () { w.close(); }, 1000);
        });
    }
    //--- envoye le bon de commande par email au fournisseur
    $scope.sendEmail = function () {
      $noHttp.post('/api/commandeEdit/',
        {
          cle: $scope.cle,
          envoieEmail: true,
          processEnvoie: true,
          emailSuplementaire: $scope.emailSuplementaire,
          commande: $scope.commande,
        }, function (data) { // c'est ok 
          $scope.showEmailplus = false;
          $translate('Commande.ConfirmMailSend').then(function (result) {
            alert(result);
          });
        }, function (data, statusCode, headers, config, statusText) {
          $scope.showEmailplus = false;
        });
    }
    //--- renvoie le fichier Excel des pièces
    $scope.download = function () {
      $noHttp.get('/api/commandeGet/', {
        cle: $scope.cle,
        excel: true,
      }, function (data, statusCode, headers, config, statusText) { // c'est ok 
        if (data && data.excelFileUrl) {
          // y a une réponse et le fichier est dispo
          var link = angular.element("<a/>");
          link.attr({
            href: data.excelFileUrl,
            target: '_blank',
            download: data.excelFileUrl
          })[0].click();
        }
      }, function (data, statusCode, headers, config, statusText) { // Erreur
      });
    }
    //--- Corrige la commande : action donne le type de correction
    $scope.corrigeCommande = function (action) {
      $noHttp.get('/api/commandeCorrige/', {
        cle: $scope.cle,
        modeRead: $scope.readOnly,
        action: action
      }, function (data) {
        getOk(data);
      }, function (data, statusCode, headers, config, statusText) {
        getKo(data, statusCode, headers, config, statusText);
      });
    }
    //-- Début de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.validation = null;
    $scope.envoieEmail = null;
    $scope.acquittee = null;
    $scope.showEmailplus = false;
    $scope.doPrintBC = true;
    $scope.processEnvoie = true;
    $scope.cle = $route.current.params.cle;
    $scope.readOnly = user.isInRole('Commande') < 3 || $route.current.params.r;
    $scope.getInfos(); // on fait tous le temps pour avoir les données satellites
  }]);

//-- Controlleur Mail config ----------------------------------------------------------------------------
app.controller('mailconfigController', ['$scope', '$noHttp', '$route', '$location', 'webStorage',
  function ($scope, $noHttp, $route, $location, webStorage) {
    $scope.user = new utilisateur(webStorage.get(userKey));
    //-- recupère les infos de la config
    $scope.getInfos = function () {
      $noHttp.get('/api/mailconfigGet/', { cle: $scope.cle }, function (data) {
        $scope.config = data.config;
      }, function (data, statusCode, headers, config, statusText) {
        $scope.config = null;
      });
    };
    //-- Enregistre les modifications
    $scope.saveModif = function () {
      $noHttp.post('/api/mailconfigGet/',
        {
          config: $scope.config,
        }, function (data) { // c'est ok 
          $location.path('/administration');
        }, function (data, statusCode, headers, config, statusText) {
          // en cas d'erreur
        });
    }
    //-- debut de la page
    var user = new utilisateur(webStorage.get(userKey));
    $scope.canAdd = user.isInRole('MailConfig') == 3;
    $scope.subjectFournisseurFocus = false;
    $scope.subjectClientFocus = false;
    $scope.getInfos();
  }]);
