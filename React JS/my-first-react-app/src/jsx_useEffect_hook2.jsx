import { useState, useEffect } from "react";
import { createRoot } from "react-dom/client";

function Counter() {
    const [count, setCount] = useState(0);
    const [calculation, setCalculation] = useState(0);

    useEffect(() => { setCalculation(() => count * 2) }, [count]);
    return (
        <>
            <p>Count: {count}</p>
            <button onClick={() => setCount((c) => c + 1)}>+</button>
            <p>Calculation: {calculation}</p>
        </>
    );
}


// function Timer() {
//     const [count, setCount] = useState(0);

//     useEffect(() => {
//         setTimeout(() => {
//             setCount((count) => count + 1);
//         }, 1000);
//     }, []); // Add empty brackets here

//     return <h1>I have rendered {count} times!</h1>
// }

createRoot(document.getElementById('root')).render(
    <Counter />
); 