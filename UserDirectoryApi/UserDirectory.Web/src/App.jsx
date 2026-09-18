import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navigation";
import UsersList from "./pages/UsersList";
import AddUser from "./pages/AddUser";

export default function App() {
    return (
        <BrowserRouter>
            <Navbar />
            <main>
                <Routes>
                    <Route path="/" element={<UsersList />} />
                    <Route path="/add" element={<AddUser />} />
                </Routes>
            </main>
        </BrowserRouter>
    );
}
