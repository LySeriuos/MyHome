using MyHome;
namespace My_Home

{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<UserProfile> list = TestData.User();
            //var x = GetDevicesCloseToWarrantyEnd(list[0]);
            foreach (UserProfile item in list)
            {
                Console.WriteLine(item);
                
            }
        }
        //static List<DevicesProfile> GetDevicesCloseToWarrantyEnd(UserProfile u)
        //{
        //    throw new NotImplementedException(); //
        //}
    }
}