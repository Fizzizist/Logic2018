using System;
using System.IO;
namespace Logic2018
{
    public class Writer
    {
        private StreamReader sr;
        public Writer()
        {
            
        }

        public void Write(int from, int to, string filepath)
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

        public void WriteWholeFile(string filepath)
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