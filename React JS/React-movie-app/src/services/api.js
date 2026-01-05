const API_KET = "5759e7f13de02332b35d101e9d4793dd";
const BASE_URL = "https://api.themoviedb.org/3";

export const getPopularMovies = async () => {
    const response = await fetch(`${BASE_URL}/movie/popular?api_key=${API_KET}`);
    const data = await response.json();
    return data.results;
};

export const searchMovies = async (query) => {
    const response = await fetch(`${BASE_URL}/search/movie?api_key=${API_KET}&query=${encodeURIComponent(query)}`);
    const data = await response.json();
    return data.results;
};