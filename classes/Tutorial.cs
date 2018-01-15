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
                    using (StreamReader sr = new StreamReader("Intro.txt"))
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
                    var problem = new ProblemConstructor();
                    Argument argument1 = problem.MakeCustomArgument("P->Q P C: Q");
                    Console.WriteLine(argument1.GetArgument());
                    Console.WriteLine("Every statement must start with a 'Show' statement, where we state what conclusion we are attempting to derive.In this case it is P.So to start the derivation, type 'Show P' into the console and hit enter.");
                    Loop1:
                    var tokens = Console.ReadLine().Split(' ');
                    if ((tokens[0]!="Show"&&tokens[1]!="P")||tokens.Length!=2) 
                    {
                        if (tokens[0] == "exit") break;
                        else
                        {
							Console.WriteLine("Invalid input, please follow the tutorial or type 'exit'");
							goto Loop1;
                        }
                    }
                    break;
            }
        }
    }
}
