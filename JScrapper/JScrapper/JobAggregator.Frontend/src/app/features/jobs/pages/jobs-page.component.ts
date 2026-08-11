import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, inject } from '@angular/core';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { JobFilters } from '../../../shared/models/job-filter.models';
import { JobSearchItem } from '../../../shared/models/job-search.models';
import { JobFiltersComponent } from '../components/job-filters.component';
import { JobListComponent } from '../components/job-list.component';
import { JobDetailsComponent } from '../components/job-details.component';
import { JobsStateService } from '../services/jobs-state.service';
import { JobLinkService } from '../services/job-link.service';

@Component({
  selector: 'app-jobs-page',
  standalone: true,
  imports: [CommonModule, ProgressSpinnerModule, JobFiltersComponent, JobListComponent, JobDetailsComponent],
  templateUrl: './jobs-page.component.html',
  styleUrl: './jobs-page.component.scss'
})
export class JobsPageComponent implements OnInit {
  private readonly state = inject(JobsStateService);
  private readonly jobLinkService = inject(JobLinkService);

  readonly loading = this.state.loading;
  readonly error = this.state.error;
  readonly jobs = this.state.items;
  readonly totalCount = this.state.totalCount;
  readonly totalPages = this.state.totalPages;
  readonly selectedJob = this.state.selectedJob;
  readonly pageNumber = this.state.pageNumber;
  readonly pageSize = this.state.pageSize;
  readonly first = computed(() => (this.pageNumber() - 1) * this.pageSize());

  ngOnInit(): void {
    this.state.load();
  }

  onFiltersChanged(filters: JobFilters): void {
    this.state.setFilters(filters);
  }

  onSortChanged(event: { sortBy: string; sortDirection: 'asc' | 'desc' }): void {
    this.state.setSorting(event.sortBy, event.sortDirection);
  }

  onPageChanged(pageNumber: number): void {
    this.state.setPage(pageNumber);
  }

  onJobSelected(job: JobSearchItem): void {
    this.state.selectJob(job);
  }

  onOpenOriginal(job: JobSearchItem): void {
    this.jobLinkService.openOriginalJob(job);
  }
}
