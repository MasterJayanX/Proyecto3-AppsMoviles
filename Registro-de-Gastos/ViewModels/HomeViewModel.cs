using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gastos.Models;
using Gastos.Services;

namespace Gastos.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	private readonly FinanceDatabaseService database = FinanceDatabaseService.Instance;

	[ObservableProperty]
	private ObservableCollection<FinancialTransaction> movimientos = new();

	[ObservableProperty]
	private string totalDisponibleTexto = "$0";

	[ObservableProperty]
	private string ingresosTexto = "$0";

	[ObservableProperty]
	private string egresosTexto = "$0";

	[ObservableProperty]
	private string resumenTexto = "Sin datos, click en el botón + para agregar una nueva transacción";

	[ObservableProperty]
	private bool isBusy;

	[RelayCommand]
	private async Task CargarAsync()
	{
		if (IsBusy)
		{
			return;
		}

		IsBusy = true;
		try
		{
			var transacciones = await database.GetTransactionsAsync();

			Movimientos = new ObservableCollection<FinancialTransaction>(transacciones);
			TotalDisponibleTexto = FormatearMoneda(CalcularBalance(transacciones));
			IngresosTexto = FormatearMoneda(CalcularIngresos(transacciones));
			EgresosTexto = FormatearMoneda(CalcularGastos(transacciones));
			ResumenTexto = Movimientos.Count == 0
				? "Sin datos, click en el botón + para agregar una nueva transacción"
				: $"{Movimientos.Count} movimientos registrados.";
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task IrACrearMovimientoAsync()
	{
		await Shell.Current.GoToAsync("//SecondPage");
	}

	private static int CalcularBalance(List<FinancialTransaction> transacciones)
	{
		return transacciones.Sum(transaccion => transaccion.IsIncome ? transaccion.Amount : -transaccion.Amount);
	}

	private static int CalcularIngresos(List<FinancialTransaction> transacciones)
	{
		return transacciones.Where(transaccion => transaccion.IsIncome).Sum(transaccion => transaccion.Amount);
	}

	private static int CalcularGastos(List<FinancialTransaction> transacciones)
	{
		return transacciones.Where(transaccion => !transaccion.IsIncome).Sum(transaccion => transaccion.Amount);
	}

	private static string FormatearMoneda(int monto)
	{
		return "$" + monto.ToString("N0", CultureInfo.GetCultureInfo("es-CL"));
	}
}