import { ChangeEvent, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { fetchTasks } from '../store/taskActions'
import Cookies from 'js-cookie';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Outlet } from 'react-router-dom';
import type { AppDispatch } from '../store/store';

export default function Layout() {
    const [storageType, setStorageType] = useState<'db' | 'xml'>('db');
    const dispatch = useDispatch<AppDispatch>();

    useEffect(() => {
        const savedType = Cookies.get('StorageType') as 'db' | 'xml' | undefined;
        if (savedType) {
            setStorageType(savedType);
        }
    }, []);

    const handleStorageTypeChange = (event : ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value as 'db' | 'xml';
        setStorageType(value);
        Cookies.set('StorageType', value);
        dispatch(fetchTasks());
    };

    return (
        <>
            <header>
                <nav className="navbar navbar-expand-sm navbar-light bg-white border-bottom mb-3">
                    <div className="container-fluid">
                        <a className="navbar-brand" href="/">MVC_Practice</a>
                        <div className="d-flex align-items-center">
                            <select value={storageType} onChange={handleStorageTypeChange} className="form-select">
                                <option value="db">DB</option>
                                <option value="xml">XML</option>
                            </select>
                        </div>
                    </div>
                </nav>
            </header>

            <main>
                <Outlet/>
            </main>

            <footer className="border-top footer text-muted mt-3">
                <div className="container">
                    &copy; 2025 - MVC_Practice - <a href="/privacy">Privacy</a>
                </div>
            </footer>
        </>
    );
}