import React from "react";
import { createBrowserRouter } from "react-router";
import App from "./App";
import Login from "./component/Login";
import About from "./pages/About";
import Products from "./pages/Products";
import Users from "./pages/Users";
import Students from "./pages/Students";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />
  },
  {
    path: "/login",
    element: <Login />
  },
  {
    path: "/about",
    element: <About />
  },
  {
    path: "/products",
    element: <Products />
  },
  {
    path: "/users",
    element: <Users />
  },
  {
    path: "/student",
    element: <Students />
  }
]);

export default router;
