import { useEffect, useState } from "react";
import PageHeader from "../../components/layout/PageHeader";
import "./User.css";
import "../../styles/pages.css";

const emptyForm = {
    name: "",
    surname: "",
};

function UsersPage() {
    const [users, setUsers] = useState([]);
    const [form, setForm] = useState(emptyForm);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");
    const [editId, setEditId] = useState(null);

    // Load users
    useEffect(() => {
        let ignore = false;

        async function fetchUsers() {
            try {
                const response = await fetch("/api/User");

                if (!response.ok)
                {
                    throw new Error("Failed to load users.");
                }

                const data = await response.json();

                if (!ignore)
                {
                    setUsers(data);
                }
            } catch (err) {
                if (!ignore)
                {
                    setError(err.message || "Something went wrong.");
                }
            } finally {
                if (!ignore)
                {
                    setLoading(false);
                }
            }
        }

        fetchUsers();

        return () => {
            ignore = true;
        };
    }, []);


    function handleChange(event) {
        const { name, value } = event.target;

        setForm((prev) => ({
            ...prev,
            [name]: value,
        }));
    }

    // Delete
    async function handleDelete(id) {
        const confirmed = window.confirm(
            "Are you sure you want to delete this user?"
        );

        if (!confirmed) return;

        setError("");

        try {
            const response = await fetch(`/api/User/${id}`, {
                method: "DELETE",
            });

            if (!response.ok)
            {
                throw new Error("Failed to delete user.");
            }

            setUsers((prev) => prev.filter((user) => user.id !== id));

        } catch (err) {
            setError(err.message || "Delete failed.");
        }
    }

    function handleUpdate(user) {
        setForm({
            name: user.name ?? "",
            surname: user.surname ?? "",
        });

        setEditId(user.id);
        setError("");
        setIsModalOpen(true);
    }

    // Update or Add
    async function handleSubmit(event) {
        event.preventDefault();

        setSaving(true);
        setError("");

        const userData = {
            name: form.name.trim(),
            surname: form.surname.trim(),
        };

        try {
            let response;
            let savedUser;

            if (editId !== null)
            {

                response = await fetch(`/api/User/${editId}`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(userData),
                });
            }
            else
            {

                response = await fetch("/api/User", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(userData),
                });
            }

            if (!response.ok)
            {
                const errorText = await response.text();
                throw new Error(errorText || "Nepavyko išsaugoti vartotojo.");
            }

            savedUser = await response.json();

            if (editId !== null)
            {
                setUsers((previous) => previous.map((user) => user.id === savedUser.id ? savedUser : user));
            }
            else
            {
                setUsers((previous) => [...previous, savedUser]);
            }

            setIsModalOpen(false);
            setForm({ ...emptyForm });
            setEditId(null);
        } catch (err) {
            setError(err.message || "Nepavyko išsaugoti vartotojo.");
        } finally {
            setSaving(false);
        }
    }


    function openModal() {
        setForm({ ...emptyForm });
        setEditId(null);
        setError("");
        setIsModalOpen(true);
    }


    function closeModal() {
        if (saving) return;

        setIsModalOpen(false);
        setForm({ ...emptyForm });
        setError("");
        setEditId(null);
    }

    return (
        <section className="page">
            <PageHeader
                title="Users"
                action={{
                    label: "Add user",
                    onClick: openModal,
                }}
            />

            {error && !isModalOpen && (
                <p className="error-message">{error}</p>
            )}

            <div className="content-card">
                <div className="table-header users-columns">
                    <span>ID</span>
                    <span>Name</span>
                    <span>Surname</span>
                    <span>Active reservations</span>
                    <span>Actions</span>
                </div>

                {loading ? (
                    <div className="empty-state">
                        <p>Loading users...</p>
                    </div>
                ) : users.length === 0 ? (
                    <div className="empty-state">
                        <h2>No users yet</h2>
                    </div>
                ) : (
                    <div className="rooms-list">
                        {users.map((user) => (
                            <div className="room-row-user" key={user.id}>
                                <span>{user.id}</span>

                                <span>{user.name}</span>

                                <span>{user.surname}</span>

                                <span>{user.reservations}</span>

                                <span>
                                    <button
                                        type="button"
                                        className="status-badge action-badge update-badge"
                                        title="Update"
                                        onClick={() => handleUpdate(user)}
                                    >
                                        Update
                                    </button>

                                    <button
                                        type="button"
                                        className="status-badge action-badge delete-badge"
                                        title="Delete"
                                        onClick={() => handleDelete(user.id)}
                                    >
                                        Delete
                                    </button>
                                </span>
                            </div>
                        ))}
                    </div>
                )}
            </div>

            {isModalOpen && (
                <div
                    className="modal-overlay"
                    onMouseDown={(event) => {
                        if (
                            event.target === event.currentTarget &&
                            !saving
                        ) {
                            closeModal();
                        }
                    }}
                >
                    <div
                        className="modal"
                        role="dialog"
                        aria-modal="true"
                        aria-labelledby="user-modal-title"
                    >
                        <div className="modal-header">
                            <div>
                                <h2 id="user-modal-title">
                                    {editId !== null
                                        ? "Edit user"
                                        : "Add user"}
                                </h2>
                            </div>

                            <button
                                type="button"
                                className="close-button"
                                onClick={closeModal}
                                aria-label="Close"
                                disabled={saving}
                            >
                                ×
                            </button>
                        </div>

                        <form onSubmit={handleSubmit}>
                            <div className="form-group">
                                <label htmlFor="name">
                                    Name
                                </label>

                                <input
                                    id="name"
                                    name="name"
                                    value={form.name}
                                    onChange={handleChange}
                                    placeholder="Lebron"
                                    required
                                    maxLength={100}
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="surname">
                                    Surname
                                </label>

                                <input
                                    id="surname"
                                    name="surname"
                                    value={form.surname}
                                    onChange={handleChange}
                                    placeholder="Jordan"
                                    required
                                    maxLength={100}
                                />
                            </div>

                            {error && (
                                <p className="error-message">
                                    {error}
                                </p>
                            )}

                            <div className="modal-actions">
                                <button
                                    type="button"
                                    className="secondary-button"
                                    onClick={closeModal}
                                    disabled={saving}
                                >
                                    Cancel
                                </button>

                                <button
                                    type="submit"
                                    className="primary-button"
                                    disabled={saving}
                                >
                                    {saving
                                        ? "Saving..."
                                        : editId !== null
                                            ? "Update user"
                                            : "Add user"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </section>
    );
}

export default UsersPage;
