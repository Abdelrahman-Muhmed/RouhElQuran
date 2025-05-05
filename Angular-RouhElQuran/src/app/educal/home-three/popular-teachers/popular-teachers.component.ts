import { Component, OnInit } from '@angular/core';
import { InstructorService } from 'src/app/Services/Instructor-Service/instructor.service';


@Component({
    selector: 'app-popular-teachers',
    templateUrl: './popular-teachers.component.html',
    styleUrls: ['./popular-teachers.component.scss'],
    standalone: false
})
export class PopularTeachersComponent implements OnInit {

   instructorData : any = [];
  constructor(private _instructorService: InstructorService ) { }

  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this._instructorService.getInstructorData().subscribe({
      next: (response) => {
        this.instructorData = response;
            console.log("Courses data:", this.instructorData);
      },
      error: (err) => {
        console.error("Error fetching courses:", err);
      }
    });
  }
}
