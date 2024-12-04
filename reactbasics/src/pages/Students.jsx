import { useState } from "react";
import "../button.css";
import { ToastContainer, toast } from "react-toastify";

const Students = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(false);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [studentCode, setStudentCode] = useState("");
  const [dateOfBirth, setDob] = useState("");
  const [address, setAddress] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault(); // Prevent the form from reloading the page
    setLoading(true);

    const studentData = {
      firstName: firstName,
      lastName: lastName,
      studentCode: studentCode,
      dateOfBirth: dateOfBirth,
      address: address
    };

    console.log("student data: ", studentData);

    try {
      const response = await fetch(
        "http://localhost:8080/students/saveStudent",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(studentData) // Convert the data to JSON format
        }
      );

      if (!response.ok) {
        throw new Error(`Failed to post data: ${response.statusText}`);
      }

      const result = response.json();
      toast.success("Student data saved successfully!");
      console.log("Posted data:", result);
    } catch (error) {
      setError(true);
      toast.error("Failed to save student data.");
      console.error("Error:", error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <ToastContainer />
      <form onSubmit={handleSubmit}>
        <div className="input-group">
          <label htmlFor="firstName">First Name</label>
          <input
            type="text"
            name="firstName"
            required
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
          />
        </div>
        <div className="input-group">
          <label htmlFor="lastName">Last Name</label>
          <input
            type="text"
            name="lastName"
            required
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
          />
        </div>
        <div className="input-group">
          <label htmlFor="studentCode">Student Code</label>
          <input
            type="text"
            name="studentCode"
            required
            value={studentCode}
            onChange={(e) => setStudentCode(e.target.value)}
          />
        </div>
        <div className="input-group">
          <label htmlFor="dateOfBirth">Date Of Birth</label>
          <input
            type="date"
            name="dateOfBirth"
            required
            value={dateOfBirth}
            onChange={(e) => setDob(e.target.value)}
          />
        </div>
        <div className="input-group">
          <label htmlFor="address">Address</label>
          <input
            type="text"
            name="address"
            required
            value={address}
            onChange={(e) => setAddress(e.target.value)}
          />
        </div>
        <button type="submit" disabled={loading}>
          {loading ? "Saving..." : "Save Student"}
        </button>
        {error && <p className="error">An error occurred. Please try again.</p>}
      </form>
    </div>
  );
};

export default Students;
