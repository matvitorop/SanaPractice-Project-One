import { ApolloClient, InMemoryCache, createHttpLink } from '@apollo/client';
import { setContext } from '@apollo/client/link/context';
import Cookies from 'js-cookie';

const httpLink = createHttpLink({
    uri: 'https://localhost:7171/graphql',
});

const authLink = setContext((_, { headers }) => {
    const storageType = Cookies.get('StorageType') || 'db';
    return {
        headers: {
            ...headers,
            'StorageType': storageType,
        }
    };
});

const client = new ApolloClient({
    link: authLink.concat(httpLink),
    cache: new InMemoryCache(),
});

export default client;