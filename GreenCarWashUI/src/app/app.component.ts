import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HomeComponent } from './home/home/home.component';
import { LoginComponent } from './Authentication/login/login.component';

@Component({
  selector: 'app-root',
  imports: [HomeComponent,LoginComponent,RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'GreenCarWashUI';
}
