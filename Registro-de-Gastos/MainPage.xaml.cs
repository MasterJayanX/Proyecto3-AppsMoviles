using Gastos.ViewModels;

namespace Gastos;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new HomeViewModel();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is HomeViewModel viewModel)
		{
			viewModel.LoadCommand.Execute(null);
		}
	}
}
