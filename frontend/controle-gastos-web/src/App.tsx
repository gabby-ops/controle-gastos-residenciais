import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Menu from "./components/Menu";
import Pessoas from "./pages/Pessoas";
import Transacoes from "./pages/Transacoes";
import Totais from "./pages/Totais";
import "./App.css";

export default function App() {
  return (
    <BrowserRouter>
      <div className="app">
        <Menu />
        <main className="conteudo">
          <Routes>
            <Route path="/" element={<Navigate to="/pessoas" />} />
            <Route path="/pessoas" element={<Pessoas />} />
            <Route path="/transacoes" element={<Transacoes />} />
            <Route path="/totais" element={<Totais />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}