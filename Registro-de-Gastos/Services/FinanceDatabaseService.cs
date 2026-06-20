using Gastos.Models;
using SQLite;

namespace Gastos.Services;

public sealed class FinanceDatabaseService
{
	private readonly SQLiteAsyncConnection connection;
	private bool initialized;
	private readonly SemaphoreSlim initializationLock = new(1, 1);

	private FinanceDatabaseService()
	{
		var databasePath = Path.Combine(FileSystem.AppDataDirectory, "finance.db3");
		connection = new SQLiteAsyncConnection(databasePath);
	}

	public static FinanceDatabaseService Instance { get; } = new();

	public async Task InitializeAsync()
	{
		if (initialized)
		{
			return;
		}

		await initializationLock.WaitAsync();
		try
		{
			if (initialized)
			{
				return;
			}

			await connection.CreateTableAsync<FinancialTransaction>();
			await EnsureDescriptionColumnAsync();
			initialized = true;
		}
		finally
		{
			initializationLock.Release();
		}
	}

	public async Task<List<FinancialTransaction>> GetTransactionsAsync()
	{
		await InitializeAsync();
		return await connection.Table<FinancialTransaction>()
			.OrderByDescending(transaction => transaction.CreatedAt)
			.ToListAsync();
	}

	public async Task<int> GetBalanceAsync()
	{
		await InitializeAsync();
		var transactions = await connection.Table<FinancialTransaction>().ToListAsync();
		return transactions.Sum(transaction => transaction.IsIncome ? transaction.Amount : -transaction.Amount);
	}

	public async Task AddTransactionAsync(FinancialTransaction transaction)
	{
		await InitializeAsync();
		await connection.InsertAsync(transaction);
	}

	private async Task EnsureDescriptionColumnAsync()
	{
		var columns = await connection.QueryAsync<TableColumnInfo>("PRAGMA table_info(FinancialTransaction);");
		if (columns.Any(column => string.Equals(column.name, nameof(FinancialTransaction.Description), StringComparison.OrdinalIgnoreCase)))
		{
			return;
		}

		await connection.ExecuteAsync($"ALTER TABLE FinancialTransaction ADD COLUMN {nameof(FinancialTransaction.Description)} TEXT NOT NULL DEFAULT '';");
	}

	private sealed class TableColumnInfo
	{
		public string name { get; set; } = string.Empty;
	}
}