import './App.css'
import Layout from './components/Layout.jsx';
import TodoList from './components/TodoList.jsx';

export default function App() {
  return (  
    <Layout>
          <div className="container">
            <TodoList />
          </div>
    </Layout>
  )
}