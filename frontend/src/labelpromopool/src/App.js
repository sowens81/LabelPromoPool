import React, {Component} from "react";
import { Switch, Route, Link } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";
import AddArtist from "./components/Add-Artist.component";
import Artist from "./components/Artist.component";
import ArtistsList from "./components/Artists-List.component";
import LoginButton from "./components/LoginButton-component";

class App extends Component {
  render() {
    return (
      <div>
        <nav className="navbar navbar-expand navbar-dark bg-dark">
          <a href="/artists" className="navbar-brand">
            Artists
          </a>
          <div className="navbar-nav mr-auto">
            <li className="nav-item">
              <Link to={"/artists"} className="nav-link">
                All Artists
              </Link>
            </li>
            <li className="nav-item">
              <Link to={"/add"} className="nav-link">
                Add
              </Link>
            </li>
          </div>
          <LoginButton className="" >Login</LoginButton>
        </nav>

        <div className="container mt-3">
          <Switch>
            <Route exact path={["/", "/artists"]} component={ArtistsList} />
            <Route exact path="/add" component={AddArtist} />
            <Route path="/artists/:id" component={Artist} />
          </Switch>
        </div>
      </div>
    );
  }
}

export default App;
