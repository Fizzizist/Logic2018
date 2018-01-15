using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
namespace Logic2018
{
    public class SaveCloud
    {
        
		private MySqlConnection connection;
		private string server;
		private string database;
		private string uid;
		private string password;

		//Constructor
		public SaveCloud()
		{
			Initialize();
		}

		//Initialize values
		private void Initialize()
		{
			server = "160.153.93.164";
			database = "Logic2018";
			uid = "logicUser";
			password = "thisislogic2018";
			string connectionString;
			connectionString = "SERVER=" + server + ";" + "DATABASE=" +
			database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

			connection = new MySqlConnection(connectionString);
		}

		//open connection to database
		private bool OpenConnection()
		{
            try
            {
                connection.Open();
                return true;
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e);
                return false;
            }
		}

		//Close connection
		private bool CloseConnection()
		{
			try
			{
				connection.Close();
				return true;
			}
			catch (MySqlException ex)
			{
                Console.WriteLine(ex.Message);
				return false;
			}
		}

		//Insert statement
		public void CreateSaveData(string id, int rows)
		{
            var queries = new string[rows];
			for (var i=0;i<rows;i++)
			{
				queries[i] = "INSERT INTO save_data (user_id, derivation, solved) VALUES('"+ id +"', "+ i + ", false)";
			}
		
			//open connection
			if (this.OpenConnection() == true)
			{
				for (var i=0;i<rows;i++)
				{
					//create command and assign the query and connection from the constructor
					var cmd = new MySqlCommand(queries[i], connection);

					//Execute command
					cmd.ExecuteNonQuery();	
				}

				//close connection
				this.CloseConnection();
			}
		}
		
		//Checks to see if userID exists in save_data yet.
		public bool CheckUserExists(string id)
		{
			string query = "SELECT COUNT(*) FROM save_data WHERE user_id = '" + id+"'";
			var result = 0;

			if (this.OpenConnection()==true)
			{
				var cmd = new MySqlCommand(query, connection);
				result = Convert.ToInt32(cmd.ExecuteScalar());
				this.CloseConnection();
			}

			if (result>0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		//Returns a boolean array of solved problems of the specific user.
		public bool[] GetSolved(string id, int rows)
		{
			var queries = new string[rows];
			var returnBool = new bool[rows];
			for (var i=0;i<rows;i++)
			{
				queries[i] = "SELECT solved FROM save_data WHERE user_id = '"+id+"' AND derivation = "+i;
			}
			
			if (this.OpenConnection()==true)
			{
				for (var i=0;i<rows;i++)
				{
					var cmd = new MySqlCommand(queries[i], connection);
					returnBool[i] = Convert.ToBoolean(cmd.ExecuteScalar());
					
				}
				this.CloseConnection();
			}
			return returnBool;
		}

		public void MakeSolvedTrue(string id, int entry)
		{
			var query = "UPDATE save_data SET solved = true WHERE user_id = '"+id+"' AND derivation = "+entry;

			if (this.OpenConnection() == true)
			{
				
				//create command and assign the query and connection from the constructor
				var cmd = new MySqlCommand(query, connection);

				//Execute command
				cmd.ExecuteNonQuery();	

				//close connection
				this.CloseConnection();
			}
		}
		//Deletes a table
		/*
		public void DropTable()
		{
			string query = "DROP TABLE save_data;";

			if (this.OpenConnection()==true)
			{
				var cmd = new MySqlCommand(query,connection);
				cmd.ExecuteNonQuery();
				this.CloseConnection();
			}
		} */

		//Create table for save data.
		/*public void CreateTable()
		{
			string query = "CREATE TABLE save_data (user_id VARCHAR(100), derivation INT, solved BOOL, PRIMARY KEY (user_id));";

			if (this.OpenConnection()==true)
			{
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.ExecuteNonQuery();

				this.CloseConnection();
			}
		}*/
        
        }
    }

