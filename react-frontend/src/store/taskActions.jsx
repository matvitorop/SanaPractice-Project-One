import { gql } from '@apollo/client';
import client from '../ApolloClient.jsx';

const GET_TASKS_AND_CATEGORIES = gql`
  query {
    activeTasks { id title dueDate categoryId }
    completedTasks { id title completedDate }
    categories { id name }
  }
`;

const ADD_TASK = gql`
  mutation AddTask($task: TaskInput!) {
    addTask(task: $task){
        id
        title
        dueDate
        categoryId
    }
  }
`;

const COMPLETE_TASK = gql`
  mutation CompleteTask($id: Int!) {
    completeTask(id: $id){
        id
        title
        completedDate
    }
  }
`;

export const fetchTasks = () => async (dispatch) => {
    dispatch({ type: 'FETCH_TASKS_REQUEST' });
    try {
        const { data } = await client.query({ query: GET_TASKS_AND_CATEGORIES, fetchPolicy: 'network-only' })
        dispatch({
            type: 'FETCH_TASKS_SUCCESS',
            payload: {
                activeTasks: data.activeTasks,
                completedTasks: data.completedTasks,
                categories: data.categories
            }
        });
    } catch (error) {
        dispatch({
            type: 'FETCH_TASKS_FAILURE',
            payload: { error: error.message }
        });
    }
};

export const addTask = (task) => async (dispatch) => {
    const { data } = await client.mutate({
        mutation: ADD_TASK,
        variables: { task }
    });
    dispatch({
        type: 'ADD_TASK',
        payload: data.addTask
    });
};

export const completeTask = (id) => async (dispatch) => {
    const { data } = await client.mutate({
        mutation: COMPLETE_TASK,
        variables: { id }
    });
    dispatch({
        type: 'COMPLETE_TASK',
        payload: data.completeTask
    });
};