﻿/*
A Class for communicating with the SQL server.
Author: Peter Vlasveld
*/

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
			server = "159.89.126.100";
			database = "Logic2018";
			uid = "logicUser";
			password = "thisislogic2018";
			string connectionString;
			connectionString = "SERVER=" + server + ";" + "DATABASE=" +
			database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

			connection = new MySqlConnection(connectionString);
		}

		//open connection to database
		public bool OpenConnection()
		{
            try
            {
                connection.Open();
                return true;
            }
            catch(MySqlException)
            {
                Console.WriteLine("There is currently a problem connecting to the server.");
                return false;
            }
		}

		//Close connection
		public bool CloseConnection()
		{
			try
			{
				connection.Close();
				//Console.WriteLine("closed."); //testing.
				return true;
			}
			catch (MySqlException ex)
			{
                Console.WriteLine(ex.Message);
				return false;
			}
		}

		//Insert statement
		public void CreateSaveData(int table, string id, int rows)
		{
            var queries = new string[rows];
			for (var i=0;i<rows;i++)
			{
				queries[i] = "INSERT INTO savedata_"+id+"_"+table+" (derivation, solved) VALUES("+ i + ", false)";
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
		public bool[] GetSolved(int table, string id, int rows)
		{
			var queries = new string[rows];
			var returnBool = new bool[rows];
			for (var i=0;i<rows;i++)
			{
				queries[i] = "SELECT solved FROM savedata_"+id+"_"+table+" WHERE derivation = "+i;
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
		
		//updates solved column for specific user
		public void MakeSolvedTrue(int table, string id, int entry)
		{
			var query = "UPDATE savedata_"+id+"_"+table+" SET solved = true WHERE derivation = "+entry;

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

		//checks if a specific problem was solved
		public bool CheckRuleSolved(int table, int derivation, string id)
		{
			var query = "SELECT solved FROM savedata_"+id+"_"+table+" WHERE derivation = "+derivation+";";

			
				//create command and assign the query and connection from the constructor
			var cmd = new MySqlCommand(query, connection);

				//Execute command
			var result = Convert.ToBoolean(cmd.ExecuteScalar());	

			return result;
		}
		
		//gets password hash
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
		
		//creates a new user and adds to database
		public void CreateNewUser()
		{
			Retry:
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
				Console.Write('*');
            }

			System.Console.Write("Retype new password: ");
            string password2 = null;
        	while (true)
            {
        		var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password2 += key.KeyChar;
				Console.Write('*');
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
			var queries = new string[Constants.NumberOfProblemSets];
			for (var i=0;i<Constants.NumberOfProblemSets;i++)
			{
				queries[i] = "CREATE TABLE savedata_"+uid+"_"+(i+1)+" (derivation INT, solved BOOL);";
			}
			try
			{
				if (this.OpenConnection() == true)
				{
				
					//create command and assign the query and connection from the constructor
					var cmd1 = new MySqlCommand(query1, connection);
					var cmds = new MySqlCommand[Constants.NumberOfProblemSets];

					cmd1.ExecuteNonQuery();
					for (var i=0;i<Constants.NumberOfProblemSets;i++)
					{
						cmds[i] = new MySqlCommand(queries[i],connection);
						cmds[i].ExecuteNonQuery();
					}
					//close connection
					this.CloseConnection();
				}
			}
			catch (MySqlException)
			{
				Console.WriteLine(' ');
				Console.WriteLine("It seems that that user ID is already taken. Try something else.");
				goto Retry;
			}
			for (var i = 1;i<=Constants.NumberOfProblemSets;i++)
			{
				this.CreateSaveData(i, uid, this.GetArgumentListLength(i));	
			}
			
			
		}
		
		//handles user authentication
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

		//checks to make sure user table is up to date
		public void UserTableCheck(string id)
		{
			MainLoop:
			var counter = 0;
			try{
				for (var i=1;i<=Constants.NumberOfProblemSets;i++)
				{
					counter = i;
					var expectedInt = this.GetArgumentListLength(i);
					var query1 = "SELECT COUNT(*) FROM savedata_"+id+"_"+i+";";
				
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
						var query2 = "INSERT INTO savedata_"+id+"_"+i+" (derivation,solved) VALUES("+Convert.ToString(result)+",false);";
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
			}
			catch (MySqlException)
			{
				this.CloseConnection();
				
				var query = "CREATE TABLE savedata_"+id+"_"+counter+" LIKE savedata_"+id+"_1;";
				if (this.OpenConnection() == true)
				{	
					//create command and assign the query and connection from the constructor
					var cmd = new MySqlCommand(query, connection);

					//Execute command
					cmd.ExecuteNonQuery();	

					//close connection
					this.CloseConnection();
				}
				goto MainLoop;
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

		//gets the length of the argument table
		public int GetArgumentListLength(int table)
		{
			var query = "SELECT COUNT(*) FROM argument_constructor_"+table+";";
			var amount = 0;
			if (this.OpenConnection() == true)
			{	
				var cmd = new MySqlCommand(query, connection);

				amount = Convert.ToInt32(cmd.ExecuteScalar());	

				this.CloseConnection();
			}
			return amount;
		}

		//sends argument data out for display
		public string[] GetArgumentDisplay(int table)
		{
			
			var result = new string[this.GetArgumentListLength(table)];
			var query = "SELECT * FROM argument_display_"+table+";";
			if (this.OpenConnection() == true)
			{	
				 
				var cmd = new MySqlCommand(query, connection);
				MySqlDataReader rdr = cmd.ExecuteReader();
				var counter = 0;
				while(rdr.Read())
				{
					var derivation = (string)rdr["derivation"];
					result[counter] = derivation;
					counter++;
				}		
				this.CloseConnection();
			}
			return result;
		}

		//gets a specific argument
		public string GetArgumentConstructorRow(int table, int num)
		{
			var result = "";
			var query = "SELECT derivation FROM argument_constructor_"+table+" where number = "+num+";";
			if (this.OpenConnection() == true)
			{	
				var cmd = new MySqlCommand(query, connection);

				result = Convert.ToString(cmd.ExecuteScalar());	

				this.CloseConnection();
			}
			return result;
		}
		//Testing
		public void InsertArgument(int table)
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
			string query1 = "INSERT INTO argument_display_"+table+" VALUES("+GetArgumentListLength(table)+", N'"+newArgument.GetArgumentDisplay()+"');";
			string query2 = "INSERT INTO argument_constructor_"+table+" VALUES("+GetArgumentListLength(table)+", '"+input+"');";
			
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

		//For use when updating display. E.G. when problem is changed, or when 
		//the way that the arguments are displayed is changed. Use this to make a 
		//whole new argument_display table from an argument_constructor table.
		public void FillArgumentDisplayTable(int num)
		{
			var numberOfQueries = this.GetArgumentListLength(num);
			var arguments = new string[numberOfQueries];
			if (this.OpenConnection()==true)
			{
				for (var i=0; i<numberOfQueries;i++)
				{
					var query="SELECT derivation FROM argument_constructor_" + num + " where number = " + i + ";";
					var cmd = new MySqlCommand(query,connection);
					arguments[i] = Convert.ToString(cmd.ExecuteScalar());
				}
				this.CloseConnection();
			}
			var argumentsForDisplay = new string[numberOfQueries];
			var problemConstructor = new ProblemConstructor();
			for (var i=0;i<numberOfQueries;i++)
			{
				Console.WriteLine(arguments[i]);
				var tempArgument = problemConstructor.MakeCustomArgument(arguments[i]);
				argumentsForDisplay[i] = tempArgument.GetArgumentDisplay();
			}
			if (this.OpenConnection()==true)
			{
				for (var i=0; i<numberOfQueries;i++)
				{
					var query="INSERT INTO argument_display_"+num+" VALUES("+i+", N'"+argumentsForDisplay[i]+"');";
					var cmd = new MySqlCommand(query,connection);
					cmd.ExecuteNonQuery();
				}
				this.CloseConnection();
			}
		}
        
        }
    }

