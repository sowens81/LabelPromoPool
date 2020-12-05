import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import { Auth0Provider } from "@auth0/auth0-react";

ReactDOM.render(
  <Auth0Provider
      domain="labelpromopool.eu.auth0.com"
      clientId="7rzkHE4DgkIt6xhMk2GY7EEIRLjNp0Ai"
      redirectUri={window.location.origin}
    >
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </Auth0Provider>,
  document.getElementById('root')
);

serviceWorker.unregister();