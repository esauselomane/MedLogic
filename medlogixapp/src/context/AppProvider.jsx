import React, { useState, useEffect } from 'react';
import AppContext from './AppContext';
import { setAuthToken } from '../services/api';

const AppProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(localStorage.getItem('token') || '');
    const [todos, setTodos] = useState([]);

    useEffect(() => {
        setAuthToken(token);
        if (token) {
            localStorage.setItem('token', token);
        } else {
            localStorage.removeItem('token');
        }
    }, [token]);

    return (
        <AppContext.Provider value={{ user, setUser, token, setToken, todos, setTodos }}>
            {children}
        </AppContext.Provider>
    );
};

export default AppProvider;
