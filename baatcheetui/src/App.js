import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import "./App.scss";
import { Authentication } from "./components/Authentication/Authentication";
import Dashboard from './components/Dashboard/Dashboard';

function App() {
  return (
    <Router>
      <div className="App">
        <Switch>
          <Route path="/dashboard" component={Dashboard} />
          <Route path="/" component={Authentication} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;
