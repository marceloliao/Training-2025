import './css/App.css'
import MovieCard from "./components/MovieCard"
import Home from './pages/Home';
import Favorites from './pages/Favorites';
import AboutUs from './pages/AboutUs';
import { Routes, Route } from 'react-router-dom';
import NavBar from './components/NavBar';
import { MovieProvider } from './contexts/MovieContext';

function Text({ input }) {
  return (
    <div>
      <p>Hello {input}!</p>
    </div>
  )
}

function App() {
  return (
    <MovieProvider>
      <NavBar />
      <main className="main-content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/favorites" element={<Favorites />} />
          <Route path="/about-us" element={<AboutUs />} />
        </Routes>
      </main>
    </MovieProvider>
  )
}

export default App