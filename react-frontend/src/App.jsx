import './App.css'
import { createRouter } from './Routing'; 
import { RouterProvider } from 'react-router-dom';

export default function App() {

    const router = createRouter();  

    return <RouterProvider router={router} />;
}