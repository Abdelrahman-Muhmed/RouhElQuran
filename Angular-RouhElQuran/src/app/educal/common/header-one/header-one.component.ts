import { Component, HostListener, OnInit,Input } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
    selector: 'app-header-one',
    templateUrl: './header-one.component.html',
    styleUrls: ['./header-one.component.scss'],
    standalone: false
})
export class HeaderOneComponent implements OnInit {

  userName: any;
  isLogin = false;
  constructor(private _authService:AuthService) {}

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


  @Input () header__white : string | undefined

  headerSticky : boolean = false;
  showCart : boolean = false;
  showSidebar : boolean = false;
  showHomeDropdown : boolean = false;
  showCoursesDropdown : boolean = false;
  showBlogDropdown : boolean = false;
  showPagesDropdown : boolean = false;

  // cart quantity
  count = 1;
  countTwo = 1;
  countThree = 1;

// sticky nav
  @HostListener('window:scroll',['$event']) onscroll () {
    if(window.scrollY > 80){
      this.headerSticky = true
    }
    else{
      this.headerSticky = false
    }
  }
// handleCartToggle
  handleCartToggle () {
    this.showCart = true;
  }
  handleCartClose () {
    this.showCart = false;
  }
  handleAddCart (number : string) {
    if(number === 'one'){
      this.count++
    }
    if(number === 'two'){
      this.countTwo++
    }
    if(number === 'three'){
      this.countThree++
    }
  }
  handleDecreaseCart (number : string){
    if(number === 'one' && this.count > 1){
      this.count--
    }
    if(number === 'two' && this.countTwo > 1){
      this.countTwo--
    }
    if(number === 'one' && this.countThree > 1){
      this.countThree--
    }
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


 

}
