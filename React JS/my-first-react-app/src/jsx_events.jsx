import { createRoot } from 'react-dom/client'

// function Football() {
//     const shoot = (a) => {
//         alert("You scored " + a + " goals!");
//     }

//     return (
//         <button onClick={() => shoot(5)}>Take the shot!</button>
//     )
// }

// Event object
function Football() {
    const shoot = (a, b) => {
        alert(`You scored ${a} goals! the event type is ${b.type}`);
    }

    return (
        <button onClick={(event) => shoot(5, event)}>Take the shot!</button>
    )
}

createRoot(document.getElementById('root')).render(
    <Football />
);
