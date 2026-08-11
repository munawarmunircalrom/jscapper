import { bootstrapApplication } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app/app.component';
import { appRoutes } from './app/app.routes';
import { apiBaseUrlInterceptor } from './app/core/http/api-base-url.interceptor';
import { API_BASE_URL } from './app/core/config/api.constants';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(appRoutes),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([apiBaseUrlInterceptor])),
    { provide: API_BASE_URL, useValue: '' }
  ]
}).catch((error) => console.error(error));