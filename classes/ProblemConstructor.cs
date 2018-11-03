/*
A Class for parsing an input string and constructing a derivation problem from it
Author: Peter Vlasveld
*/

using System;
using System.Collections.Generic;
using System.IO;
namespace Logic2018
{
    public class ProblemConstructor
    {
        public Argument argument;
		private SaveCloud saveCloud = new SaveCloud();

        public ProblemConstructor()
        {
            
        }

        public ProblemConstructor(int table, int choice)
        {
            argument = MakeCustomArgument(saveCloud.GetArgumentConstructorRow(table, choice));
        }

        //Parses the input string and makes nested Premises out of string.
		public Premise MakeCustom(string inputString)
		{
			var objectString = new List<string>();
			var count = 0;
			var unbracketed = "";
            Premise newPremise;
			if (inputString.Length == 1)
			{
				if (this.IsValidAtomic(inputString))
				{
					newPremise = new Premise(inputString);
					return newPremise;	
				}
                else
				{
					return null;
				}
			}
			else
			{
				if (inputString.Contains("("))
				{
					var _object = "";

					for (var i = 0; i < inputString.Length; i++)
					{
						if (inputString[i] == ')' && count == 1)
						{
							objectString.Add(_object);
							_object = "";
							count--;
							unbracketed += inputString[i];
						}
						else if (count > 0 && inputString[i] == ')')
						{
							count--;
							_object += inputString[i];
						}
						else if (inputString[i] == '(' && count == 0)
						{
							unbracketed += inputString[i];
							count++;
						}
						else if (count > 0 && inputString[i] == '(')
						{
							count++;
							_object += inputString[i];
						}
						else if (count > 0)
						{
							_object += inputString[i];
						}
						else if (count == 0)
						{
							unbracketed += inputString[i];
						}
					}
					//For testing.
					/*for (var i = 0; i < objectString.Count; i++)
					{
						Console.WriteLine(objectString[i]);
					}*/
					if (unbracketed=="()")
					{
						return this.MakeCustom(objectString[0]);
					}
					if (unbracketed.Contains("<->"))
					{
						var objectStringTemp = this.ConstructObjectString(inputString, unbracketed,objectString, "<->");
						objectString.Clear();
						objectString.Add(objectStringTemp[0]);
						objectString.Add(objectStringTemp[1]);
					}
					else if (unbracketed.Contains("->"))
					{
						var objectStringTemp = this.ConstructObjectString(inputString, unbracketed,objectString, "->");
						objectString.Clear();
						objectString.Add(objectStringTemp[0]);
						objectString.Add(objectStringTemp[1]);
					}
					else if (unbracketed.Contains("v"))
					{
						var objectStringTemp = this.ConstructObjectString(inputString, unbracketed,objectString, "v");
						objectString.Clear();
						objectString.Add(objectStringTemp[0]);
						objectString.Add(objectStringTemp[1]);
					}
					else if (unbracketed.Contains("^"))
					{
						var objectStringTemp = this.ConstructObjectString(inputString, unbracketed,objectString, "^");
						objectString.Clear();
						objectString.Add(objectStringTemp[0]);
						objectString.Add(objectStringTemp[1]);
					}
				}
				else
				{
					string[] tokens;
					if (inputString.Contains("<->"))
					{
						tokens = inputString.Split("<->");
						objectString.Add(tokens[0]);
						objectString.Add(tokens[1]);
						unbracketed = "<->";	
					} 
					else if (inputString.Contains("->"))
					{
						tokens = inputString.Split("->");
						objectString.Add(tokens[0]);
						objectString.Add(tokens[1]);
						unbracketed = "->";	
					}
					else if (inputString.Contains("v")) 
					{
						tokens = inputString.Split("v");
						objectString.Add(tokens[0]);
						objectString.Add(tokens[1]);
						unbracketed = "v";
					}
					else if (inputString.Contains("^")) 
					{
						tokens = inputString.Split("^");
						objectString.Add(tokens[0]);
						objectString.Add(tokens[1]);
						unbracketed = "^";
					}
					else if(inputString.Contains("~"))
					{
						objectString.Add(inputString.Substring(inputString.Length-1));
						for (var i=0;i<inputString.Length-1;i++)
						{
							unbracketed += inputString[i];
						}
					}
				}
			}

			
            //Testing
			 /*
			for (var i = 0; i < objectString.Count; i++)
			{
				Console.WriteLine(objectString[i]);
			}
			Console.WriteLine(unbracketed);
			Console.WriteLine(objectString.Count);*/


			//Deal with basic negation cases
			if (objectString.Count == 1 && unbracketed.Contains("~"))
			{
				var negationCounter = 0;

				for (var i = 0; i < unbracketed.Length; i++)
				{
					if (unbracketed[i] == '~') negationCounter++;
				}

				if (negationCounter==1&&(unbracketed.Contains("(") || unbracketed.Length == 1)) 
                {
                    var negatedPremise = MakeCustom(objectString[0]);
                    newPremise = new Premise(negatedPremise);
                    return newPremise;
                }
				else if (negationCounter>1&&unbracketed.Contains("("))
				{
					objectString[0] = "(" + objectString[0] + ")";
					for (var i=negationCounter-1;i>0;i--)
					{
						objectString[0] = "~" + objectString[0];
					}

					var negatedPremise = MakeCustom(objectString[0]);
                    newPremise = new Premise(negatedPremise);
                    return newPremise;
				}
				else
				{
					for (var i=negationCounter-1;i>0;i--)
					{
						objectString[0] = "~" + objectString[0];
					}

					var negatedPremise = MakeCustom(objectString[0]);
                    newPremise = new Premise(negatedPremise);
                    return newPremise;
				}
			}

			//More Testing
			/*for (var i=0;i<objectString.Count;i++)
			{
				Console.WriteLine(objectString[i]);
			}*/


			//testing
			//Console.WriteLine(objectString[1]);

			//Deal with conditional, biconditional, AND, and OR
			if (unbracketed.Contains("<->"))
			{
                var childA = MakeCustom(objectString[0]);
                var childB = MakeCustom(objectString[1]);
                newPremise = new Premise(2, childA, childB);
                return newPremise;
			}
			else if (unbracketed.Contains("->"))
			{
				var anticedent = MakeCustom(objectString[0]);
				var consequent = MakeCustom(objectString[1]);
				newPremise = new Premise(1, anticedent, consequent);
				return newPremise;
			}
			else if (unbracketed.Contains("^"))
			{
				var childA = MakeCustom(objectString[0]);
				var childB = MakeCustom(objectString[1]);
				newPremise = new Premise(3, childA, childB);
				return newPremise;
			}
			else if (unbracketed.Contains("v"))
			{
				var childA = MakeCustom(objectString[0]);
				var childB = MakeCustom(objectString[1]);
				newPremise = new Premise(4, childA, childB);
				return newPremise;
			}
			Console.WriteLine("Bad Premise.");
            return null;
            //For testing purposes
			/*for (var i = 0; i < objectString.Count; i++)
			{
				Console.WriteLine(objectString[i]);
			}
			Console.WriteLine(unbracketed);*/
		} //End of MakeCustom method.

        //Uses MakeCustom method to construct an argument from a string.
        public Argument MakeCustomArgument(string inputString)
        {
            var tokens = inputString.Split(' ');
            var premises = new List<Premise>();
            Premise conclusion;
            Argument newArgument;

            for (var i = 0; i < tokens.Length - 2;i++)
            {
                premises.Add(MakeCustom(tokens[i]));
            }

			//testing
			/*for (var i=0; i<premises.Count;i++)
			{
				Console.WriteLine(premises[i].GetPremise());
			}*/

            conclusion = MakeCustom(tokens[tokens.Length - 1]);

            newArgument = new Argument(premises, conclusion);
            return newArgument;
        }

		public bool IsValidAtomic(string a)
        {
            if (a.Length!=1) return false;
            if (a.Contains("P")||a.Contains("Q")||a.Contains("R")||a.Contains("S")||a.Contains("T")||a.Contains("U")||a.Contains("V")||a.Contains("W")||a.Contains("X")||a.Contains("Y")||a.Contains("Z")) return true;
            return false;
        }
		
		private List<string> ConstructObjectString(string originalInput, string unbrack, List<string> objStr, string symbol)
		{
			var temp = unbrack.Split(symbol);
			var objectStringTemp = new List<string>();
			if (!temp[0].Contains("()")) objectStringTemp.Add(temp[0]);
			else if (temp[0]=="()") objectStringTemp.Add(objStr[0]);
			else if (temp[0]=="~()")  objectStringTemp.Add("~("+objStr[0]+")");
			else 
			{
				var upToOperator = "";
				var bracketed = 0;
				var done = false;
				for (var i=0;i<originalInput.Length;i++)
				{
					if (originalInput[i]=='(') bracketed++;
					if (originalInput[i]==')') bracketed--;
					if (bracketed==0&&originalInput[i]==symbol[0]) done=true;
					if (done==false) upToOperator += originalInput[i];  
				}
				objectStringTemp.Add(upToOperator);
			}
			//Console.WriteLine(temp[1]); //Testing.
			if (!temp[1].Contains("()")) objectStringTemp.Add(temp[1]);
			else 
			{
				var upToOperator = "";
				var bracketed = 0;
				var notYet = true;
				for (var i=0;i<originalInput.Length;i++)
				{
					if (originalInput[i]=='(') bracketed++;
					if (originalInput[i]==')') bracketed--;
					if (notYet==false) upToOperator += originalInput[i];
					if (bracketed==0&&originalInput[i]==symbol[0]) 
					{
						notYet=false;
						if (symbol=="->") i++;
						if (symbol=="<->") i += 2;
					} 
				}
				objectStringTemp.Add(upToOperator);
			}
			return objectStringTemp;
		}
	}
}
