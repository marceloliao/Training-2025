import { useState } from "react";
import { createRoot } from "react-dom/client";

function MyCar() {
    const [car, setCar] = useState({
        brand: "Ford",
        model: "Mustang",
        year: "1975",
        color: "white"
    });

    const updateColor = (couleur) => {
        setCar(previousState => {
            return ({ ...previousState, color: couleur });
        });
    };

    return (
        <>
            <h1>My {car.brand}</h1>
            <p>It is a {car.color} {car.model} from {car.year}</p>
            <button type="button" onClick={() => updateColor('blue')}>Blue</button>&nbsp;
            <button type="button" onClick={() => updateColor('red')}>Red</button>&nbsp;
            <button type="button" onClick={() => updateColor('pink')}>Pink</button>&nbsp;
            <button type="button" onClick={() => updateColor('green')}>Green</button>&nbsp;
        </>
    );
}

createRoot(document.getElementById('root')).render(
    <MyCar />
);