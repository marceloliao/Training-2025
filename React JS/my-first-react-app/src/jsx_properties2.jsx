import { createRoot } from 'react-dom/client';

// function Car(props) {
//     return (
//         <div>
//             <h2>My {props.carinfo.name} {props.carinfo.model}!</h2>
//             <p>It is {props.carinfo.color} and it is from {props.carinfo.year}</p>
//         </div>
//     )
// }

function Car(props) {
    return (
        <div>
            <h2>My {props.carinfo[0]} {props.carinfo[1]}!</h2>
            <p>It is {props.carinfo[2]} and it is from {props.carinfo[3]}</p>
        </div>
    )
}

// Object props
// const carInfo = {
//     name: "Ford",
//     model: "Mustang",
//     color: "red",
//     year: 1969
// };

// Array props
const carInfo = ["Ford", "Mustang", "white", 1978];

createRoot(document.getElementById('root')).render(
    <Car carinfo={carInfo} />
)