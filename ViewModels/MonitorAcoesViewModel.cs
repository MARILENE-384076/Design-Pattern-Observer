using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media; // Para as cores Green e Red
using FinanceiroObserver.App.Interfaces;
using FinanceiroObserver.App.Models;

namespace FinanceiroObserver.App.ViewModels;

public class MonitorAcoesViewModel : IObservadorAcoes, INotifyPropertyChanged
{
    private string _precoAtual = "Aguardando...";
    private double _ultimoPrecoValor = 0;
    private Brush _corTendencia = Brushes.Black;

    public ObservableCollection<string> Historico { get; } = new();

    public string PrecoAtual 
    {
        get => _precoAtual;
        set { _precoAtual = value; OnPropertyChanged(); }
    }

    public Brush CorTendencia 
    {
        get => _corTendencia;
        set { _corTendencia = value; OnPropertyChanged(); }
    }

    public void Atualizar(Acao acao)
    {
        // Lógica de cores baseada na variação
        if (_ultimoPrecoValor > 0)
        {
            if (acao.Preco > _ultimoPrecoValor)
                CorTendencia = Brushes.MediumSeaGreen; 
            else if (acao.Preco < _ultimoPrecoValor)
                CorTendencia = Brushes.IndianRed;
        }

        _ultimoPrecoValor = acao.Preco;

        var info = $"[{DateTime.Now:HH:mm:ss}] {acao.Simbolo}: R$ {acao.Preco:F2}";
        
        // Atualiza a UI de forma segura
        System.Windows.Application.Current.Dispatcher.Invoke(() => {
            PrecoAtual = info; 
            Historico.Insert(0, info); 
            if (Historico.Count > 10) Historico.RemoveAt(10);
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}