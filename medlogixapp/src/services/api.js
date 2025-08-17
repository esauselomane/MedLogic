import axios from 'axios';
import config from '../config';

console.log(config);
console.log(process.env.REACT_APP_API_BASE_URL)

const API_BASE = 'http://localhost:5055/api';

const api = axios.create({
    baseURL: config.API_BASE_URL
});

export const setAuthToken = (token) => {
    if (token) {
        api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    } else {
        delete api.defaults.headers.common['Authorization'];
    }
};

// Auth
export const loginUser = async (credentials) => {
    const res = await api.post('/auth/login', credentials);
    return res.data;
};

export const registerUser = async (data) => {
    const res = await api.post('/auth/register', data);
    return res.data;
};

// Todos
export const getTodos = async () => {
    const res = await api.get('/todo');
    return res.data;
};

export const createTodo = async (todo) => {
    const res = await api.post('/todo', todo);
    return res.data;
};

export const updateTodo = async (id, updates) => {
    const res = await api.put(`/todo/${id}`, updates);
    return res.data;
};

export const deleteTodo = async (id) => {
    const res = await api.delete(`/todo/${id}`);
    return res.data;
};
