export interface TodoInput {
  name: string;
  isComplete: boolean;
}

export interface Todo extends TodoInput {
  id: number;
}
