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
							if (inputString[i] != '(' && inputString[i] != ')' && inputString[i] != '~' && inputString[i] != '-' && inputString[i] != '>' && inputString[i] != '<' && inputString[i] != '^' && inputString[i] != 'v')
							{
								objectString.Add(inputString[i].ToString());
							}
							else
							{
								unbracketed += inputString[i];
							}
						}
					}
				}
				else
				{
					string[] tokens;
					if(inputString.Length==2)
					{
						objectString.Add(inputString.Substring(1));
						unbracketed = "~";
					}
					else if (inputString.Contains("<->"))
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
				}
			}

            
			for (var i = 0; i < objectString.Count; i++)
			{
				Console.WriteLine(objectString[i]);
			}
			Console.WriteLine(unbracketed);
			Console.WriteLine(objectString.Count);


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
			for (var i=0;i<objectString.Count;i++)
			{
				Console.WriteLine(objectString[i]);
			}



			//reconstruct negated object strings
			if (objectString.Count > 1 && unbracketed.Contains("~"))
			{
				var firstNegatedPremise = 0;
				var firstNegatedAmount = 0;
				var secondNegatedAmount = 0;
				for (var i=0;i<unbracketed.Length;i++)
				{
					if (unbracketed[i]=='~') firstNegatedAmount++;
					if (unbracketed[i]!='~')
					{
						firstNegatedPremise = i;
						goto SecondFor;
					}
				}
				SecondFor:
				for (var i=firstNegatedPremise;i<unbracketed.Length;i++)
				{
					if (unbracketed[i]=='~') secondNegatedAmount++;
					
				}

				if (firstNegatedAmount>0) objectString[0] = "(" + objectString[0] + ")";
				for (var i=0;i<firstNegatedAmount;i++)
				{
					objectString[0] = "~" + objectString[0];
				}

				if (secondNegatedAmount>0) objectString[1] = "(" + objectString[1] + ")";
				for (var i=0;i<secondNegatedAmount;i++)
				{
					objectString[1] = "~" + objectString[1];
				}
			}

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
		
	}
}
