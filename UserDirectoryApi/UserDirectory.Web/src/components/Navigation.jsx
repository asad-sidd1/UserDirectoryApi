import { NavLink } from "react-router-dom";

export default function Navbar() {
    return (
        <nav className="navbar">
            <div className="navbar-brand">
                User Directory
            </div>

            <div className="navbar-links">
                <NavLink to="/">List</NavLink>
                <NavLink to="/add">Add</NavLink>
            </div>
        </nav>
    );
}
