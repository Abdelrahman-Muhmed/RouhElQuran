import { Component, OnInit,Input } from '@angular/core';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss'],
    standalone: false
})
export class FooterComponent implements OnInit {

  @Input () footerPadd : string | undefined

  constructor() { }

  ngOnInit(): void {
  }

}
