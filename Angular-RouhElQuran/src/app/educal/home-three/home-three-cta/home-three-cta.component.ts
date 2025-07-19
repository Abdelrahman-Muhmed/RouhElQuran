import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CourseService } from 'src/app/Services/Course-Service/course.service';
import { environment } from 'src/environments/environment';

@Component({
    selector: 'app-home-three-cta',
    templateUrl: './home-three-cta.component.html',
    styleUrls: ['./home-three-cta.component.scss'],
    standalone: false
})
export class HomeThreeCtaComponent implements OnInit {
  courseId: number = 0;
  constructor( private route: ActivatedRoute,private _CourseService: CourseService) {}

  ngOnInit(): void {
    this.courseId = Number(this.route.snapshot.paramMap.get('id'));
  }

  bookFreeCourse() {
    debugger;
    this._CourseService.bookFreeCourse(this.courseId).subscribe({
      next: (response) => {
    debugger;

        // Just For Now Abdo *_*
        alert(response); // or use a toast/snackbar if you prefer
      },
      error: (error) => {
    debugger;

        if (error.status === 400) {
          alert("Invalid user ID or request.");
        } else if (error.status === 404) {
          alert("Course not found.");
        } else {
          alert("Something went wrong. Please try again later.");
        }
      }
    });
  }
  
}
