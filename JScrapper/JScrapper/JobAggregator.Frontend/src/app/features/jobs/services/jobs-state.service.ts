import { Injectable, computed, inject, signal } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { JobsApiService } from './jobs-api.service';
import { JobFilters } from '../../../shared/models/job-filter.models';
import { JobSearchItem, JobSearchQuery, JobSearchResult } from '../../../shared/models/job-search.models';

const defaultFilters: JobFilters = {
  keyword: '',
  title: '',
  company: '',
  location: '',
  experience: '',
  employmentType: '',
  skills: [],
  source: ''
};

@Injectable({ providedIn: 'root' })
export class JobsStateService {
  private readonly jobsApi = inject(JobsApiService);

  readonly filters = signal<JobFilters>({ ...defaultFilters });
  readonly sortBy = signal('postedDate');
  readonly sortDirection = signal<'asc' | 'desc'>('desc');
  readonly pageNumber = signal(1);
  readonly pageSize = signal(20);

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly result = signal<JobSearchResult | null>(null);
  readonly selectedJob = signal<JobSearchItem | null>(null);

  readonly items = computed(() => this.result()?.items ?? []);
  readonly totalCount = computed(() => this.result()?.totalCount ?? 0);
  readonly totalPages = computed(() => this.result()?.totalPages ?? 0);

  load(): void {
    this.loading.set(true);
    this.error.set(null);

    const query = this.buildQuery();

    this.jobsApi.searchJobs(query)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (response) => {
          this.result.set(response);
          const current = this.selectedJob();
          if (!current || !response.items.some((item) => item.jobId === current.jobId)) {
            this.selectedJob.set(response.items[0] ?? null);
          }
        },
        error: () => {
          this.error.set('Failed to load jobs.');
        }
      });
  }

  setFilters(value: JobFilters): void {
    this.filters.set({ ...value });
    this.pageNumber.set(1);
    this.load();
  }

  setSorting(sortBy: string, sortDirection: 'asc' | 'desc'): void {
    this.sortBy.set(sortBy);
    this.sortDirection.set(sortDirection);
    this.pageNumber.set(1);
    this.load();
  }

  setPage(page: number): void {
    this.pageNumber.set(page);
    this.load();
  }

  selectJob(job: JobSearchItem): void {
    this.selectedJob.set(job);
  }

  private buildQuery(): JobSearchQuery {
    const filters = this.filters();

    return {
      keyword: this.clean(filters.keyword),
      title: this.clean(filters.title),
      company: this.clean(filters.company),
      location: this.clean(filters.location),
      minSalary: filters.minSalary,
      maxSalary: filters.maxSalary,
      experience: this.clean(filters.experience),
      employmentType: this.clean(filters.employmentType),
      skills: filters.skills.length > 0 ? filters.skills : undefined,
      remote: filters.remote,
      hybrid: filters.hybrid,
      source: this.clean(filters.source),
      postedFrom: filters.postedFrom?.toISOString(),
      postedTo: filters.postedTo?.toISOString(),
      sortBy: this.sortBy(),
      sortDirection: this.sortDirection(),
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    };
  }

  private clean(value: string): string | undefined {
    const normalized = value.trim();
    return normalized.length > 0 ? normalized : undefined;
  }
}
