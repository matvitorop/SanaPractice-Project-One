import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

import { ApolloClient, InMemoryCache, ApolloProvider, createHttpLink } from '@apollo/client';
import Layout from './components/Layout.jsx';

const httpLink = createHttpLink({
    uri: 'http://localhost:7171/graphql',
    Headers: {
        'StorageType': 'db'
    }
});

const client = new ApolloClient({
    link: httpLink,
    cache: new InMemoryCache(),
});

function App() {
  return (
      <ApolloProvider client={client}>
        <Layout>
        </Layout>
    </ApolloProvider>
  )
}

export default App
