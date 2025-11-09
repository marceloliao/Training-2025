import { createRoot } from "react-dom/client";
import { useState } from "react";

function FavoriteColor() {
    const [color, setColor] = useState("red");

    return (
        <>
            <h1>My favorite color is {color}</h1>
            <button type="button" onClick={() => setColor("blue")}>Blue</button>&nbsp;
            <button type="button" onClick={() => setColor("red")}>Red</button>&nbsp;
            <button type="button" onClick={() => setColor("pink")}>Pink</button>&nbsp;
            <button type="button" onClick={() => setColor("green")}>Green</button>
        </>
    );
}

createRoot(document.getElementById('root')).render(
    <FavoriteColor />
);