angular.module("eventsapp").run(["$templateCache", function ($templateCache) {
    $templateCache.put("templates/footer.html", '<span class="center text-center">Copyrights &copy; {{app.year}} - {{ $root.app | localizeString : "nameEn" : "nameAr" }}</span><br />' +
        '                    <a href="" target="http://hifive.ae/" class="center text-center">' +
        '                        <span class="center text-center">Powered By - Hifive Solutions FZC</span>' +
        '                    </a>');

    $templateCache.put("templates/sidebar.html", '<div ng-show="true" class="sidebar-wrapper">' +
'    <div ui-sidebar="" class="sidebar" ng-class="app.theme.topbar">' +
'        <div class="sidebar-nav">' +
'            <ul class="nav">' +
'                <li ui-sref-active="active">' +
'                    <a ui-sref="app.home" title="{{sidebar.nav.Home | translate}}" ripple="">' +
'                        <em class="sidebar-item-icon fa fa-home"></em>' +
'                        <span>{{"sidebar.nav.Home" | translate}}</span>' +
'                    </a>' +
'                </li>' +
'                <li ui-sref-active="active">' +
'                    <a ui-sref="app.event" title="{{sidebar.nav.Events | translate}}" ripple="">' +
'                        <em class="sidebar-item-icon fa fa-calendar"></em>' +
'                        <span>{{"sidebar.nav.Events" | translate}}</span>' +
'                    </a>' +
'                </li>' +
'                <li ui-sref-active="active">' +
'                    <a ui-sref="app.speakers" title="{{sidebar.nav.Speakers | translate}}" ripple="">' +
'                        <em class="sidebar-item-icon fa fa-bullhorn"></em>' +
'                        <span>{{"sidebar.nav.Speakers" | translate}}</span>' +
'                    </a>' +
'                </li>' +
'                <li ui-sref-active="active">' +
'                    <a ui-sref="app.exhibitors" title="{{sidebar.nav.Exhibitors | translate}}" ripple="">' +
'                        <em class="sidebar-item-icon fa fa-id-badge"></em>' +
'                        <span>{{"sidebar.nav.Exhibitors" | translate}}</span>' +
'                    </a>' +
'                </li>' +
'                <li ui-sref-active="active">' +
'                    <a ui-sref="app.sponsors" title="{{sidebar.nav.Sponsors | translate}}" ripple="">' +
'                        <em class="sidebar-item-icon fa fa-handshake-o"></em>' +
'                        <span>{{"sidebar.nav.Sponsors" | translate}}</span>' +
'                    </a>' +
'                </li>' +
'                <li ui-sref-active="active">' +
'                    <a ui-sref="app.photosVideos" title="{{sidebar.nav.Photos | translate}}" ripple="">' +
'                        <em class="sidebar-item-icon  fa fa-camera"></em>' +
'                        <span>{{"sidebar.nav.Photos" | translate}}</span>' +
'                    </a>' +
'                </li>' +
'                <li ui-sref-active="active">' +
    '                    <a ui-sref="app.register" title="{{sidebar.nav.eventReg | translate}}" ripple="">' +
    '                        <em class="sidebar-item-icon fa fa-edit"></em>' +
    '                        <span>{{"sidebar.nav.eventReg" | translate}}</span>' +
    '                    </a>' +
    '            </li>' +
    '                <li ui-sref-active="active">' +
    '                    <a ui-sref="app.SurveyAdmin" title="{{sidebar.nav.survey | translate}}" ripple="">' +
    '                        <em class="sidebar-item-icon fa fa-edit"></em>' +
    '                        <span>{{"sidebar.nav.survey" | translate}}</span>' +
    '                    </a>' +
    '            </li>' +
    '                <li ui-sref-active="active">' +
    '                    <a ui-sref="app.Questions" title="{{sidebar.nav.Questions | translate}}" ripple="">' +
    '                        <em class="sidebar-item-icon fa fa-edit"></em>' +
    '                        <span>{{"sidebar.nav.Questions" | translate}}</span>' +
    '                    </a>' +
    '            </li>' +
'            </ul>' +
'        </div>' +
'    </div>' +
'</div>');


    $templateCache.put("templates/top-navbar-dock.html", '<!-- START Top Navbar-->' +
'<nav role="navigation" ng-controller="HeaderNavController as header" class="navbar topnavbar">' +
'   <!-- START navbar header-->' +
'   <div ng-class="app.theme.brand" class="navbar-header">' +
'      <!-- Mobile buttons-->' +
'      <div class="mobile-toggles">' +
'         <!-- Button to show/hide the header menu on mobile. Visible on mobile only.-->' +
'         <a href="" ng-click="header.toggleHeaderMenu()" class="menu-toggle pull-left">' +
'            <em class="fa fa-navicon fa-fw"></em>' +
'         </a>' +
'      </div>' +
'   </div>' +
'   <!-- END navbar header-->' +
'   <!-- START Nav wrapper-->' +
'   <div uib-collapse="header.headerMenuCollapsed" class="nav-wrapper collapse navbar-collapse">' +
'      <!-- START Left navbar-->' +
'      <ul class="nav navbar-nav">' +
'         <li><a href="#/">Back</a>' +
'         </li>' +
'         <li uib-dropdown="" class="dropdown"><a href="" uib-dropdown-toggle="" class="dropdown-toggle">Dropdown</a>' +
'            <!-- START Dropdown menu-->' +
'            <ul class="dropdown-menu">' +
'               <!-- START list item-->' +
'               <li><a href="">Sub menu 1</a>' +
'               </li>' +
'               <li><a href="">Sub menu 2</a>' +
'               </li>' +
'               <li><a href="">Sub menu 3</a>' +
'               </li>' +
'            </ul>' +
'         </li>' +
'      </ul>' +
'      <!-- END Left navbar-->' +
'   </div>' +
'</nav>' +
'<!-- END Top Navbar-->');


    $templateCache.put("templates/top-navbar.html", '<!-- START Top Navbar-->' +
'<nav role="navigation" ng-controller="HeaderNavController as header" class="navbar topnavbar">' +
'    <!-- START navbar header-->' +
'    <div class="navbar-header">' +
'        <!-- Mobile buttons-->' +
'        <div class="mobile-toggles">' +
'            <!-- Button to show/hide the sidebar on mobile. Visible on mobile only.-->' +
'            <a href="" ng-click="app.sidebar.isOffscreen = !app.sidebar.isOffscreen" class="sidebar-toggle">' +
'                <em class="fa fa-navicon"></em>' +
'            </a>' +
'            <!-- Button to show/hide the header menu on mobile. Visible on mobile only.-->' +
'            <a href="" ng-click="header.toggleHeaderMenu()" class="menu-toggle hidden-material">' +
'                <em class="fa fa-ellipsis-v fa-fw"></em>' +
'            </a>' +
'        </div>' +
'    </div>' +
'    <!-- END navbar header-->' +
'    <!-- START Nav wrapper-->' +
'    <div uib-collapse="header.headerMenuCollapsed" class="nav-wrapper collapse navbar-collapse">' +
'        <!-- START Left navbar-->' +
'        <ul class="nav navbar-nav hidden-material">' +
'            <li>' +
'                <!-- Button used to collapse the left sidebar. Only visible on tablet and desktops-->' +
'                <a href="" ng-click="app.sidebar.isOffscreen = !app.sidebar.isOffscreen" class="hidden-xs">' +
'                    <em ng-class="app.sidebar.isOffscreen ? \'fa-caret-right\':\'fa-caret-left\'" class="fa"></em>' +
'                </a>' +
'            </li>' +
'            <!-- START profile screen-->' +
'            <li>' +
'                    <img class="media-object" style="padding-top: 16px;width: 40px; padding-bottom: 13px" src="app/img/image.png" />' +
'                    <span class="visible-xs-inline ml" data-translate="navbar.profile"></span>' +
'                ' +
'            </li>' +
'            <!-- END profile screen-->' +
'            <!-- START log out screen-->' +
'            <li>' +
'                <a ng-click="header.logOut()" title="{{\'navbar.signOut\' | translate}}" ripple="">' +
'                    <em class="fa fa-sign-out fa-fw"></em>' +
'                    <span class="visible-xs-inline ml" data-translate="navbar.signOut"></span>' +
'                </a>' +
'            </li>' +
'            <!-- END log out screen-->' +
'            <li>' +
'                <a title="Profile" ripple=""><em>{{header.user.firstName}} {{header.user.lastName}}</em></a>' +
'            </li>' +
'        </ul>' +
'        <!-- END Left navbar-->' +
'    </div>' +
'    <!-- END Nav wrapper-->' +
'</nav>' +
'<!-- END Top Navbar-->');
}]);