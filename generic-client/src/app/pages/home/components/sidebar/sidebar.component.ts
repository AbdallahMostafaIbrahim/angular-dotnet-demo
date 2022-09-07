import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  animations: [
    trigger('openClose', [
      state(
        'open',
        style({
          right: '20px',
          opacity: 1,
        })
      ),
      state(
        'closed',
        style({
          right: '-300px',
          opacity: 0.8,
        })
      ),
      transition('open => closed', [
        animate('200ms cubic-bezier(0.4, 0.0, 0.2, 1)'),
      ]),
      transition('closed => open', [
        animate('400ms cubic-bezier(0.4, 0.0, 0.2, 1)'),
      ]),
    ]),
    trigger('arrowOpenClose', [
      state(
        'open',
        style({
          opacity: 0,
        })
      ),
      state(
        'closed',
        style({
          opacity: 1,
        })
      ),
      transition('open => closed', [
        animate('200ms cubic-bezier(0.4, 0.0, 0.2, 1)'),
      ]),
      transition('closed => open', [
        animate('200ms cubic-bezier(0.4, 0.0, 0.2, 1)'),
      ]),
    ]),
  ],
})
export class SidebarComponent implements OnInit {
  isOpen = false;

  toggle() {
    this.isOpen = !this.isOpen;
  }

  constructor() {}

  ngOnInit(): void {}
}
