import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { Group, GroupRequest } from '../models/group'

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private apiUrl = '/api/Group'

  constructor(private http: HttpClient) { }

  getGroups(page: number = 1, pageSize: number = 10): Observable<Group[]> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
    return this.http.get<Group[]>(this.apiUrl, { params })
  }

  createGroup(newGroup: Group): Observable<Group> {
    const request: GroupRequest = {
      title: newGroup.title,
      color: newGroup.color
    }
    return this.http.post<Group>(this.apiUrl, request)
  }
  
  updateGroup(editedGroup: Group): Observable<Group> {
    const request: GroupRequest = {
        title: editedGroup.title,
        color: editedGroup.color
    }
    return this.http.put<Group>(`${this.apiUrl}/${editedGroup.id}`, request)
  }

  deleteGroup(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
