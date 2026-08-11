export interface JobFilters {
  keyword: string;
  title: string;
  company: string;
  location: string;
  minSalary?: number;
  maxSalary?: number;
  experience: string;
  employmentType: string;
  skills: string[];
  remote?: boolean;
  hybrid?: boolean;
  source: string;
  postedFrom?: Date;
  postedTo?: Date;
}
