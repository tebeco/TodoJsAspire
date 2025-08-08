/**
 * TodoItem component represents a single task in the TODO list.
 * It displays the task text and provides buttons to delete the task,
 * move the task up, and move the task down in the list.
 *
 * @param {Object} props - The properties passed to the component.
 * @param {string} props.task - The text of the task.
 * @param {function} props.deleteTaskCallback - Callback function to delete the task.
 * @param {function} props.moveTaskUpCallback - Callback function to move the task up in the list.
 * @param {function} props.moveTaskDownCallback - Callback function to move the task down in the list.
 */

export type TodoItemsProps = {
    task: string, 
    deleteTaskCallback: () => void, 
    moveTaskUpCallback: () => void, 
    moveTaskDownCallback: () => void
} 

export default ({ task, deleteTaskCallback, moveTaskUpCallback, moveTaskDownCallback } :  TodoItemsProps) => {
    return (
        <li aria-label="task">
            <span className="text">{task}</span>
            <button
                type="button"
                aria-label="Delete task"
                className="delete-button"
                onClick={() => deleteTaskCallback()}>
                🗑️
            </button>
            <button
                type="button"
                aria-label="Move task up"
                className="up-button"
                onClick={() => moveTaskUpCallback()}>
                ⇧
            </button>
            <button
                type="button"
                aria-label="Move task down"
                className="down-button"
                onClick={() => moveTaskDownCallback()}>
                ⇩
            </button>
        </li>
    );
}
