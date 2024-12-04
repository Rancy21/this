import { useState } from "react"
import Button from "./Button"
import { useNavigate } from "react-router";

const Login = () => {

    const navigate = useNavigate();

    const [count, setCount] = useState(0);
    const [firstName, setFirstName] = useState(''); 
    const [password, setPassword] = useState('');

    console.log("count is", count);


    function increment() {
        console.log("Initial count", count);
        setCount(count + 1);
        console.log("Updated count", count);
    }

    function decrement() {
        console.log("Initial Count", count);
        setCount(count - 1);
        console.log("Updated count");
    }

    function incrementBy10() {
        setCount(count + 10)
    }

    function handleSubmit(e) {
        e.preventDefault();
       
       console.log("firstname",firstName);
       console.log("password",password)
       if(firstName != null && password != null){
        navigate("/products");
       }
    }


    return (
        <div>
            <p>{count}</p>
            <button onClick={increment}>Increment</button>

            <button onClick={decrement}>Decrement</button>

            <button onClick={incrementBy10}>Increment By 10</button>

            <button onClick={() => setCount(0)}>reset</button>

            <form onSubmit={handleSubmit} >
                <div>

                    <input value={firstName} onChange={(e) => setFirstName(e.target.value)} />
                </div>

                <div>
                    <input value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
                <div>
                    <Button label="submit" />

                    <button >Login</button>
                </div>
            </form>

 





            {/* <Header />
        <h1>Login Page</h1>
        <Button label="Login"/>
        <Button label="register"/>
        <Footer /> */}
        </div >
    )
}

export default Login