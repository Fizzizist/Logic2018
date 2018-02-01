using System;
using System.IO;
namespace Logic2018
{
    public class Tutorial
    {
        public Tutorial(int part)
        {
            string input;
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
                            while (count < 40) 
                            {
                                line = sr.ReadLine();
                                if(count>=21)
                                {Console.WriteLine(line);
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
                        }
                    }
                    break;
            }
        }
    }
}
