import { useState } from "react";
import "../form.css"

const Form = () => {
    const [user,setUser] = useState({
        firstName:'',
        lastName:'',
        email:''
    });
    const handleChange = () => {

       setUser()
    }
    const handleSubmit = (e) => {
        e.preventDefault();

        console.log(e)
        console.log("user",user)
    }

  return (
    <div className='container'>
        <h1>Register Here!</h1>
        <form className="form" onSubmit={handleSubmit}>
            <div className='labelInput'>
                <label htmlFor="firstname">FirstName</label>
                <input type="text" name="firstname" id="" onChange={(e) => handleChange(e)}/>
            </div>
            <div className='labelInput'>
                <label htmlFor="lastname">LastName</label>
                <input type="text" name="lastname" id="" onChange={(e) => handleChange(e)}/>
            </div>
            <div className='labelInput'>
                <label htmlFor="email">Email</label>
                <input type="text" name="email" id="" onChange={(e) => handleChange(e)}/>
            </div>
            <button>Submit</button>
        </form>
    </div>
  )
}

export default Form