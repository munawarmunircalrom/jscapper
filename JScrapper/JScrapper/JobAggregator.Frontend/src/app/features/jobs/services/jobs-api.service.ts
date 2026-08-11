import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { JobSearchQuery, JobSearchResult } from '../../../shared/models/job-search.models';

@Injectable({ providedIn: 'root' })
export class JobsApiService {
  private readonly http = inject(HttpClient);

  searchJobs(query: JobSearchQuery): Observable<JobSearchResult> {
    let params = new HttpParams()
      .set('sortBy', query.sortBy)
      .set('sortDirection', query.sortDirection)
      .set('pageNumber', query.pageNumber)
      .set('pageSize', query.pageSize);

    const setters: Array<[keyof JobSearchQuery, string]> = [
      ['keyword', 'keyword'],
      ['title', 'title'],
      ['company', 'company'],
      ['location', 'location'],
      ['experience', 'experience'],
      ['employmentType', 'employmentType'],
      ['source', 'source'],
      ['postedFrom', 'postedFrom'],
      ['postedTo', 'postedTo']
    ];

    for (const [sourceKey, paramName] of setters) {
      const value = query[sourceKey];
      if (typeof value === 'string' && value.trim().length > 0) {
        params = params.set(paramName, value.trim());
      }
    }

    if (typeof query.minSalary === 'number') {
      params = params.set('minSalary', query.minSalary);
    }

    if (typeof query.maxSalary === 'number') {
      params = params.set('maxSalary', query.maxSalary);
    }

    if (typeof query.remote === 'boolean') {
      params = params.set('remote', query.remote);
    }

    if (typeof query.hybrid === 'boolean') {
      params = params.set('hybrid', query.hybrid);
    }

    if (query.skills && query.skills.length > 0) {
      params = params.set('skills', query.skills.join(','));
    }

    return this.http.get<JobSearchResult>('/jobs/search', { params });
  }
}
