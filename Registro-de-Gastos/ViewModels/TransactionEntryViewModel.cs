using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gastos.Models;
using Gastos.Services;

namespace Gastos.ViewModels;

public partial class TransactionEntryViewModel : ObservableObject
{
	private readonly FinanceDatabaseService database = FinanceDatabaseService.Instance;

	[ObservableProperty]
	private string glosa = string.Empty;

	[ObservableProperty]
	private string cantidad = string.Empty;

	[ObservableProperty]
	private DateTime fechaSeleccionada = DateTime.Today;

	[ObservableProperty]
	private bool esIngreso = false;

	[RelayCommand]
	private async Task GuardarAsync()
	{
		if (!int.TryParse(Cantidad, out var monto) || monto <= 0)
		{
			await Shell.Current.DisplayAlertAsync("Error", "Ingresa una cantidad válida mayor que cero.", "Aceptar");
			return;
		}

		await database.AddTransactionAsync(new FinancialTransaction
		{
			Amount = monto,
			Description = Glosa.Trim(),
			IsIncome = EsIngreso,
			CreatedAt = FechaSeleccionada
		});

		Glosa = string.Empty;
		Cantidad = string.Empty;
		FechaSeleccionada = DateTime.Today;
		EsIngreso = false;

		await Shell.Current.GoToAsync("//MainPage");
	}

	[RelayCommand]
	private async Task CancelarAsync()
	{
		await Shell.Current.GoToAsync("//MainPage");
	}
}