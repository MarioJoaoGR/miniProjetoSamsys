import { motion } from "framer-motion";
import { useNavigate } from "react-router-dom";
import { Button } from "../components/ui/button";

export default function Home() {
  const navigate = useNavigate();

  return (
    <div className="flex items-center justify-center min-h-screen bg-gradient-to-r from-blue-600 via-indigo-800 to-purple-900">
      <motion.div
        initial={{ opacity: 0, y: -50 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 1 }}
        className="text-center p-12 bg-white/30 backdrop-blur-lg rounded-3xl shadow-2xl max-w-lg w-full"
      >
        <h1 className="text-5xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-pink-500 to-yellow-400">
          Bem-vindo à Biblioteca da Samsys 📚
        </h1>
        <p className="mt-6 text-lg font-medium text-gray-100 leading-relaxed">
          Explora a nossa coleção de livros. Adiciona, edita e gere as tuas leituras com facilidade!
        </p>
        <Button
          onClick={() => navigate("/books")}
          className="mt-8 px-8 py-4 text-xl bg-gradient-to-r from-blue-500 to-indigo-400 hover:from-blue-600 hover:to-indigo-500 rounded-2xl shadow-lg transition-transform transform hover:scale-110"
        >
          📖Livros
        </Button>
      </motion.div>
    </div>
  );
}
