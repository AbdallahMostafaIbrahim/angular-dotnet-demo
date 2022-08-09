import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_ENDPOINT } from '../../_models/constants';
import { Todo, TodoInput } from '../../_models/todo';

@Injectable({
  providedIn: 'root',
})
export class TodoService {
  constructor(private httpClient: HttpClient) {}

  public getTodos() {
    return this.httpClient.get<any>(`${API_ENDPOINT}todos`);
  }

  public addTodo(todo: TodoInput) {
    return this.httpClient.post<{ todo: Todo }>(`${API_ENDPOINT}todos`, todo);
  }

  public removeTodo(id: number) {
    return this.httpClient.delete(`${API_ENDPOINT}todos/${id}`);
  }

  public markCompleted(id: number) {
    return this.httpClient.post(`${API_ENDPOINT}todos/toggle/${id}/`, {});
  }
}
