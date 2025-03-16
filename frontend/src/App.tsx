import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Books from "./pages/Books";
import BookDetails from "./pages/BookDetails"; // Importando a página de detalhes
import EditBook from "./pages/EditBook"; // Importando a página de edição
import InsertBook from "./pages/InsertBook"; // Importando a página de inserção

function App() {
  return (
    <Router> {/* Envolvendo todas as rotas com Router */}
      <Routes> {/* Definindo as rotas */}
        <Route path="/" element={<Home />} /> {/* Página inicial */}
        <Route path="/books" element={<Books />} /> {/* Página de livros */}
        <Route path="/book/:id" element={<BookDetails />} /> {/* Detalhes do livro */}
        <Route path="/edit-book/:id" element={<EditBook />} /> {/* Editar livro */}
        <Route path="/insert-book" element={<InsertBook />} /> {/* Inserir livro */}
      </Routes>
    </Router>
  );
}

export default App;
