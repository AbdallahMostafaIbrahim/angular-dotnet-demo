import { Pipe, PipeTransform } from '@angular/core';

function getValueFromKey(element: { [k: string]: any }, k: string) {
  return (
    element[
      Object.keys(element || {}).find(
        (key) => key.toLowerCase() === k.toLowerCase()
      ) || ''
    ] || ''
  );
}

@Pipe({ name: 'getValue' })
export class GetValuePipe implements PipeTransform {
  transform(element: { [k: string]: any }, k: string): any {
    if (k.includes('.')) {
      const keys = k.split('.');
      return this.transform(
        getValueFromKey(element, keys[0]),
        keys.slice(1).join('.')
      );
    }
    return getValueFromKey(element, k);
  }
}
