import { createRoot } from 'react-dom/client'

function MyForm() {
    return (
        <form>
            <label>Enter your name:&nbsp;
                <input type="text" />
            </label>
        </form>
    )
}

createRoot(document.getElementById('root')).render(
    <MyForm />
);



