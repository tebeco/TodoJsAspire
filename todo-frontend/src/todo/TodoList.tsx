import {useState, useEffect, type ChangeEvent, type FormEvent} from 'react';
import './TodoList.css';
import TodoItem from './TodoItem';
import type {Todo} from "./Todo.ts";

/**
 * Todo component represents the main TODO list application.
 * It allows users to add new todos, delete todos, and move todos up or down in the list.
 * The component maintains the state of the todo list and the new todo input.
 */
export default () => {
    // const [tasks, setTasks] = useState([]);
    const [newTaskText, setNewTaskText] = useState('');
    const [todos, setTodo] = useState<Todo[]>([]);

    const getTodo = async () => {
        fetch("/api/todos")
            .then(response => response.json())
            .then(json => setTodo(json))
            .catch(error => console.error('Error fetching todos:', error));
    }

    useEffect(() => {
        getTodo();
    }, []);

    function handleInputChange(event: ChangeEvent<HTMLInputElement>) {
        setNewTaskText(event.target.value);
    }

    async function addTask(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        if (newTaskText.trim()) {
            // call the API to add the new task
            const result = await fetch("/api/todos", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({title: newTaskText, isComplete: false})
            })
            if (result.ok) {
                await getTodo();
            }
            // TODO: Add some error handling here, inform the user if there was a problem saving the TODO item.

            setNewTaskText('');
        }
    }

    async function deleteTask(id: number) {
        console.log(`deleting todo ${id}`);
        const result = await fetch(`/api/todos/${id}`, {
            method: "DELETE"
        });

        if (result.ok) {
            await getTodo();
        }
        // TODO: Add some error handling here, inform the user if there was a problem saving the TODO item.
    }

    async function moveTaskUp(index: number) {
        console.log(`moving todo ${index} up`);
        const todo = todos[index];
        const result = await fetch(`/api/todos/move-up/${todo.id}`, {
            method: "POST"
        });

        if (result.ok) {
            await getTodo();
        } else {
            console.error('Error moving task up:', result.statusText);
        }
    }

    async function moveTaskDown(index: number) {
        const todo = todos[index];
        const result = await fetch(`/api/todos/move-down/${todo.id}`, {
            method: "POST"
        });

        if (result.ok) {
            await getTodo();
        } else {
            console.error('Error moving task down:', result.statusText);
        }
    }

    return (
        <article
            className="todo-list"
            aria-label="task list manager">
            <header>
                <h1>TODO</h1>
                <form
                    className="todo-input"
                    onSubmit={addTask}
                    aria-controls="todo-list">
                    <input
                        type="text"
                        required
                        autoFocus
                        placeholder="Enter a task"
                        value={newTaskText}
                        aria-label="Task text"
                        onChange={handleInputChange}/>
                    <button
                        className="add-button"
                        aria-label="Add task">
                        Add
                    </button>
                </form>
            </header>
            <ol id="todo-list" aria-live="polite" aria-label="task list">
                {todos.map((task, index) =>
                    <TodoItem
                        key={task.id}
                        task={task.title}
                        deleteTaskCallback={() => deleteTask(task.id)}
                        moveTaskUpCallback={() => moveTaskUp(index)}
                        moveTaskDownCallback={() => moveTaskDown(index)}
                    />
                )}
            </ol>
        </article>
    );
}
