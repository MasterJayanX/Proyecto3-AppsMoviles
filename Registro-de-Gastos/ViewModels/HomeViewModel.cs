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
	private string resumenTexto = "Se inicia con saldo en cero.";

	[ObservableProperty]
	private bool isBusy;

	[RelayCommand]
	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		IsBusy = true;
		try
		{
			var transactions = await database.GetTransactionsAsync();
			var balance = await database.GetBalanceAsync();

			Movimientos = new ObservableCollection<FinancialTransaction>(transactions);
			TotalDisponibleTexto = FormatCurrency(balance);
			IngresosTexto = FormatCurrency(transactions.Where(transaction => transaction.IsIncome).Sum(transaction => transaction.Amount));
			EgresosTexto = FormatCurrency(transactions.Where(transaction => !transaction.IsIncome).Sum(transaction => transaction.Amount));
			ResumenTexto = Movimientos.Count == 0
				? "Se inicia con saldo en cero."
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

	private static string FormatCurrency(int amount)
	{
		return "$" + amount.ToString("N0", CultureInfo.GetCultureInfo("es-CL"));
	}
}