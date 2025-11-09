import { createRoot } from 'react-dom/client';
import { forwardRef, useRef } from 'react';

const MyInput = forwardRef((props, ref) => (
    <input ref={ref} {...props} />
));

function App() {
    const inputRef = useRef();

    const focusInput = () => {
        inputRef.current.focus();
    };

    return (
        <div>
            <MyInput ref={inputRef} placeHolder="Type here..." />&nbsp;
            <button onClick={focusInput}>Focus Input</button>
        </div>
    );
}

createRoot(document.getElementById('root')).render(
    <App />
);