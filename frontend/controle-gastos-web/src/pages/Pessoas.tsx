import { useEffect, useState } from "react";
import { listarPessoas, criarPessoa, excluirPessoa } from "../api/api";
import type { Pessoa } from "../types";
import Loading from "../components/Loading";
import Toast from "../components/Toast";

export default function Pessoas() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState("");
  const [loading, setLoading] = useState(true);
  const [salvando, setSalvando] = useState(false);
  const [toast, setToast] = useState<{ msg: string; tipo: "sucesso" | "erro" } | null>(null);
  const [confirmandoExclusao, setConfirmandoExclusao] = useState<number | null>(null);

  const carregar = async () => {
    setLoading(true);
    try {
      const { data } = await listarPessoas();
      setPessoas(data);
    } catch {
      setToast({ msg: "Erro ao carregar pessoas.", tipo: "erro" });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    carregar();
  }, []);

  const handleSalvar = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!nome.trim()) {
      setToast({ msg: "O nome é obrigatório.", tipo: "erro" });
      return;
    }
    if (idade === "" || Number(idade) < 0) {
      setToast({ msg: "Informe uma idade válida.", tipo: "erro" });
      return;
    }

    setSalvando(true);
    try {
      await criarPessoa(nome, Number(idade));
      setNome("");
      setIdade("");
      setToast({ msg: "Pessoa cadastrada com sucesso!", tipo: "sucesso" });
      carregar();
    } catch (err: any) {
      const msg = err?.response?.data?.erro ?? "Erro ao cadastrar pessoa.";
      setToast({ msg, tipo: "erro" });
    } finally {
      setSalvando(false);
    }
  };

  const handleExcluir = async (id: number) => {
    try {
      await excluirPessoa(id);
      setToast({ msg: "Pessoa excluída com sucesso!", tipo: "sucesso" });
      setConfirmandoExclusao(null);
      carregar();
    } catch {
      setToast({ msg: "Erro ao excluir pessoa.", tipo: "erro" });
    }
  };

  if (loading) return <Loading />;

  return (
    <div className="page pessoas">
      <div className="page-eyebrow">
        <svg viewBox="0 0 24 24" fill="none">
          <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/>
          <circle cx="9" cy="7" r="4" stroke="currentColor" strokeWidth="2"/>
        </svg>
        Cadastros
      </div>
      <h1>Pessoas</h1>
      <p className="page-subtitle">Cadastre os moradores e gerencie quem participa dos gastos.</p>

      <div className="card">
        <h3 className="card-title">
          <svg viewBox="0 0 24 24" fill="none"><path d="M12 5v14M5 12h14" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
          Nova Pessoa
        </h3>

        <form onSubmit={handleSalvar} className="form form-grid">
          <div className="field">
            <label>Nome</label>
            <input
              placeholder="Ex: João Silva"
              value={nome}
              onChange={(e) => setNome(e.target.value)}
            />
          </div>

          <div className="field">
            <label>Idade</label>
            <input
              type="number"
              placeholder="Ex: 25"
              value={idade}
              onChange={(e) => setIdade(e.target.value)}
            />
          </div>

          <button type="submit" disabled={salvando} className="btn-icon-left">
            <svg viewBox="0 0 24 24" fill="none"><path d="M12 5v14M5 12h14" stroke="currentColor" strokeWidth="2" strokeLinecap="round"/></svg>
            {salvando ? "Salvando..." : "Salvar"}
          </button>
        </form>
      </div>

      <div className="card">
        <h3 className="card-title">Pessoas Cadastradas</h3>

        <table>
          <thead>
            <tr>
              <th>Nome</th>
              <th>Idade</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {pessoas.map((p) => (
              <tr key={p.id}>
                <td data-label="Nome">{p.nome}</td>
                <td data-label="Idade">
                  {p.idade} anos
                  {p.idade < 18 && (
                    <span className="badge badge-alerta" style={{ marginLeft: 8 }}>
                      Menor de idade
                    </span>
                  )}
                </td>
                <td data-label="Ações">
                  {confirmandoExclusao === p.id ? (
                    <>
                      <span>Confirmar? </span>
                      <button className="btn-confirmar" onClick={() => handleExcluir(p.id)}>Sim</button>
                      <button className="btn-cancelar" onClick={() => setConfirmandoExclusao(null)}>Não</button>
                    </>
                  ) : (
                    <button className="btn-excluir-icon" onClick={() => setConfirmandoExclusao(p.id)} aria-label="Excluir">
                      <svg viewBox="0 0 24 24" fill="none">
                        <path d="M3 6h18M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2m3 0v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6h14z" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                      </svg>
                    </button>
                  )}
                </td>
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