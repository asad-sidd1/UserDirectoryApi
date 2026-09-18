import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createUser } from "../services/userApi";

const initialForm = {
    name: "",
    age: "",
    city: "",
    state: "",
    pincode: ""
};

export default function AddUser() {
    const navigate = useNavigate();

    const [form, setForm] = useState(initialForm);
    const [errors, setErrors] = useState({});
    const [submitError, setSubmitError] = useState("");
    const [saving, setSaving] = useState(false);

    function validate() {
        const newErrors = {};

        const name = form.name.trim();

        if (!name) {
            newErrors.name = "Name is required.";
        } else if (name.length < 2 || name.length > 100) {
            newErrors.name = "Name must be between 2 and 100 characters.";
        }

        if (form.age === "") {
            newErrors.age = "Age is required.";
        } else if (
            !Number.isInteger(Number(form.age)) ||
            Number(form.age) < 0 ||
            Number(form.age) > 120
        ) {
            newErrors.age = "Age must be an integer between 0 and 120.";
        }

        if (!form.city.trim()) {
            newErrors.city = "City is required.";
        }

        if (!form.state.trim()) {
            newErrors.state = "State is required.";
        }

        const pincode = form.pincode.trim();

        if (!pincode) {
            newErrors.pincode = "Pincode is required.";
        } else if (pincode.length < 4 || pincode.length > 10) {
            newErrors.pincode =
                "Pincode must be between 4 and 10 characters.";
        }

        return newErrors;
    }

    function handleChange(event) {
        const { name, value } = event.target;

        setForm(current => ({
            ...current,
            [name]: value
        }));

        setErrors(current => ({
            ...current,
            [name]: undefined
        }));
    }

    async function handleSubmit(event) {
        event.preventDefault();

        const validationErrors = validate();

        if (Object.keys(validationErrors).length > 0) {
            setErrors(validationErrors);
            return;
        }

        try {
            setSaving(true);
            setSubmitError("");

            await createUser({
                name: form.name.trim(),
                age: Number(form.age),
                city: form.city.trim(),
                state: form.state.trim(),
                pincode: form.pincode.trim()
            });

            navigate("/", {
                state: {
                    successMessage: "User created successfully."
                }
            });
        } catch (err) {
            setSubmitError(
                err.message || "Unable to create user."
            );
        } finally {
            setSaving(false);
        }
    }

    return (
        <div className="page">
            <div className="page-header">
                <div>
                    <h1>Add User</h1>
                    <p>Create a new user in the directory.</p>
                </div>
            </div>

            <form className="user-form" onSubmit={handleSubmit}>
                {submitError && (
                    <div className="error-message">
                        {submitError}
                    </div>
                )}

                <div className="form-group">
                    <label htmlFor="name">Name</label>

                    <input
                        id="name"
                        name="name"
                        type="text"
                        value={form.name}
                        onChange={handleChange}
                        maxLength={100}
                    />

                    {errors.name && (
                        <span className="field-error">
                            {errors.name}
                        </span>
                    )}
                </div>

                <div className="form-group">
                    <label htmlFor="age">Age</label>

                    <input
                        id="age"
                        name="age"
                        type="number"
                        min="0"
                        max="120"
                        value={form.age}
                        onChange={handleChange}
                    />

                    {errors.age && (
                        <span className="field-error">
                            {errors.age}
                        </span>
                    )}
                </div>

                <div className="form-group">
                    <label htmlFor="city">City</label>

                    <input
                        id="city"
                        name="city"
                        type="text"
                        value={form.city}
                        onChange={handleChange}
                    />

                    {errors.city && (
                        <span className="field-error">
                            {errors.city}
                        </span>
                    )}
                </div>

                <div className="form-group">
                    <label htmlFor="state">State</label>

                    <input
                        id="state"
                        name="state"
                        type="text"
                        value={form.state}
                        onChange={handleChange}
                    />

                    {errors.state && (
                        <span className="field-error">
                            {errors.state}
                        </span>
                    )}
                </div>

                <div className="form-group">
                    <label htmlFor="pincode">Pincode</label>

                    <input
                        id="pincode"
                        name="pincode"
                        type="text"
                        value={form.pincode}
                        onChange={handleChange}
                        maxLength={10}
                    />

                    {errors.pincode && (
                        <span className="field-error">
                            {errors.pincode}
                        </span>
                    )}
                </div>

                <button
                    type="submit"
                    className="primary-button"
                    disabled={saving}
                >
                    {saving ? "Saving..." : "Add User"}
                </button>
            </form>
        </div>
    );
}
