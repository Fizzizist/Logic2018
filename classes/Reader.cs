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

        public void ReadWholeFile(string filepath)
        {
            sr = new StreamReader(filepath);
            string line;
			while ((line = sr.ReadLine()) != null)
			{
				Console.WriteLine(line);
			}
        }

        public void AddTenBlankLines()
        {
            for (var i=0;i<10;i++)
            {
                Console.WriteLine(" ");
            }
        }
    }
}