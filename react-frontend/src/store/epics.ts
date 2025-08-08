import { ofType, combineEpics, Epic } from 'redux-observable';
import { from, of } from 'rxjs';
import { mergeMap, map, catchError } from 'rxjs/operators';
import Cookies from 'js-cookie';

import {
    FETCH_TASKS_REQUEST,
    ADD_TASK_REQUEST,
    COMPLETE_TASK_REQUEST,
} from './ActionTypes';

import {
    fetchTasksSuccess,
    fetchTasksFailure,
    addTaskSuccess,
    addTaskFailure,
    completeTaskSuccess,
    completeTaskFailure,
} from './taskActions';

import type { RootState, AppDispatch } from './store';
import type { Action } from 'redux';

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

const fetchGraphQl = (query: string, variables: Record<string, any> = {}) => {
    return fetch(GRAPHQL_ENDPOINT, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'StorageType': getStorageType(),
        },
        body: JSON.stringify({ query, variables }),
    }).then(response => response.json());
};

type MyEpic = Epic<Action, Action, RootState, AppDispatch>;

export const fetchTasksEpic: MyEpic = (action$) =>
    action$.pipe(
        ofType(FETCH_TASKS_REQUEST),
        mergeMap(() =>
            from(fetchGraphQl(GET_TASKS_AND_CATEGORIES)).pipe(
                map(response => fetchTasksSuccess(response.data)),
                catchError(() => of(fetchTasksFailure()))
            )
        )
    );

export const addTaskEpic: MyEpic = (action$) =>
    action$.pipe(
        ofType(ADD_TASK_REQUEST),
        mergeMap(action =>
            from(fetchGraphQl(ADD_TASK, { task: (action as any).payload })).pipe(
                map(response => addTaskSuccess(response)),
                catchError(() => of(addTaskFailure()))
            )
        )
    );

export const completeTaskEpic: MyEpic = (action$) =>
    action$.pipe(
        ofType(COMPLETE_TASK_REQUEST),
        mergeMap(action =>
            from(fetchGraphQl(COMPLETE_TASK, { id: (action as any).payload })).pipe(
                map(response => completeTaskSuccess(response)),
                catchError(() => of(completeTaskFailure()))
            )
        )
    );

export const rootEpic = combineEpics(
    fetchTasksEpic,
    addTaskEpic,
    completeTaskEpic
);