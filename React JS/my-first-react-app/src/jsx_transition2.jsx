import { createRoot } from 'react-dom/client';
import { useState, useTransition } from 'react';

function SearchResult({ query }) {

    // Simulate slow search results
    const items = [];
    if (query) {
        for (let i = 0; i < 1000; i++) {
            items.push(<li key={i}>Result for {query} - {i}</li>)
        }
    }
    return <ul>{items}</ul>;
}

function App() {
    const [text, setText] = useState('');
    const [query, setQuery] = useState('');
    const [isPending, startTransition] = useTransition();

    const handleChange = (e) => {
        // Urgent: Update input field
        setText(e.target.value);

        // Non-urgent
        startTransition(() => {
            setQuery(e.target.value);
        });
    }

    return (
        <div>
            <input
                type="text"
                value={text}
                onChange={handleChange}
            />
            {isPending && <p>Loading results</p>}
            <SearchResult query={query} />
        </div>
    );
}

createRoot(document.getElementById('root')).render(
    <App />
);