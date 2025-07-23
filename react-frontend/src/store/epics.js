import { ofType, combineEpics } from 'redux-observable';
import { from, of } from 'rxjs';
import { mergeMap, map, catchError } from 'rxjs/operators';
import Cookies from 'js-cookie';

const GRAPHQL_ENDPOINT = 'https://localhost:7171/graphql';

const GET_TASKS_AND_CATEGORIES = `
  query {
    activeTasks { id title dueDate categoryId }
    completedTasks { id title completedDate }
    categories { id name }
  }
`;

const ADD_TASK = `
  mutation AddTask($task: TaskInput!) {
    addTask(task: $task){
        id
        title
        dueDate
        categoryId
    }
  }
`;

const COMPLETE_TASK = `
  mutation CompleteTask($id: Int!) {
    completeTask(id: $id){
        id
        title
        completedDate
    }
  }
`;

const getStorageType = () => Cookies.get('StorageType') || 'db';

const fetchGraphQl = (query, variables = {}) => {
    return fetch(GRAPHQL_ENDPOINT, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'StorageType': getStorageType(),
        },
        body: JSON.stringify({ query, variables }),
    }).then(response => response.json());
};

const fetchTasksEpic = action$ =>
    action$.pipe(
        ofType('FETCH_TASKS_REQUEST'),
        mergeMap(() =>
            from(fetchGraphQl(GET_TASKS_AND_CATEGORIES)).pipe(
            map(response => {
                const data = response.data;
                return {
                    type: 'FETCH_TASKS_SUCCESS',
                    payload: {
                        activeTasks: data.activeTasks,
                        completedTasks: data.completedTasks,
                        categories: data.categories
                    }
                };
            }),
            catchError(() => of({type: 'FETCH_TASKS_FAILURE'}))
        )
    )
);

const addTaskEpic = action$ =>
    action$.pipe(
        ofType('ADD_TASK_REQUEST'),
        mergeMap(action =>
            from(fetchGraphQl(ADD_TASK, { task: action.payload })).pipe(
                map(response => ({
                    type: 'ADD_TASK',
                    payload: response.data.addTask
                })),
                catchError(() => of({ type: 'ADD_TASK_FAILURE' }))
            )
        )
    );

const completeTaskEpic = action$ =>
    action$.pipe(
        ofType('COMPLETE_TASK_REQUEST'),
        mergeMap(action =>
            from(fetchGraphQl(COMPLETE_TASK, { id: action.payload })).pipe(
                map(response => ({
                    type: 'COMPLETE_TASK',
                    payload: response.data.completeTask
                })),
                catchError(() => of({ type: 'COMPLETE_TASK_FAILURE' }))
            )
        )
    );

export const rootEpic = combineEpics(
    fetchTasksEpic,
    addTaskEpic,
    completeTaskEpic
);