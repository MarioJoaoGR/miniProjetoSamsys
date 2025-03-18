import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getBook, editBook } from "../services/bookService";
import { getAllAuthors } from "../services/authorService"; // Importamos o serviço de autores
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

  useEffect(() => {
    const fetchBook = async () => {
      const data = await getBook(id!);
      setBook(data);
    };

    const fetchAuthors = async () => {
      const authorsData = await getAllAuthors();
      setAuthors(authorsData);
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
    await editBook(id!, book.isbn, book.title, book.authorNif, book.value);
    navigate("/books");
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
