import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'enumKeyValue'
})
export class EnumKeyValuePipe implements PipeTransform {

  transform(value, args: string[]): any {
    const keys = [];
    for (const enumMember in value) {
      if (value.hasOwnProperty(enumMember))
      {
        const isValueProperty = parseInt(enumMember, 10) >= 0;
        if (isValueProperty) {
          keys.push({ key: enumMember, value: value[enumMember] });
        }
      }
    }
    return keys;
  }

}
