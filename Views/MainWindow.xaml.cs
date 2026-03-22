using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FinanceiroObserver.App.Services;
using FinanceiroObserver.App.ViewModels;

namespace FinanceiroObserver.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // 1. Cria a ViewModel
        var viewModel = new MonitorAcoesViewModel();
        
        // 2. Conecta a ViewModel á Tela (View)
        this.DataContext = viewModel;
       
        // 3.Cria o motor e inscreve a ViewModel nele
        var motor = new MotorMercado();
        motor.Inscrever(viewModel);
    }
}