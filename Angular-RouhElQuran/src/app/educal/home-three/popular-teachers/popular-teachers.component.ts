import { Component, OnInit } from '@angular/core';
import { InstructorService } from 'src/app/Services/Instructor-Service/instructor.service';
import { environment } from 'src/environments/environment.prod';


@Component({
    selector: 'app-popular-teachers',
    templateUrl: './popular-teachers.component.html',
    styleUrls: ['./popular-teachers.component.scss'],
    standalone: false
})
export class PopularTeachersComponent implements OnInit {

   instructorData : any = [];
    apiUrl = environment.apiBaseUrl; 
    private responseResult : any;
  constructor(private _instructorService: InstructorService ) { }

  ngOnInit(): void {
    this.getData();
  }

  getData() {
    this._instructorService.getInstructorData().subscribe({
      next: (response) => {
        this.responseResult = response;
        this.instructorData = this.responseResult.data;

            console.log("Instructor data:", this.instructorData );
      },
      error: (err) => {
        console.error("Error fetching courses:", err);
      }
    });
  }
}
