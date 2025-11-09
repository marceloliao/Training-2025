import { createRoot } from 'react-dom/client'

function Car(props) {
    return (
        <h2>I am a {props.brand}!</h2>
    );
}

function Garage() {
    return (
        <>
            <h1>Who lives in my Garage?</h1>
            <Car brand="Ford" />
            <Car brand="Volvo" />
        </>
    );
}

createRoot(document.getElementById('root')).render(
    <Garage />
);

