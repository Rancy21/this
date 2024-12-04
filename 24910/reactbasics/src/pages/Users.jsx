import { useEffect, useState } from "react";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const Users = () => {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(false);

    const url = "https://jsonplaceholder.typicode.comx";

    useEffect(() => {

        async function getAllUsers() {

            await fetch(`${url}/users`, {
                method: "GET"
            })
                .then((response) => {
                    return response.json();

                }).then((result) => {
                    if (result.length >= 0) {
                        setLoading(false);
                        setUsers(result)
                        console.log("data", result);
                        toast.success("Data Fetched Succesfully")
                    }
                })
                .catch((error) => {
                    setLoading(false)
                    setError(true)
                    console.log(error)
                    toast.error("Error occured")
                });
        }

        getAllUsers();


    }, [])

    if (loading) {
        return <div><h1>Loading....</h1></div>
    }

    if (error) {
        toast.error("error occured")
        return <div><h1>Error occurred</h1></div>
    }
    console.log("users", users);
    return (
        <div>Total number of users:{users && users?.length}
            <ToastContainer />
            <table>
                <thead>
                    <tr>
                        <td>id</td>
                        <td>name</td>
                        <td>username</td>
                        <td>email</td>
                    </tr>
                </thead>
                <tbody>
                    {users.length > 0 && users.map((user, index) => {

                        return <tr key={index}>
                            <td>{user.id}</td>
                            <td>{user.name}</td>
                            <td>{user.username}</td>
                            <td>{user.email}</td>
                        </tr>
                    })}
                </tbody>
            </table>
        </div>
    )
}

export default Users