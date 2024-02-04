using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AllergyTranslator.Helpers
{
    internal static class AccessPhoto // https://learn.microsoft.com/en-us/xamarin/essentials/media-picker?tabs=ios
    {
        private static string PhotoPath { get; set; }
        public static async Task<string> TakePicture()
        {
            if (await PermissionChecker.CheckCamera())
            {
                try //this will take some time to load
                {
                    FileResult photo = await MediaPicker.CapturePhotoAsync();
                    await LoadPhotoAsync(photo);
                }
                //catch (FeatureNotSupportedException fnsEx)
                //{
                //    //Feature is not supported on the device
                //    return "Something went wrong";
                //}
                catch (Exception ex)
                {
                    //log the issue
                    SqlLibrary.Log.LogIssue(ex.Message, nameof(TakePicture), SqlLibrary.IssueList.Usability);
                    return "Something went wrong";
                }
                return PhotoPath; //return the photo path
            }
            else
            {
                return "Camera permissions are needed";
            }
        }

        public static string LoadLastPhoto()
        {
            return PhotoPath;
        }

        private static async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            if(PhotoPath!= null)
            {
                File.Delete(PhotoPath); // clear the current photo from the cashe
            }
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }
    }
}
