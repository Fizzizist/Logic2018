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
            var stillRunning = true;
            var solved = new bool[6];
            var saveCloud = new SaveCloud();
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

            Console.WriteLine("Please enter a user ID. If you do not have one yet it will automatically be created for you.");
            Console.Write("User ID:");
            var userID = Console.ReadLine();

            if (!saveCloud.CheckUserExists(userID)) saveCloud.CreateSaveData(userID, 6);

            //Main loop.
            MainLoop:
            while (stillRunning)
            {
                var show = new Show();
                Argument currentArgument;

                solved = saveCloud.GetSolved(userID, 6);

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