import { Routes } from '@angular/router';
import { DashboardPageComponent } from './features/dashboard/dashboard-page.component';
import { JobsPageComponent } from './features/jobs/pages/jobs-page.component';
import { CompaniesPageComponent } from './features/companies/companies-page.component';
import { SavedJobsPageComponent } from './features/saved-jobs/saved-jobs-page.component';
import { ApplicationsPageComponent } from './features/applications/applications-page.component';
import { AlertsPageComponent } from './features/alerts/alerts-page.component';
import { ProfilePageComponent } from './features/profile/profile-page.component';
import { AdminPageComponent } from './features/admin/admin-page.component';

export const appRoutes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'jobs' },
  { path: 'dashboard', component: DashboardPageComponent },
  { path: 'jobs', component: JobsPageComponent },
  { path: 'companies', component: CompaniesPageComponent },
  { path: 'saved-jobs', component: SavedJobsPageComponent },
  { path: 'applications', component: ApplicationsPageComponent },
  { path: 'alerts', component: AlertsPageComponent },
  { path: 'profile', component: ProfilePageComponent },
  { path: 'admin', component: AdminPageComponent },
  { path: '**', redirectTo: 'jobs' }
];