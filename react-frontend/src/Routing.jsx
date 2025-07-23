import { createBrowserRouter } from 'react-router-dom';
import Layout from './components/Layout';
import TodoList from './components/TodoList';

export function createRouter() {
    return createBrowserRouter([
        {
            path: '/',
            element: <Layout />,
            children: [
                { index: true, element: <TodoList /> }
            ]
        }
    ])
}