import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseDetailsService } from 'src/app/Services/Course-Details/course-details.service';

@Component({
    selector: 'app-pricing',
    templateUrl: './pricing.component.html',
    styleUrls: ['./pricing.component.scss'],
    standalone: false
})
export class PricingComponent implements OnInit {

  CourseID!: number;
  coursePlans: any;
  constructor(private route: ActivatedRoute, private courseDetailsService: CourseDetailsService) { }
    
  

   ngOnInit(): void {
      this.route.paramMap.subscribe(params => {
        const idParam = params.get('id');
        if (idParam) {
          this.CourseID =+  idParam;
        }
      });
    }

    private loadCourseDetails(): void {
    this.courseDetailsService.getCoursePlansByCOurseID(this.CourseID).subscribe({
      next: (course) => {
        this.coursePlans = course;
      },
      error: (err) => {
        console.error('Failed to load course:', err);
      }
    });
  }


  }
    


