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
    <link rel="stylesheet" href="assets/css/new_custom.css">
    <link href="assets/css/admin-color.css" rel="stylesheet" type="text/css" />

     <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css">
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

            <li> <a href="#" class="nav-link nav-link-lg fullscreen-btn"> <i data-feather="maximize"></i> </a> </li>

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
              <li class="breadcrumb-item">
                <h4 class="page-title m-b-0">Dashboard</h4>
              </li>
              <li class="breadcrumb-item">
                <a href="#">
                  <i data-feather="home"></i>
                </a>
              </li>
              <li class="breadcrumb-item active">Dashboard</li>
            </ul>
           
            

              <div class="row mb-4">


            <div id="#" class="col-lg-4">
               <div class="card profile-card p-4">

    <!-- Profile Image -->
    <div class="text-center">
      <img src="assets/img/banner/bannerr.jpeg" class="profileimg img-fluid" style="max-width: 100%; height: auto;">
    </div>

    <!-- Name & Designation -->
    <div class="row mt-3 align-items-center">
      <div class="col-md-6">
        <p class="mb-1 text-success"><b>Name:</b></p>
        <h5 class="mb-0">Mr Kanhaiyalal Prajapat</h5>
      </div>

      <div class="col-md-6 text-md-right mt-2 mt-md-0">
        <p class="mb-1 text-success"><b> Designation : </b></p>
        <h6 class="mb-0 text-dark"> Global Supervisor </h6>
      </div>
    </div>

    <hr>

    <!-- ID + Sponsor -->
    <div class="row info-box text-center text-md-left">

      <div class="col-md-6 border-right">
        <p class="mb-1"><b>ID No:</b> MNCBP5575733</p>
        <p class="mb-0 text-primary">Date Of Joining: Mar 01, 2025</p>
      </div>

      <div class="col-md-6 mt-2 mt-md-0">
        <p class="mb-1 text-primary"><b>Sponsor</b></p>
        <p class="mb-0">Name: <b>Mr Chandu Kathat</b></p>
        <p class="mb-0">ID No: MNCBP3341973</p>
      </div>

    </div>

    <hr>

    <!-- Referral Links -->
<div class="text-center">
  <h6 class="mb-3">My Referral Links</h6>

  <div class="d-flex justify-content-center flex-wrap gap-2">

    <!-- Copy -->
    <button class="btn btn-copy">
      <i class="fa-solid fa-copy"></i> Copy Links
    </button>

    <!-- Facebook -->
    <button class="btn btn-fb">
      <i class="fa-brands fa-facebook-f"></i>
    </button>

    <!-- Twitter (X) -->
    <button class="btn btn-twitter">
      <i class="fa-brands fa-x-twitter"></i>
    </button>

    <!-- WhatsApp -->
    <button class="btn btn-whatsapp">
      <i class="fa-brands fa-whatsapp"></i>
    </button>

    <!-- Telegram -->
    <button class="btn btn-telegram">
      <i class="fa-brands fa-telegram"></i>
    </button>

  </div>
</div>

  </div>
            </div>




            <div class="col-lg-8">

              
        <div class="dashboard-cards">
          <div class="row gy-0">

            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
              <div class="card">                
                  <div class="card-body">
                   <span class="card-icon text-danger"> <i class="fa-solid fa-file"></i> </span>
                    <div>
                      <p class="card-title"> Agreement Form </p>
                      <p class="card-label"> View and download your agreements </p>
                    </div>
                  </div>

              </div>
            </div>

            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
              <div class="card">
               
                  <div class="card-body">
                    <span class="card-icon text-danger"> <i class="fa-solid fa-cart-shopping"></i> </span>
                    <div>
                      <p class="card-title">Shop</p>
                      <p class="card-label">Purchase products & offers</p>
                    </div>
                  </div>
                
              </div>
            </div>

            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
              <div class="card">
                
                  <div class="card-body">
                   <span class="card-icon text-warning"> <i class="fa-solid fa-briefcase"></i> </span>
                    <div>
                      <p class="card-title">My Business</p>
                      <p class="card-label">Track progress & metrics</p>
                    </div>
                  </div>
               
              </div>
            </div>

            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
              <div class="card">
                 
                  <div class="card-body">
                    <span class="card-icon text-success"> <i class="fa-solid fa-user-plus"></i> </span>
                    <div>
                      <p class="card-title">New Registration</p>
                      <p class="card-label">Add new member quickly</p>
                    </div>
                  </div>
                
              </div>
            </div>

            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
              <div class="card">
                
                  <div class="card-body">
                    <span class="card-icon text-primary"> <i class="fa-solid fa-bullhorn"></i> </span>
                    <div>
                      <p class="card-title">Events & Promotions</p>
                      <p class="card-label">Latest announcements</p>
                    </div>
                  </div>
                 
              </div>
            </div>

            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
              <div class="card">                
                  <div class="card-body">
                   <span class="card-icon text-dark"> <i class="fa-solid fa-key"></i> </span>
                    <div>
                      <p class="card-title">Key To Success</p>
                      <p class="card-label">Top tips & resources</p>
                    </div>
                  </div>
               
              </div>
            </div>


                      

          </div>
        </div>


                
            </div>
            
        </div>

          



          <div class="row">

             <div class="col-xl-3 col-lg-6">

                <div class="card l-bg-style1">
                  <div class="card-statistic-3">
                    <div class="card-icon card-icon-large"><i class="fa fa-award"></i></div>
                    <div class="card-content">
                      <h4 class="card-title">My Income</h4>
                      <span>101</span>
                      <div class="progress mt-1 mb-1" data-height="8">
                        <div class="progress-bar l-bg-purple" role="progressbar" data-width="25%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
                      </div>
                      <p class="mb-0 text-sm">
                        <span class="mr-2"><i class="fa fa-arrow-up"></i> 10%</span>
                        <span class="text-nowrap">Sample text here.. </span>
                      </p>
                    </div>
                  </div>
                </div>
              </div>



              
              <div class="col-xl-3 col-lg-6">
                <div class="card l-bg-style2">
                  <div class="card-statistic-3">
                    <div class="card-icon card-icon-large"><i class="fa fa-briefcase"></i></div>
                    <div class="card-content">
                      <h4 class="card-title">My Directs</h4>
                      <span>1,258</span>
                      <div class="progress mt-1 mb-1" data-height="8">
                        <div class="progress-bar l-bg-orange" role="progressbar" data-width="25%" aria-valuenow="25"
                          aria-valuemin="0" aria-valuemax="100"></div>
                      </div>
                      <p class="mb-0 text-sm">
                        <span class="mr-2"><i class="fa fa-arrow-up"></i> 10%</span>
                        <span class="text-nowrap">Sample text here..</span>
                      </p>
                    </div>
                  </div>
                </div>
              </div>


              <div class="col-xl-3 col-lg-6">
                <div class="card l-bg-style3">
                  <div class="card-statistic-3">
                    <div class="card-icon card-icon-large"><i class="fa fa-globe"></i></div>
                    <div class="card-content">
                      <h4 class="card-title">My Team</h4>
                      <span>10,225</span>
                      <div class="progress mt-1 mb-1" data-height="8">
                        <div class="progress-bar l-bg-cyan" role="progressbar" data-width="25%" aria-valuenow="25"
                          aria-valuemin="0" aria-valuemax="100"></div>
                      </div>
                      <p class="mb-0 text-sm">
                        <span class="mr-2"><i class="fa fa-arrow-up"></i> 10%</span>
                        <span class="text-nowrap">Sample text here..</span>
                      </p>
                    </div>
                  </div>
                </div>
              </div>


              <div class="col-xl-3 col-lg-6">
                <div class="card l-bg-style4">
                  <div class="card-statistic-3">
                    <div class="card-icon card-icon-large"><i class="fa fa-money-bill-alt"></i></div>
                    <div class="card-content">
                      <h4 class="card-title">-</h4>
                      <span>$2,658</span>
                      <div class="progress mt-1 mb-1" data-height="8">
                        <div class="progress-bar l-bg-green" role="progressbar" data-width="25%" aria-valuenow="25"
                          aria-valuemin="0" aria-valuemax="100"></div>
                      </div>
                      <p class="mb-0 text-sm">
                        <span class="mr-2"><i class="fa fa-arrow-up"></i> 10%</span>
                        <span class="text-nowrap">Sample text here..</span>
                      </p>
                    </div>
                  </div>
                </div>
              </div>



          </div>  
           




            <div class="row g-4">
            
            <!-- Wallet Summary -->
            
            <!-- Referral Link Card -->
            

            <div class="col-lg-6">
                  <div class="card kyc-card p-3">

                    <h5 class="mb-3">My KYC Status</h5>

                    <div class="row text-center">

                      <div class="col-md-4 col-6 kyc-box">
                        <p class="kyc-title">AADHAAR</p>
                        <span class="badge badge-success">
                          <i class="fa fa-check-circle"></i> Approved
                        </span>
                      </div>

                      <div class="col-md-4 col-6 kyc-box">
                        <p class="kyc-title">BANK</p>
                        <span class="badge badge-rejected">
                          <i class="fa fa-times-circle"></i> Rejected
                        </span>
                      </div>

                      <div class="col-md-4 col-6 kyc-box">
                        <p class="kyc-title">PAN CARD</p>
                        <span class="badge badge-rejected">
                          <i class="fa fa-times-circle"></i> Rejected
                        </span>
                      </div>

                      <div class="col-md-4 col-6 kyc-box">
                        <p class="kyc-title">GST</p>
                        <span class="badge badge-pending">
                          <i class="fa fa-exclamation-triangle"></i> Pending
                        </span>
                      </div>

                      <div class="col-md-4 col-6 kyc-box">
                        <p class="kyc-title">FSSAI</p>
                        <span class="badge badge-pending">
                          <i class="fa fa-exclamation-triangle"></i> Pending
                        </span>
                      </div>

                    </div>

                  </div>
                           
            </div>


            <div class="col-12 col-sm-12 col-md-12 col-lg-8 col-xl-6">
                <div class="card">
                 <div class="card-header">
                 <h4> Latest News </h4>                 
                 </div>
                  <div class="card-body">
                    <br>
                    <marquee direction="up" scrollamount="5" onmouseover="this.stop();" onmouseout="this.start();">
                     <h6 class="mb-0"> News Heading </h6>
                     <p> Sample text here.. Sample text here.. </p>
                     <hr>
                     <h6 class="mb-0"> News Heading </h6>
                     <p> Sample text here.. Sample text here.. </p>
                     <hr>
                   </marquee>
                  </div>
                </div>
              </div>


          

 
            
            
             
     

        </div>




            

      


            <div class="row">
              <!-- <div class="col-12 col-sm-12 col-lg-4">
                <div class="card">
                  <div class="card-header">
                    <h4> Spin Wheel </h4>
                  </div>
                  <div class="card-body">
                    <div class="row">
                      <div class="col-12 col-sm-12 col-lg-5">
                          <img src="https://via.placeholder.com/350x350" alt="" class="img-responsive rounded-circle" width="100%">
                      </div>
                      <div class="col-12 col-sm-12 col-lg-7">
                        <h5> Spin Wheel </h5>
                        <ul>
                          <li> Sample text here... </li>
                          <li> Sample text here... </li>
                          <li> Sample text here... </li>
                          <li> Sample text here... </li>
                        </ul>
                        <hr>
                        <div class="media-cta"><a href="#" class="btn btn-outline-primary">SPIN WHEEL </a></div>
                      </div>
                    </div>
                  </div>
                </div>
              </div> -->


             


                 <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
                  <div class="card">
                    <div class="card-header">
                      <h4>Details</h4>
                    </div>
                    <div class="card-body">
                    
                        <div class="table-responsive" id="scroll">
                          <table class="table table-hover table-xl mb-0">
                            <thead>
                              <tr>
                                <th>Heading</th>
                                <th>Heading</th>
                                <th>Heading</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td class="text-truncate"> Sample Data </td>
                                <td class="text-truncate">Sample Data</td>
                                <td class="text-truncate"> Sample Data</td>
                              </tr>
                              <tr>
                                  <td class="text-truncate"> Sample Data </td>
                                  <td class="text-truncate">Sample Data</td>
                                  <td class="text-truncate"> Sample Data</td>
                                </tr>
                              

                              </tbody>
                          </table>
                        </div>
                      
                    </div>
                  </div>
                </div>


                  <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
                  <div class="card">
                    <div class="card-header">
                      <h4>Details</h4>
                    </div>
                    <div class="card-body">
                    
                        <div class="table-responsive" id="scroll">
                          <table class="table table-hover table-xl mb-0">
                            <thead>
                              <tr>
                                <th>Heading</th>
                                <th>Heading</th>
                                <th>Heading</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td class="text-truncate"> Sample Data </td>
                                <td class="text-truncate">Sample Data</td>
                                <td class="text-truncate"> Sample Data</td>
                              </tr>
                              <tr>
                                  <td class="text-truncate"> Sample Data </td>
                                  <td class="text-truncate">Sample Data</td>
                                  <td class="text-truncate"> Sample Data</td>
                                </tr>
                              

                              </tbody>
                          </table>
                        </div>
                      
                    </div>
                  </div>
                </div>


                  <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
                  <div class="card">
                    <div class="card-header">
                      <h4>Details</h4>
                    </div>
                    <div class="card-body">
                    
                        <div class="table-responsive" id="scroll">
                          <table class="table table-hover table-xl mb-0">
                            <thead>
                              <tr>
                                <th>Heading</th>
                                <th>Heading</th>
                                <th>Heading</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td class="text-truncate"> Sample Data </td>
                                <td class="text-truncate">Sample Data</td>
                                <td class="text-truncate"> Sample Data</td>
                              </tr>
                              <tr>
                                  <td class="text-truncate"> Sample Data </td>
                                  <td class="text-truncate">Sample Data</td>
                                  <td class="text-truncate"> Sample Data</td>
                                </tr>
                              

                              </tbody>
                          </table>
                        </div>
                      
                    </div>
                  </div>
                </div>


                  <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
                  <div class="card">
                    <div class="card-header">
                      <h4>Details</h4>
                    </div>
                    <div class="card-body">
                    
                        <div class="table-responsive" id="scroll">
                          <table class="table table-hover table-xl mb-0">
                            <thead>
                              <tr>
                                <th>Heading</th>
                                <th>Heading</th>
                                <th>Heading</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td class="text-truncate"> Sample Data </td>
                                <td class="text-truncate">Sample Data</td>
                                <td class="text-truncate"> Sample Data</td>
                              </tr>
                              <tr>
                                  <td class="text-truncate"> Sample Data </td>
                                  <td class="text-truncate">Sample Data</td>
                                  <td class="text-truncate"> Sample Data</td>
                                </tr>
                              

                              </tbody>
                          </table>
                        </div>
                      
                    </div>
                  </div>
                </div>

                  <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
                  <div class="card">
                    <div class="card-header">
                      <h4>Details</h4>
                    </div>
                    <div class="card-body">
                    
                        <div class="table-responsive" id="scroll">
                          <table class="table table-hover table-xl mb-0">
                            <thead>
                              <tr>
                                <th>Heading</th>
                                <th>Heading</th>
                                <th>Heading</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td class="text-truncate"> Sample Data </td>
                                <td class="text-truncate">Sample Data</td>
                                <td class="text-truncate"> Sample Data</td>
                              </tr>
                              <tr>
                                  <td class="text-truncate"> Sample Data </td>
                                  <td class="text-truncate">Sample Data</td>
                                  <td class="text-truncate"> Sample Data</td>
                                </tr>
                              

                              </tbody>
                          </table>
                        </div>
                      
                    </div>
                  </div>
                </div>

                  <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
                  <div class="card">
                    <div class="card-header">
                      <h4>Details</h4>
                    </div>
                    <div class="card-body">
                    
                        <div class="table-responsive" id="scroll">
                          <table class="table table-hover table-xl mb-0">
                            <thead>
                              <tr>
                                <th>Heading</th>
                                <th>Heading</th>
                                <th>Heading</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td class="text-truncate"> Sample Data </td>
                                <td class="text-truncate">Sample Data</td>
                                <td class="text-truncate"> Sample Data</td>
                              </tr>
                              <tr>
                                  <td class="text-truncate"> Sample Data </td>
                                  <td class="text-truncate">Sample Data</td>
                                  <td class="text-truncate"> Sample Data</td>
                                </tr>
                              

                              </tbody>
                          </table>
                        </div>
                      
                    </div>
                  </div>
                </div>
                
             

            </div>

            <!-- <div class="row">
              <div class="col-12 col-sm-12 col-lg-3">
                <div class="card">
                  <div class="card-header"> 
                    <h4> Video - 1  </h4>
                  </div>
                  <div class="card-body"> 
                    <div class="row">
                      <div class="col-12 col-sm-12 col-lg-12" align="center">
                        <img src="https://via.placeholder.com/350x350" alt="" class="img-responsive rounded" width="100%">
                        <p class="mt-1"> VIDEO NAME</p>
                        <div class="media-cta"><a href="#" class="btn btn-outline-primary">SPIN WHEEL </a></div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              
              <div class="col-12 col-sm-12 col-lg-3">
                  <div class="card">
                    <div class="card-header"> 
                      <h4> Video - 2  </h4>

                    </div>
                    <div class="card-body"> 

                      <div class="row">
                          
                        <div class="col-12 col-sm-12 col-lg-12" align="center">
                          <img src="https://via.placeholder.com/350x350" alt="" class="img-responsive rounded" width="100%">
                          <p class="mt-1"> VIDEO NAME</p>
                          <div class="media-cta"><a href="#" class="btn btn-outline-primary">SPIN WHEEL </a></div>
                        </div>
                          

                      </div>
                    </div>
                  </div>
              </div>

              <div class="col-12 col-sm-12 col-lg-3">
                  <div class="card">
                    <div class="card-header"> 
                      <h4> Video - 3  </h4>

                    </div>
                    <div class="card-body"> 

                      <div class="row">
                          
                        <div class="col-12 col-sm-12 col-lg-12" align="center">
                          <img src="https://via.placeholder.com/350x350" alt="" class="img-responsive rounded" width="100%">
                          <p class="mt-1"> VIDEO NAME</p>
                          <div class="media-cta"><a href="#" class="btn btn-outline-primary">SPIN WHEEL </a></div>
                        </div>
                          

                      </div>
                    </div>
                  </div>
              </div>

              <div class="col-12 col-sm-12 col-lg-3">
                  <div class="card">
                    <div class="card-header"> 
                      <h4> Video - 3 </h4>
                    </div>
                    <div class="card-body"> 

                      <div class="row">
                          
                        <div class="col-12 col-sm-12 col-lg-12" align="center">
                          <img src="https://via.placeholder.com/350x350" alt="" class="img-responsive rounded" width="100%">
                          <p class="mt-1"> VIDEO NAME</p>
                          <div class="media-cta"><a href="#" class="btn btn-outline-primary">SPIN WHEEL </a></div>
                        </div>
                          

                      </div>
                    </div>
                  </div>
              </div>

            </div> -->

            <!-- <div class="row">              
              <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-7">
                <div class="card">
                  <div class="card-header">
                    <h4>Successful  Name Of All Participants In The Last Draw.</h4>                  
                  </div>
                  
                  <div class="card-body">
                    <div class="media-list position-relative">
                      <div id="project-team-scroll">
                        <div class="table-responsive">
                          <table class="table table-hover table-xl mb-0 text-center">
                            <thead>
                              <tr>
                                <th>Image</th>
                                <th>Participants Name</th>
                                <th>Project Name</th>
                                <th>Date</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr>
                                <td>
                                  <img alt="image" src="https://via.placeholder.com/100x100" class="rounded-circle" width="35" data-bs-toggle="tooltip" title="" data-bs-original-title="Wildan Ahdian" aria-label="Wildan Ahdian">
                                </td>
                                <td class="text-truncate">User Name</td>
                                <td class="text-truncate">Project Name</td>
                                <td class="text-truncate">Tuesday, 03-12-2023</td>
                              </tr>

                              <tr>
                                <td>
                                  <img alt="image" src="https://via.placeholder.com/100x100" class="rounded-circle" width="35" data-bs-toggle="tooltip" title="" data-bs-original-title="Wildan Ahdian" aria-label="Wildan Ahdian">
                                </td>
                                <td class="text-truncate">User Name</td>
                                <td class="text-truncate">Project Name</td>
                                <td class="text-truncate">Tuesday, 03-12-2023</td>
                              </tr>

                              <tr>
                                <td>
                                  <img alt="image" src="https://via.placeholder.com/100x100" class="rounded-circle" width="35" data-bs-toggle="tooltip" title="" data-bs-original-title="Wildan Ahdian" aria-label="Wildan Ahdian">
                                </td>
                                <td class="text-truncate">User Name</td>
                                <td class="text-truncate">Project Name</td>
                                <td class="text-truncate">Tuesday, 03-12-2023</td>
                              </tr>

                              <tr>
                                <td>
                                  <img alt="image" src="https://via.placeholder.com/100x100" class="rounded-circle" width="35" data-bs-toggle="tooltip" title="" data-bs-original-title="Wildan Ahdian" aria-label="Wildan Ahdian">
                                </td>
                                <td class="text-truncate">User Name</td>
                                <td class="text-truncate">Project Name</td>
                                <td class="text-truncate">Tuesday, 03-12-2023</td>
                              </tr>

                              <tr>
                                <td>
                                  <img alt="image" src="https://via.placeholder.com/100x100" class="rounded-circle" width="35" data-bs-toggle="tooltip" title="" data-bs-original-title="Wildan Ahdian" aria-label="Wildan Ahdian">
                                </td>
                                <td class="text-truncate">User Name</td>
                                <td class="text-truncate">Project Name</td>
                                <td class="text-truncate">Tuesday, 03-12-2023</td>
                              </tr>
                            </tbody>
                          </table>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div> -->
  
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
          <div class="footer-left">
            Copyright &copy; 2020 <div class="bullet"></div> Design By <a href="#">Your Company Name Here</a>
          </div>
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