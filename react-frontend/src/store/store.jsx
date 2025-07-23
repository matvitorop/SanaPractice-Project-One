import { createStore, applyMiddleware, combineReducers } from 'redux';
import { createEpicMiddleware } from 'redux-observable';

import taskReducer from './taskReducer';
import { rootEpic } from './epics';
const epicMiddleware = createEpicMiddleware();

const rootReducer = combineReducers({
    tasks: taskReducer
});

const store = createStore(rootReducer, applyMiddleware(epicMiddleware));

epicMiddleware.run(rootEpic);

export default store;