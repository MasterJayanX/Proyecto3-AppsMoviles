using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gastos.Models;
using Gastos.Services;

namespace Gastos.ViewModels;

public partial class TransactionEntryViewModel : ObservableObject
{
	private readonly FinanceDatabaseService database = FinanceDatabaseService.Instance;

	[ObservableProperty]
	private string amountText = string.Empty;

	[ObservableProperty]
	private string descriptionText = string.Empty;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(OperationLabel))]
	private bool isIncome = true;

	[ObservableProperty]
	private string statusMessage = "Ingresa el monto y elige el tipo de movimiento.";

	public string OperationLabel => IsIncome ? "Ingreso" : "Egreso";

	[RelayCommand]
	private void SetIncome()
	{
		IsIncome = true;
		StatusMessage = "Movimiento configurado como ingreso.";
	}

	[RelayCommand]
	private void SetExpense()
	{
		IsIncome = false;
		StatusMessage = "Movimiento configurado como egreso.";
	}

	[RelayCommand]
	private async Task SaveAsync()
	{
		if (!int.TryParse(AmountText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount) || amount <= 0)
		{
			StatusMessage = "Ingresa una suma válida mayor que cero.";
			return;
		}

		await database.AddTransactionAsync(new FinancialTransaction
		{
			Amount = amount,
			Description = DescriptionText.Trim(),
			IsIncome = IsIncome,
			CreatedAt = DateTime.Now
		});

		var efectoMovimiento = IsIncome ? "sumado" : "restado";
		AmountText = string.Empty;
		DescriptionText = string.Empty;
		StatusMessage = $"Movimiento guardado correctamente. El monto fue {efectoMovimiento} al total.";
		await Shell.Current.GoToAsync("//MainPage");
	}

	[RelayCommand]
	private async Task GoHomeAsync()
	{
		await Shell.Current.GoToAsync("//MainPage");
	}
}