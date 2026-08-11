import { Injectable } from '@angular/core';
import { JobSearchItem } from '../../../shared/models/job-search.models';

@Injectable({ providedIn: 'root' })
export class JobLinkService {
  openOriginalJob(job: JobSearchItem): void {
    const source = (job.sources[0] ?? '').toLowerCase();
    const query = encodeURIComponent(`${job.title} ${job.company}`.trim());

    const url = this.buildProviderSearchUrl(source, query);
    window.open(url, '_blank', 'noopener,noreferrer');
  }

  private buildProviderSearchUrl(source: string, query: string): string {
    if (source.includes('linkedin')) {
      return `https://www.linkedin.com/jobs/search/?keywords=${query}`;
    }

    if (source.includes('indeed')) {
      return `https://www.indeed.com/jobs?q=${query}`;
    }

    if (source.includes('rozee')) {
      return `https://www.rozee.pk/job/jsearch/q/${query}`;
    }

    if (source.includes('jobi')) {
      return `https://jobi.pk/jobs?keywords=${query}`;
    }

    return `https://www.google.com/search?q=${query}%20job`;
  }
}
