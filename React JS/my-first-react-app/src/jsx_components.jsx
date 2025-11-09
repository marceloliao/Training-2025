import { createRoot } from "react-dom/client";

function Car(props) {
    return (
        <h2>I am a {props.couleur} Car!</h2>
    );
}

createRoot(document.getElementById('root')).render(
    <Car couleur="black" />
);
