import { createBrowserRouter, RouteObject } from 'react-router-dom';
import Layout from './components/Layout';
import TodoList from './components/TodoList';

export function createRouter() {
    const routes: RouteObject[] = [
        {
            path: '/',
            element: <Layout />,
            children: [
                { index: true, element: <TodoList /> }
            ]
        }
    ];
    return createBrowserRouter(routes);
}