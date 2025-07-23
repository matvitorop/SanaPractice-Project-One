import './App.css'
import Layout from './components/Layout.jsx';
import TodoList from './components/TodoList.jsx';
import { createRouter } from './Routing.jsx'; 
import { RouterProvider } from 'react-router-dom';

export default function App() {

    const router = createRouter();  

    return <RouterProvider router={router} />;
}