import { createRoot } from 'react-dom/client';
import { BrowserRouter, Routes, Route, NavLink } from 'react-router-dom';

// Style function for active link
const navLinkStyles = ({ isActive }) => ({
    color: isActive ? '#007bff' : '#333',
    textDecoration: isActive ? 'none' : 'underline',
    fontWeight: isActive ? 'bold' : 'normal',
    passding: '5px 10px'
});

const Header = ({ isActive }) => {
    const myStyle = {
        color: "white",
        backgroundColor: "DodgerBlue",
        padding: "10px",
        fontFamily: "Sans-Serif"
    }
}


function Home() {
    return <h1>Home Page</h1>;
}

function About() {
    return <h1>About Page</h1>;
}

function Contact() {
    return <h1>Contact Page</h1>;
}

function App() {
    return (
        <BrowserRouter>
            {/* Navigation */}
            <nav style={{ marginBottom: '20px' }}>
                <NavLink to="/" style={navLinkStyles}>Home</NavLink> |{" "}
                <NavLink to="/about" style={navLinkStyles}>About</NavLink> |{" "}
                <NavLink to="/contact" style={navLinkStyles}>Contact</NavLink>
            </nav>
            {/* Routes */}
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/about" element={<About />} />
                <Route path="/contact" element={<Contact />} />
            </Routes>
        </BrowserRouter>
    );
}

createRoot(document.getElementById('root')).render(
    <App />
);


