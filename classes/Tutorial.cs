using System;
using System.IO;
using System.Collections.Generic;
namespace Logic2018
{
    public class Tutorial
    {
        public Tutorial(int part)
        {
            switch (part)
            {
                case 1:
                    using (StreamReader sr = new StreamReader("textFiles/tutorial.txt"))
                    {
                        string line;
                        var count = 0;
                        while (count < 18) 
                        {
                            line = sr.ReadLine();
                            Console.WriteLine(line);
                            count++;
                        }
                    }
                    var problem = new ProblemConstructor(0);
                    var argument1 = problem.argument;
                    Console.WriteLine(argument1.GetArgument());
                    Loop1:
                    Console.Write("Command:");
                    var tokens1 = Console.ReadLine().Split(' ');
                    if ((tokens1[0]!="Show"&&(tokens1[1]!="P"||tokens1[1]!="C"))||tokens1.Length!=2) 
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
                        using (StreamReader sr = new StreamReader("textFiles/tutorial.txt"))
                        {
                            string line;
                            var count = 0;
                            while (count < 42) 
                            {
                                line = sr.ReadLine();
                                if(count>=21)
                                {
                                    Console.WriteLine(line);
                                }
                                count++;
                            }
                        }
                        Loop2:
                        Console.Write("Command:");
                        var tokens2 = Console.ReadLine().Split(' ');
                        if ((tokens2[0]!="MP"&&(tokens2[1]!="PR1"||tokens2[1]!="PR2"))||tokens2.Length!=3) 
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
                            using (StreamReader sr = new StreamReader("textFiles/tutorial.txt"))
                            {
                                string line;
                                var count = 0;
                                while (count < 55) 
                                {
                                    line = sr.ReadLine();
                                    if(count>=43)
                                    {
                                        Console.WriteLine(line);
                                    }
                                    count++;
                                }
                            }
                            Loop3:
                            Console.Write("Command:");
                            var tokens3 = Console.ReadLine().Split(' ');
                            if ((tokens3[0]!="DD"&&(tokens3[1]!="0"))||tokens3.Length!=2) 
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
                                using (StreamReader sr = new StreamReader("textFiles/tutorial.txt"))
                                {
                                    string line;
                                    var count = 0;
                                    while (count < 66) 
                                    {
                                        line = sr.ReadLine();
                                        if(count>=57)
                                        {
                                            Console.WriteLine(line);
                                        }
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                    break;
                    //End of Tutorial 1
                case 2:
                    Tut2Loop1:
                    using (StreamReader sr = new StreamReader("textFiles/tutorial.txt"))
                    {
                        string line;
                        var count = 0;
                        while (count < 76) 
                        {
                            line = sr.ReadLine();
                            if(count>=67)
                            {
                                Console.WriteLine(line);
                            }
                            count++;
                        }
                    }
                    var problem2 = new ProblemConstructor(7);
                    Console.WriteLine(problem2.argument.GetArgument());
                    Console.Write("Command:");
                    var tokens4 = Console.ReadLine().Split(' ');
                    if ((tokens4[0]!="Show"&&(tokens4[1]!="~R"||tokens4[1]!="C"))||tokens4.Length!=2) 
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
                        using (StreamReader sr = new StreamReader("textFiles/tutorial.txt"))
                        {
                            string line;
                            var count = 0;
                            while (count < 94) 
                            {
                                line = sr.ReadLine();
                                if(count>=76)
                                {
                                    Console.WriteLine(line);
                                }
                                count++;
                            }
                        }
                        Console.WriteLine(problem2.argument.GetArgument());
                        Console.WriteLine("Show ~R");
                        Console.Write("Command:");
                        var tokens5 = Console.ReadLine().Split(' ');
                        if (tokens5[0]!="ASS"||tokens5[1]!="ID"||tokens5.Length!=2) 
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
                            using (StreamReader sr = new StreamReader("textFiles/tutorial.txt"))
                            {
                                string line;
                                var count = 0;
                                while (count < 102) 
                                {
                                    line = sr.ReadLine();
                                    if(count>=94)
                                    {
                                        Console.WriteLine(line);
                                    }
                                    count++;
                                }
                            }
                            var IDShow = new Show();
                            IDShow.SetAssumeCounter(1);
                            var IDInv = new List<Premise>();
                            IDInv.Add(problem2.MakeCustom("R"));
                            if (IDShow.ShowPremise(problem2.argument,problem2.argument.conclusion,IDInv))
                            {
                                using (StreamReader sr = new StreamReader("textFiles/tutorial.txt"))
                                {
                                    string line;
                                    var count = 0;
                                    while (count < 111) 
                                    {
                                        line = sr.ReadLine();
                                        if(count>=104)
                                        {
                                            Console.WriteLine(line);
                                        }
                                        count++;
                                    }
                                }
                                break;
                            }
                            else break;
                        }
                    }
                    //End of Tutorial 2
                
            }
        }
    }
}
