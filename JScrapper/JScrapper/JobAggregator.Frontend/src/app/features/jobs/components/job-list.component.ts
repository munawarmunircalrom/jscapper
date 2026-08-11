import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { JobSearchItem } from '../../../shared/models/job-search.models';
import { JobCardComponent } from './job-card.component';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [CommonModule, PaginatorModule, JobCardComponent],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.scss'
})
export class JobListComponent {
  @Input() jobs: JobSearchItem[] = [];
  @Input() loading = false;
  @Input() selectedJobId: string | null = null;
  @Input() totalCount = 0;
  @Input() first = 0;
  @Input() pageSize = 20;

  @Output() readonly jobSelected = new EventEmitter<JobSearchItem>();
  @Output() readonly openOriginal = new EventEmitter<JobSearchItem>();
  @Output() readonly pageChanged = new EventEmitter<number>();

  onPage(event: PaginatorState): void {
    const pageIndex = event.page ?? 0;
    this.pageChanged.emit(pageIndex + 1);
  }
}
