import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { Task, TaskFilter, TaskRequest } from '../models/task'

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = '/api/Task'

  constructor(private http: HttpClient) { }

  getTasks(request?: TaskFilter, page: number = 1, pageSize: number = 10): Observable<Task[]> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())

    if (request?.sentence) {
      params = params.set('sentence', request.sentence)
    }
    if (request?.date) {
      params = params.set('date', request.date.toString())
    }
    if (request?.priority) {
      params = params.set('priority', Number(request.priority))
    }
    if (request?.isChecked != undefined) {
      params = params.set('isChecked', request.isChecked)
    }
    if (request?.isFlagged != undefined) {
      params = params.set('isFlagged', request.isFlagged)
    }
    if (request?.groupId) {
      params = params.set('groupId', request.groupId)
    }

    return this.http.get<Task[]>(this.apiUrl, { params })
  }

  createTask(newTask: Task): Observable<Task> {
    const request: TaskRequest = {
      title: newTask.title,
      description: newTask.description,
      date: newTask.date,
      priority: newTask.priority,
      url: newTask.url,
      isChecked: newTask.isChecked,
      isFlagged: newTask.isFlagged,
      groupId: newTask.groupId
    }
    return this.http.post<Task>(this.apiUrl, request)
  }
  
  updateTask(editedTask: Task): Observable<Task> {
    const request: TaskRequest = {
      title: editedTask.title,
      description: editedTask.description,
      date: editedTask.date,
      priority: editedTask.priority,
      url: editedTask.url,
      isChecked: editedTask.isChecked,
      isFlagged: editedTask.isFlagged,
      groupId: editedTask.groupId
    }
    return this.http.put<Task>(`${this.apiUrl}/${editedTask.id}`, request)
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
