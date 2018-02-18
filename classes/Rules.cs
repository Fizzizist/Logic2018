using System;
namespace Logic2018
{
    public class Rules
    {
        private ProblemConstructor problemConstructor = new ProblemConstructor();
        public Rules()
        {
        }

        //Checks to make sure that MP can be performed on these particular premises.
        public bool MPCheck(Premise a, Premise b)
        {
            if (a.type==1||b.type==1) return true;
            return false;
        }

        //Checks to make sure that MT can be performed on these particular premises.
        public bool MTCheck(Premise a, Premise b)
        {
			if ((a.type == 5 && b.type == 1) || (a.type == 1 && b.type == 5)) return true;
			return false;
        }

        //Checks to make sure that DNE can be performed on the particular premise.
        public bool DNECheck(Premise a){
            if (a.type == 5 && a.negated.type == 5) return true;
            return false;
        }

        

        //Generates a new premise as the resule of performing a DNE on the input premise.
        public Premise DNE(Premise a)
        {
            var newPremise = a.negated.negated;
            return newPremise;
        }

		//Generates a new premise as the result of performing a DNI on the input premise.
		public Premise DNI(Premise a)
        {
            var newPremiseInner = new Premise(a);
            var newPremiseOuter = new Premise(newPremiseInner);
            return newPremiseOuter;
        }
        
        public Premise MC1(Premise a, string b)
        {
            var MCAnticedent = problemConstructor.MakeCustom(b);
            var newPremise = new Premise(1, MCAnticedent, a);
            return newPremise;
        }
        
        public Premise MC2(Premise a, string b)
        {
            var MCConsequent = problemConstructor.MakeCustom(b);
            var MCAnticedent = a.negated;
            var newPremise = new Premise(1, MCAnticedent, MCConsequent);
            return newPremise;
        }

        //Generates a new premise as a result of performing Modus Ponens on the two input presmises.
        public Premise ModusPonens(Premise a, Premise b)
        {	
            switch (a.type)
            {
                case 0: case 2: case 3: case 4: case 5:
                    if (a._Equals(b.anti)) return b.cons;
                    else return null;
                case 1:
                    if (b.type==1&&a._Equals(b.anti)) return b.cons;
                    else if (b._Equals(a.anti)) return a.cons;
                    else return null;
                default:
                    return null;
            }
        }

        //Generates a new premise as a result of performing Modus Tolens on the two input premises.
		public Premise ModusTolens(Premise a, Premise b)
		{
            Premise negated;
			switch (a.type)
			{
				case 5:
                    if (a.negated._Equals(b.cons)) 
                    {
                        negated = new Premise(b.anti);
					    return negated;
                    }
                    else return null;
				case 1:
                    if (b.negated._Equals(a.cons))
                    {
                        negated = new Premise(a.anti);
                        return negated;    
                    }
                    else return null;
				default:
					return null;
			}
		}


        //Makes sure that a generated premise is the same as the show premise.
        public bool DDCheck(Premise a, Premise b){
            if (a._Equals(b)) return true;
            return false;
        }

        //Makes sure that their is a proper contradiction.
        public bool IDCheck(Premise a, Premise b){
            switch (a.type){
                case 5:
                    if (a.negated._Equals(b)) return true;
                    return false;
                default:
                    if (a._Equals(b.negated)) return true;
                    return false;
            }
        }
    }
}
