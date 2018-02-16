using System;
using System.IO;
using System.Collections.Generic;
namespace Logic2018
{
    public class Tutorial
    {
        private Reader writer = new Reader();
        public Tutorial(int part, string userID)
        {
            switch (part)
            {
                case 1:
                    var DDSHow = new Show(userID);
                    writer.AddTenBlankLines();
                    writer.Read(0,18,"textFiles/tutorial.txt");
                    var problem = new ProblemConstructor(1, 0);
                    var argument1 = problem.argument;
                    Console.WriteLine("*Note that PR1, PR2, etc. indicate available premises.");
                    Console.WriteLine(argument1.GetArgument());
                    Loop1:
                    Console.Write("Command:");
                    var tokens1 = Console.ReadLine().Split(' ');
                    if (tokens1.Length!=2||(tokens1[0]!="Show"&&(tokens1[1]!="P"||tokens1[1]!="C"))) 
                    {
                        if (tokens1[0] == "exit") break;
                        else
                        {
							Console.WriteLine("Invalid input, please follow the tutorial or type 'exit'");
							goto Loop1;
                        }
                    }
                    else
                    {
                        writer.Read(21,42,"textFiles/tutorial.txt");
                        Loop2:
                        Console.Write("Command:");
                        var tokens2 = Console.ReadLine().Split(' ');
                        if (tokens2.Length!=3||(tokens2[0]!="MP"&&(tokens2[1]!="PR1"||tokens2[1]!="PR2"))) 
                        {
                            if (tokens2[0] == "exit") break;
                            else
                            {
							    Console.WriteLine("Invalid input, please follow the tutorial or type 'exit'");
							    goto Loop2;
                            }
                        }
                        else
                        {
                            writer.Read(43,55,"textFiles/tutorial.txt");
                            Loop3:
                            Console.Write("Command:");
                            var tokens3 = Console.ReadLine().Split(' ');
                            if (tokens3.Length!=2||(tokens3[0]!="DD"&&(tokens3[1]!="0"))) 
                            {
                                if (tokens3[0] == "exit") break;
                                else
                                {
							        Console.WriteLine("Invalid input, please follow the tutorial or type 'exit'");
							        goto Loop3;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Solved!");
                                writer.Read(57,66,"textFiles/tutorial.txt");
                            }
                        }
                    }
                    break;
                    //End of Tutorial 1
                case 2:
                    Tut2Loop1:
                    writer.AddTenBlankLines();
                    writer.Read(67,76,"textFiles/tutorial.txt");
                    var problem2 = new ProblemConstructor(1, 7);
                    Console.WriteLine(problem2.argument.GetArgument());
                    Console.Write("Command:");
                    var tokens4 = Console.ReadLine().Split(' ');
                    if (tokens4.Length!=2||(tokens4[0]!="Show"&&(tokens4[1]!="~R"||tokens4[1]!="C"))) 
                    {
                        if (tokens4[0] == "exit") break;
                        else
                        {
							Console.WriteLine("Invalid input, please follow the tutorial or type 'exit'");
							goto Tut2Loop1;
                        }
                    }
                    else
                    {
                        Tut2Loop2:
                        writer.Read(76,94,"textFiles/tutorial.txt");
                        Console.WriteLine(problem2.argument.GetArgument());
                        Console.WriteLine("Show ~R");
                        Console.Write("Command:");
                        var tokens5 = Console.ReadLine().Split(' ');
                        if (tokens5.Length!=2||tokens5[0]!="ASS"||tokens5[1]!="ID") 
                        {
                            if (tokens5[0] == "exit") break;
                            else
                            {
							    Console.WriteLine("Invalid input, please follow the tutorial or type 'exit'");
							    goto Tut2Loop2;
                            }
                        }
                        else
                        {
                            writer.Read(94,102,"textFiles/tutorial.txt");
                            var IDShow = new Show(userID);
                            IDShow.SetAssumeCounter(1);
                            var IDInv = new List<Premise>();
                            IDInv.Add(problem2.MakeCustom("R"));
                            if (IDShow.ShowPremise(problem2.argument,problem2.argument.conclusion,IDInv))
                            {
                                writer.Read(104,111,"textFiles/tutorial.txt");
                                break;
                            }
                            else break;
                        }
                    }
                    //End of Tutorial 2
                case 3:
                    Tut3Loop1:
                    var problem3 = new ProblemConstructor(1, 1);
                    writer.AddTenBlankLines();
                    writer.Read(112,133,"textFiles/tutorial.txt");
                    Console.WriteLine(problem3.argument.GetArgument());
                    Console.Write("Command:");
                    var tokens6 = Console.ReadLine().Split(' ');
                    if (tokens6.Length!=2||(tokens6[0]!="Show"&&(tokens6[1]!="P->Q"||tokens6[1]!="C"))) 
                    {
                        if (tokens6[0] == "exit") break;
                        else
                        {
							Console.WriteLine("Invalid input, please follow the tutorial or type 'exit'");
							goto Tut3Loop1;
                        }
                    }
                    else
                    {
                        Tut3Loop2:
                        writer.AddTenBlankLines();
                        writer.Read(134,136,"textFiles/tutorial.txt");
                        Console.WriteLine(problem3.argument.GetArgument());
                        Console.WriteLine("Show P->Q");
                        Console.Write("Command:");
                        var tokens5 = Console.ReadLine().Split(' ');
                        if (tokens5.Length!=2||tokens5[0]!="ASS"||tokens5[1]!="CD") 
                        {
                            if (tokens5[0] == "exit") break;
                            else
                            {
							    Console.WriteLine("Invalid input, please follow the tutorial or type 'exit'");
							    goto Tut3Loop2;
                            }
                        }
                        else
                        {
                            writer.AddTenBlankLines();
                            writer.Read(137,141,"textFiles/tutorial.txt");
                            var IDShow = new Show(userID);
                            IDShow.SetAssumeCounter(1);
                            var IDInv = new List<Premise>();
                            IDInv.Add(problem3.MakeCustom("P"));
                            if (IDShow.ShowPremise(problem3.argument,problem3.argument.conclusion,IDInv))
                            {
                                writer.Read(141,166,"textFiles/tutorial.txt");
                                var problem3_2 = new ProblemConstructor(1, 13);
                                IDInv.Clear();
                                IDShow.ClearInventory();
                                IDInv.Add(problem3_2.MakeCustom("P"));
                                if (IDShow.ShowPremise(problem3_2.argument,problem3_2.argument.conclusion,IDInv))
                                {
                                    writer.Read(166,174,"textFiles/tutorial.txt");
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    //End of Tutorial 3
            }
        }
    }
}
