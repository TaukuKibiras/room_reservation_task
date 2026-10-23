import PageHeader from "../../components/layout/PageHeader";
import "./Reservation.css";
import "../../styles/pages.css";

function ReservationsPage() {
    return (
        <section className="page">
            <PageHeader
                title="Reservations"
                action={{
                    label: "Add reservation",
                    onClick: () => { },
                }}
            />

            <div className="content-card">
                <div className="table-header reservations-columns">
                    <span>ID</span>
                    <span>Meeting</span>
                    <span>Room</span>
                    <span>Organizer</span>
                    <span>Date & time</span>
                    <span>Attendees</span>
                    <span>Status</span>
                    <span>Actions</span>
                </div>

                <div className="empty-state">
                    <h2>No reservations yet</h2>
                </div>
            </div>
        </section>
    );
}

export default ReservationsPage;