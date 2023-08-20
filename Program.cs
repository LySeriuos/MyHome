using MyHome;

namespace My_Home
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List <string> theList = DeviceDetails.GetVideo();
            foreach (var item in theList)
            {
                Console.WriteLine(item);
            }
        }
    }
}