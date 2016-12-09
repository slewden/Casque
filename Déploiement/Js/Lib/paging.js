//--- Pagination directive -----------------------------------------------------------
angular.module('no.paging', []).directive('noPaging', function () {
  //---------------------------------------------------------------------------------
  //-- Assign default scope values from settings
  //-- Feel free to tweak / fork these for your application
  //-- @param {Object} scope - The local directive scope object
  //-- @param {Object} attrs - The local directive attribute object
  function setScopeValues(scope, attrs) {
    scope.List = [];
    scope.Hide = false;
    scope.dots = scope.dots || '...';
    scope.page = parseInt(scope.page) || 1;
    scope.total = parseInt(scope.total) || 0;
    scope.ulClass = scope.ulClass || 'pagination';
    scope.adjacent = parseInt(scope.adjacent) || 2;
    scope.activeClass = scope.activeClass || 'active';
    scope.disabledClass = scope.disabledClass || 'disabled';
    scope.hideIfEmpty = scope.$eval(attrs.hideIfEmpty);
    scope.showPrevNext = scope.$eval(attrs.showPrevNext);
    scope.titleFirst = scope.titleFirst || 'First Page';
    scope.titlePrevious = scope.titlePrevious || 'Previous Page';
    scope.titleNext = scope.titleNext || 'Next Page';
    scope.titleLast = scope.titleLast || 'Last Page';
    scope.titlePage = scope.titlePage || 'Page ';
  }
  //---------------------------------------------------------------------------------
  //-- Validate and clean up any scope values
  //-- This happens after we have set the scope values
  //--  @param {Object} scope - The local directive scope object
  //--  @param {int} pageCount - The last page number or total page count 
  function validateScopeValues(scope, pageCount) {
    if (scope.page > pageCount) { // Block where the page is larger than the pageCount
      scope.page = pageCount;
    }
    if (scope.page <= 0) { // Block where the page is less than 0
      scope.page = 1;
    }
    if (scope.adjacent <= 0) { // Block where adjacent value is 0 or below
      scope.adjacent = 2;
    }
    if (pageCount <= 1) { // Hide from page if we have 1 or less pages if directed to hide empty
      scope.Hide = scope.hideIfEmpty;
    }
  }
  //---------------------------------------------------------------------------------
  //-- Assign the method action to take when a page is clicked
  //-- @param {Object} scope - The local directive scope object
  //-- @param {int} page - The current page of interest
  function internalAction(scope, page) {
    if (scope.page == page) { // Block clicks we try to load the active page
      return;
    }
    // Update the page in scope 
    scope.page = page;
    // Pass our parameters to the paging action
    scope.pagingAction({
      page: scope.page,
      pageSize: scope.pageSize,
      total: scope.total
    });
  }
  //---------------------------------------------------------------------------------
  //-- Add the first, previous, next, and last buttons if desired   
  //-- The logic is defined by the mode of interest
  //-- This method will simply return if the scope.showPrevNext is false
  //-- or if there are no pages to display
  //-- @param {Object} scope - The local directive scope object
  //-- @param {int} pageCount - The last page number or total page count
  //-- @param {string} mode - The mode of interest either prev or last 
  function addPrevNext(scope, pageCount, mode) {
    if (!scope.showPrevNext || pageCount < 1) { // Ignore if we are not showing
      return;                                   // or there are no pages to display
    }
    // Local variables to help determine logic
    var disabled, alpha, beta;
    // Determine logic based on the mode of interest
    // Calculate the previous / next page and if the click actions are allowed
    if (mode === 'prev') {
      disabled = scope.page - 1 <= 0;
      var prevPage = scope.page - 1 <= 0 ? 1 : scope.page - 1;
      alpha = { value: "<<", title: scope.titleFirst, page: 1 };
      beta = { value: "<", title: scope.titlePrevious, page: prevPage };
    } else { // Next
      disabled = scope.page + 1 > pageCount;
      var nextPage = scope.page + 1 >= pageCount ? pageCount : scope.page + 1;
      alpha = { value: ">", title: scope.titleNext, page: nextPage };
      beta = { value: ">>", title: scope.titleLast, page: pageCount };
    }
    var alphaItem = {    // Build the first list item
      value: alpha.value,
      title: alpha.title,
      liClass: disabled ? scope.disabledClass : '',
      action: function () {
        if (!disabled) {
          internalAction(scope, alpha.page);
        }
      }
    };
    var betaItem = {    // Build the second list item
      value: beta.value,
      title: beta.title,
      liClass: disabled ? scope.disabledClass : '',
      action: function () {
        if (!disabled) {
          internalAction(scope, beta.page);
        }
      }
    };
    // Add the items
    scope.List.push(alphaItem);
    scope.List.push(betaItem);
  }
  //---------------------------------------------------------------------------------
  //-- Adds a range of numbers to our list 
  //-- The range is dependent on the start and finish parameters
  //-- @param {int} start - The start of the range to add to the paging list
  //-- @param {int} finish - The end of the range to add to the paging list 
  //-- @param {Object} scope - The local directive scope object
  function addRange(start, finish, scope) {
    var i = 0;
    for (i = start; i <= finish; i++) {
      var item = {
        value: i,
        title: scope.titlePage + ' ' + i,
        liClass: scope.page == i ? scope.activeClass : '',
        action: function () {
          internalAction(scope, this.value);
        }
      };
      scope.List.push(item);
    }
  }
  //---------------------------------------------------------------------------------
  //-- Add Dots ie: 1 2 [...] 10 11 12 [...] 56 57
  //-- This is my favorite function not going to lie
  //--  @param {Object} scope - The local directive scope object
  function addDots(scope) {
    scope.List.push({ value: scope.dots });
  }
  //---------------------------------------------------------------------------------
  //-- Add the first or beginning items in our paging list  
  //-- We leverage the 'next' parameter to determine if the dots are required
  //-- @param {Object} scope - The local directive scope object
  //-- @param {int} next - the next page number in the paging sequence
  function addFirst(scope, next) {
    addRange(1, 2, scope);
    // We ignore dots if the next value is 3
    // ie: 1 2 [...] 3 4 5 becomes just 1 2 3 4 5 
    if (next != 3) {
      addDots(scope);
    }
  }
  //---------------------------------------------------------------------------------
  //-- Add the last or end items in our paging list  
  //-- We leverage the 'prev' parameter to determine if the dots are required
  //-- @param {int} pageCount - The last page number or total page count 
  //-- @param {Object} scope - The local directive scope object
  //-- @param {int} prev - the previous page number in the paging sequence
  function addLast(pageCount, scope, prev) {
    // We ignore dots if the previous value is one less that our start range
    // ie: 1 2 3 4 [...] 5 6  becomes just 1 2 3 4 5 6 
    if (prev != pageCount - 2) {
      addDots(scope);
    }
    addRange(pageCount - 1, pageCount, scope);
  }
  //---------------------------------------------------------------------------------
  //-- The main build function used to determine the paging logic
  //-- Feel free to tweak / fork values for your application
  //-- @param {Object} scope - The local directive scope object
  //-- @param {Object} attrs - The local directive attribute object
  function build(scope, attrs) {
    if (!scope.pageSize || scope.pageSize <= 0) { // Block divide by 0 and empty page size
      scope.pageSize = 1;
    }
    // Determine the last page or total page count
    var pageCount = Math.ceil(scope.total / scope.pageSize);
    // Set the default scope values where needed
    setScopeValues(scope, attrs);
    // Validate the scope values to protect against strange states
    validateScopeValues(scope, pageCount);
    // Create the beginning and end page values 
    var start, finish;
    // Calculate the full adjacency value 
    var fullAdjacentSize = (scope.adjacent * 2) + 2;
    // Add the Next and Previous buttons to our list
    addPrevNext(scope, pageCount, 'prev');
    // If the page count is less than the full adjacnet size
    // Then we simply display all the pages, Otherwise we calculate the proper paging display
    if (pageCount <= (fullAdjacentSize + 2)) {
      start = 1;
      addRange(start, pageCount, scope);
    } else {
      // Determine if we are showing the beginning of the paging list 
      // We know it is the beginning if the page - adjacent is <= 2
      if (scope.page - scope.adjacent <= 2) {
        start = 1;
        finish = 1 + fullAdjacentSize;
        addRange(start, finish, scope);
        addLast(pageCount, scope, finish);
      }
        // Determine if we are showing the middle of the paging list
        // We know we are either in the middle or at the end since the beginning is ruled out above
        // So we simply check if we are not at the end 
        // Again 2 is hard coded as we always display two pages after the dots
      else if (scope.page < pageCount - (scope.adjacent + 2)) {
        start = scope.page - scope.adjacent;
        finish = scope.page + scope.adjacent;
        addFirst(scope, start);
        addRange(start, finish, scope);
        addLast(pageCount, scope, finish);
      }
        // If nothing else we conclude we are at the end of the paging list
        // We know this since we have already ruled out the beginning and middle above
      else {
        start = pageCount - fullAdjacentSize;
        finish = pageCount;
        addFirst(scope, start);
        addRange(start, finish, scope);
      }
    }
    // Add the next and last buttons to our paging list
    addPrevNext(scope, pageCount, 'next');
  }
  //---------------------------------------------------------------------------------
  //-- The angular return value required for the directive
  //-- Feel free to tweak / fork values for your application
  return {
    restrict: 'EA',    // Restrict to elements and attributes
    scope: {           // Assign the angular scope attribute formatting
      page: '=',                   // Current page
      pageSize: '=',               // Elements by page
      total: '=',                  // total of elements
      dots: '@',                   // symbol for empty block
      hideIfEmpty: '@',            // true : false
      ulClass: '@',                // Total bloc Css class. Default : pagination
      activeClass: '@',            // Current Page Css class. Default : active
      disabledClass: '@',          // Inactive Page Css classe. Default : disabled
      adjacent: '@',               // split factor
      showPrevNext: '@',           // display First Prev / Next Last
      pagingAction: '&',           // event function called when page changes
      titleFirst: '@',             // Title for Page first. default 'First Page'
      titlePrevious: '@',          // Title for Page Prev. default 'Previous Page'
      titlePage: '@',              // Title for other pages + PageIndex. default 'Page ' + PageIndex
      titleNext: '@',              // Title for Page Next. default 'Next Page'
      titleLast: '@'               // Title for Page Last. default 'Last Page'
    },
    template:           // Assign the angular directive template HTML
        '<ul ng-hide="Hide" ng-class="ulClass"> ' +
            '<li ' +
                'title="{{Item.title}}" ' +
                'ng-class="Item.liClass" ' +
                'ng-click="Item.action()" ' +
                'ng-repeat="Item in List"> ' +
                    '<span ng-bind="Item.value"></span> ' +
            '</li>' +
        '</ul>',
    // Link the directive to enable our scope watch values
    link: function (scope, element, attrs) {
      // Hook in our watched items 
      scope.$watchCollection('[page,pageSize,total]', function () {
        build(scope, attrs);
      });
    }
  };
});
