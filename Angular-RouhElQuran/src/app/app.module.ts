import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatTabsModule } from '@angular/material/tabs';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EducalModule } from './educal/educal.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { authInterceptor } from './auth.interceptor';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
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
  providers: [provideHttpClient(
    withInterceptors([authInterceptor])
  )],
  bootstrap: [AppComponent]
})
export class AppModule { }
function rovideHttpClient(arg0: any): import("@angular/core").Provider | import("@angular/core").EnvironmentProviders {
  throw new Error('Function not implemented.');
}

