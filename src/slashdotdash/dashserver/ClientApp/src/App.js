import React from "react";
import { applyMiddleware, combineReducers, compose, createStore } from "redux";
import { Provider } from "react-redux";
import { ThemeProvider } from "styled-components";
import dashboardReducer from "./reducers/dashboard";
import thunk from "redux-thunk";
import { Dashboard } from "./pages/dashboard";
import theme from "./theme";
import { Layout } from "./components";

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
        <Layout>
          <Dashboard />
        </Layout>
      </Provider>
    </ThemeProvider>
  );
}

export default App;
