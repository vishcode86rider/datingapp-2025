import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-start-button',
  imports: [],
  templateUrl: './start-button.html',
  styleUrl: './start-button.css',
})
export class StartButton {
  disabled = input<boolean>();
  selected = input<boolean>();
  clickEvent = output<Event>();

  onClick(event: Event) {
    this.clickEvent.emit(event);
  }

}
