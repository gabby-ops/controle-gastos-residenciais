import axios from "axios";
import type { Pessoa, Transacao, TotaisResponse } from "../types";

// A URL da API vem de variável de ambiente (.env), com fallback para
// a porta padrão do template do .NET. Basta criar um arquivo .env
// (copiando o .env.example) se a porta do seu backend for diferente.
const baseURL = import.meta.env.VITE_API_URL ?? "http://localhost:5231/api";

const api = axios.create({ baseURL });

// Pessoas
export const listarPessoas = () => api.get<Pessoa[]>("/pessoas");

export const criarPessoa = (nome: string, idade: number) =>
  api.post<Pessoa>("/pessoas", { nome, idade });

export const excluirPessoa = (id: number) => api.delete(`/pessoas/${id}`);

// Transações
export const listarTransacoes = () => api.get<Transacao[]>("/transacoes");

export const criarTransacao = (
  descricao: string,
  valor: number,
  tipo: "Receita" | "Despesa",
  pessoaId: number
) => api.post<Transacao>("/transacoes", { descricao, valor, tipo, pessoaId });

// Totais
export const obterTotais = () => api.get<TotaisResponse>("/totais");

export default api;