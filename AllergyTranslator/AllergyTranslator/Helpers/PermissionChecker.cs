using Amazon.S3.Model.Internal.MarshallTransformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AllergyTranslator.Helpers
{
    internal class PermissionChecker //https://learn.microsoft.com/en-us/xamarin/essentials/?context=xamarin%2Fxamarin-forms
    {
        public static async Task<bool> CheckCamera() // needed for OCR
        {
            try
            {
                PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status == PermissionStatus.Granted)
                {
                    return true; // access camera things
                }
                else
                {
                    //display a UI popup explaining the need of this permission

                    //Ask for the permission
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status == PermissionStatus.Granted)
                    {
                        return true;
                    }
                    else if(status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        return false;//tell the users to change their setting since IOS only allows one request
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch //has no camera perms
            { 
                return false;
            }
        }

        public static async Task<bool> CheckImages() // needed for OCR
        {
            try
            {
                PermissionStatus statusread = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                PermissionStatus statuswrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

                if (statusread == PermissionStatus.Granted && statuswrite == PermissionStatus.Granted)
                {
                    return true; // access image things
                }
                else
                {
                    //display a UI popup explaining the need of this permission

                    //Ask for the permission
                    statusread = await Permissions.RequestAsync<Permissions.StorageRead>();
                    statuswrite = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    if (statusread == PermissionStatus.Granted && statuswrite == PermissionStatus.Granted)
                    {
                        return true;
                    }
                    else if ((statusread == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS) ||
                        (statuswrite == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS))
                    {
                        return false;//tell the users to change their setting since IOS only allows one request
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch //has no storage perms
            {
                return false;
            }
        }

        public static bool CheckWiFi() // needed for translator //dosn't need to be a task
        {
            try
            {
                var profiles = Connectivity.ConnectionProfiles;

                if (profiles.Contains(ConnectionProfile.WiFi))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }catch //has no wifi perms
            {
                return false;
            }
        }
    }
}
