using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AllergyTranslator.Services
{
    internal static class ForceCloseService
    {
        public static void CloseApplication()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
