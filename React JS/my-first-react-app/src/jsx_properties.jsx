import { createRoot } from 'react-dom/client';

function Car(myWhateverName) {
    return (
        <h2>I am a {myWhateverName.model}</h2>
    )
}

createRoot(document.getElementById('root')).render(
    <Car model="Mitsubishi" />
)