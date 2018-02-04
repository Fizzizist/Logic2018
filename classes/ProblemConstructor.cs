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

        public ProblemConstructor(int choice)
        {
            argument = MakeCustomArgument(saveCloud.GetArgumentConstructorRow(choice));
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
                newPremise = new Premise(inputString);
                return newPremise;
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
					for (var i = 0; i < inputString.Length; i++)
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

            //Testing.
			/*for (var i = 0; i < objectString.Count; i++)
			{
				Console.WriteLine(objectString[i]);
			}
			Console.WriteLine(unbracketed);*/


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

			//reconstruct negated object strings
			if (objectString.Count > 1 && unbracketed.Contains("~"))
			{
				var negationCounter = 0;
				for (var i = 0; i < unbracketed.Length; i++)
				{
					if (unbracketed[i] == '~') negationCounter++;
				}
				if (negationCounter == 2)
				{
					for (var i = 0; i < 2; i++)
					{
						objectString[i] = "~(" + objectString[i] + ")";
					}
				}
				else
				{
					if (unbracketed[0] == '~') objectString[0] = "~(" + objectString[0] + ")";
					else objectString[1] = "~(" + objectString[1] + ")";
				}
			}

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

            conclusion = MakeCustom(tokens[tokens.Length - 1]);

            newArgument = new Argument(premises, conclusion);
            return newArgument;
        }

		
	}
}
