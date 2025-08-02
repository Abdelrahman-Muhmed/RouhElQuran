import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CourseService } from 'src/app/Services/Course-Service/course.service';
import { environment } from 'src/environments/environment.prod';


@Component({
    selector: 'app-home-three-courses',
    templateUrl: './home-three-courses.component.html',
    styleUrls: ['./home-three-courses.component.scss'],
    standalone: false
})
export class HomeThreeCoursesComponent implements OnInit {

  coursesData : any = [];
   apiUrl = environment.apiBaseUrl; 
  staticFilesPath = environment.staticFilesPath;
  private responseResult : any;

  constructor(private _CoursesService : CourseService,private router: Router) { }

  ngOnInit(): void {

    this.GetCourses();
  }

  GetCourses() {
  this._CoursesService.getAllCourses().subscribe({
      next: (response) => {
        this.responseResult = response;
        this.coursesData = this.responseResult.data;
            console.log( this.coursesData)
            console.log("Course data: asd", this.coursesData );
      },
      error: (err) => {
        console.error("Error fetching courses:", err);
      }
    });
  }
   buildImageUrl(fileName: string): string {
    return `${this.apiUrl}${this.staticFilesPath}/${fileName}`;
  }

  goToCourse(id: number): void {
  this.router.navigate(['/course-details', id]);
}
}
