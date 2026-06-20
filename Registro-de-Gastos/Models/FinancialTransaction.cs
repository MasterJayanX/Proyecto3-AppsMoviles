using System.Globalization;
using SQLite;

namespace Gastos.Models;

public class FinancialTransaction
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	public int Amount { get; set; }

	public string Description { get; set; } = string.Empty;

	public bool IsIncome { get; set; }

	public DateTime CreatedAt { get; set; }

	[Ignore]
	public string MovementTypeLabel => IsIncome ? "Ingreso" : "Egreso";

	[Ignore]
	public string AmountLabel => (IsIncome ? "+" : "-") + FormatCurrency(Amount);

	[Ignore]
	public string CreatedAtLabel => CreatedAt.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);

	[Ignore]
	public string TypeColor => IsIncome ? "#23C483" : "#E45B73";

	private static string FormatCurrency(int amount)
	{
		return "$" + amount.ToString("N0", CultureInfo.GetCultureInfo("es-CL"));
	}
}