using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FinanceiroObserver.App.Interfaces;
using FinanceiroObserver.App.Models;

namespace FinanceiroObserver.App.ViewModels;

// A ViewModel precisa "assinar" o contrato de Observador para receber os dados
public class MonitorAcoesViewModel : IObservadorAcoes, INotifyPropertyChanged
{
    private string _precoAtual = "Aguardando...";
    
    // Lista para exibir o histórico na tela (WPF usam ObservableCollection)
    public ObservableCollection<string> Historico { get; } = new();

    public string PrecoAtual //tela (XAML) vai "observar".
    {
        get => _precoAtual;
        set
        {
            _precoAtual = value;
            OnPropertyChanged(); // Avisa a tela que o preço mudou
        }
    }

    // Este é o método é chamado a cada 2 segundos
    public void Atualizar(Acao acao)
    {
        // Como o Timer do Motor roda em outra thread.
        var info = $"[{DateTime.Now:HH:mm:ss}] {acao.Simbolo}: R$ {acao.Preco:F2}";
        
        PrecoAtual = info; 
        
        // Mantém apenas as últimas 10 variações no histórico
        // O Dispatcher separa o código do timer para não travar a interface gráfica.
        App.Current.Dispatcher.Invoke(() => {  
            Historico.Insert(0, info); //coloca o preço novo no topo da lista
            if (Historico.Count > 10) Historico.RemoveAt(10); //deixa os 10 últimos preços, removendo os demais.
        });
    }

    #region Implementação de INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged; // Evento que a tela assina para saber se ouve mundanças
    protected void OnPropertyChanged([CallerMemberName] string? name = null) //CallerMemberName - descobre sozinho qual propriedade chamou o método,
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    #endregion
}