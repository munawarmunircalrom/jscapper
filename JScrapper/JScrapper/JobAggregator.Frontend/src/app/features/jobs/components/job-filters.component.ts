import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { DatePickerModule } from 'primeng/datepicker';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TagModule } from 'primeng/tag';
import { JobFilters } from '../../../shared/models/job-filter.models';

interface SortOption {
  label: string;
  value: { sortBy: string; sortDirection: 'asc' | 'desc' };
}

@Component({
  selector: 'app-job-filters',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CardModule,
    InputTextModule,
    InputNumberModule,
    DatePickerModule,
    CheckboxModule,
    SelectModule,
    ButtonModule,
    TagModule
  ],
  templateUrl: './job-filters.component.html',
  styleUrl: './job-filters.component.scss'
})
export class JobFiltersComponent {
  @Output() readonly filtersChanged = new EventEmitter<JobFilters>();
  @Output() readonly sortChanged = new EventEmitter<{ sortBy: string; sortDirection: 'asc' | 'desc' }>();

  readonly filters = signal<JobFilters>({
    keyword: '',
    title: '',
    company: '',
    location: '',
    minSalary: undefined,
    maxSalary: undefined,
    experience: '',
    employmentType: '',
    skills: [],
    remote: undefined,
    hybrid: undefined,
    source: '',
    postedFrom: undefined,
    postedTo: undefined
  });

  readonly skillsInput = signal('');

  readonly sortOptions: SortOption[] = [
    { label: 'Newest', value: { sortBy: 'postedDate', sortDirection: 'desc' } },
    { label: 'Oldest', value: { sortBy: 'postedDate', sortDirection: 'asc' } },
    { label: 'Salary High to Low', value: { sortBy: 'salary', sortDirection: 'desc' } },
    { label: 'Salary Low to High', value: { sortBy: 'salary', sortDirection: 'asc' } },
    { label: 'Title A-Z', value: { sortBy: 'title', sortDirection: 'asc' } }
  ];

  selectedSort: SortOption = this.sortOptions[0];

  updateTextFilter(
    key: 'keyword' | 'title' | 'company' | 'location' | 'experience' | 'employmentType' | 'source',
    value: string
  ): void {
    this.filters.set({ ...this.filters(), [key]: value });
  }

  updateNumberFilter(key: 'minSalary' | 'maxSalary', value: number | null | undefined): void {
    this.filters.set({ ...this.filters(), [key]: value ?? undefined });
  }

  updateDateFilter(key: 'postedFrom' | 'postedTo', value: Date | null | undefined): void {
    this.filters.set({ ...this.filters(), [key]: value ?? undefined });
  }

  updateFlag(key: 'remote' | 'hybrid', value: boolean): void {
    this.filters.set({ ...this.filters(), [key]: value ? true : undefined });
  }

  onApply(): void {
    this.filtersChanged.emit({ ...this.filters() });
    this.sortChanged.emit({ ...this.selectedSort.value });
  }

  onReset(): void {
    this.filters.set({
      keyword: '',
      title: '',
      company: '',
      location: '',
      minSalary: undefined,
      maxSalary: undefined,
      experience: '',
      employmentType: '',
      skills: [],
      remote: undefined,
      hybrid: undefined,
      source: '',
      postedFrom: undefined,
      postedTo: undefined
    });
    this.skillsInput.set('');
    this.selectedSort = this.sortOptions[0];
    this.onApply();
  }

  addSkillsFromInput(): void {
    const raw = this.skillsInput();
    const nextSkills = raw
      .split(',')
      .map((value) => value.trim())
      .filter((value) => value.length > 0);

    const current = this.filters();
    const merged = Array.from(new Set([...current.skills, ...nextSkills]));

    this.filters.set({ ...current, skills: merged });
    this.skillsInput.set('');
  }

  removeSkill(skill: string): void {
    const current = this.filters();
    this.filters.set({
      ...current,
      skills: current.skills.filter((s) => s !== skill)
    });
  }
}
