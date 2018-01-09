using System;
namespace Logic2018
{
    public class Premise
    {
        public int type;
        public int makeType;
        public Premise anti;
        public Premise cons;
        public Premise child1;
        public Premise child2;
        public Premise negated;
        public string atomic;

        //Atomic constructor.
        public Premise(string atomicInput)
        {
            type = 0;
            atomic = atomicInput;
        }

        //Conditional, biconditional, AND, and OR constructor.
        //1: Conditional, 2:Biconditional, 3:AND, 4:OR
        public Premise(int typeInput, Premise a, Premise b)
        {
            type = typeInput;
            switch (type)
            {
                case 1:
                    anti = a;
                    cons = b;
                    break;
                case 2:
                case 3:
                case 4:
                    child1 = a;
                    child2 = b;
                    break;
            }
        }

        //Negation constructor.
        public Premise(Premise a)
        {
            type = 5;
            negated = a;
        }

        //Returns a string containing the contents of the premise.
        //0:atomic 1:conditional 2:biconditional 3:and 4:or 5:negation
        public string GetPremise()
        {
            switch (type)
            {
                case 0:
                    return atomic;
                case 1:
                    if (anti.type != 0) return "(" + anti.GetPremise() + ")" + "→" + cons.GetPremise();
                    if (cons.type != 0) return anti.GetPremise() + "→" + "(" + cons.GetPremise() + ")";
                    return anti.GetPremise() + "→" + cons.GetPremise();
                case 2:
                    if (child1.type != 0) return "(" + child1.GetPremise() + ")" + "→" + child2.GetPremise();
                    if (child2.type != 0) return child1.GetPremise() + "→" + "(" + child2.GetPremise() + ")";
                    return child1.GetPremise() + "⇔" + child2.GetPremise();
                case 3:
                    if (child1.type != 0) return "(" + child1.GetPremise() + ")" + "→" + child2.GetPremise();
                    if (child2.type != 0) return child1.GetPremise() + "→" + "(" + child2.GetPremise() + ")";
                    return child1.GetPremise() + "^" + child2.GetPremise();
                case 4:
                    if (child1.type != 0) return "(" + child1.GetPremise() + ")" + "→" + child2.GetPremise();
                    if (child2.type != 0) return child1.GetPremise() + "→" + "(" + child2.GetPremise() + ")";
                    return child1.GetPremise() + "∨" + child2.GetPremise();
                case 5:
                    if (negated.type == 0 || negated.type == 5) return "~" + negated.GetPremise();
                    return "~" + "(" + negated.GetPremise() + ")";
                default:
                    return "";
            }
        }

        //Changes the atomic if the premise is an atomic.
        public void ChangeAtomic(string Atom)
        {
            atomic = Atom;
        }

        //Recursively compares two premises to make sure their contents are equal.
        public bool _Equals(Premise a)
        {
            if (this.type == a.type)
            {
                switch (this.type)
                {
                    case 0:
                        if (this.atomic == a.atomic) return true;
                        break;
                    case 1:
                        if (this.anti._Equals(a.anti) && this.cons._Equals(a.cons)) return true;
                        break;
                    case 2:
                    case 3:
                    case 4:
                        if (this.child1._Equals(a.child1) && this.child2._Equals(a.child2)) return true;
                        break;
                    case 5:
                        if (this.negated._Equals(a.negated)) return true;
                        break;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
            return false;
        }
    }
}