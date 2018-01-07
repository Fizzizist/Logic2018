using System;
using System.Collections.Generic;
namespace Logic2018
{
    //Contains an argument with a list of premises and a conclusion, all
    //of which are instances of the Premise class.
    public class Argument
    {
        public List<Premise> premises = new List<Premise>();
        public Premise conclusion;

        public Argument(List<Premise> premisesInput, Premise conclusionInput)
        {
            premises = premisesInput;
            conclusion = conclusionInput;
        }

        //Returns a string containing the full argument.
        public string GetArgument()
        {
            var _out = "";
            var count = 1;
            for (var i = 0; i < premises.Count; i++)
            {
                _out = _out + "PR" + Convert.ToString(count) + ":" + premises[i].GetPremise() + " ";
                count++;
            }
            _out = _out + "∴" + conclusion.GetPremise();
            return _out;
        }
    }
}
