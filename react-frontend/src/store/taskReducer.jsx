import {
    FETCH_TASKS_REQUEST,
    FETCH_TASKS_SUCCESS,
    ADD_TASK,
    COMPLETE_TASK,
    FETCH_TASKS_FAILURE,
} from './ActionTypes'

const initialState =
{
    loading: false,
    error: false, 
    activeTasks: [],
    completedTasks: [],
    categories: []
};

export default function taskReducer(state = initialState, action) {
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