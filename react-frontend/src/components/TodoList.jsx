import { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchTasks, addTask, completeTask } from '../store/taskActions'

export default function TodoList() {
    const dispatch = useDispatch();
    const { activeTasks, completedTasks, categories, loading, error } = useSelector(state => state.tasks);

    const [title, setTitle] = useState('');
    const [duedate, setDueDate] = useState('');
    const [categoryId, setCategoryId] = useState('');

    useEffect(() => {
        dispatch(fetchTasks());
    }, [dispatch]);
    

    const handleAddTask = async (e) => {
        e.preventDefault();

        await dispatch(addTask({
            title,
            duedate: duedate ? new Date(duedate).toISOString() : null,
            categoryId: categoryId ? parseInt(categoryId) : null
        }));

        setTitle('');
        setDueDate('');
        setCategoryId('');
    };

    const handleCompleteTask = async (id) => {
        await dispatch(completeTask(id));
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
                    {categories.map(cat => (
                        <option key={cat.id} value={cat.id}>{cat.name}</option>
                    ))}
                </select>
                <div className="mt-3 text-end">
                    <button type="submit" className="btn btn-primary">Add</button>
                </div>
            </form>

            <h4 className="mb-3">Active tasks</h4>
            <ul className="list-group mb-4">
                {activeTasks.map(task => (
                    <li key={task.id} className="list-group-item d-flex justify-content-between align-items-center">
                        <div>
                            <strong>{task.title}</strong>
                            {task.dueDate && <small className="text-muted ms-2">before {task.dueDate}</small>}
                            {task.categoryId && (
                                <span className="badge bg-info text-dark ms-2">
                                    {categories.find(c => c.id === task.categoryId)?.name}
                                </span>
                            )}
                        </div>
                        <button className="btn btn-success btn-sm" onClick={() => handleCompleteTask(task.id)}>Done</button>
                    </li>
                ))}
            </ul>

            <h4 className="mb-3">Completed tasks</h4>
            <ul className="list-group">
                {completedTasks.map(task => (
                    <li key={task.id} className="list-group-item text-muted" style={{ textDecoration: 'line-through' }}>
                        <strong>{task.title}</strong>
                        {task.completedDate && <small className="ms-2">(completed {task.completedDate})</small>}
                    </li>
                ))}
            </ul>
        </div>
    );
}