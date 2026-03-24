using System;
using System.Collections.Generic;
using FinanceiroObserver.App.Interfaces;
using FinanceiroObserver.App.Models;

namespace FinanceiroObserver.App.Services;

public class MotorMercado
{
    private readonly List<IObservadorAcoes> _observadores = new();
    private readonly System.Timers.Timer _timer;
    private readonly Random _random = new();

    public MotorMercado()
    {
        _timer = new System.Timers.Timer(2000); // 2 segundos
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
        double novoPreco = 100 + (_random.NextDouble() * 10);
        var acao = new Acao { Simbolo = "PETR4", Preco = novoPreco };

        foreach (var observador in _observadores)
        {
            observador.Atualizar(acao);
        }
    }
}