import React, { Fragment, useEffect, useReducer } from 'react';
import {
  Route,
  Router,
  Switch
} from "react-router-dom";
import { reducer, initialState, AppContext } from './store';
import {createBrowserHistory} from 'history';
import { ToastContainer } from 'react-toastify';
import TrackerWindow from './pages/TrackerWindow/TrackerWindow';
import LoginModal from './pages/Login/LoginModal';
import agent from './api/agent';
import RegisterModal from './pages/Login/RegisterModal';

export const App: React.FC = () => {
  const [state, dispatch] = useReducer(reducer, initialState);  

  useEffect(() => {
    agent.Login.quickAuthorizationCheck();
  }, []);

  return (
    <Fragment>
      <ToastContainer position="bottom-right" />
      <AppContext.Provider value={{ state, dispatch }}> 
        <Router history={history}>
          <Switch>
            <Route exact path="/" component={TrackerWindow} />
            <Route exact path="/login" component={LoginModal} />
            <Route exact path="/register" component={RegisterModal} />
          </Switch>
        </Router>
      </AppContext.Provider>
    </Fragment>
  );
}

export const history = createBrowserHistory();