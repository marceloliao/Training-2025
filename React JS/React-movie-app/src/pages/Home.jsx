import MovieCard from "../components/MovieCard";
import { useState } from 'react';

function Home() {

    const [searchQuery, setSearchQuery] = useState("");


    const movies = [
        { id: 1, title: "Spiderman II", release_date: "2024" },
        { id: 2, title: "Wonder Woman", release_date: "2018" },
        { id: 3, title: "Superman", release_date: "2014" }
    ];

    const handleSearch = (e) => {
        e.preventDefault();
        alert(searchQuery);
        setSearchQuery("Changed to this");
    };

    return (
        <div className="home">
            <form onSubmit={handleSearch} className="search-form">
                <input
                    type="text"
                    placeholder="Search for movies..."
                    className="search-input"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                />
                <button type="submit" className="search-button">Search</button>
            </form>

            <div className="movie-grid">
                {movies.map((movie) => (
                    // movie.title.toLowerCase().startsWith(searchQuery) && <MovieCard movie={movie} key={movie.id} />
                    <MovieCard movie={movie} key={movie.id} />
                ))}
            </div>
        </div>
    )
}

export default Home;