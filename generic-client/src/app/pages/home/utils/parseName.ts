import { Pipe, PipeTransform } from '@angular/core';
import { FieldFlatNode } from 'src/app/lib/interfaces/model';

@Pipe({ name: 'parseName' })
export class ParseNamePipe implements PipeTransform {
  transform(s: FieldFlatNode): any {
    const split = s.name.split('.');
    if (s.name.includes('.')) {
      return [...split.slice(0, split.length - 1), s.displayName].join('->');
    }
    return s.displayName;
  }
}
