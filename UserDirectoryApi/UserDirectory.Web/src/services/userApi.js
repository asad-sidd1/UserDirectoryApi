const API_URL = "/api/users";

export async function getUsers() {
 const response = await fetch(API_URL);

    if (!response.ok) {
        throw new Error("Unable to load users.");
    }

    return response.json();
}

export async function createUser(user) {
    const response = await fetch(API_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(user)
    });

    if (!response.ok) {
        const error = await response.json().catch(() => null);

        throw new Error(
            error?.title || "Unable to create user."
        );
    }

    return response.json();
}

export async function updateUser(id, user) {
    const response = await fetch(`${API_URL}/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            ...user,
            id
        })
    });

    if (!response.ok) {
        throw new Error("Unable to update user.");
    }
}

export async function deleteUser(id) {
    const response = await fetch(`${API_URL}/${id}`, {
        method: "DELETE"
    });

    if (!response.ok) {
        throw new Error("Unable to delete user.");
    }
}
