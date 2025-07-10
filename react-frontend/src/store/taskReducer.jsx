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
      case 'FETCH_TASKS_REQUEST':
          return {
              ...state,
              loading: true,
              error: null
          }
      case 'FETCH_TASKS_SUCCESS':
          return {
              ...state,
              loading: false,
              activeTasks: action.payload.activeTasks,
              completedTasks: action.payload.completedTasks,
              categories: action.payload.categories
          }
      case 'FETCH_TASKS_FAILURE':
          return {
              ...state,
              loading: false,
              error: "Error, try later",
          };
      case 'ADD_TASK':
            return {
                ...state,
                activeTasks: [...state.activeTasks, action.payload]
            };
      case 'COMPLETE_TASK':
          { const completedTasks = state.activeTasks.find(task => task.id == action.payload.id);
          return {
              ...state,
              activeTasks: state.activeTasks.filter(task => task.id != action.payload.id),
              completedTasks: [...state.completedTasks, completedTasks]
          } }
      default:
          return state;
  }
}