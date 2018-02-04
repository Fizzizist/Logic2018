using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;

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
            catch(MySqlException)
            {
                Console.WriteLine("There is currently a problem connecting to the server.");
				Console.WriteLine("Type 'save' to save locally");
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
				queries[i] = "INSERT INTO savedata_"+id+"(derivation, solved) VALUES("+ i + ", false)";
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
				queries[i] = "SELECT solved FROM savedata_"+id+" WHERE derivation = "+i;
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
			var query = "UPDATE savedata_"+id+" SET solved = true WHERE derivation = "+entry;

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

		public string GetHash(string input)
		{
			var query = "SELECT sha1('" + input + "');";

			string result;
			if (this.OpenConnection() == true)
			{
				
				//create command and assign the query and connection from the constructor
				var cmd = new MySqlCommand(query, connection);

				//Execute command
				result = Convert.ToString(cmd.ExecuteScalar());	

				//close connection
				this.CloseConnection();
			}
			else 
			{
				result = "";
			}
			return result;
		}

		public void CreateNewUser()
		{
			Console.Write("type new UserID:");
			uid = Console.ReadLine();
			string password = null;

			PasswordLoop:
			System.Console.Write("type new password: ");
            string password1 = null;
        	while (true)
            {
        		var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password1 += key.KeyChar;
            }

			System.Console.Write("Retype new password: ");
            string password2 = null;
        	while (true)
            {
        		var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password2 += key.KeyChar;
            }

			if (password1==password2)
			{
				password = password1;
			}
			else
			{
				Console.WriteLine("Those passwords don't match. Try again.");
				goto PasswordLoop;
			}

			var query1 = "INSERT INTO user (user_id, password) VALUES ('"+uid+"', sha1('"+password+"'));";
			var query2 = "CREATE TABLE savedata_"+uid+"(derivation INT, solved BOOL);";

			if (this.OpenConnection() == true)
			{
				
				//create command and assign the query and connection from the constructor
				var cmd1 = new MySqlCommand(query1, connection);
				var cmd2 = new MySqlCommand(query2, connection);

				//Execute command
				cmd1.ExecuteNonQuery();
				cmd2.ExecuteNonQuery();	

				//close connection
				this.CloseConnection();
			}

			this.CreateSaveData(uid, this.GetArgumentListLength());
			
		}

		public bool UserAuthenticate(string user, string pass)
		{
			var hashpass = this.GetHash(pass);

			var query = "SELECT password FROM user WHERE user_id = '"+user+"';";
			string result = null;
			if (this.OpenConnection() == true)
			{
				//create command and assign the query and connection from the constructor
				var cmd = new MySqlCommand(query, connection);

				//Execute command
				result = Convert.ToString(cmd.ExecuteScalar());	

				//close connection
				this.CloseConnection();
			}

			if (result==hashpass)
				return true;
			else
				return false;
		}

		public void UserTableCheck(string id)
		{
			var expectedInt = this.GetArgumentListLength();
			var query1 = "SELECT COUNT(*) FROM savedata_"+id+";";
			
			var result = 0;
			QueryLoop:
			if (this.OpenConnection() == true)
			{
				//create command and assign the query and connection from the constructor
				var cmd = new MySqlCommand(query1, connection);

				//Execute command
				result = Convert.ToInt32(cmd.ExecuteScalar());	

				//close connection
				this.CloseConnection();
			}
			if (result<expectedInt)
			{
				var query2 = "INSERT INTO savedata_"+id+" (derivation,solved) VALUES("+Convert.ToString(result)+",false);";
				if (this.OpenConnection() == true)
				{	
					//create command and assign the query and connection from the constructor
					var cmd = new MySqlCommand(query2, connection);

					//Execute command
					cmd.ExecuteNonQuery();	

					//close connection
					this.CloseConnection();
				}
				goto QueryLoop;
			}
		}

		public string GetUserID()
		{
			return uid;
		}

		public bool CheckConnection()
		{
			if (this.OpenConnection()==true)
			{
				this.CloseConnection();
				return true;
			}
			return false;
		}

		public int GetArgumentListLength()
		{
			var amount = 0;
			var query = "SELECT COUNT(*) FROM argument_display;";
			if (this.OpenConnection() == true)
			{	
				var cmd = new MySqlCommand(query, connection);

				amount = Convert.ToInt32(cmd.ExecuteScalar());	

				this.CloseConnection();
			}
			return amount;
		}

		public string[] GetArgumentDisplay()
		{
			var result = new string[GetArgumentListLength()];
			var queries = new string[result.Length];
			for (var i=0;i<queries.Length;i++)
			{
				queries[i] = "SELECT derivation FROM argument_display WHERE number = "+i+";";
			}
			if (this.OpenConnection() == true)
			{	
				for (var i=0;i<queries.Length;i++)
				{
					var cmd = new MySqlCommand(queries[i], connection);
					result[i] = Convert.ToString(cmd.ExecuteScalar());	
				}	
				this.CloseConnection();
			}
			return result;
		}

		public string GetArgumentConstructorRow(int num)
		{
			var result = "";
			var query = "SELECT derivation FROM argument_constructor where number = "+num+";";
			if (this.OpenConnection() == true)
			{	
				var cmd = new MySqlCommand(query, connection);

				result = Convert.ToString(cmd.ExecuteScalar());	

				this.CloseConnection();
			}
			return result;
		}
		//Testing
		public void InsertArgument()
		{
			Loop:
			Console.Write("Argument:");
			var input = Console.ReadLine();
			if (input=="exit") goto End;
			var problemConstructor = new ProblemConstructor();
			var newArgument = problemConstructor.MakeCustomArgument(input);
			if (newArgument==null)
			{
				Console.WriteLine("Bad Syntax, try again.");
				goto Loop;
			}
			string query1 = "INSERT INTO argument_display VALUES("+GetArgumentListLength()+", N'"+newArgument.GetArgumentDisplay()+"');";
			string query2 = "INSERT INTO argument_constructor VALUES("+GetArgumentListLength()+", '"+input+"');";
			
			if (this.OpenConnection()==true)
			{
				var cmd1 = new MySqlCommand(query1,connection);
				var cmd2 = new MySqlCommand(query2,connection);
				cmd1.ExecuteNonQuery();
				cmd2.ExecuteNonQuery();
				this.CloseConnection();
			}
			
			End:;
		} 

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

