using System;
using System.Collections.Generic;

namespace Logic2018
{
    public class ProblemSet
    {
        private bool stillRunning = true;
        private SaveCloud saveCloud = new SaveCloud();
        private ProblemConstructor problemConstructor = new ProblemConstructor();
        private Reader writer = new Reader();
        private Argument currentArgument;
        private List<Premise> mainInventory = new List<Premise>();
        private Show show = new Show();
        public ProblemSet(int problemSet, string userID)
        {
            
            WorkingWithConditionals:
            while (stillRunning)
            {
                

                var solved = saveCloud.GetSolved(problemSet, userID, saveCloud.GetArgumentListLength(problemSet));

                Console.WriteLine("Choose an argument to derive:");

                Loop1:
                var argumentDisplay = saveCloud.GetArgumentDisplay(problemSet);
                var upTo = argumentDisplay.Length-1;
                for (var i = 0; i <= upTo; i++)
                {
                    if (solved[i] == true) Console.WriteLine(i+": "+argumentDisplay[i]+" (Solved)");
                    else Console.WriteLine(i+": "+argumentDisplay[i]);
                }
                Console.Write("Choice:");
                var choice = Console.ReadLine();
                var choiceInt = 0;
                try 
                {
                    choiceInt = Convert.ToInt32(choice);
                    problemConstructor = new ProblemConstructor(choiceInt);
                    currentArgument = problemConstructor.argument;
                    Console.WriteLine(currentArgument.GetArgument());
                }
                catch(Exception e)
                {
                    switch (choice)
                    {
                        case "help":
                            writer.ReadWholeFile("textFiles/helpShow.txt");
                            goto Loop1;
                        case "exit":
                            stillRunning = false;
                            goto Done;
                        
                        case "make-argument":
                            Console.WriteLine("Into which problem Set?");
                            Console.Write("Choice:");
                            saveCloud.InsertArgument(Convert.ToInt32(Console.ReadLine()));
                            goto WorkingWithConditionals;
                        default:
                            Console.WriteLine("That is not a valid choice. Try again.");
                            Console.WriteLine(e); //testing
                            goto Loop1;
                    }
                }

                problemConstructor = new ProblemConstructor(choiceInt);
                currentArgument = problemConstructor.argument;
                Console.WriteLine(currentArgument.GetArgument());
                Console.Write("Command: ");
                string[] tokens = Console.ReadLine().Split(' ');
                var command = tokens[0];

                
                switch (command)
                {
                    case "help":
                        writer.ReadWholeFile("textFiles/helpShow.txt");
                        break;
                    case "Show":
                        Show:
                        if (!show.CheckTokenLength(tokens,2)) 
                        {
                            Console.WriteLine("Show statement must be followed by one command.");
                            break;
                        }
                        switch (tokens[1])
                        {
                            case "C": case "c":
                                if (show.ShowPremise(currentArgument, currentArgument.conclusion, mainInventory))
                                {
                                    Console.WriteLine("Solved!");
                                    solved[Convert.ToInt32(choice)] = true;
                                    saveCloud.MakeSolvedTrue(userID, Convert.ToInt32(choice));
                                    break;
                                }
                                else
                                {
                                    stillRunning = false;
                                    break;
                                }
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
                        if (command.Equals("show", StringComparison.CurrentCultureIgnoreCase)) goto Show;
                        Console.WriteLine("Unrecognized input. type 'help' for more information.");
                        break;
                }
            }
            Done:;            
        }
    }
}