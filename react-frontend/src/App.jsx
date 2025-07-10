import './App.css'
import { ApolloProvider } from '@apollo/client';
import client from './ApolloClient.jsx';
import Layout from './components/Layout.jsx';
import TodoList from './components/TodoList.jsx';

export default function App() {
  return (
      <ApolloProvider client={client}>
        <Layout>
              <div className="container">
                <TodoList />
              </div>
        </Layout>
    </ApolloProvider>
  )
}