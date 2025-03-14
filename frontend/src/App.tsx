import React, { useEffect, useState } from "react";
import api from "./services/api";

function App() {
  const [data, setData] = useState<string>("");

  useEffect(() => {
    api.get("http://localhost:5000/api/Book/GetAll")
      .then(response => {
        console.log("Resposta do backend:", response.data); // 🟢 Verificar se há dados
        setData(JSON.stringify(response.data, null, 2)); // Formatar JSON
      })
      .catch(error => console.error("Erro ao buscar dados:", error));
  }, []);

  return (
    <div>
      <h1>Frontend React + TypeScript</h1>
      <p>Dados do backend: {data}</p>
    </div>
  );
}

export default App;
