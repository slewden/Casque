//-- traductions --------------------------------------------------------------------------------------------------
app.config(['$translateProvider', function ($translateProvider) {
  // add translation table
  $translateProvider
    .translations(selecteLanguage, translations)
    .preferredLanguage(selecteLanguage);
}]);

//-- Directives ---------------------------------------------------------------------------------------------------
//-- To Focus
app.directive('focusMe', function () {
  return {
    scope: { trigger: '=focusMe' },
    link: function (scope, element) {
      scope.$watch('trigger', function (value) {
        if (value === true) {
          //console.log('trigger',value);
          //$timeout(function() {
          element[0].focus();
          scope.trigger = false;
          //});
        }
      });
    }
  };
});
//-- Enter
app.directive('myEnter', function () {
  return function (scope, element, attrs) {
    element.bind("keydown keypress", function (event) {
      if (event.which === 13) {
        scope.$apply(function () {
          scope.$eval(attrs.myEnter);
        });

        event.preventDefault();
      }
    });
  };
});
//-- To DateTime 
app.directive('inputDate', ['$filter', 'ModalService', function ($filter, ModalService) {
  return {
    restrict: 'E',
    require: 'ngModel',
    scope: {
      ngModel: '=',
      required: '=?',
      withHours: '@'
    },
    template:
              '<button type="button" class="btn btn-cadre" data-ng-click="choixDate()"><i class="fa fa-calendar"></i></button>',
    link: function (scope, element, attr, ctrl) {

      scope.withHours = !attr.withHours ? true : (attr.withHours == "true");
      scope.format = scope.withHours ? 'short' : 'shortDate';
      ctrl.$render = function () {
        checkValidDate(ctrl.$viewValue);
      };
      //-- vérifie la date
      var checkValidDate = function (value) {
        if (!value) {
          ctrl.$setValidity('date', !scope.required);
          return;
        }
        var date = Date.parseFrenchShort(value);
        ctrl.$setValidity('date', date != null);
      };
      //-- renvoie la position du bouton el
      var getOffset = function (el) {
        var x = 0;
        var y = 0;
        if (el.tagName == 'I') { // on a clické sur l'icone
          el = el.parentElement;
        }
        var w = el.offsetWidth;
        var h = el.offsetHeight;
        while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
          x += el.offsetLeft;
          y += el.offsetTop;
          el = el.offsetParent;
        }
        return { top: y, left: x, width: w, height: h };
      }
      //-- choix date valide
      scope.choixDate = function () {
        var bt = event.target; 
        ModalService.showModal({
          templateUrl: "../Partials/Calendrier.html",
          controller: "modalCalendar",
          inputs: {
            date: ctrl.$modelValue,
            withHours: scope.withHours,
            position: getOffset(bt)
          }
        }).then(function (modal) {
          modal.close.then(function (result) {
            if (result) {
              if (result == 'x') {
                ctrl.$setViewValue(null);
              } else {
                ctrl.$setViewValue($filter('date')(result, scope.format));
              }
            }
          });
        });
      }
    }
  };
}]);
//-- To color
app.directive('colorChooser', ['$filter', 'ModalService', function ($filter, ModalService) {
  return {
    restrict: 'E',
    require: 'ngModel',
    scope: {
      ngModel: '=',
      required: '=?',
      hideCode: '@?',
      key: '@'
    },
    template:
              '<div style="display:inline-block;width:100px;height:20px;border:1px solid #A9A9A9;cursor:pointer;background-color:grey;" data-ng-click="choixColor()">'
     + '  <div style="height:20px;width:75px;float:left;line-height: 20px;padding-left: 4px;padding-right: 4px;" data-ng-style="{\'background-color\' : ngModel, \'color\' : opppositeColor}">{{  ngModelAff }}</div>'
     + '  <span style="padding-left: 4px;background-color:grey;"><i class="fa fa-caret-down"></i></span>'
     + '</div>',
    link: function (scope, element, attr, ctrl) {
      //-- renvoie la couleur opposée
      var invertColor = function (hexTripletColor) {
        if (hexTripletColor) {
          var color = hexTripletColor;
          color = color.substring(1);           // remove #
          color = parseInt(color, 16);          // convert to integer
          color = 0xFFFFFF ^ color;             // invert three bytes
          color = color.toString(16);           // convert to hex
          color = ("000000" + color).slice(-6); // pad with leading zeros
          color = "#" + color;                  // prepend #
          return color;
        } else {
          return '';
        }
      }
      //-- renvoie la position du bouton el
      var getOffset = function (el) {
        var x = 0;
        var y = 0;
        if (el.tagName == 'I') { // on a clické sur l'icone
          el = el.parentElement;
        }
        var w = el.offsetWidth;
        var h = el.offsetHeight;
        while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
          x += el.offsetLeft;
          y += el.offsetTop;
          el = el.offsetParent;
        }
        return { top: y, left: x, width: w, height: h };
      }
      //-- choix date valide
      scope.choixColor = function () {
        var bt = event.target; //document.getElementsByClassName("btn-cadre");
        ModalService.showModal({
          templateUrl: "../Partials/ColorPicker.html",
          controller: "colorPiker",
          inputs: {
            color: scope.ngModel,
            key: scope.key,
            position: getOffset(bt)
          }
        }).then(function (modal) {
          modal.close.then(function (result) {
            if (result != 'x') {
              ctrl.$setViewValue(result);
              scope.opppositeColor = invertColor(result);
              scope.ngModelAff = scope.hideCode ? '' : scope.ngModel;
            }
          });
        });
      }

      scope.$watchGroup(['ngModel', 'hideCode'], function () {
        scope.ngModelAff = scope.hideCode ? '' : scope.ngModel;
      });


      scope.ngModelAff = (scope.hideCode ? '' : scope.ngModel);
      scope.key = attr.key;
      scope.opppositeColor = invertColor(scope.ngModel);
    }
  };
}]);
//-- Notify
app.directive('notifyZone', [function () {
  return {
    restrict: 'E',
    require: 'ngModel',
    scope: {
      ngModel: '=',
    },
    template:
         '<div class="notify-message" data-ng-show="ngModel">'
       + '  <div class="notify-message-left">'
       + '    {{ ngModel }} '
       + '  </div>'
       + '  <div class="notify-message-right">'
       + '    <div class="btn" data-ng-click="clearNotification()" title="{{ \'Common.TipsHide\' | translate }}"><i class="fa fa-times"></i></div>'
       + '  </div>'
       + '</div>',
    link: function (scope, element, attr, ctrl) {
      scope.clearNotification = function () {
        scope.ngModel = null;
      }
    }
  };
}]);

app.directive('helpSubjectFournisseur', [function () {
  return {
    restrict: 'E',
    replace: true,
    template:'<div data-ng-show="subjectFournisseurFocus" class="bloc-doc">'
           + ' <span>{{ \'MailConfig.HelpSubjectFournisseur\' | translate }}</span>'
           + ' <ul>'
           + '  <li>##FournisseurNom## : Nom du fournisseur</li>'
           + '  <li>##FournisseurAdresseCommande## : Adresse de commande</li>'
           + '  <li>##EnvoieEmail## : Date du premier envoie de l\'email</li>'
           + '  <li>##Numero## : Numéro de la commande</li>'
           + '  <li>##Saisie## : Date de début de saisie de la commande</li>'
           + '  <li>##StatutNom## : Statut de la commande</li>'
           + '  <li>##NombreProduit## : Nombre total de produits dans la commande</li>'
           + '  <li>##NombreProduitEtiquette## :  Nombre total de produits étiquettés dans la commande</li>'
           + '  <li>##MontantCommande## : Montant total de la commande</li>'
           + ' </ul>'
           + '</div>',
  };
}]);

app.directive('helpSubjectClient', [function () {
  return {
    restrict: 'E',
    replace: true,
    template: '<div data-ng-show="subjectClientFocus" class="bloc-doc">'
            + ' <span>{{ \'MailConfig.HelpSubjectClient\' | translate }}</span>'
            + ' <ul>'
            + '  <li>##ClientNom## : Nom du client</li>'
            + '  <li>##ClientAdresseLivraison## : Adresse de livraison</li>'
            + '  <li>##Creation## : Date de début de création de la livraison</li>'
            + '  <li>##Reference## : Référence de la livraison</li>'
            + '  <li>##NombreCarton## : Nombre total de cartons dans la livraison</li>'
            + '  <li>##NombrePiece## : Nombre total de pièces livrées</li>'
            + '  <li>##UtilisateurNom## : Nom de l\'opérateur qui a fait la livraison</li>'
            + '  <li>##Validation## :  Date de validation de l livraison</li>'
            + '  <li>##MontantCommande## : Montant total de la commande</li>'
            + ' </ul>'
            + '</div>',
  };
}]);

app.directive('multipleEmails', function () {
    return {
      require: 'ngModel',
      link: function (scope, element, attrs, ctrl) {
        ///var emailsRegex = /^[\W]*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4}[\W]*,{1}[\W]*)*([\w+\-.%]+@[\w\-.]+\.[A-Za-z]{2,4})[\W]*$/;
        var emailsRegex = /^[\W]*((?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)+(\.[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)?)[\W]*,{1}[\W]*)*((?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)+(\.[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)?))[\W]*$/;
        ctrl.$parsers.unshift(function (viewValue) {
          if (emailsRegex.test(viewValue)) {
            ctrl.$setValidity('multipleEmails', true);
            return viewValue;
          } else {
            ctrl.$setValidity('multipleEmails', false);
            return undefined;
          }
        });
      }
    };
  });

//-- Percentage 
app.directive('percentageBar', [function () {
  return {
    restrict: 'E',
    replace: true,
    scope: {
      percent: '@',
      message: '@'
    },
    template: '<div class="percentage-bar" data-ng-class="{\'error-message\': percent0}">'
            + ' <div class="percentage-left" data-ng-style="stWidth"></div>'
            + ' <div class="percentage-right" data-ng-style="st100MoinsWidth">&nbsp;<span data-ng-if="percent0" class="percentage-error">{{ message }}</<span></div>'
            + '</div>',
    link: function (scope, element, attr, ctrl) {
      scope.$watch("percent", function () {
        var v = attr.percent;
        if (Number(v) < 0) {
          v = 0;
        } else if (Number(v) > 100) {
          v = 100;
        }

        scope.thePercentage = v;
        scope.percent0 = v == 0;
        scope.stWidth = { width: scope.thePercentage + '%' };
        scope.st100MoinsWidth = { width: (100 - scope.thePercentage) + '%' };
      })
    }
  }
}]);
//-- Compare to 
app.directive("compareTo", [function () {
  return {
    require: "ngModel",
    scope: {
      otherModelValue: "=compareTo"
    },
    link: function (scope, element, attributes, ngModel) {

      ngModel.$validators.compareTo = function (modelValue) {
        return modelValue == scope.otherModelValue;
      };

      scope.$watch("otherModelValue", function () {
        ngModel.$validate();
      });
    }
  };
}]);
//-- Validator Téléphone 
app.directive('checkPhone', function () {
  var PHONE_REGEXP = /^\+?(?:[0-9][\x20-\.]?){6,14}[0-9]$/i;

  return {
    require: 'ngModel',
    restrict: '',
    link: function (scope, elm, attrs, ctrl) {
      ctrl.$validators.checkPhone = function (modelValue) {
        return ctrl.$isEmpty(modelValue) || PHONE_REGEXP.test(modelValue);
      };
    }
  };
});
//-- Validator Adresse Ip
app.directive('checkAdresseIp', function () {
  var ADRESSE_IP_REGEXP = /\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b/
  ;

  return {
    require: 'ngModel',
    restrict: '',
    link: function (scope, elm, attrs, ctrl) {
      ctrl.$validators.checkAdresseIp = function (modelValue) {
        return ctrl.$isEmpty(modelValue) || ADRESSE_IP_REGEXP.test(modelValue);
      };
    }
  };
});
//-- File-Model for upload file
app.directive('fileModel', ['$parse', function ($parse) {
  return {
    restrict: 'A',
    link: function (scope, element, attrs) {
      var model = $parse(attrs.fileModel);
      var modelSetter = model.assign;

      element.bind('change', function () {
        scope.$apply(function () {
          modelSetter(scope, element[0].files[0]);
        });
      });
    }
  };
}]);
//-- Directives ---------------------------------------------------------------------------------------------------

//-- Filters ------------------------------------------------------------------------------------------------------
//-- date venant d'un web service
app.filter('datews', ['$filter', function ($filter) {
  return function (d, format) {
    if (d) {
      if (typeof d.getMonth !== 'function') {
        if (d.indexOf('/Date(') == 0) { // gère le format du type : /Date(1433282277027+0200)/
          var x = d.substring(6, d.length - 2);
          var q = x.substring(0, x.indexOf('+'));
          var d = new Date(Number(q));
        }
        else if (!isNaN(d)) { // Au cas ou ??
          var x = new Date(d)
          if (x != 'Invalid Date') {
            d = x;
          }
        }
      }

      if (typeof d.getMonth !== 'function') {
        var d2 = Date.parseFrenchShort(d);
        if (d2) {
          d = d2;
        }
      }

      // verifie les bornes
      if (d <= new Date(1753, 0, 1)) {
        return '';
      } else if (d >= new Date(9999, 11, 31)) {
        return '';
      } else {
        return $filter('date')(d, format);
      }
    } else {
      return '';
    }
  };
}]);
//-- pour Masquer les nombre à 0 ou date& heure vides
app.filter('masquezero', ['$window', function ($window) {
  return function (input) {
    if (input == '00:00') {
      return null;
    }

    if ($window.isNaN(input)) {
      return input;
    }

    if (input === 0) {
      return null;
    }

    return input;
  };
}]);
//-- pour passer en valeur absolue
app.filter('absolute', [function () {
  return function(num) { return Math.abs(num); }
}]);
//-- range Filter 
app.filter('range', [function () {
  return function (input, total) {
    total = parseInt(total);
    for (var i = 0; i < total; i++) {
      input.push(i);
    }
    return input;
  };
}]);
//-- Skip Filter 
app.filter('skip', [function () {
  return function (input, skipCount) {
    if (!input) return input;
    return input.slice(skipCount);
  };
}]);
//-- This filter makes the assumption that the input will be in decimal form (i.e. 17% is 0.17).
app.filter('percentage', ['$filter', function ($filter) {
  return function (input, decimals) {
    return $filter('number')(input * 100, decimals) + '%';
  };
}]);

//-- Filters ------------------------------------------------------------------------------------------------------

//-- Controlleurs -------------------------------------------------------------------------------------------------
//-- Controlleur du calendrier
app.controller('modalCalendar', ['$scope', '$filter', 'date', 'withHours', 'position', 'close', function ($scope, $filter, date, withHours, position, close) {
  //-- initialise le calendrier
  $scope.computeCalendar = function (td, mode) {
    td = td || new Date();
    var calendar = {};
    calendar.dateRef = new Date(td.getFullYear(), td.getMonth(), 1, 0, 0, 0, 0); // 1er du mois
    calendar.dateFin = calendar.dateRef.clone().addMonths(1); //  1er du mois suivant
    calendar.date1 = calendar.dateRef.clone();
    var now = new Date();
    calendar.todayGT = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0, 0).getTime();
    var i = 0
    while (calendar.date1.getDay() != 1) {
      calendar.date1.addDays(-1);
    }
    // les entete de colonnes
    calendar.cols = [];
    var dt = calendar.date1.clone();
    for (var i = 0; i < 7; i++) {
      calendar.cols.push({ date: dt.clone() });
      dt.addDays(1);
    }
    // les lignes de jour
    calendar.jRows = [];
    dt = calendar.date1.clone();
    for (var j = 0; j < 6; j++) {
      var co = [];
      for (var i = 0; i < 7; i++) {
        co.push({
          date: dt.clone(),
          dateTxt: (dt.getTime() == calendar.todayGT ? 'Ajourd\'hui le ' : '') + $filter('date')(dt),
          dateGT: dt.getTime(),
          inMonth: dt >= calendar.dateRef && dt < calendar.dateFin
        });
        dt = dt.addDays(1);
      }
      calendar.jRows.push({ cols: co });
    }
    // les lignes de mois
    calendar.mRows = [];
    calendar.dateMRef = calendar.dateRef.clone();
    if (calendar.dateMRef.getMonth() > 6) {
      calendar.dateMRef.addMonths(6);
    }
    var yj1 = new Date(calendar.dateMRef.getFullYear(), 0, 1, 0, 0, 0, 0); // 1er du mois
    var yj2 = yj1.clone().addMonths(12);

    dt = calendar.dateRef.clone();
    for (var j = 0; j < 4; j++) {
      var co = [];
      for (var i = 0; i < 4; i++) {
        co.push({
          date: dt.clone(),
          dateTxt: (dt.getTime() == calendar.todayGT ? 'Ajourd\'hui le ' : '') +  $filter('date')(dt),
          dateGT: dt.getTime(),
          inYear: dt >= yj1 && dt < yj2
        });
        dt = dt.addMonths(1);
      }
      calendar.mRows.push({ cols: co });
    }

    calendar.mode = mode || 0;
    calendar.anM1 = calendar.dateRef.clone().addMonths(-12);
    calendar.anP1 = calendar.dateRef.clone().addMonths(12);
    calendar.moisM1 = calendar.dateRef.clone().addMonths(-1);
    calendar.moisP1 = calendar.dateRef.clone().addMonths(1);
    return calendar;
  }
  //-- swap l'affichage du calendrier (choix des mois ou change de mois)
  $scope.displayDate = function (dt, mode) {
    if (!dt) {
      return;
    }
    var dat = $scope.calendar.theDate;
    $scope.calendar = $scope.computeCalendar(dt, mode);
    $scope.putADate(dat);
  }
  //-- positionne une date
  $scope.putADate = function (dt) {
    if (dt) {
      $scope.calendar.theDate = dt.clone();
      $scope.calendar.theDateGT = $scope.calendar.theDate.getTime();
      $scope.calendar.theDate1GT = new Date($scope.calendar.theDate.getFullYear(), $scope.calendar.theDate.getMonth(), 1, 0, 0, 0, 0).getTime(); // 1er du mois
    } else {
      $scope.calendar.theDate = null;
      $scope.calendar.theDateGT = null;
      $scope.calendar.theDate1GT = null;
    }
  }
  //-- sélectionne une nouvelle date
  $scope.selectTheDate = function (dt) {
    if (!dt) {
      return;
    }
    if ($scope.calendar.hours) {
      dt.setHours($scope.calendar.hours);
    } else {
      dt.setHours(0);
    }
    if ($scope.calendar.minutes) {
      dt.setMinutes($scope.calendar.minutes);
    } else {
      dt.setMinutes(0);
    }
    $scope.close(dt);
  }
  //-- place une heure
  $scope.putAnHour = function (dt) {
    if (dt) {
      $scope.calendar.hours = (dt.getHours() < 10 ? '0' : '') + dt.getHours();
      $scope.calendar.minutes = (dt.getMinutes() < 10 ? '0' : '') + dt.getMinutes();
    } else {
      $scope.calendar.hours = 0;
      $scope.calendar.minutes = 0;
    }
  }
  //-- positionne à maintenant
  $scope.setNow = function () {
    $scope.putAnHour(new Date());
    $scope.selectTheDate(new Date());
  }
  //-- efface la date
  $scope.setEmpty = function () {
    $scope.close('x');
  }
  //-- valie les choix d'heure
  $scope.validCurrent = function () {
    if ($scope.calendar.theDate) {
      $scope.selectTheDate($scope.calendar.theDate);
    }
  }
  //-- ferme la modale
  $scope.close = function (result) {
    close(result, 5); // close, but give 500ms for animation
  };
  //-- check is date
  var isDate = function (date) {
    return ((new Date(date) !== "Invalid Date" && !isNaN(new Date(date))));
  }
  //-- debut de la page
  var dat = null;
  if (date) {
    var d2 = Date.parseFrenchShort(date);
    if (d2) {
      dat = d2;
    } else {
      dat = Date.parseJson(date);
    }
  }
  $scope.calendar = $scope.computeCalendar(dat);
  $scope.putADate(dat);
  $scope.putAnHour(dat);
  $scope.withHours = withHours.toString() === "true";
  $scope.position = { left: Math.max(0, Number(position.left) + Number(position.width) - 230), top: Number(position.top) + Number(position.height) };
}]);
//-- controleur du color piker
app.controller('colorPiker', ['$scope', '$timeout', 'webStorage', 'color', 'key', 'position', 'close', function ($scope, $timeout, webStorage, color, key, position, close) {
  //-- couleurs de la palette
  $scope.colors = [
    // version Light : 4 ligne only
    ['#000000', '#222222', '#444444', '#666666', '#888888', '#AAAAAA', '#CCCCCC', '#EEEEEE', '#FFFFFF'],
    ['#FFFF00', '#FF9900', '#FF0000', '#FF0099', '#FF00FF', '#FF99FF', '#9900FF', '#0000FF', '#000099'],
    ['#FFFF99', '#99FF99', '#99FF00', '#00FF00', '#00FF99', '#99FFFF', '#00FFFF', '#0099FF', '#009999'],
    ['#660000', '#783f04', '#7f6000', '#666600', '#274e13', '#0c343d', '#073763', '#20124d', '#4c1130']
    // version full Office
    //['#000000', '#222222', '#444444', '#666666', '#888888', '#AAAAAA', '#CCCCCC', '#FFFFFF'],
    //['#FF0000', '#FF9900', '#FFFF00', '#00FF00', '#00FFFF', '#0000FF', '#9900FF', '#FF00FF'],
    //['#F4CCCC', '#FCE5CD', '#FFF2CC', '#D9EAD3', '#D0E0E3', '#CFE2F3', '#D9D2E9', '#EAD1DC'],
    //["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
    //["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
    //["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
    //["#900", "#b45f06", "#bf9000", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
    //["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
  ];
  //$scope.customColors.push();
  //-- fonctions utiles de conversions
  var rgbToHex = function (r, g, b) {
    if (r > 255 || g > 255 || b > 255) {
      return '';
    }
    return ((r << 16) | (g << 8) | b).toString(16);
  }
  var rgb2hsv = function (r0, g0, b0) {
    var rr, gg, bb,
        r = r0 / 255,
        g = g0 / 255,
        b = b0 / 255,
        h, s,
        v = Math.max(r, g, b),
        diff = v - Math.min(r, g, b),
        diffc = function (c) {
          return (v - c) / 6 / diff + 1 / 2;
        };
    if (diff == 0) {
      h = s = 0;
    } else {
      s = diff / v;
      rr = diffc(r);
      gg = diffc(g);
      bb = diffc(b);
      if (r === v) {
        h = bb - gg;
      } else if (g === v) {
        h = (1 / 3) + rr - bb;
      } else if (b === v) {
        h = (2 / 3) + gg - rr;
      }
      if (h < 0) {
        h += 1;
      } else if (h > 1) {
        h -= 1;
      }
    }
    return {
      h: Math.round(h * 360),
      s: Math.round(s * 100),
      v: Math.round(v * 100)
    };
  }
  var hsv2rgb = function (h, s, v) {
    var r, g, b, i, f, p, q, t;
    if (arguments.length === 1) {
      s = h.s, v = h.v, h = h.h;
    }
    i = Math.floor(h * 6);
    f = h * 6 - i;
    p = v * (1 - s);
    q = v * (1 - f * s);
    t = v * (1 - (1 - f) * s);
    switch (i % 6) {
      case 0: r = v, g = t, b = p; break;
      case 1: r = q, g = v, b = p; break;
      case 2: r = p, g = v, b = t; break;
      case 3: r = p, g = q, b = v; break;
      case 4: r = t, g = p, b = v; break;
      case 5: r = v, g = p, b = q; break;
    }
    return {
      r: Math.round(r * 255),
      g: Math.round(g * 255),
      b: Math.round(b * 255)
    };
  }
  //-- dessin de la palette de couleurs primaires
  var draw = function () {
    var type = 'v';
    var canvas = document.getElementById('vChoice');
    var ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    var vGrad = ctx.createLinearGradient(0, 0, 0, canvas.height);
    vGrad.addColorStop(0 / 6, '#F00');
    vGrad.addColorStop(1 / 6, '#FF0');
    vGrad.addColorStop(2 / 6, '#0F0');
    vGrad.addColorStop(3 / 6, '#0FF');
    vGrad.addColorStop(4 / 6, '#00F');
    vGrad.addColorStop(5 / 6, '#F0F');
    vGrad.addColorStop(6 / 6, '#F00');
    ctx.fillStyle = vGrad;
    ctx.fillRect(0, 0, canvas.width, canvas.height);
  }
  //-- dessin de la palette des saturation / lumières
  var draw2 = function (color) {
    var canvas = document.getElementById('zChoice');
    var ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.fillStyle = color;
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    var vGrad = ctx.createLinearGradient(0, 0, canvas.width, 0);
    vGrad.addColorStop(1, 'rgba(255,255,255,0)');
    vGrad.addColorStop(0, 'rgba(255,255,255,1)');
    ctx.fillStyle = vGrad;
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    var hGrad = ctx.createLinearGradient(0, 0, 0, canvas.height);
    hGrad.addColorStop(0, 'rgba(0,0,0,0)');
    hGrad.addColorStop(1, 'rgba(0,0,0,1)');
    ctx.fillStyle = hGrad;
    ctx.fillRect(0, 0, canvas.width, canvas.height);
  }
  //-- met à jour le scope fonction de la couleur choisie
  var updateValuesFromColor = function (colorRGB) {
    $scope.updateInProgress = true;
    if (colorRGB) {
      $scope.colorEmpty = false;
      $scope.currentColor = colorRGB; // de la forme #RRGGBB : RR GG BB en hexa
      $scope.currentR = parseInt($scope.currentColor.slice(1, 3), 16);
      $scope.currentG = parseInt($scope.currentColor.slice(3, 5), 16);
      $scope.currentB = parseInt($scope.currentColor.slice(5, 7), 16);
      var h2 = rgb2hsv($scope.currentR, $scope.currentG, $scope.currentB);
      $scope.hue = h2.h;
      $scope.saturation = h2.s;
      $scope.lumiere = h2.v;
      $scope.hsv2 = 'hsl(' + h2.h + ',' + h2.s + '%,' + h2.v + '%)';
    } else {
      $scope.colorEmpty = true;
      $scope.currentColor = '';
      $scope.currentR = '';
      $scope.currentG = '';
      $scope.currentB = '';
      $scope.hue = '';
      $scope.saturation = '';
      $scope.lumiere = '';
      $scope.hsv2 = '';
    }
    $scope.updateInProgress = false;
  }
  //-- Event Click sur choix d'une couleur primaire
  $scope.onClickVChoice = function (e) {
    var canvas = document.getElementById('vChoice');
    var rect = canvas.getBoundingClientRect();
    var x = e.clientX - rect.left;
    var y = e.clientY - rect.top;
    var ctx = canvas.getContext('2d');
    var p = ctx.getImageData(x, y, 1, 1).data;
    $scope.clV = "#" + ("000000" + rgbToHex(p[0], p[1], p[2])).slice(-6).toUpperCase();
    draw2($scope.clV);
    $scope.hue = Math.round((360 * y / (rect.height - 1))) % 360;
    var rgb = hsv2rgb($scope.hue / 360, $scope.saturation / 100, $scope.lumiere / 100);
    var color = "#" + ("000000" + rgbToHex(rgb.r, rgb.g, rgb.b)).slice(-6).toUpperCase();
    updateValuesFromColor(color);
  }
  //-- Event Mouse move sur choix d'une couleur primaire
  $scope.onMouseMoveVChoice = function (e) {
    if ((e.buttons & 1) == 1) { // bt gauche enfoncé
      $scope.onClickVChoice(e);
    }
  }
  //-- Event clik sur choix d'une saturation + lumière
  $scope.onClickZChoice = function (e) {
    var canvas = document.getElementById('zChoice');
    var rect = canvas.getBoundingClientRect();
    var x = e.clientX - rect.left;
    var y = e.clientY - rect.top;
    $scope.saturation = Math.round(x / (rect.width - 1) * 100) % 100;
    $scope.lumiere = 100 - Math.round(y / (rect.height - 1) * 100) % 100;
    var rgb = hsv2rgb($scope.hue / 360, $scope.saturation / 100, $scope.lumiere / 100);
    var color = "#" + ("000000" + rgbToHex(rgb.r, rgb.g, rgb.b)).slice(-6).toUpperCase();
    updateValuesFromColor(color);
  }
  //-- Event mouse move sur choix d'une saturation + lumière
  $scope.onMouseMoveZChoice = function (e) {
    if ((e.buttons & 1) == 1) { // bt gauche enfoncé
      $scope.onClickZChoice(e);
    }
  }
  //-- Event suivi d'un changement d'une valeur RGB
  $scope.changeRGB = function () {
    if ($scope.updateInProgress) {
      return;
    }
    var r = $scope.currentR;
    var g = $scope.currentG;
    var b = $scope.currentB;
    var ir = parseInt(r);
    var ig = parseInt(g);
    var ib = parseInt(b);
    if (ir == NaN || ir < 0) {
      ir = 0;
    } else if (ir > 255) {
      ir = 255;
    }
    if (ig == NaN || ig < 0) {
      ig = 0;
    } else if (ig > 255) {
      ig = 255;
    }
    if (ib == NaN || ib < 0) {
      ib = 0;
    } else if (ib > 255) {
      ib = 255;
    }

    $scope.updateInProgress = true;
    if (($scope.currentR == null || $scope.currentR === '')
     && ($scope.currentG == null || $scope.currentG === '')
     && ($scope.currentB == null || $scope.currentB === '')) {
      $scope.colorEmpty = true;
      $scope.currentColor = '';
      $scope.hue = '';
      $scope.saturation = '';
      $scope.lumiere = '';
    } else {
      var color = "#" + ("000000" + rgbToHex(ir, ig, ib)).slice(-6).toUpperCase();
      $scope.colorEmpty = false;
      $scope.currentColor = color;
      var h2 = rgb2hsv($scope.currentR, $scope.currentG, $scope.currentB);
      $scope.hue = h2.h;
      $scope.saturation = h2.s;
      $scope.lumiere = h2.v;
      $scope.hsv2 = 'hsl(' + h2.h + ',' + h2.s + '%,' + h2.v + '%)';
      var cl2 = hsv2rgb(h2.h / 360, 1, 1);
      var color2 = "#" + ("000000" + rgbToHex(cl2.r, cl2.g, cl2.b)).slice(-6).toUpperCase();
      $scope.clV = color2;
      draw2($scope.clV);
    }
    $scope.updateInProgress = false;
  }
  //-- Gestion des scroller RGB
  var getXChange = function (e, id) {
    var div = document.getElementById(id);
    var rect = div.getBoundingClientRect();
    var x = e.clientX - rect.left;
    return Math.round(x / (rect.width - 2) * 255) % 256;
  }
  //-- Event scroller Mouse Move R
  $scope.onMouseMoveR = function (e) {
    if ((e.buttons & 1) == 1) { // bt gauche enfoncé
      $scope.currentR = getXChange(e, 'rChoice');
    }
  }
  //-- Event scroller Click R
  $scope.onClickR = function (e) {
    $scope.currentR = getXChange(e, 'rChoice');
  }
  //-- Event scroller Mouse Move G
  $scope.onMouseMoveG = function (e) {
    if ((e.buttons & 1) == 1) { // bt gauche enfoncé
      $scope.currentG = getXChange(e, 'gChoice');
    }
  }
  //-- Event scroller Click G
  $scope.onClickG = function (e) {
    $scope.currentG = getXChange(e, 'gChoice');
  }
  //-- Event scroller Mouse Move B
  $scope.onMouseMoveB = function (e) {
    if ((e.buttons & 1) == 1) { // bt gauche enfoncé
      $scope.currentB = getXChange(e, 'bChoice');
    }
  }
  //-- Event scroller Click B
  $scope.onClickB = function (e) {
    $scope.currentB = getXChange(e, 'bChoice');
  }
  //-- Event Click dans une case de couleur prédéfinie
  $scope.setColor = function (color) {
    $scope.clV = color;
    updateValuesFromColor(color);
    draw2($scope.clV);
  }
  //-- Initialisation avec la couleur d'origine fournie
  var initColors = function () {
    $scope.clV = $scope.colorOrigine;
    if ($scope.colorOrigine) {
      var h2 = rgb2hsv(parseInt($scope.clV.slice(1, 3), 16), parseInt($scope.clV.slice(3, 5), 16), parseInt($scope.clV.slice(5, 7), 16));
      var canvas = document.getElementById('vChoice');
      var rect = canvas.getBoundingClientRect();
      $scope.hue = h2.h * (rect.height - 1) / 360;
      var canvas = document.getElementById('zChoice');
      var rect = canvas.getBoundingClientRect();
      $scope.saturation = h2.s * (rect.width - 1) / 100;
      $scope.lumiere = h2.v * (rect.height - 1) / 100;
    } else {
      $scope.hue = 0;
      $scope.saturation = 0;
      $scope.lumiere = 0;
    }
  }
  //-- ferme la modale Ok
  $scope.confirm = function () {
    if (key && $scope.currentColor) { // y a une clé (et une couleur sélectionnée) on récupère lr contenu de la clé
      var keyValues = webStorage.get(key);
      if (!keyValues) { // elle existe pas on crée le tableau
        keyValues = [];
      }
      if (keyValues.indexOf($scope.currentColor) == -1) { // la couleur choisie est nouvelle
        keyValues.unshift($scope.currentColor);
        while (keyValues.length > 9) { // le tableau est trop grand on tronque
          keyValues.pop();
        }
        webStorage.add(key, keyValues); // tableau modifié : on le sauve
      }
    }
    close($scope.currentColor, 5); // close, but give 500ms for animation
  };
  //-- ferme la modale Ko
  $scope.cancel = function () {
    close('x', 5); // close, but give 500ms for animation
  };

  //-- Load des couleurs personnalisées
  if (key) {
    var keyValues = webStorage.get(key);
    if (keyValues) {
      $scope.customColors = keyValues;
    }
  }
  //-- debut de la page
  $timeout(function () {
    $scope.updateInProgress = false;
    $scope.colorOrigine = color;
    $scope.colorOrigineEmpty = color == null || color == '';
    initColors();
    updateValuesFromColor($scope.colorOrigine);
    $scope.$watch('currentR', $scope.changeRGB);
    $scope.$watch('currentG', $scope.changeRGB);
    $scope.$watch('currentB', $scope.changeRGB);
    draw();
    draw2($scope.clV);
  }, 250);
  $scope.position = { left: Math.max(0, Number(position.left) + Number(position.width) - 171), top: Number(position.top) + Number(position.height) };
}]);
//-- Controlleurs ------------------------------------------------------------------------------------

//-- Modules ------------------------------------------------------------------------------------------------------
//-- Burger Menu 
angular.module('burger-menu', [])
    .run(['$rootScope', '$bgMenu', function ($rootScope, $bgMenu) {
      $rootScope.$bgMenu = $bgMenu;
    }])
    .provider("$bgMenu", function () {
      this.$get = [function () {
        var menu = {};
        menu.show = function show() {
          var menu = angular.element(document.querySelector('#bg-nav'));
          menu.addClass('show');
          menu = angular.element(document.querySelector('#bg-masker'));
          menu.addClass('show');
          var bt = angular.element(document.querySelector('#bg-bt-open'));
          if (bt) bt.addClass('show');
          bt = angular.element(document.querySelector('#bg-bt-close'));
          if (bt) bt.addClass('show');
        };
        menu.hide = function hide() {
          var menu = angular.element(document.querySelector('#bg-nav'));
          menu.removeClass('show');
          menu = angular.element(document.querySelector('#bg-masker'));
          menu.removeClass('show');
          var bt = angular.element(document.querySelector('#bg-bt-open'));
          if (bt) bt.removeClass('show');
          bt = angular.element(document.querySelector('#bg-bt-close'));
          if (bt) bt.removeClass('show');
        };
        menu.toggle = function toggle() {
          var menu = angular.element(document.querySelector('#bg-nav'));
          menu.toggleClass('show');
          menu = angular.element(document.querySelector('#bg-masker'));
          menu.toggleClass('show');
          var bt = angular.element(document.querySelector('#bg-bt-open'));
          if (bt) bt.toggleClass('show');
          bt = angular.element(document.querySelector('#bg-bt-close'));
          if (bt) bt.toggleClass('show');
        };
        return menu;
      }];
    });

//-- Caddy 
angular.module('caddy', [])
    .run(['$rootScope', '$caddy', function ($rootScope, $caddy) {
      $rootScope.$caddy = $caddy;
    }])
    .provider("$caddy", function () {
      this.$get = [function () {
        var cad = {};
        cad.show = function show() {
          var m = angular.element(document.querySelector('#cd-nav'));
          m.addClass('show');
          m = angular.element(document.querySelector('#cd-masker'));
          m.addClass('show');
        };
        cad.hide = function hide() {
          var m = angular.element(document.querySelector('#cd-nav'));
          m.removeClass('show');
          m = angular.element(document.querySelector('#cd-masker'));
          m.removeClass('show');
        };
        cad.toggle = function toggle() {
          var m = angular.element(document.querySelector('#cd-nav'));
          m.toggleClass('show');
          m = angular.element(document.querySelector('#cd-masker'));
          m.toggleClass('show');
        };
        return cad;
      }];
    });
//-- Modules ------------------------------------------------------------------------------------------------------

//-- Completion des fonctions dates -------------------------------------------------------------------------------
Date.isLeapYear = function (year) {
  return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
};
Date.getDaysInMonth = function (year, month) {
  return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
};
Date.parseJson = function (d) {
  if (d) {
    if (typeof d.getMonth !== 'function') {
      if (d.indexOf('/Date(') == 0) { // gère le format du type : /Date(1433282277027+0200)/
        var x = d.substring(6, d.length - 2);
        var q = x.substring(0, x.indexOf('+'));
        var d = new Date(Number(q));
      }
      else if (!isNaN(d)) { // Au cas ou ??
        var x = new Date(d)
        if (x != 'Invalid Date') {
          d = x;
        }
      }
      return d;
    } else { // c'est une date
      return d;
    }
  } else {
    return null;
  }
}
Date.parseFrenchShort = function (value) {
  if (value) { // c'est rempli
    if (typeof value.getMonth === 'function') { // c'est déjà une date
      return value;
    } else if (typeof value.match === 'function') { // c'est bien une string
      var matches = value.match(/^(\d{2})\/(\d{2})\/(\d{4})( (\d{2}):(\d{2}))?$/);
      if (matches === null) { // invalide
        return null;
      } else {
        var year = parseInt(matches[3], 10);
        var month = parseInt(matches[2], 10) - 1; // les mois vont de 0 à 11
        if (month < 0 || month > 11) { // mois invalide
          return null;
        }
        var day = parseInt(matches[1], 10);
        if (day < 0 || day > 31) { // jour invalide
          return null;
        }
        var hour = matches[5] != undefined ? parseInt(matches[5], 10) : 0;
        if (hour < 0 || hour > 23) { // heure invalide
          return null;;
        }
        var minute = matches[6] != undefined ? parseInt(matches[6], 10) : 0;
        if (minute < 0 || minute > 59) { // minnte invalide
          return null;
        }
        var date = new Date(year, month, day, hour, minute);
        if (date.getFullYear() === year
            || date.getMonth() === month
            || date.getDate() === day
            || date.getHours() === hour
            || date.getMinutes() === minute) {
          return date;
        }
      }
    }
  }
  return null;
}
Date.prototype.isLeapYear = function () {
  return Date.isLeapYear(this.getFullYear());
};
Date.prototype.getDaysInMonth = function () {
  return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
};
Date.prototype.addDays = function (days) {
  this.setDate(this.getDate() + days * 1);
  return this;
}
Date.prototype.addMonths = function (value) {
  var n = this.getDate();
  this.setDate(1);
  this.setMonth(this.getMonth() + value);
  this.setDate(Math.min(n, this.getDaysInMonth()));
  return this;
};
Date.prototype.clone = function () {
  var dt = new Date(this.valueOf());
  dt.setHours(0);
  dt.setMinutes(0);
  dt.setSeconds(0);
  dt.setMilliseconds(0);
  return dt;
}

//-- Completion des fonctions dates -------------------------------------------------------------------------------
var utilisateur = function (object) {
  if (object) {
    for (var i in object) {
      this[i] = object[i];
    }
    this.isInRole = function (x) {
      if (this.menus && this.menus.length) {
        for (var i = 0; i < this.menus.length; i++) {
          if (this.menus[i] && this.menus[i].cle == x) {
            return this.menus[i].accesInt;
          }
        }
      }
      return 0;
    };
  }
}
