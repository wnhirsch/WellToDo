import { Component, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { Group } from '../models/group';
import { GroupService } from '../services/group.service';
import { isEqual } from 'lodash';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.scss']
})
export class GroupListComponent implements OnInit {
  @Output() selectedGroupChange = new EventEmitter<Group>();

  // Variables
  groups: Group[] = []
  newGroup?: Group = undefined
  editGroup?: Group = undefined
  selectedGroup?: Group = undefined
  isEditing: boolean = false
  page: number = 1
  pageSize: number = 10

  constructor(private groupService: GroupService, private translateService: TranslateService) { }

  ngOnInit(): void {
    this.getGroups()
  }

  // View Methods
  getGroupList(): Group[] {
    if (this.newGroup) {
      return [this.newGroup, ...this.groups]
    } else {
      return this.groups
    }
  }

  // Service Methods
  getGroups() {
    if (this.page === -1) { return }

    this.groupService.getGroups(this.page, this.pageSize).subscribe(groups => {
      this.groups = [...this.groups, ...groups]
      this.page = (groups.length === this.pageSize) ? (this.page + 1) : -1
    })
  }

  createGroup(group: Group) {
    this.groupService.createGroup(group).subscribe(() => {
      this.resetService()
    })
  }

  updateGroup(group: Group) {
    this.groupService.updateGroup(group).subscribe(() => {
      this.resetService()
    })
  }

  deleteGroup(deletedGroup: Group) {
    this.groupService.deleteGroup(deletedGroup.id).subscribe(() => {
      this.resetService()
    })
  }

  resetService() {
    this.page = 1
    this.groups = []
    this.getGroups()
  }

  // Event Handlers
  @HostListener('window:beforeunload', ['$event'])
  onBeforeUnload(event: BeforeUnloadEvent) {
    if (this.newGroup || this.editGroup) {
      event.preventDefault()
      event.returnValue = ""

      if (this.newGroup) {
        if (this.newGroup.title.length > 0) {
          this.createGroup(this.newGroup)
        }
        this.newGroup = undefined
      } else if (this.editGroup) {
        if (this.editGroup.title.length > 0) {
          this.updateGroup(this.editGroup)
        }
        this.editGroup = undefined
      }
    }
  }

  onEditGroupsClick() {
    if (this.isEditing) {
      this.newGroup = undefined
    } else {
      this.selectedGroup = undefined
      this.selectedGroupChange.emit(undefined)
    }

    this.isEditing = !this.isEditing
  }

  onAddGroupClick() {
    this.newGroup = {
      id: -1,
      title: "",
      color: "#FFFFFF"
    }
  }

  onCardClick(group: Group) {
    if (this.isEditing) { return }
    if (this.selectedGroup?.id === group.id) {
      this.selectedGroup = undefined
      this.selectedGroupChange.emit(undefined)
    } else {
      this.selectedGroup = group
      this.selectedGroupChange.emit(group)
    }
  }

  onCardFocusIn(event: FocusEvent, group: Group) {
    if (!this.isEditing) { return }
    if (group.id != this.newGroup?.id) {
      this.newGroup = undefined
      this.editGroup = { ...group }
    }
  }

  onCardFocusOut(event: FocusEvent, group: Group) {
    if (!this.isEditing) { return }
    const cardElement = event.currentTarget as HTMLElement;
    const focusedElement = event.relatedTarget as HTMLElement;
    
    if (focusedElement === null
      || !cardElement.contains(focusedElement))
    {
      this.selectedGroup = undefined
      if (group.id === this.newGroup?.id) {
        this.newGroup = undefined
        if (group.title.length > 0) {
          this.createGroup(group)
        }
      } else if (group.id === this.editGroup?.id && !isEqual(group, this.editGroup)) {
        if (group.title.length > 0) {
          this.editGroup = undefined
          this.updateGroup(group)
        } else {
          group.title = this.editGroup.title
          this.editGroup = undefined
        }
      }
    }
  }

  onChangeColor(event: Event, group: Group) {
    group.color = (event.target as HTMLInputElement).value
  }

  onDeleteGroupClick(group: Group) {
    this.deleteGroup(group)
  }
}
