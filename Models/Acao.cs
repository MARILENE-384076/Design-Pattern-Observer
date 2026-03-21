namespace FinanceiroObserver.App.Models;

    // usei o 'record' pela imutabilidade e facilidade de transporte, além de possuir o construtor implicito.
    public record Acao(string Simbolo, double Preco);


