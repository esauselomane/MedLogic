import React, { useContext, useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { getTodos, deleteTodo, updateTodo, createTodo } from '../services/api';
import AppContext from '../context/AppContext';
import TodoForm from './TodoForm';

const TodoList = () => {
    const { todos, setTodos } = useContext(AppContext);
    const [editingId, setEditingId] = useState(null);
    const [editText, setEditText] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedTodo, setSelectedTodo] = useState(null);
    const [callBackEditAdd, setCallBackEditAdd] = useState(null);

    useEffect(() => {
        const fetchTodos = async () => {
            try{
            const data = await getTodos();
            setTodos(data);
            } catch (error) {
                if(error.code =="ERR_NETWORK")
                {
                    toast.error('API not running. Ensure API project is running or confirm the correct port in ".env" file.');
                }
                else if(error.response.status == 401)
                {
                    <Navigate to="/login" />;
                }
            }
        };
        fetchTodos();
    }, [setTodos]);

    const handleAddClick = (e) => {
        setCallBackEditAdd(() => AddTodo);
        e.preventDefault();
        setSelectedTodo(null); // No initial data for new todo
        setIsModalOpen(true);
    };

    const handleDelete = async (e, id) => {
        e.preventDefault();
        await deleteTodo(id);
        setTodos((prev) => prev.filter((todo) => todo.id !== id));
    };

    const handleToggle = async (todo) => {
        const updated = await updateTodo(todo.id, { completed: !todo.completed });
        setTodos((prev) =>
            prev.map((t) => (t.id === todo.id ? updated : t))
        );
    };

    const handleClose = () => {
    setIsModalOpen(false);
    setSelectedTodo(null);
    setEditingId(null);
  };

    const startEditing = (e, todo) => {
        e.preventDefault();
        setCallBackEditAdd(() => saveEdit);
        setEditingId(todo.id);
        setSelectedTodo(todo); 
        setIsModalOpen(true);
    };

    const saveEdit = async (e, todo) => {
        e.preventDefault();
        
        const updated = await updateTodo(todo.id, todo);
        setTodos((prev) =>
            prev.map((t) => (t.id === todo.id ? updated : t))
        );
        setEditingId(null);
    };

    const AddTodo = async (e, todo) => {
        e.preventDefault();
        debugger;
        const added = await createTodo(todo);
        setTodos([...todos, added]);
        setEditingId(null);
    };

    return (
        <><TodoForm
            isOpen={isModalOpen}
            onClose={handleClose}
            onSubmit={callBackEditAdd}
            initialData={selectedTodo} />

            <form>
            <button onClick={(e) => handleAddClick(e)}>Add Todo</button>

            {todos && todos.length > 0 &&
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: 'left', padding: '0.5rem' }}>Title</th>
                        <th style={{ textAlign: 'left', padding: '0.5rem' }}>Description</th>
                        <th style={{ textAlign: 'left', padding: '0.5rem' }}>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {todos.map((todo) => (
                        <tr key={todo.id}>
                            <td><span style={{
                                            textDecoration: todo.isCompleted ? 'line-through' : 'none',
                                            cursor: 'pointer',
                                        }}>
                                            {todo.title}
                                            </span></td>
                            <td>
                                <span
                                        style={{
                                            textDecoration: todo.isCompleted ? 'line-through' : 'none',
                                            cursor: 'pointer',
                                        }}
                                    >
                                        {todo.description}
                                    </span>
                            </td>
                            <td>
                                {editingId === todo.id ? (
                                    <div style = {{ padding: '16px 0' }}>
                                        <button onClick={(e) => saveEdit(e, todo.id)}>Save</button>{' '}
                                        <button onClick={(e) => setEditingId(e, null)}>Cancel</button>
                                    </div>
                                ) : (
                                    <div style = {{ padding: '16px 0' }}>
                                        <button onClick={(e) => startEditing(e, todo)}>Edit</button>{' '}
                                        <button onClick={(e) => handleDelete(e, todo.id)}>Delete</button>
                                    </div>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
}

            </form>
            </>
    );
};

export default TodoList;
