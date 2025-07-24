import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InstructorDetailsService } from 'src/app/Services/Instructor-Details/instructor-details.service';
import { environment } from 'src/environments/environment.prod';

@Component({
  selector: 'app-instructor-details-area',
  templateUrl: './instructor-details-area.component.html',
  styleUrls: ['./instructor-details-area.component.scss'],
  standalone: false
})
export class InstructorDetailsAreaComponent implements OnInit {

  instructorData: any;
  apiUrl = environment.apiBaseUrl;
  instructorId!: number;
   staticFilesPath = environment.staticFilesPath;
   
  constructor(
    private route: ActivatedRoute,
    private _InstructorDetailsService: InstructorDetailsService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.instructorId = +idParam;
        console.log('Instructor ID:', this.instructorId);
        this.getData(this.instructorId);
      }
    });
  }

  getData(instructorId: number) {
    this._InstructorDetailsService.getInstructorDataById(instructorId).subscribe({
      next: (response) => {
        this.instructorData = response;
        console.log('Instructor data:', this.instructorData);
      },
      error: (err) => {
        console.error('Error fetching instructor data:', err);
      }
    });
  }
   buildImageUrl(fileName: string): string {
    return `${this.apiUrl}${this.staticFilesPath}/${fileName}`;
  }
}
