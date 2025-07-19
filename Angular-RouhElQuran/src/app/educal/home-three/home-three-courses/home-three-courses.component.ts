import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CourseService } from 'src/app/Services/Course-Service/course.service';
import { environment } from 'src/environments/environment';


@Component({
    selector: 'app-home-three-courses',
    templateUrl: './home-three-courses.component.html',
    styleUrls: ['./home-three-courses.component.scss'],
    standalone: false
})
export class HomeThreeCoursesComponent implements OnInit {

  coursesData : any = [];
   apiUrl = environment.apiBaseUrl; 
  
  constructor(private _CoursesService : CourseService) { }

  ngOnInit(): void {

    this.GetCourses();
  }

  GetCourses() {
  this._CoursesService.getAllCourses().subscribe({
      next: (response) => {
        this.coursesData = response;
            console.log("Course data:", this.coursesData );
      },
      error: (err) => {
        console.error("Error fetching courses:", err);
      }
    });
  }
}
