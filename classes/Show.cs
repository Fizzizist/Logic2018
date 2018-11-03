/*
Main problem solving Class for user to actually solve the derivation.
Show classes can be nested within one another.
Author: Peter Vlasveld
*/

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
		private Reader writer = new Reader();
		private bool MC1Unlocked = false;
		private bool MC2Unlocked = false;
		private SaveCloud saveCloud = new SaveCloud();
		private string userID;
		private bool redo = false;
        public Show(string id)
        {
			userID = id;
			assumeCounter = 0;
			if (saveCloud.OpenConnection())
			{
				MC1Unlocked = saveCloud.CheckRuleSolved(2,1, userID);
				MC2Unlocked = saveCloud.CheckRuleSolved(2,17,userID);
				saveCloud.CloseConnection();
			}
        }

		public void CheckRuleLocks()
		{
			if (saveCloud.OpenConnection())
			{
				MC1Unlocked = saveCloud.CheckRuleSolved(2,1, userID);
				MC2Unlocked = saveCloud.CheckRuleSolved(2,17,userID);
				saveCloud.CloseConnection();
			}
		}

        //Main loop for derivation of the argument's conclusion.
        public bool ShowPremise(Argument argument, Premise toShow, List<Premise> mainInv)
        {
			redo = false;
            for (var j = 0; j < mainInv.Count;j++)
            {
                inventory.Add(mainInv[j]);
            }
            for (var i = 0; i < argument.premises.Count;i++)
            {
                argumentPremises.Add(argument.premises[i]);
            }
			MainShow:
			
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
						writer.ReadWholeFile("textFiles/helpShow.txt");
						goto MainShow;

                    case "exit":
						inventory.Clear();
						assumeCounter = 0;
                        return false;
					case "redo":
						inventory.Clear();
						assumeCounter = 0;
						redo = true;
						return false;
					//Modus Ponens
					case "MP": case "mp":
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
					case "MT": case "mt":
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
                    case "DN": case "dn":
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
                    case "DNE": case "dne":
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
                    case "DNI": case "dni":
						var inDNI = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 2)) break;
						inDNI = this.SetInputPremises(tokens, argument);
                        if (inDNI == null) break;
						inventory.Add(rules.DNI(inDNI[0]));
                        Console.WriteLine(argument.GetArgument());
                        this.ListSheet(toShow);
						break;

					case "SR": case "sr":
						var inSR = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 2)) break;
						inSR = this.SetInputPremises(tokens, argument);
                        if (inSR == null) break;
						inventory.Add(inSR[0].child2);
						Console.WriteLine(argument.GetArgument());
                        this.ListSheet(toShow);
						break;

					case "SL": case "sl":
						var inSL = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 2)) break;
						inSL = this.SetInputPremises(tokens, argument);
                        if (inSL == null) break;
						inventory.Add(inSL[0].child1);
						Console.WriteLine(argument.GetArgument());
                        this.ListSheet(toShow);
						break;

					case "S": case "s":
						var inS = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 2)) break;
						inS = this.SetInputPremises(tokens, argument);
                        if (inS == null) break;
						inventory.Add(inS[0].child2);
						inventory.Add(inS[0].child1);
						Console.WriteLine(argument.GetArgument());
                        this.ListSheet(toShow);
						break;

					case "AddR": case "addr":
						AddR:
						var inAddR = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 2)) break;
						inAddR = this.SetInputPremises(tokens, argument);
                        if (inAddR == null) break;
						var AddRPremise = rules.AddR(inAddR[0]);
						if (AddRPremise==null)
						{
							Console.WriteLine("That is not a valid addition.");
							break;
						}
						else
						{
							inventory.Add(AddRPremise);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						}
					
					case "AddL": case "addl":
						var inAddL = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 2)) break;
						inAddL = this.SetInputPremises(tokens, argument);
                        if (inAddL == null) break;
						var AddLPremise = rules.AddL(inAddL[0]);
						if (AddLPremise==null)
						{
							Console.WriteLine("That is not a valid addition.");
							break;
						}
						else
						{
							inventory.Add(AddLPremise);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						}

					case "Add": case "add":
						goto AddR;

					case "Adj": case "adj":
						var inAdj = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens,3)) break;
						inAdj = this.SetInputPremises(tokens,argument);
						if (inAdj == null) break;
						inventory.Add(rules.Adj(inAdj[0],inAdj[1]));
						Console.WriteLine(argument.GetArgument());
                        this.ListSheet(toShow);
						break;

					case "MTP": case "mtp":
						var inMTP = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens,3)) break;
						inMTP = this.SetInputPremises(tokens,argument);
						if (inMTP == null) break;
						var MTPPremise = rules.MTP(inMTP[0],inMTP[1]);
						if (MTPPremise==null)
						{
							Console.WriteLine("Cannot perform this rule on these premises.");
							break;
						}
						else
						{
							inventory.Add(MTPPremise);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						}

					case "BCL": case "bcl":
						var inBCL = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens,2)) break;
						inBCL = this.SetInputPremises(tokens,argument);
						if (inBCL == null) break;
						var BCLPremise = rules.BCL(inBCL[0]);
						if (BCLPremise==null)
						{
							Console.WriteLine("This rule cannot be performed on that premise.");
							break;
						}
						else
						{
							inventory.Add(BCLPremise);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						}

					case "BCR": case "bcr":
						var inBCR = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens,2)) break;
						inBCR = this.SetInputPremises(tokens,argument);
						if (inBCR == null) break;
						var BCRPremise = rules.BCR(inBCR[0]);
						if (BCRPremise==null)
						{
							Console.WriteLine("This rule cannot be performed on that premise.");
							break;
						}
						else
						{
							inventory.Add(BCRPremise);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						}

					case "BC": case "bc":
						var inBC = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens,2)) break;
						inBC = this.SetInputPremises(tokens,argument);
						if (inBC == null) break;
						var BCPremise1 = rules.BCL(inBC[0]);
						var BCPremise2 = rules.BCR(inBC[0]);
						if (BCPremise1==null||BCPremise2==null)
						{
							Console.WriteLine("This rule cannot be performed on that premise.");
							break;
						}
						else
						{
							inventory.Add(BCPremise1);
							inventory.Add(BCPremise2);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						}

					case "CB": case "cb":
						var inCB = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens,3)) break;
						inCB = this.SetInputPremises(tokens,argument);
						if (inCB == null) break;
						var CBPremise = rules.CB(inCB[0], inCB[1]);
						if (CBPremise==null)
						{
							Console.WriteLine("This rule cannot be performed on that premise.");
							break;
						}
						else
						{
							inventory.Add(CBPremise);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						}

					case "MC1": case "mc1":
						MC1:
						if (!MC1Unlocked)
						{
							Console.WriteLine("Solve derivation 1 of problem set 2 to unlock this rule.");
							break;
						}
						if (!this.CheckTokenLength(tokens,2)) break;
						var inMC1 = this.SetInputPremises(tokens,argument);
						if (inMC1==null) break;
						Console.Write("New Anticedent:");
						var MC1String = Console.ReadLine();
						inventory.Add(rules.MC1(inMC1[0],MC1String));
						Console.WriteLine(argument.GetArgument());
						this.ListSheet(toShow);
						break;

					case "MC2": case "mc2":
						MC2:
						if (!MC2Unlocked)
						{
							Console.WriteLine("Solve derivation 17 of problem set 2 to unlock this rule.");
							break;
						}
						if (!this.CheckTokenLength(tokens, 2)) break;
						var inMC2 = this.SetInputPremises(tokens,argument);
						if (inMC2==null) break;
						Console.Write("New Consequent:");
						var MC2String = Console.ReadLine();
						inventory.Add(rules.MC2(inMC2[0],MC2String));
						Console.WriteLine(argument.GetArgument());
						this.ListSheet(toShow);
						break;

					case "MC": case "mc":
						if (!this.CheckTokenLength(tokens, 2)) break;
						var inMC = this.SetInputPremises(tokens,argument);
						if (inMC==null) break;
						if (inMC[0].type==5) goto MC2;
						else goto MC1;

					case "Show":
						Show:
						if (!this.CheckTokenLength(tokens, 2)) break;
						var problemConstructor = new ProblemConstructor();
						var newPremise = problemConstructor.MakeCustom(tokens[1]);
						if (newPremise==null)
						{
							Console.WriteLine("Invalid premise");
							break;
						}
						var newShow = new Show(userID);
						if (newShow.ShowPremise(argument, newPremise, inventory))
						{
							inventory.Add(newPremise);
							Console.WriteLine(argument.GetArgument());
							this.ListSheet(toShow);
							break;
						} else if (newShow.GetRedo())
						{
							inventory.Clear();
							assumeCounter = 0;
							redo = true;
							return false;
						} 
						break;


					//Assume CD or ID
					case "ASS": case "ass":
						if (!this.CheckTokenLength(tokens, 2)) break;
						if (assumeCounter>0) 
						{
							Console.WriteLine("Only 1 assume statement can be made per Show statement.");
							break;
						}
						if (tokens[1].Equals("ID", StringComparison.CurrentCultureIgnoreCase))
						{
							inventory.Add(new Premise(toShow));
							Console.WriteLine(argument.GetArgument());
                            this.ListSheet(toShow);
							assumeCounter++;
							break;
						}
						else if (tokens[1].Equals("CD", StringComparison.CurrentCultureIgnoreCase))
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
					case "ID": case "id":
						//decides whether input is from lines or argument
                        var inID = new Premise[tokens.Length - 1];
						if (!this.CheckTokenLength(tokens, 3)) break;
                        inID = this.SetInputPremises(tokens, argument);
                        if (inID == null) break;

						if (rules.IDCheck(inID[0], inID[1]))
						{
							assumeCounter--;
							inventory.Clear();
                            return true;
						}
						else
						{
							Console.WriteLine("Nope! Try again.");
							break;
						}

					//direct derivation
					case "DD": case "dd":
						if (!this.CheckTokenLength(tokens, 2)) break;
						try
						{
                            var temp = Convert.ToInt32(tokens[1]);
                            if (inventory[temp]._Equals(toShow))
							{
								inventory.Clear();
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
                    case "CD": case "cd":
                        if (!this.CheckTokenLength(tokens, 2)) break;
                        try
                        {
                            var temp = Convert.ToInt32(tokens[1]);
							if (inventory[temp]._Equals(toShow.cons))
							{
								assumeCounter--;
								inventory.Clear();
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
						if (command.Equals("show", StringComparison.CurrentCultureIgnoreCase)) goto Show;
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
                Console.WriteLine("Bad input. Try again.");
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
				try
				{
					premises[i - 1] = inventory[Convert.ToInt32(tokens[i])];
				}
				catch (Exception)
				{
					try
					{
						if (tokens[i].Substring(0, 2).Equals("PR", StringComparison.CurrentCultureIgnoreCase))
						{
							premises[i - 1] = argument.premises[Convert.ToInt32(tokens[i].Substring(2)) - 1];
						}
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
		
		
		public void SetAssumeCounter(int a){
			assumeCounter = a;
		}
		
		//clears all current premises at users disposal
		public void ClearInventory()
		{
			inventory.Clear();
		}

		//sends back that user wants to redo the problem
		public bool GetRedo()
		{
			return redo;
		}
    }
}
