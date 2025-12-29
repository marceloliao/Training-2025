import { createRoot } from "react-dom/client";
import { useRef } from 'react';

function App() {
    const inputElement = useRef();

    // const focusInput = () => {
    //     inputElement.current.focus();
    // };

    const clearInput = () => {
        inputElement.current.value = "";
    };

    // const changeValue = () => {
    //     inputElement.value = "I clicked";
    // };

    return (
        <>
            <p>Using useRef to access DOM element</p>
            <input type="text" ref={inputElement} />&nbsp;
            <button onClick={clearInput}>Clear Input</button>
        </>
    );
}
createRoot(document.getElementById('root')).render(
    <App />
)