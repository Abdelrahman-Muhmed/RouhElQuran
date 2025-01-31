import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatTabsModule } from '@angular/material/tabs';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EducalModule } from './educal/educal.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
// import { HttpClientModule } from '@angular/common/http';
@NgModule({
  declarations: [
    AppComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    EducalModule,
    BrowserAnimationsModule,
    MatTabsModule,
    // HttpClientModule
   CommonModule

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
