import { useEffect, useState } from "react";
import { obterTotais } from "../api/api";
import type { TotaisResponse } from "../types";
import Loading from "../components/Loading";

export default function Totais() {
  const [totais, setTotais] = useState<TotaisResponse | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    obterTotais()
      .then((res) => setTotais(res.data))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <Loading />;
  if (!totais) return <p>Erro ao carregar totais.</p>;

  return (
    <div className="page totais">
      <div className="page-eyebrow">Relatórios</div>
      <h1>Totais</h1>
      <p className="page-subtitle">Resumo financeiro por pessoa e saldo geral da residência.</p>

      <div className="stat-cards">
        <div className="stat-card">
          <div className="stat-label">
            <svg viewBox="0 0 24 24" fill="none"><path d="M23 6l-9.5 9.5-5-5L1 18" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/></svg>
            Total de Receitas
          </div>
          <div className="stat-value positivo">R$ {totais.totalGeralReceitas.toFixed(2)}</div>
        </div>

        <div className="stat-card">
          <div className="stat-label">
            <svg viewBox="0 0 24 24" fill="none"><path d="M23 18l-9.5-9.5-5 5L1 6" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/></svg>
            Total de Despesas
          </div>
          <div className="stat-value negativo">R$ {totais.totalGeralDespesas.toFixed(2)}</div>
        </div>

        <div className="stat-card stat-card-dark">
          <div className="stat-label">
            <svg viewBox="0 0 24 24" fill="none"><rect x="1" y="4" width="22" height="16" rx="2" stroke="currentColor" strokeWidth="2"/></svg>
            Saldo Líquido Geral
          </div>
          <div className="stat-value">R$ {totais.saldoLiquidoGeral.toFixed(2)}</div>
        </div>
      </div>

      <h3 className="section-title">Resumo por Pessoa</h3>

      <table>
        <thead>
          <tr>
            <th>Pessoa</th>
            <th>Receitas</th>
            <th>Despesas</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {totais.pessoas.map((p) => (
            <tr key={p.nome}>
              <td data-label="Pessoa">{p.nome}</td>
              <td data-label="Receitas" className="positivo">R$ {p.totalReceitas.toFixed(2)}</td>
              <td data-label="Despesas" className="negativo">R$ {p.totalDespesas.toFixed(2)}</td>
              <td data-label="Saldo" className={p.saldo < 0 ? "negativo" : "positivo"}>
                R$ {p.saldo.toFixed(2)}
              </td>
            </tr>
          ))}
        </tbody>
        <tfoot>
          <tr>
            <td data-label="Pessoa">TOTAL GERAL</td>
            <td data-label="Receitas" className="positivo">R$ {totais.totalGeralReceitas.toFixed(2)}</td>
            <td data-label="Despesas" className="negativo">R$ {totais.totalGeralDespesas.toFixed(2)}</td>
            <td data-label="Saldo" className={totais.saldoLiquidoGeral < 0 ? "negativo" : "positivo"}>
              R$ {totais.saldoLiquidoGeral.toFixed(2)}
            </td>
          </tr>
        </tfoot>
      </table>
    </div>
  );
}