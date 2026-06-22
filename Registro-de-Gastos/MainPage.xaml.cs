using Gastos.ViewModels;
using Gastos.Services;

namespace Gastos;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new HomeViewModel();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		// Inicializar base de datos si no está inicializada
		await FinanceDatabaseService.Instance.InitializeAsync();

		if (BindingContext is HomeViewModel viewModel)
		{
			viewModel.CargarCommand.Execute(null);
		}
	}
}
