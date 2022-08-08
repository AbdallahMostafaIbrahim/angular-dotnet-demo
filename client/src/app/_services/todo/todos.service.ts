import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_ENDPOINT } from '../../lib/interfaces/constants';
import { NetworkResult } from '../../lib/interfaces/network';
import { Todo } from '../../lib/interfaces/todo';

@Injectable({
  providedIn: 'root',
})
export class TodoService {
  constructor(private httpClient: HttpClient) {}

  public getTodos() {
    return this.httpClient.get<NetworkResult>(`${API_ENDPOINT}todos`);
  }

  public addTodo(todo: Todo) {
    return this.httpClient.post(`${API_ENDPOINT}todos`, todo);
  }

  public removeTodo(id: string) {
    return this.httpClient.delete(`${API_ENDPOINT}todos/${id}`);
  }

  public markCompleted(id: string) {
    return this.httpClient.post(`${API_ENDPOINT}todos/toggle/${id}/`, {});
  }
}
