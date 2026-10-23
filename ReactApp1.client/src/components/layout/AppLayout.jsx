
import Sidebar from "./Sidebar";
import "../../styles/layout.css";

function AppLayout({ activePage, onNavigate, children }) {
    return (
        <div className="app-layout">
            <Sidebar
                activePage={activePage}
                onNavigate={onNavigate}
            />

            <main className="main-content">
                {children}
            </main>
        </div>
    );
}

export default AppLayout;
