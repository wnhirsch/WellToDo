export interface Task {
  id: number
  title: string
  description?: string
  date: Date
  priority?: TaskPriority
  url?: string
  isChecked: boolean
  isFlagged: boolean
  groupId?: number
}

export interface TaskFilter {
  sentence?: string
  date?: Date
  priority?: TaskPriority
  isChecked?: boolean
  isFlagged?: boolean
  groupId?: number
}

export interface TaskRequest {
  title: string
  description?: string
  date: Date
  priority?: TaskPriority
  url?: string
  isChecked: boolean
  isFlagged: boolean
  groupId?: number
}

export enum TaskPriority {
  Low = 1,
  Medium = 2,
  High = 3
}