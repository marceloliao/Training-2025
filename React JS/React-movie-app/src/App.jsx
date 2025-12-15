import './App.css'
import MovieCard from "./components/MovieCard"
import Home from './pages/Home';

function Text({ input }) {
  return (
    <div>
      <p>Hello {input}!</p>
    </div>
  )
}

function App() {

  return (
    <>
      <Home />
    </>
  )
}

export default App