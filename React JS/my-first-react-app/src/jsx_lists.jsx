import { createRoot } from 'react-dom/client'

// This function works, but complains there is no key 
// function MyCars() {
//     const cars = ['Ford', 'BMW', 'Audi'];
//     return (
//         <>
//             <h1>My Cars:</h1>
//             <ul>
//                 {cars.map((car) => <li>I am a {car}</li>)}
//             </ul>
//         </>
//     );
// }

// This one has id as key
// function MyCars() {
//     const cars = [
//         { id: 1001, brand: 'Ford' },
//         { id: 1002, brand: 'BMW' },
//         { id: 1003, brand: 'Audi' }
//     ];
//     return (
//         <>
//             <h1>My Cars:</h1>
//             <ul>
//                 {cars.map((car) => <li key={car.id}>I am a {car.brand} of id {car.id}</li>)}
//             </ul>
//         </>
//     );
// }

function MyCars() {
    const cars = ['Ford', 'BMW', 'Audi'];
    return (
        <>
            <h1>My Cars:</h1>
            <ul>
                {cars.map((car, index) => <li key={index}>I have a {car}, the index is {index}.</li>)}
            </ul>
        </>
    );
}

createRoot(document.getElementById('root')).render(
    <MyCars />
);
