import { createRoot } from "react-dom/client";

function Fruit() {
    const x = 7;
    return (
        <h1>{x < 10 ? "Banana" : "Apple"}</h1>
    );
};

createRoot(document.getElementById("root")).render(
    <Fruit />
);