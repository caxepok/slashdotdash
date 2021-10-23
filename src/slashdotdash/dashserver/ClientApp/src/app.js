import React from "react";
import { applyMiddleware, combineReducers, compose, createStore } from "redux";
import { Provider } from "react-redux";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import dashboardReducer from "./reducers/dashboard";
import thunk from "redux-thunk";
import { Dashboard, DashboardOld } from "./pages/dashboard";
import theme from "./theme";
import { Layout } from "./components";
import { Shops } from "./pages/shops/shops";
import { Plan } from "./pages/plan";
import { Redirect } from "react-router";

function App() {
  const composeEnhancers =
    process.env.NODE_ENV === "development" ? window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose : compose;

  const store = createStore(
    combineReducers({
      dashboard: dashboardReducer,
    }),
    composeEnhancers(applyMiddleware(thunk)),
  );

  return (
    <ThemeProvider theme={theme}>
      <Provider store={store}>
        <Router>
          <Layout>
            <Switch>
              <Route path={"/shops"} component={Shops} />
              <Route path={"/plan"} component={Plan} />
              <Route path={"/dashboard/old"} component={DashboardOld} />
              <Route path={"/dashboard"} component={Dashboard} />
              <Redirect to={"/dashboard"} />
            </Switch>
          </Layout>
        </Router>
      </Provider>
    </ThemeProvider>
  );
}

export default App;
