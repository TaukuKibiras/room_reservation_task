import { useEffect, useState } from "react";
import PageHeader from "../../components/layout/PageHeader";
import "./MeetingRoom.css";
import "../../styles/pages.css";
import "../../styles/form.css";

const emptyForm = {
    name: "",
    capacity: 1,
    location: "",
    equipment: "",
    isActive: true,
};

function MeetingRoomsPage() {
    const [rooms, setRooms] = useState([]);
    const [form, setForm] = useState({ ...emptyForm });
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState("");
    const [editId, setEditId] = useState(null);

    // Load meeting rooms
    useEffect(() => {
        let ignore = false;

        async function fetchRooms() {
            try {
                const response = await fetch("/api/MeetingRoom");

                if (!response.ok)
                {
                    throw new Error("Failed to load meeting rooms.");
                }

                const data = await response.json();

                if (!ignore)
                {
                    setRooms(data);
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

        fetchRooms();

        return () => {
            ignore = true;
        };
    }, []);

    function handleChange(event) {
        const { name, value, type, checked } = event.target;

        setForm((previous) => ({
            ...previous,
            [name]: type === "checkbox" ? checked : value,
        }));
    }

    // Delete
    async function handleDelete(id) {
        const confirmed = window.confirm(
            "Are you sure you want to delete this room?"
        );

        if (!confirmed) return;

        setError("");

        try {
            const response = await fetch(`/api/MeetingRoom/${ id } `, {
                method: "DELETE",
            });

            if (!response.ok)
            {
                throw new Error("Failed to delete room.");
            }

            setRooms((previous) => previous.filter((room) => room.id !== id));

        } catch (err) {
            setError(err.message || "Delete failed.");
        }
    }

    function handleUpdate(room) {
        setForm({
            name: room.name ?? "",
            capacity: room.capacity ?? 1,
            location: room.location ?? "",
            equipment: room.equipment ?? "",
            isActive: room.isActive ?? true,
        });

        setEditId(room.id);
        setError("");
        setIsModalOpen(true);
    }

    // Update or Add
    async function handleSubmit(event) {
        event.preventDefault();

        setSaving(true);
        setError("");

        const roomData = {
            name: form.name.trim(),
            capacity: Number(form.capacity),
            location: form.location.trim(),
            equipment: form.equipment.trim(),
            isActive: form.isActive,
        };

        try {
            let response;
            let savedRoom;

            if (editId !== null)
            {
                response = await fetch(`/api/MeetingRoom/${ editId } `, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(roomData),
                });
            }
            else
            {
                response = await fetch("/api/MeetingRoom", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(roomData),
                });
            }

            if (!response.ok)
            {
                const errorText = await response.text();
                throw new Error(errorText || "Nepavyko išsaugoti salės.");
            }

            savedRoom = await response.json();

            if (editId !== null)
            {
                setRooms((previous) =>previous.map((room) =>room.id === savedRoom.id ? savedRoom : room));
            }
            else
            {
                setRooms((previous) => [...previous, savedRoom]);
            }

            setIsModalOpen(false);
            setForm({ ...emptyForm });
            setEditId(null);
        } catch (err) {
            setError(err.message || "Nepavyko išsaugoti salės.");
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
        setEditId(null);
        setError("");
    }

    return (
        <section className="page">
            <PageHeader
                title="Meeting Rooms"
                action={{
                    label: "Add room",
                    onClick: openModal,
                }}
            />

            {error && !isModalOpen && (
                <p className="error-message">{error}</p>
            )}

            <div className="content-card">
                <div className="table-header meetingroom-columns">
                    <span>ID</span>
                    <span>Name</span>
                    <span>Capacity</span>
                    <span>Floor / Location</span>
                    <span>Equipment</span>
                    <span>Status</span>
                    <span>Actions</span>
                </div>

                {loading ? (
                    <div className="empty-state">
                        <p>Loading meeting rooms...</p>
                    </div>
                ) : rooms.length === 0 ? (
                    <div className="empty-state">
                        <h2>No meeting rooms yet</h2>
                    </div>
                ) : (
                    <div className="rooms-list">
                        {rooms.map((room) => (
                            <div
                                className="room-row-meetingroom"
                                key={room.id}
                            >
                                <span>{room.id}</span>
                                <span>{room.name}</span>
                                <span>{room.capacity} people</span>
                                <span>{room.location}</span>
                                <span>{room.equipment || "None"}</span>

                                <span>
                                    <span
                                        className={`status - badge ${
    room.isActive
        ? "status-active"
        : "status-inactive"
} `}
                                    >
                                        {room.isActive ? "Active" : "Inactive"}
                                    </span>
                                </span>

                                <span>
                                    <button
                                        type="button"
                                        className="status-badge action-badge update-badge"
                                        title="Update"
                                        onClick={() => handleUpdate(room)}
                                    >
                                        Update
                                    </button>

                                    <button
                                        type="button"
                                        className="status-badge action-badge delete-badge"
                                        title="Delete"
                                        onClick={() => handleDelete(room.id)}
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
                        )
                        {
                            closeModal();
                        }
                    }}
                >
                    <div
                        className="modal"
                        role="dialog"
                        aria-modal="true"
                        aria-labelledby="room-modal-title"
                    >
                        <div className="modal-header">
                            <div>
                                <h2 id="room-modal-title">
                                    {editId !== null
                                        ? "Edit meeting room"
                                        : "Add meeting room"}
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
                                <label htmlFor="name">Room name</label>
                                <input
                                    id="name"
                                    name="name"
                                    value={form.name}
                                    onChange={handleChange}
                                    placeholder="Conference Room"
                                    required
                                    maxLength={100}
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="capacity">
                                    Maximum capacity
                                </label>
                                <input
                                    id="capacity"
                                    name="capacity"
                                    type="number"
                                    min="1"
                                    max="1000"
                                    value={form.capacity}
                                    onChange={handleChange}
                                    required
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="location">
                                    Floor / Location
                                </label>
                                <input
                                    id="location"
                                    name="location"
                                    value={form.location}
                                    onChange={handleChange}
                                    placeholder="2nd floor"
                                    required
                                    maxLength={200}
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="equipment">Equipment</label>
                                <input
                                    id="equipment"
                                    name="equipment"
                                    value={form.equipment}
                                    onChange={handleChange}
                                    placeholder="Projector, TV, Whiteboard"
                                />
                            </div>

                            <label className="checkbox-label">
                                <input
                                    type="checkbox"
                                    name="isActive"
                                    checked={form.isActive}
                                    onChange={handleChange}
                                />
                                Room is available for booking
                            </label>

                            {error && (
                                <p className="error-message">{error}</p>
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
                                            ? "Update room"
                                            : "Add room"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </section>
    );
}

export default MeetingRoomsPage;
