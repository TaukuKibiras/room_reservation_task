const navigationItems = [
    { id: "users", label: "Users" },
    { id: "rooms", label: "Meeting rooms" },
    { id: "reservations", label: "Reservations" },
];

function Sidebar({ activePage, onNavigate }) {
    return (
        <aside className="sidebar">
            <nav className="sidebar-nav">
                {navigationItems.map((item) => (
                    <button
                        key={item.id}
                        className={`nav-item ${activePage === item.id ? "active" : ""
                            }`}
                        onClick={() => onNavigate(item.id)}
                    >
                        <span>{item.label}</span>
                    </button>
                ))}
            </nav>
        </aside>
    );
}

export default Sidebar;
