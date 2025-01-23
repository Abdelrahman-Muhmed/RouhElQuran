import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-checkout-area',
    templateUrl: './checkout-area.component.html',
    styleUrls: ['./checkout-area.component.scss'],
    standalone: false
})
export class CheckoutAreaComponent implements OnInit {

  showCbox : boolean = false;
  showShipBox : boolean = false;

  handleCbox () {
    this.showCbox = !this.showCbox;
  }

  handleShipBox () {
    this.showShipBox = !this.showShipBox;
  }
  constructor() { }

  ngOnInit(): void {
  }

}
