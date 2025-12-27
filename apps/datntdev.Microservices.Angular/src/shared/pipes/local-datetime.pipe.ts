import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'localDateTime',
  standalone: false
})
export class LocalDateTimePipe implements PipeTransform {
  transform(value: string | Date | null | undefined): string {
    if (!value) return '';
    
    const date = value instanceof Date ? value : new Date(value.endsWith('Z') ? value : `${value}Z`);

    // Get local time components (JavaScript automatically converts from UTC to local)
    const localYear = date.getFullYear();
    const localMonth = String(date.getMonth() + 1).padStart(2, '0');
    const localDay = String(date.getDate()).padStart(2, '0');
    const localHours = String(date.getHours()).padStart(2, '0');
    const localMinutes = String(date.getMinutes()).padStart(2, '0');
    const localSeconds = String(date.getSeconds()).padStart(2, '0');

    return `${localYear}-${localMonth}-${localDay} ${localHours}:${localMinutes}:${localSeconds}`;
  }
}

