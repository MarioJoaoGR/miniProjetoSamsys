import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getBook } from "../services/bookService";
import { Button } from "../components/ui/button";
import { motion } from "framer-motion";

const BookDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [book, setBook] = useState<any>(null);

  useEffect(() => {
    const fetchBook = async () => {
      const data = await getBook(id!);
      setBook(data);
    };
    fetchBook();
  }, [id]);

  if (!book) return <div>Loading...</div>;

  return (
    <div className="flex items-center justify-center min-h-screen bg-gradient-to-r from-blue-600 to-indigo-900 text-white">
      <motion.div
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.8 }}
        className="text-center p-10 bg-white/10 backdrop-blur-md rounded-2xl shadow-lg max-w-lg w-full"
      >
        <h1 className="text-4xl font-bold mb-4">Detalhes do Livro</h1>
        <div className="mb-6">
          <h2 className="text-2xl font-semibold">{book.title}</h2>
          <p className="text-xl mt-2">ISBN: {book.isbn}</p>
          <p className="text-xl mt-2">Autor: {book.authorName}</p>
          <p className="text-xl mt-2">Preço: {book.value} €</p>
        </div>

        <div className="flex justify-between mt-6">
          <Button
            onClick={() => navigate("/books")}
            className="px-6 py-3 text-lg bg-white text-blue-600 hover:bg-blue-100 rounded-xl"
          >
            Voltar à Lista
          </Button>
          <Button
            onClick={() => navigate(`/edit-book/${book.id}`)}
            className="px-6 py-3 text-lg bg-white text-blue-600 hover:bg-blue-100 rounded-xl"
          >
            Editar
          </Button>
        </div>
      </motion.div>
    </div>
  );
};

export default BookDetails;
