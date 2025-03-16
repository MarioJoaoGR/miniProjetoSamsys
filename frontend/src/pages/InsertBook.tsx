import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createBook } from "../services/bookService";
import { Button } from "../components/ui/button";
import { motion } from "framer-motion";

const InsertBook = () => {
  const navigate = useNavigate();
  const [book, setBook] = useState({ isbn: "", title: "", authorNif: "", value: "" });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    setBook((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await createBook(book.isbn, book.title, book.authorNif, book.value);
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
        <h1 className="text-4xl font-bold mb-4">Inserir Novo Livro</h1>

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
          <input
            type="text"
            name="authorNif"
            value={book.authorNif}
            onChange={handleChange}
            placeholder="NIF do Autor"
            className="p-2 rounded-xl bg-white text-blue-600 w-full"
          />
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
            Adicionar Livro
          </Button>
        </form>
      </motion.div>
    </div>
  );
};

export default InsertBook;
