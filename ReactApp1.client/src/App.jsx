import { useState } from "react";
import AppLayout from "./components/layout/AppLayout";
import UsersPage from "./pages/Users/UsersPage";
import MeetingRoomsPage from "./pages/MeetingRooms/MeetingRoomsPage";
import ReservationsPage from "./pages/Reservations/ReservationsPage";


const pages = {
    users: UsersPage,
    rooms: MeetingRoomsPage,
    reservations: ReservationsPage,
};

function App() {
    const [activePage, setActivePage] = useState("users");

    const CurrentPage = pages[activePage];

    return (
        <AppLayout
            activePage={activePage}
            onNavigate={setActivePage}
        >
            <CurrentPage />
        </AppLayout>
    );
}

export default App;
