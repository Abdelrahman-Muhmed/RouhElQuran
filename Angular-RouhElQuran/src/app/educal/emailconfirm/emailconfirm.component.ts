// emailconfirm.component.ts
import { Component, OnInit, AfterViewInit, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-emailconfirm',
  imports: [],
  templateUrl: './emailconfirm.component.html',
  styleUrl: './emailconfirm.component.scss'
})
export class EmailconfirmComponent implements OnInit, AfterViewInit {
  email: string = '';
  emailProviderUrl: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private elementRef: ElementRef
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
      this.emailProviderUrl = this.getEmailProviderUrl(this.email);
    });
  }

  ngAfterViewInit() {
    this.initializeAnimations();
  }

  getEmailProviderUrl(email: string): string {
    if (!email) return '';
    const domain = email.split('@')[1]?.toLowerCase();
    
    if (domain?.includes('gmail.com')) return 'https://mail.google.com/';
    if (domain?.includes('yahoo.com')) return 'https://mail.yahoo.com/';
    if (domain?.includes('outlook.com') || domain?.includes('hotmail.com') || domain?.includes('live.com')) {
      return 'https://outlook.live.com/';
    }
    if (domain?.includes('icloud.com')) return 'https://www.icloud.com/mail';
    
    return 'https://' + domain;
  }

  private initializeAnimations() {
    // Add click effect to floating icons
    const floatingIcons = this.elementRef.nativeElement.querySelectorAll('.floating-icon');
    floatingIcons.forEach((icon: HTMLElement) => {
      icon.addEventListener('click', () => {
        icon.style.transform = 'scale(1.2)';
        setTimeout(() => {
          icon.style.transform = '';
        }, 200);
      });
    });

    // Animate the confirmation icon on load
    setTimeout(() => {
      const confirmationIcon = this.elementRef.nativeElement.querySelector('.confirmation__icon');
      if (confirmationIcon) {
        confirmationIcon.style.animation = 'pulse 2s infinite';
      }
    }, 500);
  }

  resendEmail() {
    // Implement resend email logic here
    console.log('Resending email to:', this.email);
    // You can call your API service here
    // this.authService.resendConfirmationEmail(this.email).subscribe(...);
    
    // Show a temporary message or toast notification
    alert('Confirmation email sent again! Please check your inbox.');
  }

  contactSupport() {
    // Implement contact support logic
    console.log('Contacting support');
    // You can navigate to support page or open email client
    // this.router.navigate(['/support']);
    // or
    window.location.href = 'mailto:abdelrahmangomaa847@gmail.com?subject=Email Confirmation Help';
  }

  // Optional: Method to go back to login
  goToLogin() {
    this.router.navigate(['/sign-in']);
  }

  // Optional: Method to go to home page
  goToHome() {
    this.router.navigate(['/']);
  }
}