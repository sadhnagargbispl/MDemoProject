<!DOCTYPE html>
<html lang="en">

  <head>
    <meta charset="UTF-8">
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, shrink-to-fit=no" name="viewport">
    <title>Welcome to Admin Dashboard</title>

    <!-- General CSS Files -->
    <link rel="stylesheet" href="assets/css/app.min.css">
    <link rel="stylesheet" href="assets/css/style.css">
    <link rel="stylesheet" href="assets/css/components.css">
    <link rel="stylesheet" href="assets/bundles/jqvmap/dist/jqvmap.min.css">
    <link rel="stylesheet" href="assets/css/custom.css">
  </head>

  <body>

    <div id="app">
      <div class="main-wrapper main-wrapper-1">
        <div class="navbar-bg"></div>
        <nav class="navbar navbar-expand-lg main-navbar sticky">
          <div class="form-inline me-auto">
            <ul class="navbar-nav mr-3">
              <li>
                <a href="#" data-bs-toggle="sidebar" class="nav-link nav-link-lg collapse-btn"> <i data-feather="menu"></i></a>
              </li>
              <li>
                <form class="form-inline me-auto">
                  <div class="search-element d-flex">
                    <input class="form-control" type="search" placeholder="Search" aria-label="Search">
                    <button class="btn" type="submit">
                      <i class="fas fa-search"></i>
                    </button>
                  </div>
                </form>
              </li>
            </ul>
          </div>
          <ul class="navbar-nav navbar-right">
            <li>
              <a href="#" class="nav-link nav-link-lg fullscreen-btn">
                <i data-feather="maximize"></i>
              </a>
            </li>

            <li class="dropdown dropdown-list-toggle">
              <a href="#" data-bs-toggle="dropdown" class="nav-link nav-link-lg message-toggle">
                <i data-feather="mail" class="mailAnim"></i>
                <span class="badge headerBadge1"> </span> 
              </a>
              <div class="dropdown-menu dropdown-list dropdown-menu-right pullDown">
                <div class="dropdown-header"> Messages
                  <div class="float-right">
                    <a href="#">Mark All As Read</a>
                  </div>
                </div>
                <div class="dropdown-list-content dropdown-list-message">
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-avatar text-white"> 
                      <img alt="image" src="assets/img/users/user-1.png" class="rounded-circle">
                    </span> 
                    <span class="dropdown-item-desc"> 
                      <span class="message-user">John  Deo</span>
                      <span class="time messege-text">Please check your mail !!</span>
                      <span class="time">2 Min Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-avatar text-white">
                      <img alt="image" src="assets/img/users/user-2.png" class="rounded-circle">
                    </span> 
                    <span class="dropdown-item-desc"> <span class="message-user">Sarah Smith</span> 
                      <span class="time messege-text">Request for leave application</span>
                      <span class="time">5 Min Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-avatar text-white">
                      <img alt="image" src="assets/img/users/user-5.png" class="rounded-circle">
                    </span> 
                    <span class="dropdown-item-desc"> <span class="message-user">Jacob Ryan</span> 
                        <span class="time messege-text">Your payment invoice is generated.</span> 
                        <span class="time">12 Min Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-avatar text-white">
                      <img alt="image" src="assets/img/users/user-4.png" class="rounded-circle">
                    </span> 
                    <span class="dropdown-item-desc"> 
                      <span class="message-user">Lina Smith</span> 
                        <span class="time messege-text">hii John, I have upload doc related to task.</span> 
                        <span class="time">30 Min Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-avatar text-white">
                      <img alt="image" src="assets/img/users/user-3.png" class="rounded-circle">
                    </span> 
                    <span class="dropdown-item-desc"> 
                      <span class="message-user">Jalpa Joshi</span> 
                      <span class="time messege-text">Please do as specify. Let me know if you have any query.</span> 
                      <span class="time">1 Days Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-avatar text-white">
                      <img alt="image" src="assets/img/users/user-2.png" class="rounded-circle">
                    </span> 
                    <span class="dropdown-item-desc"> 
                      <span class="message-user">Sarah Smith</span> 
                      <span class="time messege-text">Client Requirements</span>
                      <span class="time">2 Days Ago</span>
                    </span>
                  </a>
                </div>
                <div class="dropdown-footer text-center">
                  <a href="#">View All <i class="fas fa-chevron-right"></i></a>
                </div>
              </div>
            </li>

            <li class="dropdown dropdown-list-toggle">
              <a href="#" data-bs-toggle="dropdown" class="nav-link notification-toggle nav-link-lg"><i data-feather="bell"></i> </a>
              <div class="dropdown-menu dropdown-list dropdown-menu-right pullDown">
                <div class="dropdown-header"> Notifications
                  <div class="float-right">
                    <a href="#">Mark All As Read</a>
                  </div>
                </div>
                <div class="dropdown-list-content dropdown-list-icons">
                  <a href="#" class="dropdown-item dropdown-item-unread"> 
                    <span class="dropdown-item-icon l-bg-orange text-white"> <i class="far fa-envelope"></i> </span> 
                    <span class="dropdown-item-desc"> You got new mail, please check. 
                      <span class="time">2 Min Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-icon l-bg-purple text-white"> <i
                        class="fas fa-bell"></i>
                    </span> 
                    <span class="dropdown-item-desc"> Meeting with <b>John Deo</b> and <b>Sarah Smith </b> at 10:30 am 
                      <span class="time">10 Hours Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-icon l-bg-green text-white"> <i class="far fa-check-circle"></i> </span> 
                    <span class="dropdown-item-desc"> Sidebar Bug is fixed by Kevin 
                      <span class="time">12 Hours Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-icon bg-danger text-white"> <i class="fas fa-exclamation-triangle"></i> </span> 
                    <span class="dropdown-item-desc"> Low disk space error!!!. 
                      <span class="time">17 Hours Ago</span>
                    </span>
                  </a> 
                  
                  <a href="#" class="dropdown-item"> 
                    <span class="dropdown-item-icon bg-info text-white"> <i class="fas fa-bell"></i> </span> 
                    <span class="dropdown-item-desc"> Welcome! 
                      <span class="time">Yesterday</span>
                    </span>
                  </a>
                </div>

                <div class="dropdown-footer text-center">
                <a href="#">View All <i class="fas fa-chevron-right"></i></a>
                </div>
              </div>
            </li>

            <li class="dropdown">
              <a href="#" data-bs-toggle="dropdown" class="nav-link dropdown-toggle nav-link-lg nav-link-user"> 
                <img alt="image" src="assets/img/user.png" class="user-img-radious-style"> 
                <span class="d-sm-none d-lg-inline-block"></span>
              </a>
              <div class="dropdown-menu dropdown-menu-right pullDown">
                <div class="dropdown-title">Hello, 99Plot User</div>
                <a href="#" class="dropdown-item has-icon"> <i class="far fa-user"></i> Profile </a> 
                <a href="#" class="dropdown-item has-icon"> <i class="fas fa-bolt"></i> Activities </a> 
                <a href="#" class="dropdown-item has-icon"> <i class="fas fa-cog"></i> Settings </a>
                <div class="dropdown-divider"></div>
                <a href="#" class="dropdown-item has-icon text-danger"> <i class="fas fa-sign-out-alt"></i> Logout </a>
              </div>
            </li>
          </ul>
        </nav>

        <div class="main-sidebar sidebar-style-2">
          <aside id="sidebar-wrapper">
            <div class="sidebar-brand">
              <a href="#"> <img alt="image" src="../panel-images/demo_logo.png" class="header-logo" /> </a>
            </div>
            <div class="sidebar-user">
              <div class="sidebar-user-picture">
                <img alt="image" src="assets/img/user.png">
              </div>
              <div class="sidebar-user-details">
                <div class="user-name">Sample text here</div>
                <div class="user-role">Administrator</div>
                <div class="sidebar-userpic-btn">
                  <a href="#" data-bs-toggle="tooltip" title="Profile"> <i data-feather="user"></i> </a>
                  <a href="#" data-bs-toggle="tooltip" title="Mail"> <i data-feather="mail"></i> </a>
                  <a href="#" data-bs-toggle="tooltip" title="Chat"> <i data-feather="message-square"></i> </a>
                  <a href="#" data-bs-toggle="tooltip" title="Log Out"> <i data-feather="log-out"></i> </a>
                </div>
              </div>
            </div>
            <ul class="sidebar-menu">
              <li class="dropdown active">
                <a href="#" class="menu-toggle nav-link has-dropdown"><i data-feather="monitor"></i> <span>Dashboard</span></a>
                <ul class="dropdown-menu">
                  <li><a class="nav-link" href="#">Dashboard 1</a></li>
                  <li><a class="nav-link" href="#">Dashboard 2</a></li>
                </ul>
              </li>

              <li class="dropdown">
                <a href="#" class="menu-toggle nav-link has-dropdown"><i data-feather="monitor"></i><span>Admin Menu</span></a> 
                <ul class="dropdown-menu">
                  <li><a class="nav-link" href="#">Sub Menu</a></li>
                  <li><a class="nav-link" href="#">Sub Menu</a></li>
                </ul>
              </li>

              <li class="dropdown"><a href="#" class="nav-link"><i data-feather="monitor"></i><span>Calendar</span></a></li>

              <li class="dropdown"><a href="#" class="nav-link"><i data-feather="monitor"></i><span>Task</span></a></li>
            </ul>
          </aside>
        </div>

        <div class="main-content">
          <section class="section">
            <ul class="breadcrumb breadcrumb-style ">
              <li class="breadcrumb-item"><h4 class="page-title m-b-0">Dashboard</h4></li>
              <li class="breadcrumb-item">
                <a href="#"> <i data-feather="home"></i></a>
              </li>
              <li class="breadcrumb-item active">Dashboard</li>
            </ul>
            

            <div class="row">
              <div class="col-12 col-sm-12 col-lg-12">
                <div class="card">
                  <div class="card-header">
                    <h4>Referral Link </h4>
                  </div>
                  
                  <div class="card-body">
                   
                    <p> Sample Text here.. </p>

                    <form action="#" method="post" enctype="multipart/form-data" class="row g-3">
                        <div class="col-md-6">
                            <label for="textInput" class="form-label">Text</label>
                            <input id="textInput" name="textInput" type="text" class="form-control" placeholder="Plain text">
                        </div>

                        <div class="col-md-6">
                            <label for="passwordInput" class="form-label">Password</label>
                            <input id="passwordInput" name="passwordInput" type="password" class="form-control" placeholder="Password">
                        </div>

                        <div class="col-md-6">
                            <label for="emailInput" class="form-label">Email</label>
                            <input id="emailInput" name="emailInput" type="email" class="form-control" placeholder="name@example.com">
                        </div>

                        <div class="col-md-6">
                            <label for="searchInput" class="form-label">Search</label>
                            <input id="searchInput" name="searchInput" type="search" class="form-control" placeholder="Search...">
                        </div>

                        <div class="col-md-6">
                            <label for="telInput" class="form-label">Telephone</label>
                            <input id="telInput" name="telInput" type="tel" class="form-control" placeholder="+1234567890">
                        </div>

                        <div class="col-md-6">
                            <label for="urlInput" class="form-label">URL</label>
                            <input id="urlInput" name="urlInput" type="url" class="form-control" placeholder="https://example.com">
                        </div>

                        <div class="col-md-4">
                            <label for="numberInput" class="form-label">Number</label>
                            <input id="numberInput" name="numberInput" type="number" class="form-control" min="0" max="100" step="1" value="10">
                        </div>

                        <div class="col-md-4">
                            <label for="rangeInput" class="form-label">Range</label>
                            <input id="rangeInput" name="rangeInput" type="range" class="form-range" min="0" max="100" value="25">
                        </div>

                        <div class="col-md-4">
                            <label for="colorInput" class="form-label">Color</label>
                            <input id="colorInput" name="colorInput" type="color" class="form-control form-control-color" value="#0d6efd">
                        </div>

                        <div class="col-md-4">
                            <label for="dateInput" class="form-label">Date</label>
                            <input id="dateInput" name="dateInput" type="date" class="form-control">
                        </div>

                        <div class="col-md-4">
                            <label for="timeInput" class="form-label">Time</label>
                            <input id="timeInput" name="timeInput" type="time" class="form-control">
                        </div>

                        <div class="col-md-4">
                            <label for="datetimeInput" class="form-label">Date & Time</label>
                            <input id="datetimeInput" name="datetimeInput" type="datetime-local" class="form-control">
                        </div>

                        <div class="col-md-4">
                            <label for="monthInput" class="form-label">Month</label>
                            <input id="monthInput" name="monthInput" type="month" class="form-control">
                        </div>

                        <div class="col-md-4">
                            <label for="weekInput" class="form-label">Week</label>
                            <input id="weekInput" name="weekInput" type="week" class="form-control">
                        </div>

                        <div class="col-12">
                            <label class="form-label">Checkboxes</label>
                            <div class="form-check">
                                <input id="check1" name="checks[]" class="form-check-input" type="checkbox" value="option1" checked>
                                <label class="form-check-label" for="check1">Option 1</label>
                            </div>
                            <div class="form-check">
                                <input id="check2" name="checks[]" class="form-check-input" type="checkbox" value="option2">
                                <label class="form-check-label" for="check2">Option 2</label>
                            </div>
                        </div>

                        <div class="col-12">
                            <label class="form-label">Radio Buttons</label>
                            <div class="form-check">
                                <input id="radioA" name="radioGroup" class="form-check-input" type="radio" value="A" checked>
                                <label class="form-check-label" for="radioA">A</label>
                            </div>
                            <div class="form-check">
                                <input id="radioB" name="radioGroup" class="form-check-input" type="radio" value="B">
                                <label class="form-check-label" for="radioB">B</label>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <label for="fileInput" class="form-label">File</label>
                            <input id="fileInput" name="fileInput" type="file" class="form-control">
                        </div>

                        <div class="col-md-6">
                            <label for="selectInput" class="form-label">Select</label>
                            <select id="selectInput" name="selectInput" class="form-select">
                                <option value="">Choose...</option>
                                <option value="1" selected>One</option>
                                <option value="2">Two</option>
                                <option value="3">Three</option>
                            </select>
                        </div>

                        <div class="col-12">
                            <label for="textareaInput" class="form-label">Textarea</label>
                            <textarea id="textareaInput" name="textareaInput" class="form-control" rows="3" placeholder="Write something..."></textarea>
                        </div>

                        <input type="hidden" name="hiddenField" value="hiddenValue">

                        <div class="col-12 d-flex gap-2">
                            <button type="submit" class="btn btn-primary">Submit</button>
                            <button type="reset" class="btn btn-secondary">Reset</button>
                            <button type="button" class="btn btn-light">Button</button>
                        </div>
                    </form>
                    

                  </div>
                </div>
              </div>               
            </div>  
          </section>

          <div class="settingSidebar">
            <a href="javascript:void(0)" class="settingPanelToggle"> <i class="fa fa-spin fa-cog"></i> </a>
            <div class="settingSidebar-body ps-container ps-theme-default">
              <div class=" fade show active">
                <div class="setting-panel-header">Setting Panel </div>
                <div class="p-15 border-bottom">
                  <h6 class="font-medium m-b-10">Select Layout</h6>
                  <div class="selectgroup layout-color w-50">
                    <label class="selectgroup-item">
                      <input type="radio" name="value" value="1" class="selectgroup-input-radio select-layout" checked>
                      <span class="selectgroup-button">Light</span>
                    </label>
                    <label class="selectgroup-item">
                      <input type="radio" name="value" value="2" class="selectgroup-input-radio select-layout">
                      <span class="selectgroup-button">Dark</span>
                    </label>
                  </div>
                </div>

                <div class="p-15 border-bottom">
                  <h6 class="font-medium m-b-10">Sidebar Color</h6>
                  <div class="selectgroup selectgroup-pills sidebar-color">
                    <label class="selectgroup-item">
                      <input type="radio" name="icon-input" value="1" class="selectgroup-input select-sidebar">
                      <span class="selectgroup-button selectgroup-button-icon" data-bs-toggle="tooltip"  data-original-title="Light Sidebar"><i class="fas fa-sun"></i></span>
                    </label>
                    <label class="selectgroup-item">
                      <input type="radio" name="icon-input" value="2" class="selectgroup-input select-sidebar" checked>
                      <span class="selectgroup-button selectgroup-button-icon" data-bs-toggle="tooltip" data-original-title="Dark Sidebar"><i class="fas fa-moon"></i></span>
                    </label>
                  </div>
                </div>

                <div class="p-15 border-bottom">
                  <h6 class="font-medium m-b-10">Color Theme</h6>
                  <div class="theme-setting-options">
                    <ul class="choose-theme list-unstyled mb-0">
                      <li title="white" class="active">
                        <div class="white"></div>
                      </li>
                      <li title="cyan">
                        <div class="cyan"></div>
                      </li>
                      <li title="black">
                        <div class="black"></div>
                      </li>
                      <li title="purple">
                        <div class="purple"></div>
                      </li>
                      <li title="orange">
                        <div class="orange"></div>
                      </li>
                      <li title="green">
                        <div class="green"></div>
                      </li>
                      <li title="red">
                        <div class="red"></div>
                      </li>
                    </ul>
                  </div>
                </div>

                <div class="p-15 border-bottom">
                  <div class="theme-setting-options">
                    <label class="m-b-0">
                      <input type="checkbox" name="custom-switch-checkbox" class="custom-switch-input" id="mini_sidebar_setting">
                      <span class="custom-switch-indicator"></span>
                      <span class="control-label p-l-10">Mini Sidebar</span>
                    </label>
                  </div>
                </div>

                <div class="p-15 border-bottom">
                  <div class="theme-setting-options">
                    <label class="m-b-0">
                      <input type="checkbox" name="custom-switch-checkbox" class="custom-switch-input" id="sticky_header_setting">
                      <span class="custom-switch-indicator"></span>
                      <span class="control-label p-l-10">Sticky Header</span>
                    </label>
                  </div>
                </div>

                <div class="mt-4 mb-4 p-3 align-center rt-sidebar-last-ele">
                  <a href="#" class="btn btn-icon icon-left btn-primary btn-restore-theme">
                    <i class="fas fa-undo"></i> Restore Default
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>

        <footer class="main-footer">
          <div class="footer-left"> Copyright &copy; 2025 </div>
          <div class="footer-right"> </div>
        </footer>
      </div>
    </div>

    <!-- General JS Scripts -->
    <script src="assets/js/app.min.js"></script>
    <script src="assets/bundles/apexcharts/apexcharts.min.js"></script>
    <script src="assets/bundles/jqvmap/dist/jquery.vmap.min.js"></script>
    <script src="assets/bundles/jqvmap/dist/maps/jquery.vmap.world.js"></script>
    <script src="assets/js/page/index.js"></script>
    <script src="assets/js/scripts.js"></script>
    <script src="assets/js/custom.js"></script>

    <!-- <script src="../disable.js"></script> -->
  </body>
</html>