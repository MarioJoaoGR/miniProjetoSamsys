import { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { useNavigate } from "react-router-dom";
import { deleteBook, filterBooks } from "../services/bookService";
import { Button } from "../components/ui/button";
import { toast } from "react-toastify";

const Books = () => {
  const navigate = useNavigate();
  const [filteredBooks, setFilteredBooks] = useState<any[]>([]);
  const [paginatedBooks, setPaginatedBooks] = useState<any[]>([]);
  const [filter, setFilter] = useState({ isbn: "", title: "", authorName: "", valueOrder: "" });
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(4);
  const [totalPages, setTotalPages] = useState(0);

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        const data = await filterBooks(filter.isbn, filter.title, filter.authorName, filter.valueOrder);
        setFilteredBooks(data);
        setTotalPages(Math.ceil(data.length / itemsPerPage));
        if (currentPage > 1 && data.length === 0) setCurrentPage(1);
      } catch (error: any) {
        const errorMessage = error.response?.data?.message || "Erro ao buscar livros.";
        toast.error(errorMessage);
      }
    };
    fetchBooks();
  }, [filter, itemsPerPage]);

  useEffect(() => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    setPaginatedBooks(filteredBooks.slice(startIndex, startIndex + itemsPerPage));
  }, [filteredBooks, currentPage]);

  const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFilter((prev) => ({ ...prev, [name]: value }));
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteBook(id);
      setFilteredBooks(filteredBooks.filter((book) => book.id !== id));
      toast.success("Livro excluído com sucesso!");
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || "Erro ao excluir o livro.";
      toast.error(errorMessage);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gradient-to-r from-blue-600 to-indigo-900 text-white">
      <motion.div
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.8 }}
        className="text-center p-10 bg-white/10 backdrop-blur-md rounded-2xl shadow-lg max-w-lg w-full"
      >
        <h1 className="text-4xl font-bold mb-4">Coleção de Livros</h1>
        <Button onClick={() => navigate("/insert-book")} className="mt-6 px-6 py-3 bg-white text-blue-600 rounded-xl">
          Inserir Novo Livro
        </Button>

        <div className="mb-6 flex flex-col gap-4 mt-6">
          <input type="text" name="isbn" value={filter.isbn} onChange={handleFilterChange} placeholder="Filtrar por ISBN" className="p-2 rounded-xl bg-white text-blue-600" />
          <input type="text" name="title" value={filter.title} onChange={handleFilterChange} placeholder="Filtrar por Título" className="p-2 rounded-xl bg-white text-blue-600" />
          <input type="text" name="authorName" value={filter.authorName} onChange={handleFilterChange} placeholder="Filtrar por Autor" className="p-2 rounded-xl bg-white text-blue-600" />
          <select name="valueOrder" value={filter.valueOrder} onChange={handleFilterChange} className="p-2 rounded-xl bg-white text-blue-600">
            <option value="">Ordenar por Preço</option>
            <option value="increasing">Ascendente</option>
            <option value="decreasing">Descendente</option>
          </select>
        </div>

        <ul className="space-y-4">
          {paginatedBooks.map((book) => (
            <li key={book.id} className="flex justify-between items-center p-4 bg-white/10 rounded-xl shadow-lg">
              <span className="text-xl">{book.title}</span>
              <div>
                <Button onClick={() => navigate(`/book/${book.id}`)} className="mr-2 px-4 py-2 bg-white text-blue-600 rounded-xl">Details</Button>
                <Button onClick={() => navigate(`/edit-book/${book.id}`)} className="mr-2 px-4 py-2 bg-white text-blue-600 rounded-xl">Edit</Button>
                <Button onClick={() => handleDelete(book.id)} className="px-4 py-2 bg-white text-red-600 rounded-xl">Delete</Button>
              </div>
            </li>
          ))}
        </ul>

        {/* Seção de paginação com seletor */}
        <div className="flex justify-center space-x-4 mt-4">
          {currentPage > 1 && (
            <Button onClick={() => setCurrentPage((prev) => prev - 1)} className="px-4 py-2 bg-white text-blue-600 rounded-xl">
              Anterior
            </Button>
          )}

          {/* Seletor de página */}
          <select
            value={currentPage}
            onChange={(e) => setCurrentPage(Number(e.target.value))}
            className="px-4 py-2 bg-white text-blue-600 rounded-xl"
          >
            {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
              <option key={page} value={page}>
                {page}
              </option>
            ))}
          </select>

          {currentPage < totalPages && (
            <Button onClick={() => setCurrentPage((prev) => prev + 1)} className="px-4 py-2 bg-white text-blue-600 rounded-xl">
              Próxima
            </Button>
          )}
        </div>

        <div className="mt-4 text-white">
          Página {currentPage} de {totalPages}
        </div>
      </motion.div>
    </div>
  );
};

export default Books;
