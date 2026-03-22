using System;
using System.Collections.Generic;
using System.Timers;
using FinanceiroObserver.App.Interfaces;
using FinanceiroObserver.App.Models;

namespace FinanceiroObserver.App.Services;

public class MotorMercado
{
    /// Cria a lista de Observadores
    // CORREÇÃO: Faltava o '=' antes de new()
    private readonly List<IObservadorAcoes> _observadores = new();
    
    /// Declara o temporizador usando o nome completo para evitar conflitos (Ambiguous Reference)
    private readonly System.Timers.Timer _timer;
    
    /// Cria um gerador de números aleatórios para simular a variação da bolsa.
    private readonly Random _random = new();
    
    public MotorMercado()
    {
        // CORREÇÃO: Usando System.Timers.Timer explicitamente
        _timer = new System.Timers.Timer(2000); 
        _timer.Elapsed += (s, e) => GerarVariacao();
        _timer.AutoReset = true; // Garante que ele continue repetindo
        _timer.Start();
    }

    // CORREÇÃO: O nome da variável agora é exatamente '_observadores'
    public void Inscrever(IObservadorAcoes observador) => _observadores.Add(observador);
    
    public void Desinscrever(IObservadorAcoes observador) => _observadores.Remove(observador);

    private void GerarVariacao()
    {
        double novoPreco = 100 + (_random.NextDouble() * 10);
        var acao = new Acao("PETR4", Math.Round(novoPreco, 2));

        // Notifica todos os inscritos
        foreach (var obs in _observadores)
        {
            obs.Atualizar(acao);
        }
    }
}