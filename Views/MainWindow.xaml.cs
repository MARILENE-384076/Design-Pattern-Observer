using System.Windows;
using FinanceiroObserver.App.Services;
using FinanceiroObserver.App.ViewModels;

namespace FinanceiroObserver.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var viewModel = new MonitorAcoesViewModel();
        this.DataContext = viewModel;
       
        var motor = new MotorMercado();
        motor.Inscrever(viewModel);
    }
}