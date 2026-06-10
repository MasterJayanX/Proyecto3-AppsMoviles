namespace Gastos.Models;

public class TipCalculator
{
    public int CalcularSubtotalPorPersona(double total, int personas)
    {
        return personas > 0 ? (int)(total / personas) : 0;
    }

    public int CalcularPropinaPorPersona(double total, int porcentajePropina, int personas)
    {
        if (personas <= 0)
        {
            return 0;
        }

        var totalPropina = total * porcentajePropina / 100.0;
        return (int)(totalPropina / personas);
    }

    public int CalcularTotalPorPersona(double total, int porcentajePropina, int personas)
    {
        if (personas <= 0)
        {
            return 0;
        }

        var totalConPropina = total + (total * porcentajePropina / 100.0);
        return (int)(totalConPropina / personas);
    }
}
