import TodoForm from '../components/TodoForm';
import TodoList from '../components/TodoList';
import '../styles/global.css';

const OnLogout = () => {
    localStorage.removeItem('token');
    window.location.reload();
}

const Home = () => (
    <div>
        <h1>MedLogix Todo App</h1>
        <button style={{marginRight: "5px"}} onClick= {OnLogout}>
          Sign out
        </button> 
        <TodoForm />
        <TodoList />
    </div>
);

export default Home;
