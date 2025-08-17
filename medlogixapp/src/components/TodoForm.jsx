import React, { useState, useEffect } from 'react';
import '../styles/global.css'; 

const TodoForm = ({ isOpen, onClose, onSubmit, initialData }) => {
  const [title, setTitle] = useState('');
  const [id, setId] = useState(0);
  const [description, setDescription] = useState('');
  const [isCompleted, setIsCompleted] = useState(false);

  useEffect(() => {
    if (initialData) {
      setTitle(initialData.title || '');
      setDescription(initialData.description || '');
      setIsCompleted(initialData.isCompleted);
      setId(initialData.id)

    } else {
      setTitle('');
      setDescription('');
    }
  }, [initialData]);

  const handleToggle = () => {
    setIsCompleted(!isCompleted);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!title.trim()) return;
    if (!description.trim()) return;
    onSubmit(e, { title, description,isCompleted , id });
    resetForm();
    onClose();
  };

  const resetForm = () => {
    setTitle('');
    setId(0);
    setDescription('');
    setIsCompleted(false);
  }

  if (!isOpen) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>{initialData ? 'Edit Todo' : 'Add Todo'}</h2>
        <form>
          <div style={{ marginBottom: '1rem' }}>
            <label htmlFor="title">Title:</label>
            <input
              id="title"
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
              style={{ width: '100%', padding: '0.5rem', marginTop: '0.25rem' }}
            />
          </div>
          <div style={{ marginBottom: '1rem' }}>
            <label htmlFor="description">Description:</label>
            <textarea
              id="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              rows={4}
              required
              style={{ width: '100%', padding: '0.5rem', marginTop: '0.25rem' }}
            />
          </div>

            <div className="toggle-container">
                <input
                    type="checkbox"
                    id="toggle-switch"
                    className="toggle-checkbox"
                    checked={isCompleted}
                    onChange={handleToggle} // Use onChange for checkbox input
                />
                <label htmlFor="toggle-switch" className="toggle-label">
                    <span className="toggle-inner" />
                    <span className="toggle-switch-handle" />
                </label>
            </div>

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '1rem' }}>
            <button type="button" onClick={() => { resetForm(); onClose(); }}>Cancel</button>
            <button type="submit" onClick= {(e) => handleSubmit(e)}>{initialData ? 'Update' : 'Add'}</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default TodoForm;
