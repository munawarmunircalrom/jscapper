import { describe, expect, it, vi, beforeEach, afterEach } from 'vitest';
import { JobLinkService } from './job-link.service';
import { JobSearchItem } from '../../../shared/models/job-search.models';

describe('JobLinkService', () => {
  const service = new JobLinkService();

  const baseJob: JobSearchItem = {
    jobId: '1',
    title: 'Senior .NET Developer',
    company: 'Acme',
    description: null,
    location: 'Lahore',
    salaryMin: null,
    salaryMax: null,
    currency: null,
    employmentType: null,
    experience: null,
    workMode: null,
    postedAtUtc: null,
    skills: [],
    sources: ['LinkedIn']
  };

  beforeEach(() => {
    vi.stubGlobal('window', { open: vi.fn() });
  });

  afterEach(() => {
    vi.unstubAllGlobals();
    vi.restoreAllMocks();
  });

  it('opens provider-specific URL for LinkedIn source', () => {
    service.openOriginalJob(baseJob);

    expect(window.open).toHaveBeenCalledOnce();
    expect(window.open).toHaveBeenCalledWith(
      expect.stringContaining('linkedin.com/jobs/search'),
      '_blank',
      'noopener,noreferrer'
    );
  });

  it('falls back to google search for unknown source', () => {
    service.openOriginalJob({ ...baseJob, sources: ['UnknownSource'] });

    expect(window.open).toHaveBeenCalledWith(
      expect.stringContaining('google.com/search'),
      '_blank',
      'noopener,noreferrer'
    );
  });
});
