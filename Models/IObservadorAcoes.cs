using FinanceiroObserver.App.Models;

namespace FinanceiroObserver.App.Interfaces;

/// O Contrato (Interface) que os observadores devem implementar
public interface IObservadorAcoes
{
    /// Método chamado pelo Sujeito (Motor) para notificar o Observador sobre uma mudança.
    void Atualizar(Acao acao);
}