import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { InstructorDetailsService } from 'src/app/Services/Instructor-Details/instructor-details.service';
import { ReviewService } from 'src/app/Services/Review/review.service';
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
  private responseResult : any;
    responseMessage : any;
    stars = [1,2,3,4,5];
    hoverRating: number | null = null;
    isSuccess: boolean = false;
    CourseId!: number;

  constructor(
    private route: ActivatedRoute,
    private _InstructorDetailsService: InstructorDetailsService,
    private _ReviewService : ReviewService,
    
  ) {}
    //Review Form Validiate 
     ReviewForm : FormGroup = new FormGroup ({
      rate : new FormControl(null,Validators.required),
      reviewSummary : new FormControl(null,Validators.required),
      CourseID : new FormControl(null),
      InstructorID : new FormControl(null),
  
      
     });
  
 //Custome Validation
    validatePassword(): void {
      const reviewSummaryControl = this.ReviewForm.get('reviewSummary');

      const value = reviewSummaryControl?.value;

      if (!value) return;

      const errors: any = {};

      if (/[^a-zA-Z0-9 ]/.test(value)) {
        errors.specialChars = 'Summary must not contain special characters';
      }


      if (Object.keys(errors).length > 0) {
        reviewSummaryControl?.setErrors(errors);
      } else {
        reviewSummaryControl?.setErrors(null);
      }
    }
    setRating(value: number): void {
      this.ReviewForm.get('rate')?.setValue(value);
      this.ReviewForm.get('rate')?.markAsTouched();
    }
    
    resetHover(): void {
      this.hoverRating = null;
    }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.instructorId = +idParam;
        console.log('Instructor ID:', this.instructorId);
        this.getData(this.instructorId);

        //for reviw
           this.ReviewForm.get('InstructorID')?.setValue(this.instructorId);

      }
    });

    //For review 
      this.ReviewForm.get('reviewSummary')?.valueChanges.subscribe(() => {
      this.validatePassword();
    })
  }

  getData(instructorId: number) {
    this._InstructorDetailsService.getInstructorDataById(instructorId).subscribe({
      next: (response) => {
        this.responseResult = response;
        this.instructorData = this.responseResult.data;
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


   //Add Review 
   addInstructorReview() {
  const formValue = this.ReviewForm.value;

  const reviewDto = {
    InstructorId: formValue.InstructorID,
    CourseId : formValue.courseID,
    rating: formValue.rate,
    comment: formValue.reviewSummary
  };

  this._ReviewService.addReview(reviewDto).subscribe({
    next: (response) => {
    this.responseResult = response as any;
     this.responseMessage =  this.responseResult.data
      this.isSuccess = true;
     console.log("Sucess:", response)
      setTimeout(() => {
        this.getData(this.CourseId); 
        this.isSuccess = false;
        this.responseMessage = null;
      }, 3000);
    },
    error: (error) => {
      this.responseMessage = error.error?.message || 'An error occurred';
      this.isSuccess = false;

      setTimeout(() => this.responseMessage = null, 5000);
    }
  });
}
}
