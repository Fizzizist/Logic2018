/*
A class for reading in a file.
Author: Peter Vlasveld
*/
using System;
using System.IO;
namespace Logic2018
{
    public class Reader
    {
        private StreamReader sr;
        public Reader()
        {
            
        }
	
	//Prints a file to the screen within specific line numbers.
        public void Read(int from, int to, string filepath)
        {
            sr = new StreamReader(filepath);
            string line;
            var count = 0;
            while (count < to) 
            {
                line = sr.ReadLine();
                if(count>=from)
                {
                    Console.WriteLine(line);
                }
                count++;
            }
        }
	
	//Prints the whole file to the screen.
        public void ReadWholeFile(string filepath)
        {
            sr = new StreamReader(filepath);
            string line;
			while ((line = sr.ReadLine()) != null)
			{
				Console.WriteLine(line);
			}
        }

	//Adds ten lines to output for readability.
        public void AddTenBlankLines()
        {
            for (var i=0;i<10;i++)
            {
                Console.WriteLine(" ");
            }
        }
    }
}
