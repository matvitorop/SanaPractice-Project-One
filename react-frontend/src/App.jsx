import './App.css'

import { ApolloClient, InMemoryCache, ApolloProvider, createHttpLink } from '@apollo/client';
import Layout from './components/Layout.jsx';
import TodoList from './components/TodoList.jsx';


const httpLink = createHttpLink({
    uri: 'https://localhost:7171/graphql',
    headers: {
        'StorageType': 'db'
    }
});

const client = new ApolloClient({
    link: httpLink,
    cache: new InMemoryCache(),
});

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