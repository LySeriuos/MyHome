using My_Home;
using MyHome;

internal static class DeviceDetailsHelpers
{

    public static void GetDetails(DeviceType deviceType)
    {

        switch (deviceType)
        {
            case DeviceType.Audio:
                DeviceDetails.GetAudio();
                break;

            case DeviceType.Video:
                DeviceDetails.GetVideo();
                break;

            case DeviceType.Computer:
                DeviceDetails.GetComputer();
                break;
            case DeviceType.MobileDevice:
                DeviceDetails.GetMobileDevice();
                break;
            case DeviceType.Kitchen:
                DeviceDetails.GetKitchen();
                break;
            case DeviceType.Bathroom:
                DeviceDetails.GetBathroom();
                break;
            case DeviceType.Garden:
                DeviceDetails.GetGarden();
                break;
            case DeviceType.Cleaning:
                DeviceDetails.GetCleaning();
                break;
            case DeviceType.Security:
                DeviceDetails.GetSecurity();
                break;
            case DeviceType.Other:
                DeviceDetails.GetOtherDevices();
                break;
            case DeviceType.MultiDevice:
                DeviceDetails.GetMultiDevices();
                break;
        }
    }
}