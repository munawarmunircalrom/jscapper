import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { JobSearchItem } from '../../../shared/models/job-search.models';

@Component({
  selector: 'app-job-details',
  standalone: true,
  imports: [CommonModule, CardModule, TagModule, ButtonModule],
  templateUrl: './job-details.component.html',
  styleUrl: './job-details.component.scss'
})
export class JobDetailsComponent {
  @Input() job: JobSearchItem | null = null;
  @Output() readonly openOriginal = new EventEmitter<JobSearchItem>();

  get postedDate(): string {
    if (!this.job?.postedAtUtc) {
      return 'N/A';
    }

    return new Date(this.job.postedAtUtc).toLocaleString();
  }
}
