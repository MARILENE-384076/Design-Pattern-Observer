using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using FinanceiroObserver.App.Interfaces;
using FinanceiroObserver.App.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace FinanceiroObserver.App.ViewModels;

public class HistoricoItem
{
    public string Texto { get; set; } = string.Empty;
    public Brush Cor { get; set; } = Brushes.Gray;
}

public class MonitorAcoesViewModel : IObservadorAcoes, INotifyPropertyChanged
{
    private string _precoAtual = "Aguardando...";
    private double _ultimoPrecoValor = 0;
    private Brush _corTendencia = Brushes.Black;

    public ObservableCollection<HistoricoItem> Historico { get; } = new();
    public SeriesCollection SeriesCollection { get; set; }
    private ChartValues<double> _valoresGrafico;

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

    public MonitorAcoesViewModel()
    {
        _valoresGrafico = new ChartValues<double>();
        SeriesCollection = new SeriesCollection
        {
            new LineSeries
            {
                Title = "PETR4",
                Values = _valoresGrafico,
                PointGeometry = null,
                StrokeThickness = 3,
                Fill = Brushes.Transparent
            }
        };
    }

    public void Atualizar(Acao acao)
    {
        Brush corAtual = Brushes.Gray;
        if (_ultimoPrecoValor > 0)
        {
            if (acao.Preco > _ultimoPrecoValor) corAtual = Brushes.MediumSeaGreen;
            else if (acao.Preco < _ultimoPrecoValor) corAtual = Brushes.IndianRed;
        }

        _ultimoPrecoValor = acao.Preco;

        System.Windows.Application.Current.Dispatcher.Invoke(() => {
            CorTendencia = corAtual;
            ((LineSeries)SeriesCollection[0]).Stroke = corAtual;
            
            PrecoAtual = $"[{DateTime.Now:HH:mm:ss}] {acao.Simbolo}: R$ {acao.Preco:F2}";
            Historico.Insert(0, new HistoricoItem { Texto = PrecoAtual, Cor = corAtual });
            
            if (Historico.Count > 10) Historico.RemoveAt(10);
            
            _valoresGrafico.Add(acao.Preco);
            if (_valoresGrafico.Count > 25) _valoresGrafico.RemoveAt(0);
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}