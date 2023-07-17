import { Component, OnInit, HostListener, Input, Output, EventEmitter, SimpleChanges, OnChanges } from '@angular/core'
import { TaskService } from '../services/task.service'
import { Task, TaskFilter, TaskPriority } from '../models/task'
import { isEqual } from 'lodash';
import { Group } from '../models/group';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit, OnChanges {
  @Input() showMenu: boolean = false;
  @Input() filter: TaskFilter = {};
  @Input() group?: Group = undefined;
  @Output() showMenuChange = new EventEmitter<boolean>();

  // Variables
  tasks: Task[] = []
  newTask?: Task = undefined
  editTask?: Task = undefined
  selectedTask?: Number = undefined
  page: number = 1
  pageSize: number = 10

  // Constants
  maxUrlLength: number = 2048

  constructor(private taskService: TaskService, private translateService: TranslateService) { }

  ngOnInit(): void {
    this.getTasks()
  }

  ngOnChanges(changes: SimpleChanges): void {
    for (const propName in changes) {
      if (propName === "filter" && !changes[propName].firstChange) {
        this.resetService()
      } else if (propName === "group" && !changes[propName].firstChange) {
        this.filter.groupId = this.group?.id
        this.resetService()
      }
    }
  }

  // View Methods
  getPriorityExclamation(priority: TaskPriority): string {
    switch (priority) {
      case TaskPriority.High:
        return '!!!'
      case TaskPriority.Medium:
        return '!!'
      default:
        return '!'
    }
  }

  getPriorityText(priority?: TaskPriority): string {
    switch (priority) {
      case TaskPriority.High:
        return this.translateService.instant('task.priority.high')
      case TaskPriority.Medium:
        return this.translateService.instant('task.priority.medium')
      case TaskPriority.Low:
        return this.translateService.instant('task.priority.low')
      default:
        return this.translateService.instant('task.priority.null')
    }
  }

  getPriorityClass(priority?: TaskPriority): string {
    switch (priority) {
      case TaskPriority.High:
        return 'priority-high'
      case TaskPriority.Medium:
        return 'priority-medium'
      case TaskPriority.Low:
        return 'priority-low'
      default:
        return ''
    }
  }

  getTaskList(): Task[] {
    if (this.newTask) {
      return [this.newTask, ...this.tasks]
    } else {
      return this.tasks
    }
  }

  getTaskLateClass(task: Task): string {
    if (new Date(task.date + "Z") < new Date()) {
      return "date-late"
    } else {
      return ""
    }
  }

  initLocalDate(task: Task) {
    const element = document.getElementById('task-date-' + task.id) as HTMLInputElement
    if (element) {
      var localDate = new Date(task.date)
      const tkOffset = localDate.getTimezoneOffset()
      localDate.setMinutes(localDate.getMinutes() - tkOffset)
      element.value = localDate.toISOString().substring(0, 16)
    }
  }

  // Service Methods
  getTasks() {
    if (this.page === -1) { return }

    this.taskService.getTasks(this.filter, this.page, this.pageSize).subscribe(tasks => {
      const tzOffset = new Date().getTimezoneOffset();
      this.tasks = [...this.tasks, ...tasks.map(task => {
        task.date = new Date(task.date + "Z")
        return task
      })]
      this.page = (tasks.length === this.pageSize) ? (this.page + 1) : -1
    })
  }

  createTask(task: Task) {
    this.taskService.createTask(task).subscribe(() => {
      this.resetService()
    })
  }

  updateTask(task: Task) {
    this.taskService.updateTask(task).subscribe(() => {
      this.resetService()
    })
  }

  deleteTask(deletedTask: Task) {
    this.taskService.deleteTask(deletedTask.id).subscribe(() => {
      this.resetService()
    })
  }

  resetService() {
    this.page = 1
    this.tasks = []
    this.getTasks()
  }

  // Event Handlers
  @HostListener('window:beforeunload', ['$event'])
  onBeforeUnload(event: BeforeUnloadEvent) {
    if (this.newTask || this.editTask) {
      event.preventDefault()
      event.returnValue = ""

      if (this.newTask) {
        if (this.newTask.title.length > 0) {
          this.createTask(this.newTask)
        }
        this.newTask = undefined
      } else if (this.editTask) {
        if (this.editTask.title.length > 0) {
          this.updateTask(this.editTask)
        } else {
          this.deleteTask(this.editTask)
        }
        this.editTask = undefined
      }
    }
  }

  toggleMenu() {
    this.showMenu = !this.showMenu
    this.showMenuChange.emit(this.showMenu)
  }

  onAddTaskClick() {
    this.newTask = {
      id: -1,
      title: "",
      date: this.filter?.date ?? new Date(),
      priority: this.filter?.priority,
      isChecked: this.filter?.isChecked ?? false,
      isFlagged: this.filter?.isFlagged ?? false,
      groupId: this.filter?.groupId
    }
    this.selectedTask = -1
    this.initLocalDate(this.newTask)
  }

  onCardFocusIn(event: FocusEvent, task: Task) {
    

    const checkboxElement = event.target as HTMLElement;
    if (checkboxElement.id === `checkbox-${task.id}`) {
      this.selectedTask = undefined
    } else {
      this.selectedTask = task.id
      this.initLocalDate(task)
    }

    if (task.id != this.newTask?.id) {
      this.newTask = undefined
      this.editTask = { ...task }
    }
  }

  onCardFocusOut(event: FocusEvent, task: Task) {
    const cardElement = event.currentTarget as HTMLElement;
    const focusedElement = event.relatedTarget as HTMLElement;
    
    if (focusedElement === null
      || !cardElement.contains(focusedElement))
    {
      this.selectedTask = undefined
      if (task.id === this.newTask?.id) {
        this.newTask = undefined
        if (task.title.length > 0) {
          this.createTask(task)
        }
      } else if (task.id === this.editTask?.id && !isEqual(task, this.editTask)) {
        this.editTask = undefined
        if (task.title.length > 0) {
          this.updateTask(task)
        } else {
          this.deleteTask(task)
        }
      }
    }
  }

  onCheckboxChange(event: Event, task: Task) {
    const isChecked = (event.target as HTMLInputElement).checked;
    task.isChecked = isChecked;
  }

  onDateChange(event: Event, task: Task) {
    task.date = new Date((event.target as HTMLInputElement).value)
  }

  onURLFocusOut(event: FocusEvent, task: Task) {
    var value = (event.target as HTMLElement).innerText.replace(/\n/g, '').trim().substring(0, this.maxUrlLength)
    if (value != this.translateService.instant('task.url.placeholder')) {
      task.url = value;
      (event.target as HTMLElement).innerText = task.url
    }
  }

  onURLKeyDown(event: KeyboardEvent, task: Task) {
    if (event.key === 'Enter') {
      event.preventDefault()
    }

    var value = (event.target as HTMLElement).innerText.replace(/\n/g, '').trim().substring(0, this.maxUrlLength)
    if (value === this.translateService.instant('task.url.placeholder')) {
      (event.target as HTMLElement).innerText = ""
    }
  }

  openTaskLink(task: Task): void {
    if (!task.url?.startsWith("http://") && !task.url?.startsWith("https://")) {
      window.open("http://" + task.url, '_blank');
    } else {
      window.open(task.url, '_blank');
    }
  }

  togglePriority(task: Task) {
    switch (task.priority) {
      case TaskPriority.High:
        task.priority = undefined
        break;
      case TaskPriority.Medium:
        task.priority = TaskPriority.High
        break;
      case TaskPriority.Low:
        task.priority = TaskPriority.Medium
        break;
      default:
        task.priority = TaskPriority.Low
        break;
    }
  }

  toggleFlag(task: Task) {
    task.isFlagged = !task.isFlagged
  }
}
