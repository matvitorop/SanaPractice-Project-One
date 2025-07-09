import { useState } from 'react';
import { useQuery, useMutation, gql } from '@apollo/client';

export default function TodoList() {
    const { loading, error, data, refetch } = useQuery(GET_TASKS_AND_CATEGORIES);
    const [addTask] = useMutation(ADD_TASK);
    const [completeTask] = useMutation(COMPLETE_TASK);

    const [title, setTitle] = useState('');
    const [duedate, setDueDate] = useState('');
    const [categoryId, setCategoryId] = useState('');

    const handleAddTask = async (e) => {
        e.preventDefault();

        await addTask({
            variables: {
                task: {
                    title,
                    duedate: duedate ? new Date(duedate).toISOString() : null,
                    categoryId: categoryId ? parseInt(categoryId) : null
                }
            }
        });

        setTitle('');
        setDueDate('');
        setCategoryId('');
        refetch();
    };

    const handleCompleteTask = async (id) => {
        await completeTask({ variables: { id } });
        refetch();
    };

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error loading tasks</p>;

    return (
        <div style={{ maxWidth: 600, margin: '0 auto' }}>
            <h2 className="text-center mt-4">ToDo List</h2>

            <form onSubmit={handleAddTask} className="mb-4">
                <input
                    type="text"
                    className="form-control form-control-lg"
                    placeholder="Task to do"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    required
                />
                <input
                    type="datetime-local"
                    className="form-control mt-2"
                    min={new Date().toISOString().slice(0, 16)}
                    value={duedate}
                    onChange={(e) => setDueDate(e.target.value)}
                />
                <select
                    className="form-control mt-2"
                    value={categoryId}
                    onChange={(e) => setCategoryId(e.target.value)}
                >
                    <option value="">Without category</option>
                    {data.categories.map(cat => (
                        <option key={cat.id} value={cat.id}>{cat.name}</option>
                    ))}
                </select>
                <div className="mt-3 text-end">
                    <button type="submit" className="btn btn-primary">Add</button>
                </div>
            </form>

            <h4 className="mb-3">Active tasks</h4>
            <ul className="list-group mb-4">
                {data.activeTasks.map(task => (
                    <li key={task.id} className="list-group-item d-flex justify-content-between align-items-center">
                        <div>
                            <strong>{task.title}</strong>
                            {task.dueDate && <small className="text-muted ms-2">before {task.dueDate}</small>}
                            {task.categoryId && (
                                <span className="badge bg-info text-dark ms-2">
                                    {data.categories.find(c => c.id === task.categoryId)?.name}
                                </span>
                            )}
                        </div>
                        <button className="btn btn-success btn-sm" onClick={() => handleCompleteTask(task.id)}>Done</button>
                    </li>
                ))}
            </ul>

            <h4 className="mb-3">Completed tasks</h4>
            <ul className="list-group">
                {data.completedTasks.map(task => (
                    <li key={task.id} className="list-group-item text-muted" style={{ textDecoration: 'line-through' }}>
                        <strong>{task.title}</strong>
                        {task.completedDate && <small className="ms-2">(completed {task.completedDate})</small>}
                    </li>
                ))}
            </ul>
        </div>
    );
}