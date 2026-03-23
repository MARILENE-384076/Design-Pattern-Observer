using System;
using System.Collections.Generic;
using FinanceiroObserver.App.Interfaces;
using FinanceiroObserver.App.Models;

namespace FinanceiroObserver.App.Services;

public class MotorMercado
{
    // Lista de inscritos (Observers)
    private readonly List<IObservadorAcoes> _observadores = new();
    
    // Especificando o Timer do System.Timers para evitar erro de "Ambiguous Reference"
    private readonly System.Timers.Timer _timer;
    private readonly Random _random = new();

    public MotorMercado()
    {
        // Configura para rodar a cada 2 segundos
        _timer = new System.Timers.Timer(2000);
        _timer.Elapsed += (s, e) => GerarVariacao();
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    public void Inscrever(IObservadorAcoes observador)
    {
        if (!_observadores.Contains(observador))
            _observadores.Add(observador);
    }

    private void GerarVariacao()
    {
        // Simula variação de preço
        double novoPreco = 100 + (_random.NextDouble() * 10);
        
        // Criando o objeto Acao (formato compatível com a Model corrigida)
        var acao = new Acao { Simbolo = "PETR4", Preco = novoPreco };

        // Notifica todos os observadores na lista
        foreach (var observador in _observadores)
        {
            observador.Atualizar(acao);
        }
    }
}