import { Component } from '@angular/core'
import { TaskFilter } from './models/task';
import { Group } from './models/group';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  
  // Variables
  title = 'WellToDo'
  showMenu: boolean = false
  filter: TaskFilter = {}
  group?: Group = undefined

  updateShowMenu(value: boolean) {
    this.showMenu = value;
  }

  updateFilter(value: TaskFilter) {
    this.filter = value;
  }

  updateSelectedGroup(value?: Group) {
    this.group = value
  }
}
