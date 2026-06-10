using Gastos.ViewModels;
using System.Text;

namespace Gastos;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }

    private void OnTotalEntradaTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry || string.IsNullOrEmpty(e.NewTextValue))
        {
            return;
        }

        var soloDigitos = new StringBuilder(e.NewTextValue.Length);

        foreach (var caracter in e.NewTextValue)
        {
            if (char.IsDigit(caracter))
            {
                soloDigitos.Append(caracter);
            }
        }

        var textoFiltrado = soloDigitos.ToString();

        if (textoFiltrado != e.NewTextValue)
        {
            entry.Text = textoFiltrado;
        }
    }
}
