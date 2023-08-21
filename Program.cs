using MyHome;
namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<UserProfile> list = TestData.User();
           foreach (UserProfile item in list)
            {
                
                Console.WriteLine(item.UserName + item.Email);
            }
        }
    }
}