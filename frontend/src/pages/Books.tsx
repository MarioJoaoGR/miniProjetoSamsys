import { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { useNavigate } from "react-router-dom";
import { deleteBook, filterBooks } from "../services/bookService";
import { Button } from "../components/ui/button";

const Books = () => {
  const navigate = useNavigate();
  const [books, setBooks] = useState<any[]>([]);
  const [filteredBooks, setFilteredBooks] = useState<any[]>([]);
  const [paginatedBooks, setPaginatedBooks] = useState<any[]>([]);
  const [filter, setFilter] = useState({
    isbn: "",
    title: "",
    authorName: "",
    valueOrder: "",
  });
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(4);
  const [totalPages, setTotalPages] = useState(0);

  const updatePagination = () => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const paginated = filteredBooks.slice(startIndex, startIndex + itemsPerPage);
    setPaginatedBooks(paginated);
  };

  // Atualiza a página e o total de páginas após a filtragem
  useEffect(() => {
    const fetchBooks = async () => {
      const data = await filterBooks(filter.isbn, filter.title, filter.authorName, filter.valueOrder);
      setFilteredBooks(data);

      const newTotalPages = Math.ceil(data.length / itemsPerPage);
      setTotalPages(newTotalPages);

      // Se não houver resultados, reiniciar para a página 1
      if (newTotalPages === 0) {
        setCurrentPage(1);
      }
    };
    fetchBooks();
  }, [filter, itemsPerPage]);

  useEffect(() => {
    updatePagination();
  }, [filteredBooks, currentPage]);

  const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;

    setFilter((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleDelete = async (id: string) => {
    await deleteBook(id);
    setFilteredBooks(filteredBooks.filter((book) => book.id !== id));
  };

  const handleViewDetails = (id: string) => {
    navigate(`/book/${id}`);
  };

  const handleEdit = (id: string) => {
    navigate(`/edit-book/${id}`);
  };

  const nextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage((prev) => prev + 1);
    }
  };

  const previousPage = () => {
    if (currentPage > 1) {
      setCurrentPage((prev) => prev - 1);
    }
  };

  const isNextPageAvailable = currentPage < totalPages && filteredBooks.slice(currentPage * itemsPerPage, (currentPage + 1) * itemsPerPage).length > 0;
  const isPreviousPageAvailable = currentPage > 1;

  return (
    <div className="flex items-center justify-center min-h-screen bg-gradient-to-r from-blue-600 to-indigo-900 text-white">
      <motion.div
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.8 }}
        className="text-center p-10 bg-white/10 backdrop-blur-md rounded-2xl shadow-lg max-w-lg w-full"
      >
        <h1 className="text-4xl font-bold mb-4">Coleção de Livros</h1>

        {/* Botão para Inserir Novo Livro */}
        <Button
          onClick={() => navigate("/insert-book")}
          className="mt-6 px-6 py-3 text-lg bg-white text-blue-600 hover:bg-blue-100 rounded-xl transition-all"
        >
          Inserir Novo Livro
        </Button>

        {/* Filtros */}
        <div className="mb-6 flex flex-col gap-4 mt-6">
          <input
            type="text"
            name="isbn"
            value={filter.isbn}
            onChange={handleFilterChange}
            placeholder="Filtrar por ISBN"
            className="p-2 rounded-xl bg-white text-blue-600"
          />
          <input
            type="text"
            name="title"
            value={filter.title}
            onChange={handleFilterChange}
            placeholder="Filtrar por Título"
            className="p-2 rounded-xl bg-white text-blue-600"
          />
          <input
            type="text"
            name="authorName"
            value={filter.authorName}
            onChange={handleFilterChange}
            placeholder="Filtrar por Autor"
            className="p-2 rounded-xl bg-white text-blue-600"
          />
          <select
            name="valueOrder"
            value={filter.valueOrder}
            onChange={handleFilterChange}
            className="p-2 rounded-xl bg-white text-blue-600"
          >
            <option value="">Ordenar por Preço</option>
            <option value="increasing">Ascendente</option>
            <option value="decreasing">Descendente</option>
          </select>
        </div>

        {/* Lista de Livros */}
        <ul className="space-y-4">
          {paginatedBooks.map((book) => (
            <li key={book.id} className="flex justify-between items-center p-4 bg-white/10 rounded-xl shadow-lg">
              <span className="text-xl">{book.title}</span>
              <div>
                <Button
                  onClick={() => handleViewDetails(book.id)}
                  className="mr-2 px-4 py-2 text-sm bg-white text-blue-600 hover:bg-blue-100 rounded-xl"
                >
                  Details
                </Button>
                <Button
                  onClick={() => handleEdit(book.id)}
                  className="mr-2 px-4 py-2 text-sm bg-white text-blue-600 hover:bg-blue-100 rounded-xl"
                >
                  Edit
                </Button>
                <Button
                  onClick={() => handleDelete(book.id)}
                  className="px-4 py-2 text-sm bg-white text-red-600 hover:bg-red-100 rounded-xl"
                >
                  Delete
                </Button>
              </div>
            </li>
          ))}
        </ul>

        {/* Navegação de Páginas */}
        <div className="flex justify-center space-x-4 mt-4">
          {isPreviousPageAvailable && (
            <Button
              onClick={previousPage}
              className="px-4 py-2 text-sm bg-white text-blue-600 hover:bg-blue-100 rounded-xl"
            >
              Anterior
            </Button>
          )}
          {isNextPageAvailable && (
            <Button
              onClick={nextPage}
              className="px-4 py-2 text-sm bg-white text-blue-600 hover:bg-blue-100 rounded-xl"
            >
              Próxima
            </Button>
          )}
        </div>

        {/* Exibição da Página Atual / Total de Páginas */}
        <div className="mt-4 text-white">
          Página {currentPage} de {totalPages}
        </div>
      </motion.div>
    </div>
  );
};


export default Books;


