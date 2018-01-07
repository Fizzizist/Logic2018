using System;
namespace Logic2018
{
    public class Rules
    {
        public Rules()
        {
        }

        //Checks to make sure that MP can be performed on these particular premises.
        public bool MPCheck(Premise a, Premise b)
        {
            if ((a.type==0&&b.type==1)||(a.type == 1 && b.type == 0)) return true;
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

        //Generates a new premise as a result of performing Modus Ponens on the two input presmises.
        public Premise ModusPonens(Premise a, Premise b)
        {	
            switch (a.type)
            {
                case 0:
                    return b.cons;
                case 1:
                    return a.cons;
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
                    negated = new Premise(b.anti);
					return negated;
				case 1:
                    negated =new Premise(a.anti);
                    return negated;
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
