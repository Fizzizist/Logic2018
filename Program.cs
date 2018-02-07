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

            //Testing
            /*Console.WriteLine("Premise:");
            var testingCommand = Console.ReadLine();
            problemConstructor = new ProblemConstructor();
            var testPremise = problemConstructor.MakeCustom(testingCommand);
            Console.WriteLine(testPremise.GetPremise());*/

            
            InitialLoop:
            var initialInt = 0;

            if (!saveCloud.CheckConnection())
            {
                goto WorkingWithConditionals;
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

            using (StreamReader sr = new StreamReader("textFiles/Intro.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null) 
                {
                    Console.WriteLine(line);
                }
            }

            MainMenu:
            while (stillRunning)
            {
                Console.WriteLine("Choose from the following menu options:");
                Console.WriteLine("1. Tutorial");
                Console.WriteLine("2. Problem set 1 (Working with Conditionals)");
                int mainChoice = 0;
                var mainInput = "";
                try
                {
                    mainInput = Console.ReadLine();
                    mainChoice = Convert.ToInt32(mainInput);
                }
                catch (Exception)
                {
                    if (mainInput == "exit") stillRunning = false;
                    else 
                    {
                        Console.WriteLine("That is not a valid choice. Try Again");
                        goto MainMenu;
                    }
                }
                switch (mainChoice)
                {
                    case 1:
                        var tutorial = new Tutorial(1);
                        goto MainMenu;
                    case 2:
                        goto WorkingWithConditionals;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        goto MainMenu;
                }
            }

            //Main loop.
            WorkingWithConditionals:
            while (stillRunning)
            {
                var show = new Show();
                Argument currentArgument;

                solved = saveCloud.GetSolved(userID, saveCloud.GetArgumentListLength());

                Console.WriteLine("Choose an argument to derive:");

                Loop1:
                var argumentDisplay = saveCloud.GetArgumentDisplay();
                var upTo = saveCloud.GetArgumentListLength();
				for (var i = 0; i < upTo; i++)
				{
                    if (solved[i] == true) Console.WriteLine(i+": "+argumentDisplay[i]+" (Solved)");
                    else Console.WriteLine(i+": "+argumentDisplay[i]);
				}
                Console.Write("Choice:");
                var choice = Console.ReadLine();
                try 
                {
                    var choiceInt = Convert.ToInt32(choice);
                    problemConstructor = new ProblemConstructor(choiceInt);
                    currentArgument = problemConstructor.argument;
                    Console.WriteLine(currentArgument.GetArgument());
                }
                catch(Exception e)
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
                            goto MainMenu;
                        case "save":
                            using (BinaryWriter writer = new BinaryWriter(File.Open("saves/save.dat",FileMode.Create)))
                            {
                                for (var i = 0; i < 5;i++)
                                {
                                    writer.Write(solved[i]);
                                }
                            }
                            goto WorkingWithConditionals;
                        
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
                            goto WorkingWithConditionals;
                        case "make-argument":
                            saveCloud.InsertArgument();
                            goto WorkingWithConditionals;
                        default:
							Console.WriteLine("That is not a valid choice. Try again.");
                            Console.WriteLine(e); //testing
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