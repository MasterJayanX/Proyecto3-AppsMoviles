using Gastos.ViewModels;

namespace Gastos;

public partial class SecondPage : ContentPage
{
	public SecondPage()
	{
		InitializeComponent();
		BindingContext = new TransactionEntryViewModel();
	}
}