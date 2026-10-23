function PageHeader({ title, action }) {
    return (
        <header className="page-header">
            <div>
                <h1>{title}</h1>
            </div>

            {action && (
                <button className="primary-button" onClick={action.onClick}>
                    <span>+</span>
                    {action.label}
                </button>
            )}
        </header>
    );
}

export default PageHeader;
