import { useEffect, useState } from "react";
import { getUsers, deleteUser } from "../services/userApi";

export default function UsersList() {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    async function loadUsers() {
        try {
            setLoading(true);
            setError("");

            const data = await getUsers();
            setUsers(data);
        } catch (err) {
            setError(err.message || "Unable to load users.");
        } finally {
            setLoading(false);
        }
    }

    async function handleDelete(id) {
        if (!window.confirm("Are you sure you want to delete this user?")) {
            return;
        }

        try {
            await deleteUser(id);
            setUsers(current =>
                current.filter(user => user.id !== id)
            );
        } catch (err) {
            setError(err.message || "Unable to delete user.");
        }
    }

    useEffect(() => {
        loadUsers();
    }, []);

    if (loading) {
        return (
            <div className="loading">
                <div className="spinner"></div>
                Loading users...
            </div>
        );
    }

    return (
        <div className="page">
            <div className="page-header">
                <div>
                    <h1>Users</h1>
                    <p>Manage your user directory.</p>
                </div>
            </div>

            {error && (
                <div className="error-message">
                    {error}
                </div>
            )}

            {users.length === 0 ? (
                <div className="empty-state">
                    <h2>No users found</h2>
                    <p>Add your first user to get started.</p>
                </div>
            ) : (
                <div className="table-container">
                    <table>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Age</th>
                                <th>City</th>
                                <th>State</th>
                                <th>Pincode</th>
                                <th>Actions</th>
                            </tr>
                        </thead>

                        <tbody>
                            {users.map(user => (
                                <tr key={user.id}>
                                    <td>{user.name}</td>
                                    <td>{user.age}</td>
                                    <td>{user.city}</td>
                                    <td>{user.state}</td>
                                    <td>{user.pincode}</td>
                                    <td>
                                        <button
                                            className="delete-button"
                                            onClick={() => handleDelete(user.id)}
                                        >
                                            Delete
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}
