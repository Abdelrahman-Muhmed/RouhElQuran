import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Route } from '@angular/router';
import { CourseDetailsService } from 'src/app/Services/Course-Details/course-details.service';
import { ReviewService } from 'src/app/Services/Review/review.service';
import { environment } from 'src/environments/environment.prod';

// import Swiper core and required modules
import SwiperCore, { Pagination, Autoplay } from "swiper";

// install Swiper modules
SwiperCore.use([ Pagination, Autoplay])

@Component({
    selector: 'app-course-details-area',
    templateUrl: './course-details-area.component.html',
    styleUrls: ['./course-details-area.component.scss'],
    standalone: false
})
export class CourseDetailsAreaComponent implements OnInit {

  courseData = [
    {
      id: 1,
      courseImage: "assets/img/course/course-1.jpg",
      listImg: "assets/img/course/list/course-1.jpg",
      lesson: "43",
      title: "Become a product Manager learn the skills & job.",
      rating: "4.5",
      teacherImg: "assets/img/course/teacher/teacher-1.jpg",
      teacherName: "Jim Séchen",
      category: "Art & Design",
      price: "21.00",
      oldPrice: "33.00"
    },
    {
      id: 2,
      courseImage: "assets/img/course/course-2.jpg",
      listImg: "assets/img/course/list/course-2.jpg",
      lesson: "72",
      title: "Fundamentals of music theory Learn new",
      rating: "4.0",
      teacherImg: "assets/img/course/teacher/teacher-2.jpg",
      teacherName: "Barry Tone",
      category: "UX Design",
      price: "32.00",
      oldPrice: "68.00",
      color: "sky-blue"
    },
    {
      id: 3,
      courseImage: "assets/img/course/course-3.jpg",
      listImg: "assets/img/course/list/course-3.jpg",
      lesson: "35",
      title: "Bases Matemáticas dios Álgebra Ecuacion",
      rating: "4.3",
      teacherImg: "assets/img/course/teacher/teacher-3.jpg",
      teacherName: "Samuel Serif",
      category: "Development",
      price: "13.00",
      oldPrice: "19.00",
      color: "green"
    },
    {
      id: 4,
      courseImage: "assets/img/course/course-4.jpg",
      listImg: "assets/img/course/list/course-4.jpg",
      lesson: "60",
      title: "Strategy law and organization Foundation",
      rating: "3.5",
      teacherImg: "assets/img/course/teacher/teacher-4.jpg",
      teacherName: "Elon Gated",
      category: "Development",
      price: "62.00",
      oldPrice: "97.00",
      color: "blue"
    },
    {
      id: 5,
      courseImage: "assets/img/course/course-5.jpg",
      listImg: "assets/img/course/list/course-5.jpg",
      lesson: "28",
      title: "The business Intelligence analyst Course 2022",
      rating: "4.5",
      teacherImg: "assets/img/course/teacher/teacher-5.jpg",
      teacherName: "Eleanor Fant",
      category: "Marketing",
      price: "25.00",
      oldPrice: "36.00",
      color: "orange"
    },
  ]

    CourseData: any;
    apiUrl = environment.apiBaseUrl;
    CourseId!: number;
    staticFilesPath = environment.staticFilesPath;
    responseResult: any;
    responseMessage : any;
    stars = [1,2,3,4,5];
    hoverRating: number | null = null;
    isSuccess: boolean = false;
  
    constructor(
      private route: ActivatedRoute,
      private _CourseDetailsService: CourseDetailsService,
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
          this.CourseId = +idParam;
          console.log('Instructor ID:', this.CourseId);
          this.getData(this.CourseId);
          
          //for review   
           this.ReviewForm.get('CourseID')?.setValue(this.CourseId);
        }
      });

    this.ReviewForm.get('reviewSummary')?.valueChanges.subscribe(() => {
      this.validatePassword();
    })
    }
  
    getData(CourseId: number) {
      this._CourseDetailsService.getCourseDataById(CourseId).subscribe({
        next: (response) => {
           this.responseResult = response as any;
          this.CourseData = this.responseResult.data;
          console.log(this.apiUrl)
          console.log('Course data:', this.responseResult);
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
   addCourseReview() {
  const formValue = this.ReviewForm.value;

  const reviewDto = {
    courseID: formValue.CourseID,
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
