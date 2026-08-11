export interface JobSearchItem {
  jobId: string;
  title: string;
  company: string;
  description?: string | null;
  location?: string | null;
  salaryMin?: number | null;
  salaryMax?: number | null;
  currency?: string | null;
  employmentType?: string | null;
  experience?: string | null;
  workMode?: string | null;
  postedAtUtc?: string | null;
  skills: string[];
  sources: string[];
}

export interface JobSearchResult {
  items: JobSearchItem[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  sortBy: string;
  sortDirection: 'asc' | 'desc';
  totalPages: number;
}

export interface JobSearchQuery {
  keyword?: string;
  title?: string;
  company?: string;
  location?: string;
  minSalary?: number;
  maxSalary?: number;
  experience?: string;
  employmentType?: string;
  skills?: string[];
  remote?: boolean;
  hybrid?: boolean;
  source?: string;
  postedFrom?: string;
  postedTo?: string;
  sortBy: string;
  sortDirection: 'asc' | 'desc';
  pageNumber: number;
  pageSize: number;
}
