import { Component, OnInit } from '@angular/core';
import { CoursesService } from 'src/app/Services/courses.service';

@Component({
    selector: 'app-home-three-courses',
    templateUrl: './home-three-courses.component.html',
    styleUrls: ['./home-three-courses.component.scss'],
    standalone: false
})
export class HomeThreeCoursesComponent implements OnInit {

  cursesData : any = [];
  constructor(private _CursesService : CoursesService) { }

  ngOnInit(): void {
    this.GetCourses();
  }
 
  GetCourses() {
    this._CursesService.getAllCourses().subscribe({
      next: (response) => {  
        this.cursesData = response;

      },
      error: (err) => {
        console.error("Error fetching courses:", err);
      }
    });
  }
}
