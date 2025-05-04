import { Component, OnInit } from '@angular/core';
import SwiperCore, { Autoplay,EffectFade } from "swiper";

SwiperCore.use([Autoplay,EffectFade]);

@Component({
    selector: 'app-hero-slider',
    templateUrl: './hero-slider.component.html',
    styleUrls: ['./hero-slider.component.scss'],
    standalone: false
})
export class HeroSliderComponent implements OnInit {

  sliderNavData = [
    {
      id: 1,
      img: 'assets/img/slider/nav/quran-recitation.jpg',
      title: "10 Courses",
      subtitle: "Quran Recitation",
      bgColor: "orange-bg",
    },
    {
      id: 2,
      img: 'assets/img/slider/nav/hadith-studies.jpg',
      title: "6 Courses",
      subtitle: "Hadith Studies",
      bgColor: "blue-bg",
    },
    {
      id: 3,
      img: 'assets/img/slider/nav/arabic-language.jpg',
      title: "12 Courses",
      subtitle: "Arabic Language",
      bgColor: "pink-bg",
    },
    {
      id: 4,
      img: 'assets/img/slider/nav/islamic-history.jpg',
      title: "8 Courses",
      subtitle: "Islamic History",
      bgColor: "green-bg",
    },
  ];
  

  constructor() { }

  ngOnInit(): void {
  }

}
