using System;
using System.Collections.Generic;
using System.IO;
namespace Logic2018
{
    public class Show
    {
        private List<Premise> inventory = new List<Premise>();
        private List<Premise> argumentPremises = new List<Premise>();
        private Rules rules = new Rules();
		private int assumeCounter;
		

        public Show()
        {
			assumeCounter = 0;
        }

        //Main loop for derivation of the argument's conclusion.
        public bool ShowPremise(Argument argument, Premise toShow, List<Premise> mainInv)
        {
            for (var j = 0; j < mainInv.Count;j++)
            {
                inventory.Add(mainInv[j]);
            }
            for (var i = 0; i < argument.premises.Count;i++)
            {
                argumentPremises.Add(argument.premises[i]);
            }

            Console.WriteLine(argument.GetArgument());
			this.ListSheet(toShow);

            var notSolved = true;
            string command;

            while(notSolved)
            {
                Console.Write("Command: ");
                string[] tokens = Console.ReadLine().Split(' ');
                command = tokens[0];
				switch (command)
				{
					case "help":
						using (StreamReader sr = new StreamReader("helpShow.txt"))
						{
							string line;
							while ((line = sr.ReadLine()) != null)
							{
								Console.WriteLine(line);
							}
						}
						break;

                    case "exit":
                        return false;
					//Modus Ponens
					case "MP":
                        var inMP = new Premise[tokens.Length - 1];

                        if (!this.CheckTokenLength(tokens, 3)) break;

                        inMP = this.SetInputPremises(tokens, argument);

                        if (inMP == null) break;

						if (!rules.MPCheck(inMP[0], inMP[1]))
						{
							Console.WriteLine("Cannot perform this rule on these premises");
							break;
						}

						var resultPremise = rules.ModusPonens(inMP[0],inMP[1]);
						if (resultPremise==null) break;
						inventory.Add(resultPremise);
                        Console.WriteLine(argument.GetArgument());
                        this.ListSheet(toShow);
						break;

					//Modus Tolens
					case "MT":
						var inMT = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 3)) break;
                        inMT = this.SetInputPremises(tokens, argument);
                        if (inMT == null) break;

						if (!rules.MTCheck(inMT[0], inMT[1]))
						{
							Console.WriteLine("Cannot perform this rule on these premises");
							break;
						}

						var MTresultPremise = rules.ModusTolens(inMT[0],inMT[1]);
						if (MTresultPremise==null) break;
						inventory.Add(MTresultPremise);
                        Console.WriteLine(argument.GetArgument());
                        this.ListSheet(toShow);
						break;

                        //Double Negation generic
                    case "DN":
                        var inDN = new Premise[tokens.Length - 1];
                        if (!this.CheckTokenLength(tokens, 2)) break;
                        inDN = this.SetInputPremises(tokens, argument);

                        if (inDN == null) break;

                        if (rules.DNECheck(inDN[0]))
                        {
                            inventory.Add(rules.DNE(inDN[0]));
                            Console.WriteLine(argument.GetArgument());
                            this.ListSheet(toShow);
                            break;
                        }
                        else
                        {
                            inventory.Add(rules.DNI(inDN[0]));
                            Console.WriteLine(argument.GetArgument());
                            this.ListSheet(toShow);
                            break;
                        }

                        //DNE specific
                    case "DNE":
						var inDNE = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 2)) break;
						inDNE = this.SetInputPremises(tokens, argument);
                        if (inDNE == null) break;

						if (rules.DNECheck(inDNE[0]))
						{
							inventory.Add(rules.DNE(inDNE[0]));
                            Console.WriteLine(argument.GetArgument());
                            this.ListSheet(toShow);
							break;
						}
						else
						{
                            Console.WriteLine("DNE cannot be performed on that premise. Type 'help' for rule info.");
							break;
						}

                        //DNI specific
                    case "DNI":
						var inDNI = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 2)) break;
						inDNI = this.SetInputPremises(tokens, argument);
                        if (inDNI == null) break;
						inventory.Add(rules.DNI(inDNI[0]));
                        Console.WriteLine(argument.GetArgument());
                        this.ListSheet(toShow);
						break;

					case "Show":
						if (!this.CheckTokenLength(tokens, 2)) break;
						var problemConstructor = new ProblemConstructor();
						var newPremise = problemConstructor.MakeCustom(tokens[1]);
						if (newPremise==null)
						{
							Console.WriteLine("Invalid premise");
							break;
						}
						var newShow = new Show();
						if (newShow.ShowPremise(argument, newPremise, inventory))
						{
							inventory.Add(newPremise);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						}
						break;


					//Assume CD or ID
					case "ASS":
						if (!this.CheckTokenLength(tokens, 2)) break;
						if (assumeCounter>0) 
						{
							Console.WriteLine("Only 1 assume statement can be made per Show statement.");
							break;
						}
						if (tokens[1] == "ID")
						{
							inventory.Add(new Premise(toShow));
							Console.WriteLine(argument.GetArgument());
                            this.ListSheet(toShow);
							assumeCounter++;
							break;
						}
						else if (tokens[1] == "CD")
						{
							if (toShow.type == 1)
							{
								inventory.Add(toShow.anti);
								Console.WriteLine(argument.GetArgument());
                                this.ListSheet(toShow);
								assumeCounter++;
								break;
							}
							else
							{
								Console.WriteLine("Argument's conlusion must be conditional in order to perform a conditional derivation");
								break;
							}
						}
						else
						{
							Console.WriteLine("ASS must be followed by CD or ID only.");
							break;
						}

					//closing the ID
					case "ID":
						//decides whether input is from lines or argument
                        var inID = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 3)) break;
                        inID = this.SetInputPremises(tokens, argument);
                        if (inID == null) break;

						if (rules.IDCheck(inID[0], inID[1]))
						{
                            return true;
						}
						else
						{
							Console.WriteLine("Nope! Try again.");
							break;
						}

					//direct derivation
					case "DD":
						if (!this.CheckTokenLength(tokens, 2)) break;
						try
						{
                            var temp = Convert.ToInt32(tokens[1]);
                            if (inventory[temp]._Equals(toShow))
							{
                                return true;
							}
							else
							{
								Console.WriteLine("Nope! Try again.");
								break;
							}
						}
						catch (Exception)
						{
							Console.WriteLine("Error: Premise must be a reference to a line");
							break;
						}
                    case "CD":
                        if (!this.CheckTokenLength(tokens, 2)) break;
                        try
                        {
                            var temp = Convert.ToInt32(tokens[1]);
							if (inventory[temp]._Equals(toShow.cons))
							{
								return true;
							}
							else
							{
								Console.WriteLine("Nope! Try again.");
								break;
							}
                        }
						catch (Exception)
						{
							if (tokens[1].Contains("PR"))
							{
								try 
								{
									if (argument.premises[Convert.ToInt32(tokens[1].Substring(2))-1]._Equals(toShow.cons)) return true;
								}
								catch (Exception e)
								{
									Console.WriteLine("That is not a premise.");
									Console.WriteLine(e);
									break;
								}
							}
							Console.WriteLine("Error: Premise must be a reference to a line");
							break;
						}
					default:
						Console.WriteLine("bad input: enter 'help' for more information.");
						break;
				}
            }
            return true;
        }

        //Prints the Show statement with a list of currently generated premises at the users disposal.
        public void ListSheet(Premise Prem)
        {
            Console.WriteLine("Show: " + Prem.GetPremise());
            for (var i = 0; i < inventory.Count;i++)
            {
                Console.WriteLine(Convert.ToString(i)+": " + inventory[i].GetPremise());
            }
        }

        //Checks to make sure that input is the right format.
        //E.g. [Rule] [Premise] [Premise] must be 3 tokens long.
        public bool CheckTokenLength(string[] tokens, int desired)
        {
            if (tokens.Length != desired)
            {
                Console.WriteLine("Bad input. type 'help' for more information or try again.");
                return false;
            }
            else
            {
                return true;
            }
        }

        //Takes the input tokens and generates premise array;
        public Premise[] SetInputPremises(string[] tokens, Argument argument)
        {
            var premises = new Premise[tokens.Length-1];
			for (var i = 1; i < tokens.Length; i++)
			{
				if (tokens[i].Substring(0, 1) == "P")
				{
                    premises[i - 1] = argument.premises[Convert.ToInt32(tokens[i].Substring(2)) - 1];
				}
				else
				{
                    try
                    {
                        premises[i - 1] = inventory[Convert.ToInt32(tokens[i])];
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Premise input must begin with 'PR' or be an integer.");
                        return null;
                    }
				}
			}
            return premises;
        }
    }
}
