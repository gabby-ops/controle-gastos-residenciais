import { useEffect, useState } from "react";
import { listarTransacoes, criarTransacao, listarPessoas } from "../api/api";
import type { Transacao, Pessoa, TipoTransacao } from "../types";
import Loading from "../components/Loading";
import Toast from "../components/Toast";

export default function Transacoes() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState("");
  const [tipo, setTipo] = useState<TipoTransacao>("Despesa");
  const [pessoaId, setPessoaId] = useState<string>("");
  const [loading, setLoading] = useState(true);
  const [salvando, setSalvando] = useState(false);
  const [toast, setToast] = useState<{ msg: string; tipo: "sucesso" | "erro" } | null>(null);

  const carregar = async () => {
    setLoading(true);
    try {
      const [respTransacoes, respPessoas] = await Promise.all([
        listarTransacoes(),
        listarPessoas(),
      ]);
      setTransacoes(respTransacoes.data);
      setPessoas(respPessoas.data);
    } catch {
      setToast({ msg: "Erro ao carregar dados.", tipo: "erro" });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    carregar();
  }, []);

  const handleSalvar = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!descricao.trim()) {
      setToast({ msg: "A descrição é obrigatória.", tipo: "erro" });
      return;
    }
    if (!valor || Number(valor) <= 0) {
      setToast({ msg: "O valor deve ser maior que zero.", tipo: "erro" });
      return;
    }
    if (!pessoaId) {
      setToast({ msg: "Selecione uma pessoa.", tipo: "erro" });
      return;
    }

    setSalvando(true);
    try {
      await criarTransacao(descricao, Number(valor), tipo, Number(pessoaId));
      setDescricao("");
      setValor("");
      setToast({ msg: "Transação cadastrada com sucesso!", tipo: "sucesso" });
      carregar();
    } catch (err: any) {
      const msg = err?.response?.data?.erro ?? "Erro ao cadastrar transação.";
      setToast({ msg, tipo: "erro" });
    } finally {
      setSalvando(false);
    }
  };

  if (loading) return <Loading />;

  return (
    <div className="page transacoes">
      <div className="page-eyebrow">
        <svg viewBox="0 0 24 24" fill="none">
          <path d="M17 1l4 4-4 4" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          <path d="M3 11V9a4 4 0 0 1 4-4h14" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          <path d="M7 23l-4-4 4-4" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
          <path d="M21 13v2a4 4 0 0 1-4 4H3" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
        </svg>
        Financeiro
      </div>
      <h1>Transações</h1>
      <p className="page-subtitle">Registre receitas e despesas de cada morador.</p>

      <div className="card">
        <h3 className="card-title">
          <svg viewBox="0 0 24 24" fill="none"><path d="M12 5v14M5 12h14" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
          Nova Transação
        </h3>

        <form onSubmit={handleSalvar} className="form form-grid">
          <div className="field">
            <label>Descrição</label>
            <input
              placeholder="Ex: Mercado do mês"
              value={descricao}
              onChange={(e) => setDescricao(e.target.value)}
            />
          </div>

          <div className="field">
            <label>Valor (R$)</label>
            <input
              type="number"
              placeholder="0,00"
              value={valor}
              onChange={(e) => setValor(e.target.value)}
            />
          </div>

          <div className="field">
            <label>Tipo</label>
            <select value={tipo} onChange={(e) => setTipo(e.target.value as TipoTransacao)}>
              <option value="Despesa">Despesa</option>
              <option value="Receita">Receita</option>
            </select>
          </div>

          <div className="field">
            <label>Pessoa</label>
            <select value={pessoaId} onChange={(e) => setPessoaId(e.target.value)}>
              <option value="">Selecione...</option>
              {pessoas.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.nome}
                </option>
              ))}
            </select>
          </div>
        </form>

        <button
          type="submit"
          onClick={handleSalvar}
          disabled={salvando}
          className="btn-primary"
          style={{ marginTop: 16 }}
        >
          <svg viewBox="0 0 24 24" fill="none"><path d="M12 5v14M5 12h14" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
          {salvando ? "Salvando..." : "Salvar Transação"}
        </button>
      </div>

      <div className="card">
        <h3 className="card-title">Transações Registradas</h3>

        <table>
          <thead>
            <tr>
              <th>Descrição</th>
              <th>Valor</th>
              <th>Tipo</th>
              <th>Pessoa</th>
            </tr>
          </thead>
          <tbody>
            {transacoes.map((t) => (
              <tr key={t.id}>
                <td data-label="Descrição"><strong>{t.descricao}</strong></td>
                <td data-label="Valor">R$ {t.valor.toFixed(2)}</td>
                <td data-label="Tipo">
                  <span className={`badge ${t.tipo === "Receita" ? "badge-receita" : "badge-despesa"}`}>
                    {t.tipo === "Receita" ? "↗" : "↘"} {t.tipo}
                  </span>
                </td>
                <td data-label="Pessoa">{t.pessoaNome}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {toast && (
        <Toast mensagem={toast.msg} tipo={toast.tipo} onFechar={() => setToast(null)} />
      )}
    </div>
  );
}