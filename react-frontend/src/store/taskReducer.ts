import {
    FETCH_TASKS_REQUEST,
    FETCH_TASKS_SUCCESS,
    ADD_TASK,
    COMPLETE_TASK,
    FETCH_TASKS_FAILURE,
} from './ActionTypes'

import type { Task, Category, TaskState } from './types';

const initialState : TaskState =
{
    loading: false,
    error: false, 
    activeTasks: [],
    completedTasks: [],
    categories: []
};

type Action =
    | { type: typeof FETCH_TASKS_REQUEST }
    | {
        type: typeof FETCH_TASKS_SUCCESS;
        payload: {
            activeTasks: Task[];
            completedTasks: Task[];
            categories: Category[];
        };
    }
    | { type: typeof FETCH_TASKS_FAILURE }
    | { type: typeof ADD_TASK; payload: Task }
    | { type: typeof COMPLETE_TASK; payload: Task }
    | { type: string; payload?: any };

export default function taskReducer(state = initialState, action : Action) {
  switch (action.type) {
      case FETCH_TASKS_REQUEST:
          return {
              ...state,
              loading: true,
              error: null
          }
      case FETCH_TASKS_SUCCESS:
          return {
              ...state,
              loading: false,
              activeTasks: action.payload.activeTasks,
              completedTasks: action.payload.completedTasks,
              categories: action.payload.categories
          }
      case FETCH_TASKS_FAILURE:
          return {
              ...state,
              loading: false,
              error: "Error, try later",
          };
      case ADD_TASK:
            return {
                ...state,
                activeTasks: [...state.activeTasks, action.payload]
            };
      case COMPLETE_TASK:
          return {
              ...state,
              activeTasks: state.activeTasks.filter(task => task.id != action.payload.id),
              completedTasks: [action.payload, ...state.completedTasks]
          };
      default:
          return state;
  }
}