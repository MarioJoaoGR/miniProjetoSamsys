import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getBook, editBook } from "../services/bookService";
import { getAllAuthors } from "../services/authorService";
import { Button } from "../components/ui/button";
import { motion } from "framer-motion";

const EditBook = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  interface Book {
    isbn: string;
    title: string;
    authorNif: string;
    value: string;
  }

  interface Author {
    nif: string;
    fullName: string;
  }

  const [book, setBook] = useState<Book>({ isbn: "", title: "", authorNif: "", value: "" });
  const [authors, setAuthors] = useState<Author[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null); // Estado para mensagem de erro

  useEffect(() => {
    const fetchBook = async () => {
      try {
        const data = await getBook(id!);
        setBook(data);
      } catch (error) {
        setErrorMessage("Failed to fetch book data!"); // Exibe erro ao buscar o livro
      }
    };

    const fetchAuthors = async () => {
      try {
        const authorsData = await getAllAuthors();
        setAuthors(authorsData);
      } catch (error) {
        setErrorMessage("Failed to fetch authors!"); // Exibe erro ao buscar autores
      }
    };

    fetchBook();
    fetchAuthors();
  }, [id]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setBook((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await editBook(id!, book.isbn, book.title, book.authorNif, book.value);
      navigate("/books"); // Redireciona após a edição bem-sucedida
    } catch (error) {
      setErrorMessage("Failed to update Book!"); 
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
        <h1 className="text-4xl font-bold mb-4">Editar Livro</h1>

        {/* Exibe a mensagem de erro, se houver */}
        {errorMessage && (
          <div className="mb-4 text-red-500">
            <p>{errorMessage}</p>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="text"
            name="isbn"
            value={book.isbn}
            onChange={handleChange}
            placeholder="ISBN"
            className="p-2 rounded-xl bg-white text-blue-600 w-full"
          />
          <input
            type="text"
            name="title"
            value={book.title}
            onChange={handleChange}
            placeholder="Título"
            className="p-2 rounded-xl bg-white text-blue-600 w-full"
          />
          
          {/* Dropdown para selecionar o autor */}
          <select
            name="authorNif"
            value={book.authorNif}
            onChange={handleChange}
            className="p-2 rounded-xl bg-white text-blue-600 w-full"
          >
            <option value="">Selecione um Autor</option>
            {authors.map((author) => (
              <option key={author.nif} value={author.nif}>
                {author.fullName} ({author.nif})
              </option>
            ))}
          </select>

          <input
            type="text"
            name="value"
            value={book.value}
            onChange={handleChange}
            placeholder="Preço"
            className="p-2 rounded-xl bg-white text-blue-600 w-full"
          />
          <Button
            type="submit"
            className="w-full px-6 py-3 text-lg bg-white text-blue-600 hover:bg-blue-100 rounded-xl"
          >
            Salvar Alterações
          </Button>
        </form>
      </motion.div>
    </div>
  );
};

export default EditBook;
