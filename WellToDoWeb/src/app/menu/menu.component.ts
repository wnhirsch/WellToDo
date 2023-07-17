import { Component, Input, Output, EventEmitter } from '@angular/core';
import { TaskFilter, TaskPriority } from '../models/task';
import { Group } from '../models/group';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent {
  @Input() showMenu: boolean = false;
  @Output() showMenuChange = new EventEmitter<boolean>();
  @Output() filterChange = new EventEmitter<TaskFilter>();
  @Output() selectedGroupChange = new EventEmitter<Group>();

  constructor(private translateService: TranslateService) { }

  // Variables
  filter: TaskFilter = {}

  // Constants
  maxLength = 4000

  // View Methods
  getCheckText(): string {
    switch (this.filter.isChecked) {
      case true:
        return this.translateService.instant('menu.check.true')
      case false:
        return this.translateService.instant('menu.check.false')
      default:
        return this.translateService.instant('menu.check.null')
    }
  }

  getCheckClass(): string {
    switch (this.filter.isChecked) {
      case true:
        return "check-true"
      case false:
        return "check-false"
      default:
        return ""
    }
  }

  getPriorityText(): string {
    switch (this.filter.priority) {
      case TaskPriority.High:
        return this.translateService.instant('menu.priority.high')
      case TaskPriority.Medium:
        return this.translateService.instant('menu.priority.medium')
      case TaskPriority.Low:
        return this.translateService.instant('menu.priority.low')
      default:
        return this.translateService.instant('menu.priority.null')
    }
  }

  getPriorityClass(): string {
    switch (this.filter.priority) {
      case TaskPriority.High:
        return "priority-high"
      case TaskPriority.Medium:
        return "priority-medium"
      case TaskPriority.Low:
        return "priority-low"
      default:
        return ""
    }
  }

  getFlagText(): string {
    switch (this.filter.isFlagged) {
      case true:
        return this.translateService.instant('menu.flag.true')
      case false:
        return this.translateService.instant('menu.flag.false')
      default:
        return this.translateService.instant('menu.flag.null')
    }
  }

  getFlagClass(): string {
    switch (this.filter.isFlagged) {
      case true:
        return "flag-true"
      case false:
        return "flag-false"
      default:
        return ""
    }
  }

  // Event Handlers
  toggleMenu() {
    this.showMenu = !this.showMenu
    this.showMenuChange.emit(this.showMenu)
  }

  onInputFocusOut(event: FocusEvent) {
    this.filterChange.emit({ ...this.filter });
  }

  toggleCheck() {
    switch (this.filter.isChecked) {
      case true:
        this.filter.isChecked = undefined
        break;
      case false:
        this.filter.isChecked = true
        break;
      default:
        this.filter.isChecked = false
        break;
    }
    this.filterChange.emit({ ...this.filter });
  }

  togglePriority() {
    switch (this.filter.priority) {
      case TaskPriority.High:
        this.filter.priority = undefined
        break;
      case TaskPriority.Medium:
        this.filter.priority = TaskPriority.High
        break;
      case TaskPriority.Low:
        this.filter.priority = TaskPriority.Medium
        break;
      default:
        this.filter.priority = TaskPriority.Low
        break;
    }
    this.filterChange.emit({ ...this.filter });
  }

  toggleFlag() {
    switch (this.filter.isFlagged) {
      case true:
        this.filter.isFlagged = undefined
        break;
      case false:
        this.filter.isFlagged = true
        break;
      default:
        this.filter.isFlagged = false
        break;
    }
    this.filterChange.emit({ ...this.filter });
  }

  updateSelectedGroup(value?: Group) {
    this.filter.groupId = value?.id
    this.selectedGroupChange.emit(value)
  }
}
