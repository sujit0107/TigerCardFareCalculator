using System;

namespace TigerCard
{
    class Program
    {
        static void Main()
        {
            try
            {
                InputFileReader reader = new InputFileReader();
                reader.ReadInputFile();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
