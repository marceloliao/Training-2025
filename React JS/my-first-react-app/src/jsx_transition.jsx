import { createRoot } from 'react-dom/client';
import { useState, useTransition } from 'react';

function SearchBar() {
    const [text, setText] = useState('');
    const [results, setResults] = useState('');
    const [isPending, startTransition] = useTransition();

    const handleChange = (e) => {
        // Urgent: Update input right away
        setText(e.target.value);

        // Non-urgent: Update search result
        startTransition(() => {
            setResults(e.target.value);
        });
    }

    return (
        <div>
            <input value={text} onChange={handleChange} />
            {isPending ? (
                <p>Loading...</p>
            ) : (
                <p>Search results for: {results}</p>
            )}
        </div>
    );
}

createRoot(document.getElementById('root')).render(
    <SearchBar />
);