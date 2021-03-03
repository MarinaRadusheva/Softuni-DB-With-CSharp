namespace MiniORM
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// Used for wrapping a database connection with a using statement and
	/// automatically closing it when the using statement ends
	/// </summary>
	internal class ConnectionManager : IDisposable
	{
		private readonly DatabaseConnection connection;

		public ConnectionManager(DatabaseConnection connection)
		{
			this.connection = connection;

			this.connection.Open();
		}

		public void Dispose()
		{
			this.connection.Close();
		}
	}
}