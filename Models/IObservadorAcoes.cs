using FinanceiroObserver.App.Models;

namespace FinanceiroObserver.App.Interfaces;

public interface IObservadorAcoes
{
    void Atualizar(Acao acao);
}