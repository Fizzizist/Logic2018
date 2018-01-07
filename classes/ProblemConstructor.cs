using System;
using System.Collections.Generic;
namespace Logic2018
{
    public enum AtomicsEnum { P, Q, R, S, T, U, V, W, X, Y, Z };
    public class ProblemConstructor
    {
        private List<Premise> atomics = new List<Premise>();
        private List<Premise> premises = new List<Premise>();
        private Premise conclusion;
        private string[] atomicsArray = { "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public Argument argument;
        public ProblemConstructor(int choice)
        {
            switch (choice)
            {
                //P->Q. P C:Q.
                case 0:
                    this.MakeAtomics(2);
                    premises.Add(new Premise(1, atomics[0], atomics[1]));
                    premises.Add(atomics[0]);

                    conclusion = atomics[1];

                    argument = new Argument(premises, conclusion);
                    break;

				//P->R R->Q ∴P->Q
				case 1:
                    this.MakeAtomics(3);
                    premises.Add(new Premise(1, atomics[(int)AtomicsEnum.P], atomics[(int)AtomicsEnum.R]));
                    premises.Add(new Premise(1, atomics[(int)AtomicsEnum.R], atomics[(int)AtomicsEnum.Q]));
                    conclusion = new Premise(1, atomics[(int)AtomicsEnum.P], atomics[(int)AtomicsEnum.Q]);
                    argument = new Argument(premises, conclusion);
                    break;

				//~P. Q->P ∴~Q
                case 2:
                    this.MakeAtomics(2);
                    premises.Add(new Premise(atomics[(int)AtomicsEnum.P]));
                    premises.Add(new Premise(1, atomics[(int)AtomicsEnum.Q], atomics[(int)AtomicsEnum.P]));
                    conclusion = new Premise(atomics[(int)AtomicsEnum.Q]);
                    argument = new Argument(premises, conclusion);
                    break;

				//~~(P->Q). P ∴ Q
                case 3:
                    this.MakeAtomics(2);
                    Premise PR1Conditional = new Premise(1, atomics[(int)AtomicsEnum.P], atomics[(int)AtomicsEnum.Q]);
                    Premise PR1FirstNegation = new Premise(PR1Conditional);
                    premises.Add(new Premise(PR1FirstNegation));
                    premises.Add(atomics[(int)AtomicsEnum.P]);
                    conclusion = atomics[(int)AtomicsEnum.Q];
                    argument = new Argument(premises, conclusion);
                    break;

				//P. R->~Q. P->Q ∴ ~R
                case 4:
                    this.MakeAtomics(3);
                    premises.Add(atomics[(int)AtomicsEnum.P]);
                    Premise PR2Negation = new Premise(atomics[(int)AtomicsEnum.Q]);
                    premises.Add(new Premise(1, atomics[(int)AtomicsEnum.R], PR2Negation));
                    premises.Add(new Premise(1, atomics[(int)AtomicsEnum.P], atomics[(int)AtomicsEnum.Q]));
                    conclusion = new Premise(atomics[(int)AtomicsEnum.R]);
                    argument = new Argument(premises, conclusion);
                    break;

				//P→(Q→R). (Q→R)→S. ~S ∴ ~P
                case 5:
                    this.MakeAtomics(4);
                    Premise PR1Conditional_5 = new Premise(1, atomics[(int)AtomicsEnum.Q], atomics[(int)AtomicsEnum.R]);
                    premises.Add(new Premise(1, atomics[(int)AtomicsEnum.P], PR1Conditional_5));
                    premises.Add(new Premise(1, PR1Conditional_5, atomics[(int)AtomicsEnum.S]));
                    premises.Add(new Premise(atomics[(int)AtomicsEnum.S]));
                    conclusion = new Premise(atomics[(int)AtomicsEnum.P]);
                    argument = new Argument(premises, conclusion);
                    break;
				default:
                    Console.WriteLine("That argument doesn't exist.");
                    break;
            }
        }

        public void MakeAtomics(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                atomics.Add(new Premise(atomicsArray[i]));
            }
        }

        public Premise MakeCustom(string inputString)
        {
            Premise newPremise;

            //Base case.
            if (inputString.Length == 1)
            {
                var isAtomic = false;
                for (var i = 0; i < atomicsArray.Length; i++)
                {
                    if (inputString == atomicsArray[i]) isAtomic = true;
                }
                if (isAtomic == true)
                {
                    newPremise = new Premise(inputString);
                    return newPremise;
                }
                else
                {
                    Console.WriteLine("One of your atomics is not valid.");
                    return null;
                }
            }
            else
            {
                //e.g. ~(P->(R->Q))^(S->R)
                var unbracketed = "";
                var objectString = new List<string>();
                //If it contains brackets make object strings to pass back into method.
                if (inputString.Contains("("))
                {
                    var firstBracket = 0;
                    var counter = 0;
                    var firstClosingBracket = 0;
                    var premiseString = "";
                    for (var i = 0; i < inputString.Length; i++)
                    {
                        if (inputString[i] == '(' && counter == 0)
                        {
                            firstBracket = i;
                            unbracketed += inputString[i];
                            counter++;
                        }
                        else if (inputString[i] == '(' && counter > 0)
                        {
                            counter++;
                        }
                        else if (inputString[i] != '(' && counter == 0)
                        {
                            unbracketed += inputString[i];
                        }

                        if (inputString[i] == ')' && counter == 1)
                        {
                            firstClosingBracket = i;
                            objectString.Add(premiseString);
                            premiseString = "";
                            unbracketed += inputString[i];
                            counter--;
                        }
                        else if (inputString[i] == ')' && counter > 1)
                        {
                            counter--;
                        }
                        if (counter > 0 && i != firstBracket)
                        {
                            premiseString += inputString[i];
                        }
                    }
                    //unbracketed=~()^()
                }
                else
                {
                    unbracketed = inputString;
                    if (unbracketed.Contains("~"))
                    {
                        newPremise = new Premise(this.MakeCustom(inputString.Substring(1)));
                        return newPremise;
                    }

                }

                //Deal with negations: add negation to object strings before they are
                //constructed into premises.
                if (unbracketed.Contains("~"))
                {
                    if (objectString.Count == 1)
                    {
                        newPremise = new Premise(this.MakeCustom(objectString[0]));
                        return newPremise;
                    }
                    else
                    {
                        var negationCount = 0;
                        for (var i = 0; i < unbracketed.Length; i++)
                        {
                            if (unbracketed[i] == '~') negationCount++;
                        }
                        if (negationCount == 2)
                        {
                            objectString[0] = "~(" + objectString[0] + ")";
                            objectString[1] = "~(" + objectString[1] + ")";
                        }
                        else if (negationCount == 1 && unbracketed[0] == '~')
                        {
                            objectString[0] = "~(" + objectString[0] + ")";
                        }
                        else
                        {
                            objectString[1] = "~(" + objectString[1] + ")";
                        }
                    }
                }

                //Construct premises based on object strings.
                if (unbracketed.Contains("->"))
                {
                    
                    newPremise = MakeChildrenCustom(1, objectString[0], objectString[1]);
                    return newPremise;
                }
                else if (unbracketed.Contains("<->"))
                {
                    newPremise = MakeChildrenCustom(2, objectString[0], objectString[1]);
                    return newPremise;
                }
                else if (unbracketed.Contains("^"))
                {
                    newPremise = MakeChildrenCustom(3, objectString[0], objectString[1]);
                    return newPremise;
                }
                else if (unbracketed.Contains("v"))
                {
                    newPremise = MakeChildrenCustom(4, objectString[0], objectString[1]);
                    return newPremise;
                }
                Console.WriteLine("Bad input.");
                return null;
            }
        }

        private Premise MakeChildrenCustom(int type, string a, string b)
        {
            var premiseArray = new Premise[2];
            premiseArray[0] = new Premise(this.MakeCustom((a)));
            premiseArray[1] = new Premise(this.MakeCustom((b)));
            var outPremise = new Premise(type, premiseArray[0], premiseArray[1]);
            return outPremise;
        }
    }
}
