import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
    selector: 'app-header-three',
    templateUrl: './header-three.component.html',
    styleUrls: ['./header-three.component.scss'],
    standalone: false
})
export class HeaderThreeComponent implements OnInit {

  userName: any;
  isLogin = false;


  constructor(private _authService:AuthService, private _router: Router) {}

  async ngOnInit(): Promise<void> {
    this._authService.authStatus.subscribe((status:boolean) => {
      this.isLogin = status;
      if(status == true) {
        this.userName =
        this._authService.UserData[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
        ];
      }
    
    });
    if (localStorage.getItem('token') != null) {
      //When SaveUserLoginData is called, it emits an event that NavBarComponent listens
      // for, updating the UI in real-time without requiring a page reload.
      await this._authService.SaveUserLoginData();
    }
  }

  headerSticky : boolean = false;
  searchBar : boolean = false;
  showSidebar : boolean = false;
  showHomeDropdown : boolean = false;
  showCoursesDropdown : boolean = false;
  showBlogDropdown : boolean = false;
  showPagesDropdown : boolean = false;

  @HostListener('window:scroll',['$event']) onscroll () {
    if(window.scrollY > 80){
      this.headerSticky = true
    }
    else{
      this.headerSticky = false
    }
  }

  handleSearch () {
    if(!this.searchBar){
      this.searchBar = true;
    }
    else{
      this.searchBar = true;
    }
  }
  handleSearchClose () {
    this.searchBar = false;
  }

  // handleSidebar
  handleSidebar () {
    this.showSidebar = true;
  }
  handleSidebarClose () {
    this.showSidebar = false;
  }

  // home dropdown
  homeDropdown () {
    this.showHomeDropdown = !this.showHomeDropdown
  }
  // coursesDropdown
  coursesDropdown () {
    this.showCoursesDropdown = !this.showCoursesDropdown
  }

  // blogDropdown
  blogDropdown () {
    this.showBlogDropdown = !this.showBlogDropdown
  }
  // pagesDropDown
  pagesDropDown () {
    this.showPagesDropdown = !this.showPagesDropdown
  }


//Logout 
logOut() {
  this._authService.RemoveUserlogoutData().then(() => {
    this._router.navigate(['/']).then(() => {
    });
  });
}
}
