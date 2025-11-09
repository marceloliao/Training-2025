import { createRoot } from 'react-dom/client'

function Car({ color, brand, ...rest }) {
    return (
        <h2>My {brand} {rest.model} is {color} from the year {rest.year}!</h2>
    );
}

createRoot(document.getElementById('root')).render(
    <Car brand="Ford" model="Mustang" color="red" year={1969} />
);



