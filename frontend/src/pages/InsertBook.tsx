import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { createBook } from "../services/bookService";
import { getAllAuthors } from "../services/authorService";
import { Button } from "../components/ui/button";
import { motion } from "framer-motion";
import { toast } from "react-toastify"; // Toastify importado diretamente

const InsertBook = () => {
  const navigate = useNavigate();

  const [book, setBook] = useState({
    isbn: "",
    title: "",
    authorNif: "",
    value: "",
  });

  const [authors, setAuthors] = useState<{ nif: string; fullName: string }[]>([]);
  const [loading, setLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false); // Controla quando validar o autor

  // Buscar os autores ao carregar a página
  useEffect(() => {
    const fetchAuthors = async () => {
      try {
        const authorsData = await getAllAuthors();
        setAuthors(authorsData);
      } catch (error: any) {
        const errorMessage = error.response?.data?.message || "Erro ao buscar autores!";
        toast.error(errorMessage);
      } finally {
        setLoading(false);
      }
    };

    fetchAuthors();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setBook((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true); // Ativa a validação ao tentar submeter

    if (!book.authorNif) {
      toast.error("Por favor, selecione um autor!"); // Mensagem só aparece se tentar enviar sem autor
      return;
    }

    try {
      await createBook(book.isbn, book.title, book.authorNif, book.value);
      toast.success("Livro inserido com sucesso!");
      navigate("/books");
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || "Falha ao inserir livro!";
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
        <h1 className="text-4xl font-bold mb-4">Inserir Novo Livro</h1>

        {loading && <p className="text-white mb-4">Carregando autores...</p>}

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

          {/* Dropdown para selecionar o NIF do autor */}
          <select
            name="authorNif"
            value={book.authorNif}
            onChange={handleChange}
            className="p-2 rounded-xl bg-white text-blue-600 w-full"
            disabled={loading || authors.length === 0}
          >
            <option value="" disabled>Selecione o Autor (NIF)</option>
            {authors.map((author) => (
              <option key={author.nif} value={author.nif}>
                {author.fullName} ({author.nif})
              </option>
            ))}
          </select>

          {/* Mensagem de erro só aparece se tentar submeter sem autor */}
          {isSubmitting && !book.authorNif && (
            <p className="text-red-400 text-sm">Selecione um autor antes de enviar!</p>
          )}

          <input
            type="text"
            name="value"
            value={book.value}
            onChange={handleChange}
            placeholder="Preço (€)"
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
