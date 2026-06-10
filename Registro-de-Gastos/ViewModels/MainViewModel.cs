using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Gastos.Models;
using Microsoft.Maui.Controls;

namespace Gastos.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly TipCalculator calculadora = new();
    private string totalEntrada = string.Empty;
    private int personas = 1;
    private int porcentajePropina;
    private string subtotalPorPersona = "$0";
    private string propinaPorPersona = "$0";
    private string totalPorPersona = "$0";
    private string textoPorcentajePropina = "Propina: 0%";

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel()
    {
        ComandoIncrementarPersonas = new Command(IncrementarPersonas);
        ComandoDisminuirPersonas = new Command(DisminuirPersonas);
        EstablecerPorcentajePropina = new Command<string>(EjecutarEstablecerPorcentajePropina);

        Recalcular();
    }

    public string TotalEntrada
    {
        get => totalEntrada;
        set
        {
            if (totalEntrada == value)
            {
                return;
            }

            totalEntrada = value;
            OnPropertyChanged(nameof(TotalEntrada));
            Recalcular();
        }
    }

    public int Personas
    {
        get => personas;
        private set
        {
            if (personas == value)
            {
                return;
            }

            personas = Math.Max(1, value);
            OnPropertyChanged(nameof(Personas));
            Recalcular();
        }
    }

    public int PorcentajePropina
    {
        get => porcentajePropina;
        set
        {
            if (porcentajePropina == value)
            {
                return;
            }

            porcentajePropina = Math.Clamp(value, 0, 50);
            TextoPorcentajePropina = $"Propina: {porcentajePropina}%";
            OnPropertyChanged(nameof(PorcentajePropina));
            Recalcular();
        }
    }

    public string TextoPorcentajePropina
    {
        get => textoPorcentajePropina;
        private set
        {
            if (textoPorcentajePropina == value)
            {
                return;
            }

            textoPorcentajePropina = value;
            OnPropertyChanged(nameof(TextoPorcentajePropina));
        }
    }

    public string SubtotalPorPersona
    {
        get => subtotalPorPersona;
        private set
        {
            if (subtotalPorPersona == value)
            {
                return;
            }

            subtotalPorPersona = value;
            OnPropertyChanged(nameof(SubtotalPorPersona));
        }
    }

    public string PropinaPorPersona
    {
        get => propinaPorPersona;
        private set
        {
            if (propinaPorPersona == value)
            {
                return;
            }

            propinaPorPersona = value;
            OnPropertyChanged(nameof(PropinaPorPersona));
        }
    }

    public string TotalPorPersona
    {
        get => totalPorPersona;
        private set
        {
            if (totalPorPersona == value)
            {
                return;
            }

            totalPorPersona = value;
            OnPropertyChanged(nameof(TotalPorPersona));
        }
    }

    public ICommand ComandoIncrementarPersonas { get; }
    public ICommand ComandoDisminuirPersonas { get; }
    public ICommand EstablecerPorcentajePropina { get; }

    private void IncrementarPersonas()
    {
        Personas++;
    }

    private void DisminuirPersonas()
    {
        if (Personas > 1)
        {
            Personas--;
        }
    }

    private void EjecutarEstablecerPorcentajePropina(string parametro)
    {
        if (int.TryParse(parametro, out var porcentajeParseado))
        {
            PorcentajePropina = porcentajeParseado;
        }
    }

    private void Recalcular()
    {
        var total = ParsearTotalEntrada(TotalEntrada);

        var subtotal = calculadora.CalcularSubtotalPorPersona(total, Personas);
        var propina = calculadora.CalcularPropinaPorPersona(total, PorcentajePropina, Personas);
        var totalPersona = calculadora.CalcularTotalPorPersona(total, PorcentajePropina, Personas);

        SubtotalPorPersona = FormatearMoneda(subtotal);
        PropinaPorPersona = FormatearMoneda(propina);
        TotalPorPersona = FormatearMoneda(totalPersona);
    }

    private static int ParsearTotalEntrada(string entrada)
    {
        if (int.TryParse(entrada, NumberStyles.Number, CultureInfo.InvariantCulture, out var valor))
        {
            return valor;
        }

        if (int.TryParse(entrada, NumberStyles.Number, CultureInfo.CurrentCulture, out valor))
        {
            return valor;
        }

        return 0;
    }

    private static string FormatearMoneda(int monto)
    {
        return "$" + monto.ToString("0", CultureInfo.InvariantCulture);
    }

    private void OnPropertyChanged(string propiedad)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
    }
}
