import MovieCard from "../components/MovieCard";
import { useState, useEffect, use } from 'react';
import { searchMovies, getPopularMovies } from "../services/api";
import '../css/Home.css';

function Home() {

    const [searchQuery, setSearchQuery] = useState("");
    const [movies, setMovies] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadPopularMovies = async () => {

            try {
                const popularMovies = await getPopularMovies();
                setMovies(popularMovies);

            } catch (error) {
                console.log(err);
                setError("Failed to load popular movies.");
            }
            finally {
                setLoading(false);
            }
        };

        loadPopularMovies();
    }, []);

    // const movies = [
    //     { id: 3, title: "Superman", release_date: "2014" }
    // ];

    const handleSearch = async (e) => {
        e.preventDefault();
        if (!searchQuery.trim()) return; // Ignore empty search

        if (loading) return; // Prevent multiple searches while loading 

        setLoading(true);

        try {
            const searchResults = await searchMovies(searchQuery);
            setMovies(searchResults);
            setError(null);
        } catch (error) {
            console.log(error);
            setError("Failed to search movies.");

        } finally {
            setLoading(false);
        }
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

            {error && <div className="error">{error}</div>}

            {loading ? (<div className="loading">Loading movies...</div>) :
                (<div className="movies-grid">
                    {movies.map((movie) => (
                        // movie.title.toLowerCase().startsWith(searchQuery) && <MovieCard movie={movie} key={movie.id} />
                        <MovieCard movie={movie} key={movie.id} />
                    ))}
                </div>)
            }
        </div>
    )
}

export default Home;