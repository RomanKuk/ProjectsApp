import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'localizedDate',
  pure: false
})
export class LocalizedDatePipe implements PipeTransform {

  constructor() {
  }

  transform(value: Date): string {

    const dateOptions = {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    };

    const date = new Date(value).toLocaleDateString('uk', dateOptions);
    const index = date.lastIndexOf('р');

    return date.slice(0, index - 1);
    }
}
