import { BrowserRouter, Routes, Route } from "react-router-dom";
import IndexPage from "./pages/IndexPage";
import MoviePage from "./pages/MoviePage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<IndexPage />} />
        <Route path="/movie/:movieId" element={<MoviePage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;