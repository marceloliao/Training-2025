import { createRoot } from 'react-dom/client';

function Car() {

    const myFunc = () => {
        alert("Hello World!");
    };
    return (
        <button onClick={myFunc} disabled={false}>Click Me</button>
    )
};

function Color() {
    const mystyles = {
        color: "orange",
        fontSize: "50px",
        backgroundColor: "lightblue"
    }

    return (
        <>
            <h1 style={mystyles}>My color</h1>
        </>
    )
}

// createRoot(document.getElementById('root')).render(
//     <Car />
// )
createRoot(document.getElementById('root')).render(
    <Color />
)