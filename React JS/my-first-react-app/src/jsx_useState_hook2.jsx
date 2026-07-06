import { useState } from 'react';
import { createRoot } from 'react-dom/client';

function FavoriteColor() {
    const [color, setColor] = useState('red');

    return (
        <div>
            <h1>My favorite color is {color}!</h1>
            <button onClick={() => setColor('blue')}>Blue</button>
            <button onClick={() => setColor('green')}>Green</button>
            <button onClick={() => setColor('red')}>Red</button>
        </div>
    );


}

createRoot(document.getElementById('root')).render(
    <FavoriteColor />
);