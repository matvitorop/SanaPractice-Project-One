import { createStore, applyMiddleware, combineReducers } from 'redux';
import { createEpicMiddleware } from 'redux-observable';

import taskReducer from './taskReducer';
import { rootEpic } from './epics';

const epicMiddleware = createEpicMiddleware<
    any, 
    any, 
    RootState
>();

const rootReducer = combineReducers({
    tasks: taskReducer
});

const store = createStore(rootReducer, applyMiddleware(epicMiddleware));

epicMiddleware.run(rootEpic);

export type RootState = ReturnType<typeof rootReducer>;
export type AppDispatch = typeof store.dispatch;

export default store;