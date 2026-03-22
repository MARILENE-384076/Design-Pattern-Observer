using System;
using System.Collections.Generic;
using System.Timers;
using FinanceiroObserver.App.Interfaces;
using FinanceiroObserver.App.Models;

namespace FinanceiroObserver.App.Services;

public class MotorMercado
{
    /// Cria a lista de Observadores
    private readonly List<IObservadorAcoes> _observadorAcoes = new();
    
    /// Declara o temporizador para executar ações em intervalos regulares.
    private readonly Timer _timer;
    
    /// Cria um gerador de números aleatórios para simular a variação da bolsa.
    private readonly Random _random = new();
    
    public MotorMercado()
    {
        _timer = new Timer(2000); // Notifica a cada 2 segundos
        _timer.Elapsed += (s, e) => GerarVariacao();
        _timer.Start();
    }
    public void Inscrever(IObservadorAcoes observador) => _observadores.Add(observador);
    public void Desinscrever(IObservadorAcoes observador) => _observadores.Remove(observador);

    private void GerarVariacao()
    {
        double novoPreco = 100 + (_random.NextDouble() * 10);
        var acao = new Acao("PETR4", Math.Round(novoPreco, 2));

        foreach (var obs in _observadores)
        {
            obs.Atualizar(acao);
        }
        
    }
    
}