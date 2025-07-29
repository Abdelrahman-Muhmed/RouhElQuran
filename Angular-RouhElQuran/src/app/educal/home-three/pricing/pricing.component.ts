import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseDetailsService } from 'src/app/Services/Course-Details/course-details.service';
import { PaymentService } from 'src/app/Services/Payment-Service/payment.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-pricing',
  templateUrl: './pricing.component.html',
  styleUrls: ['./pricing.component.scss'],
  standalone: false
})
export class PricingComponent implements OnInit {

  CourseID!: number;
  coursePlans: any;
  constructor(private route: ActivatedRoute, private courseDetailsService: CourseDetailsService ,
    private paymentService: PaymentService
  ) { }



  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.CourseID = +  idParam;
      }
    });

    this.loadCourseDetails();
  }

  private loadCourseDetails(): void {

    this.courseDetailsService.getCoursePlansByCourseID(this.CourseID).subscribe({
      next: (response) => {
        console.log('Course plans loaded:', response);
        this.coursePlans = response;
      },
      error: (err) => {
        console.error('Failed to load course:', err);
      }
    });
  }

  createCheckOut(planId: number): void {
    this.paymentService.createPayment(planId).subscribe({
      next: (res) => {
        window.location.href = res.paymentUrl;
      },
      error: (err) => {
        console.error('Payment failed', err);
      }
    });
  }
}



