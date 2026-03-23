using System.Windows;
using FinanceiroObserver.App.Services;
using FinanceiroObserver.App.ViewModels;

namespace FinanceiroObserver.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // 1. Instancia a ViewModel
        var viewModel = new MonitorAcoesViewModel();
        
        // 2. Conecta ao DataContext da Janela
        this.DataContext = viewModel;
       
        // 3. Inicia o Motor e Inscreve a ViewModel
        var motor = new MotorMercado();
        motor.Inscrever(viewModel);
    }
}