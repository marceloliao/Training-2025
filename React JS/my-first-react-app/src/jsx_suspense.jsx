import { createRoor, createRoot } from 'react-dom/client';
import { Suspense } from 'react';
import MyFruits from './Fruits';

function App() {
    return (
        <div>
            <Suspense fallback={<div>Loading...</div>}>
                <MyFruits />
            </Suspense>
        </div>
    );
}

createRoot(document.getElementById('root')).render(
    <App />
);