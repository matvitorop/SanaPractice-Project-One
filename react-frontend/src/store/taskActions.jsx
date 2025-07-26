import {
    FETCH_TASKS_REQUEST,
    ADD_TASK_REQUEST,
    COMPLETE_TASK_REQUEST,
    FETCH_TASKS_SUCCESS,
    ADD_TASK,
    COMPLETE_TASK,
    FETCH_TASKS_FAILURE,
    ADD_TASK_FAILURE,
    COMPLETE_TASK_FAILURE
} from './ActionTypes'

export const fetchTasks = () => ({ type: FETCH_TASKS_REQUEST });

export const addTask = (task) => ({ type: ADD_TASK_REQUEST, payload: task });

export const completeTask = (id) => ({ type: COMPLETE_TASK_REQUEST, payload: id });

export const fetchTasksSuccess = (response) => ({
    type: FETCH_TASKS_SUCCESS,
    payload: {
        activeTasks: response.activeTasks,
        completedTasks: response.completedTasks,
        categories: response.categories
    }
});

export const addTaskSuccess = (response) => ({
    type: ADD_TASK,
    payload: response.data.addTask
});

export const completeTaskSuccess = (response) => ({
    type: COMPLETE_TASK,
    payload: response.data.completeTask
});

export const fetchTasksFailure = () => ({ type: FETCH_TASKS_FAILURE });
export const addTaskFailure = () => ({ type: ADD_TASK_FAILURE });
export const completeTaskFailure = () => ({ type: COMPLETE_TASK_FAILURE });