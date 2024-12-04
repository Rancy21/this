import { NavLink, useNavigate } from "react-router";
import "./App.css";

const App = () => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate("/about");
  };
  return (
    <div>
      {/* <h1>Hello World!</h1>

      <NavLink to="/login">Login</NavLink>

      <NavLink to="/student">About</NavLink> */}

      <button onClick={() => navigate("/student")}> Save a student</button>
    </div>
  );
};

export default App;
