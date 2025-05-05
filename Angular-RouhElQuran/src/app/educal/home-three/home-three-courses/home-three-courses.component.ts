import { Component, OnInit } from '@angular/core';
import { CoursesService } from 'src/app/Services/courses.service';
import { InstructorCoursesServiceService } from 'src/app/Services/Instructor-Courses-Servic/instructor-courses.service.service';

@Component({
    selector: 'app-home-three-courses',
    templateUrl: './home-three-courses.component.html',
    styleUrls: ['./home-three-courses.component.scss'],
    standalone: false
})
export class HomeThreeCoursesComponent implements OnInit {

  coursesData : any = [];
  constructor(private _InstructorCoursesService : InstructorCoursesServiceService) { }

  ngOnInit(): void {
    debugger;
    console.log(this.coursesData)
    this.GetCourses();
  }

  GetCourses() {
    debugger;
    this._InstructorCoursesService.getAllInstructorCourses().subscribe({
      next: (response) => {
        this.coursesData = response;
            console.log("Courses data:", this.coursesData);
      },
      error: (err) => {
        console.error("Error fetching courses:", err);
      }
    });
  }
}
