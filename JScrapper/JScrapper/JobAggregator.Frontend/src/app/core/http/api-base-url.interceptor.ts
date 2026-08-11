import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { API_BASE_URL } from '../config/api.constants';

export const apiBaseUrlInterceptor: HttpInterceptorFn = (request, next) => {
  if (/^https?:\/\//i.test(request.url)) {
    return next(request);
  }

  const baseUrl = inject(API_BASE_URL, { optional: true }) ?? '';
  const normalizedBase = baseUrl.replace(/\/$/, '');
  const normalizedPath = request.url.startsWith('/') ? request.url : `/${request.url}`;

  return next(request.clone({ url: `${normalizedBase}${normalizedPath}` }));
};
