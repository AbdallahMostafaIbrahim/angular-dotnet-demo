import { Component, OnInit } from '@angular/core';
import * as uuid from 'uuid';
import { Todo } from '../../lib/interfaces/todo';
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
    this.todoProvider
      .getTodos()
      .subscribe((res) => (this.todos = res['todos']));
  }

  addTodo() {
    const todo: Todo = {
      id: uuid.v4(),
      name: this.todoInput,
      isComplete: false,
    };
    this.todos.push(todo);
    this.todoProvider.addTodo(todo).subscribe((d) => {
      console.log(d);
    });
  }

  deleteTodo(id: string) {
    this.todoProvider.removeTodo(id).subscribe((d) => {
      console.log(d);
    });
    this.todos = this.todos.filter((todo) => todo.id !== id);
  }

  markComplete(id: string) {
    this.todos = this.todos.map((todo) =>
      todo.id === id ? { ...todo, isComplete: !todo.isComplete } : todo
    );
    this.todoProvider.markCompleted(id).subscribe(() => console.log('toggle'));
  }

  handleInputKeydown(e: KeyboardEvent) {
    if (e.key === 'Enter') {
      this.addTodo();
      this.todoInput = '';
    }
  }
}
