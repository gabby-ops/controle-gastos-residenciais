interface ToastProps {
  mensagem: string;
  tipo: "sucesso" | "erro";
  onFechar: () => void;
}

export default function Toast({ mensagem, tipo, onFechar }: ToastProps) {
  return (
    <div className={`toast toast-${tipo}`} onClick={onFechar}>
      {mensagem}
    </div>
  );
}