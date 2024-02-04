using System;
using Xamarin.Essentials;

namespace AllergyTranslator.Helpers
{
    internal static class InternetChecker //https://learn.microsoft.com/en-us/xamarin/essentials/connectivity?tabs=android
    {
        public static bool CheckInternetAccess()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    // Connection to internet is available
                    return true;
                }
            }catch (Exception ex)
            {
                SqlLibrary.Log.LogIssue(ex.Message, nameof(CheckInternetAccess), SqlLibrary.IssueList.Settings);
            }

            return false;
        }

        //public InternetChecker()
        //{
        //    // Register for connectivity changes, be sure to unsubscribe when finished
        //    Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        //}
        //void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        //{
        //    var access = e.NetworkAccess;
        //    var profiles = e.ConnectionProfiles;
        //}
        //~InternetChecker() //destructor
        //{
        //    //unsubscribe on delete
        //    Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        //}
    }
}
