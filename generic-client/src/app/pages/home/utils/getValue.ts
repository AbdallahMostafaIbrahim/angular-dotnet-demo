import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'getValue' })
export class GetValuePipe implements PipeTransform {
  transform(element: { [k: string]: any }, k: string) {
    console.log(element, k);
    return element[
      Object.keys(element).find(
        (key) => key.toLowerCase() === k.toLowerCase()
      ) || ''
    ];
  }
}
