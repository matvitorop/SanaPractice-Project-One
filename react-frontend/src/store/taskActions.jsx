export const fetchTasks = () => ({ type: 'FETCH_TASKS_REQUEST' });

export const addTask = (task) => ({ type: 'ADD_TASK_REQUEST', payload: task });

export const completeTask = (id) => ({ type: 'COMPLETE_TASK_REQUEST', payload: id });