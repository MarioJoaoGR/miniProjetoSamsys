import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { ToastContainer } from "react-toastify"; // Importando o ToastContainer
import "react-toastify/dist/ReactToastify.css"; // Importando os estilos do Toastify

import Home from "./pages/Home";
import Books from "./pages/Books";
import BookDetails from "./pages/BookDetails";
import EditBook from "./pages/EditBook";
import InsertBook from "./pages/InsertBook";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/books" element={<Books />} />
        <Route path="/book/:id" element={<BookDetails />} />
        <Route path="/edit-book/:id" element={<EditBook />} />
        <Route path="/insert-book" element={<InsertBook />} />
      </Routes>

      {/* Adicionando o ToastContainer no nível global do app */}
      <ToastContainer
        position="top-right"
        autoClose={3000} // Fecha automaticamente após 3 segundos
        hideProgressBar={false} // Exibe a barra de progresso
        newestOnTop={true}
        closeOnClick
        pauseOnFocusLoss
        draggable
        pauseOnHover
      />
    </Router>
  );
}

export default App;
