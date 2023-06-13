using System;
using System.Data.SqlClient;
using System.Net.Sockets;

namespace Negocio
{
	public class AccesoDB
	{
		private SqlConnection connection;
		private SqlCommand command;
		private SqlDataReader reader;

		// Datos de la Base
		//DB Alejandro
		string serverName = "localhost\\SQLEXPRESS";
		//DB Diego
		//
		//DB Fede
		//

		
		//DB Nombre
		string dataBase = "RestoDB";

		//Datos de la Base - SOMEE
		//string connectionString = WebConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

		public AccesoDB()
		{
			connection = new SqlConnection($"server={serverName}; database={dataBase}; integrated security=true; TrustServerCertificate=True");
			//connection = new SqlConnection(connectionString);
			command = new SqlCommand();
		}

		public SqlDataReader Reader
		{
			get { return reader; }
		}

		public void setQuery(string query)
		{
			command.CommandType = System.Data.CommandType.Text;
			command.CommandText = query;
		}

		public void executeReader()
		{
			command.Connection = connection;
			try
			{
				connection.Open();
				reader = command.ExecuteReader();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool executeNonQuery()
		{
			command.Connection = connection;
			try
			{
				connection.Open();
				if (command.ExecuteNonQuery() > 0)
					return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return false;
		}

		public int executeScalar()
		{
			command.Connection = connection;
			try
			{
				connection.Open();
				return Convert.ToInt32(command.ExecuteScalar());
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void closeConnection()
		{
			connection.Close();
		}

		public void setParameter(string nombre, object valor)
		{
			command.Parameters.AddWithValue(nombre, valor);
		}


	}
}