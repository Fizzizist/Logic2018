using System;
using System.Collections.Generic;
using System.IO;

namespace Logic2018
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string command;
            string userID = null;
            var stillRunning = true;
            var saveCloud = new SaveCloud();
            var solved = new bool[saveCloud.GetArgumentListLength()];
            ProblemConstructor problemConstructor;
            var mainInventory = new List<Premise>();

            //Print introduction.
            using (StreamReader sr = new StreamReader("textFiles/Intro.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null) 
                {
                    Console.WriteLine(line);
                }
            }
            InitialLoop:
            var initialInt = 0;

            if (!saveCloud.CheckConnection())
            {
                goto MainLoop;
            }

            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("1. New User");
            Console.WriteLine("2. Existing User");
            Console.Write("Input:");

            try
            {
                initialInt = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("That's not a number. Try again.");
                goto InitialLoop;
            }

            switch (initialInt)
            {
                case 1:
                    saveCloud.CreateNewUser();
                    userID = saveCloud.GetUserID();
                    break;
                case 2:
                    Console.Write("User ID:");
                    userID = Console.ReadLine();
                    System.Console.Write("password: ");
                    string password = null;
                    while (true)
                    {
                        var key = System.Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                            break;
                        password += key.KeyChar;
                    }
                    if (saveCloud.UserAuthenticate(userID, password))
                    {
                        saveCloud.UserTableCheck(userID);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("UserID and password did not match what is in the system. Please try again.");
                        goto InitialLoop;
                    }
                default:
                    Console.WriteLine("Invalid response. Try again.");
                    goto InitialLoop;
            }

            //Main loop.
            MainLoop:
            while (stillRunning)
            {
                var show = new Show();
                Argument currentArgument;

                solved = saveCloud.GetSolved(userID, saveCloud.GetArgumentListLength());

                Console.WriteLine("Choose an argument to derive:");

                Loop1:
                //Print argument list.
				using (StreamReader sr = new StreamReader("textFiles/Arguments.list"))
				{
					string line;
                    var counter = 0;
					while ((line = sr.ReadLine()) != null)
					{
						Console.Write(line);
                        if (solved[counter] == true) Console.WriteLine("(Solved)");
                        else Console.WriteLine("");
                        counter++;
					}
				}
                var choice = Console.ReadLine();
                try 
                {
                    var choiceInt = Convert.ToInt32(choice);
                    problemConstructor = new ProblemConstructor(choiceInt);
                    currentArgument = problemConstructor.argument;
                    Console.WriteLine(currentArgument.GetArgument());
                }
                catch(Exception)
                {
                    switch (choice)
                    {
                        case "help":
							using (StreamReader sr = new StreamReader("textFiles/helpMain.txt"))
							{
								string line;
								while ((line = sr.ReadLine()) != null)
								{
									Console.WriteLine(line);
								}
							}
							goto Loop1;
                        case "exit":
                            stillRunning = false;
                            goto MainLoop;
                        case "save":
                            using (BinaryWriter writer = new BinaryWriter(File.Open("saves/save.dat",FileMode.Create)))
                            {
                                for (var i = 0; i < 5;i++)
                                {
                                    writer.Write(solved[i]);
                                }
                            }
                            goto MainLoop;
                        
                        case "load":
							if (File.Exists("saves/save.dat"))
							{
								using (BinaryReader reader = new BinaryReader(File.Open("saves/save.dat", FileMode.Open)))
								{
									for (var i = 0; i < 5; i++)
									{
										solved[i] = reader.ReadBoolean();
									}
								}
							}
                            goto MainLoop;

                        case "tutorial":
                            var tutorial = new Tutorial(1);
                            goto MainLoop;

                        default:
							Console.WriteLine("That is not a valid choice. Try again.");
							goto Loop1;
                    }
                }

                Console.Write("Command: ");
                string[] tokens = Console.ReadLine().Split(' ');
                command = tokens[0];

                //Main menu commands.
                switch (command)
                {
					case "help":
						using (StreamReader sr = new StreamReader("textFiles/helpMain.txt"))
						{
							string line;
							while ((line = sr.ReadLine()) != null)
							{
								Console.WriteLine(line);
							}
						}
						break;
                    case "Show":
                        if (!show.CheckTokenLength(tokens,2)) 
                        {
                            Console.WriteLine("Show statement must be followed by one command.");
                            break;
                        }
                        switch (tokens[1])
                        {
                            case "C":
                                if (show.ShowPremise(currentArgument, currentArgument.conclusion, mainInventory))
                                {
                                    Console.WriteLine("Solved!");
                                    solved[Convert.ToInt32(choice)] = true;
                                    saveCloud.MakeSolvedTrue(userID, Convert.ToInt32(choice));
                                    break;
                                }
                                break;
                            default:
                                Premise custom = problemConstructor.MakeCustom(tokens[1]);
                                if (custom == null)
                                {
                                    Console.WriteLine("Bad Premise. Try again.");
                                    break;
                                }
                                if (show.ShowPremise(currentArgument, custom, mainInventory))
                                {
                                    if (custom._Equals(currentArgument.conclusion))
                                    {
										Console.WriteLine("Solved!");
										solved[Convert.ToInt32(choice)] = true;
                                        saveCloud.MakeSolvedTrue(userID, Convert.ToInt32(choice));
										break;
                                    }
                                    else
                                    {
                                        mainInventory.Add(custom);
                                        Console.WriteLine(custom.GetPremise()+" "+currentArgument.conclusion.GetPremise());
                                        break;
                                    }
                                }
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Unrecognized input. type 'help' for more information.");
                        break;
                }
            }
        }
    }
}