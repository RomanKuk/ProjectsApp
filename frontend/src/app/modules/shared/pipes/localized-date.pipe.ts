import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'localizedDate',
  pure: false
})
export class LocalizedDatePipe implements PipeTransform {

  constructor() {
  }

  transform(value: Date): string {

    let dateOptions = {
      year: "numeric",
      month: "long",
      day: "numeric"
    };

    let date = new Date(value).toLocaleDateString("uk", dateOptions);
    var index = date.lastIndexOf('р');

    return date.slice(0, index - 1);
    }
}