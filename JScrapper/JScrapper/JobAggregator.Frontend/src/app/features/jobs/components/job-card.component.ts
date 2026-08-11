import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { JobSearchItem } from '../../../shared/models/job-search.models';

@Component({
  selector: 'app-job-card',
  standalone: true,
  imports: [CommonModule, ButtonModule, TagModule],
  templateUrl: './job-card.component.html',
  styleUrl: './job-card.component.scss'
})
export class JobCardComponent {
  @Input({ required: true }) job!: JobSearchItem;
  @Input() selected = false;

  @Output() readonly selectedJob = new EventEmitter<JobSearchItem>();
  @Output() readonly openOriginal = new EventEmitter<JobSearchItem>();

  get postedDate(): string {
    if (!this.job.postedAtUtc) {
      return 'N/A';
    }

    return new Date(this.job.postedAtUtc).toLocaleDateString();
  }

  get salaryText(): string {
    const min = this.job.salaryMin;
    const max = this.job.salaryMax;
    const currency = this.job.currency ?? '';

    if (min == null && max == null) {
      return 'Not specified';
    }

    if (min != null && max != null) {
      return `${currency} ${min.toLocaleString()} - ${max.toLocaleString()}`.trim();
    }

    const single = min ?? max;
    return `${currency} ${single?.toLocaleString() ?? ''}`.trim();
  }
}
