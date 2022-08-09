import { Component, OnInit } from '@angular/core';
import * as uuid from 'uuid';
import { Todo, TodoInput } from '../../_models/todo';
import { TodoService } from '../../_services/todo/todos.service';

@Component({
  selector: 'app-todos',
  templateUrl: './todos.component.html',
  styleUrls: ['./todos.component.scss'],
})
export class TodosComponent implements OnInit {
  todos: Todo[] = [];

  todoInput: string = '';

  constructor(private todoProvider: TodoService) {}

  ngOnInit(): void {
    this.fetchTodos();
  }

  fetchTodos() {
    this.todoProvider.getTodos().subscribe((res) => (this.todos = res.todos));
  }

  addTodo() {
    const todo: TodoInput = {
      name: this.todoInput,
      isComplete: false,
    };
    this.todoProvider.addTodo(todo).subscribe((d) => {
      this.todos.push(d.todo);
    });
  }

  deleteTodo(id: number) {
    this.todoProvider.removeTodo(id).subscribe((d) => {
      this.todos = this.todos.filter((todo) => todo.id !== id);
    });
  }

  markComplete(id: number) {
    this.todoProvider.markCompleted(id).subscribe(() => {
      this.todos = this.todos.map((todo) =>
        todo.id === id ? { ...todo, isComplete: !todo.isComplete } : todo
      );
    });
  }

  handleInputKeydown(e: KeyboardEvent) {
    if (e.key === 'Enter') {
      this.addTodo();
      this.todoInput = '';
    }
  }
}
