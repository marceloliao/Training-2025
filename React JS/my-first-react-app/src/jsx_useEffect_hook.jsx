import { useState, useEffect } from "react";
import { createRoot } from "react-dom/client";

function Timer() {
    const [count, setCount] = useState(0);

    useEffect(() => {
        setTimeout(() => {
            setCount((count) => count + 1);
        }, 1000);
    }, []); // Add empty brackets here

    return <h1>I have rendered {count} times!</h1>
}

createRoot(document.getElementById('root')).render(
    <Timer />
); 