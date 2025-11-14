import { createRoot } from "react-dom/client";
import { useState, useRef, useEffect } from 'react';

function App() {
    const [inputValue, setInputValue] = useState("");
    const count = useRef(0);

    useEffect(() => {
        count.current = count.current + 1;
    });

    return (
        <>
            <p>Type in the input field:</p>
            <input type="text" value={inputValue} onChange={(e) => setInputValue(e.target.value)} />
            <h1>Render count: {count.current}</h1>
        </>
    );
}

createRoot(document.getElementById('root')).render(
    <App />
)