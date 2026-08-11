import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <header class="header">
      <div class="brand">Job Aggregator</div>
      <nav class="nav">
        <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
        <a routerLink="/jobs" routerLinkActive="active">Jobs</a>
        <a routerLink="/companies" routerLinkActive="active">Companies</a>
        <a routerLink="/saved-jobs" routerLinkActive="active">Saved</a>
        <a routerLink="/applications" routerLinkActive="active">Applications</a>
        <a routerLink="/alerts" routerLinkActive="active">Alerts</a>
        <a routerLink="/profile" routerLinkActive="active">Profile</a>
        <a routerLink="/admin" routerLinkActive="active">Admin</a>
      </nav>
    </header>

    <main class="page-container">
      <router-outlet></router-outlet>
    </main>
  `,
  styles: [
    `
      .header {
        position: sticky;
        top: 0;
        z-index: 10;
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 0.9rem 1rem;
        background: #111827;
        color: #f9fafb;
      }

      .brand {
        font-weight: 700;
      }

      .nav {
        display: flex;
        gap: 0.75rem;
        flex-wrap: wrap;
      }

      .nav a {
        text-decoration: none;
        color: #d1d5db;
      }

      .nav a.active {
        color: #ffffff;
        font-weight: 600;
      }
    `
  ]
})
export class AppShellComponent {}
