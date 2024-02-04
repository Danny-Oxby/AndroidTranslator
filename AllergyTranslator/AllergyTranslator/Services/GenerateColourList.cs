using AllergyTranslator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AllergyTranslator.Services
{
    internal class GenerateColourList
    {
        private static List<ColourModel> colours;

        public static List<ColourModel> GetColours()
        {
            if (colours == null)
            {
                colours = new List<ColourModel>
                {
                    new ColourModel() { ColourHex = Color.Red, ColourName = "Red" },
                    new ColourModel() { ColourHex = Color.Blue, ColourName = "Blue" },
                    new ColourModel() { ColourHex = Color.Green, ColourName = "Green" },
                    new ColourModel() { ColourHex = Color.Yellow, ColourName = "Yellow" },
                    new ColourModel() { ColourHex = Color.Orange, ColourName = "Orange" },
                    new ColourModel() { ColourHex = Color.Lavender, ColourName = "Lavender" },
                    new ColourModel() { ColourHex = Color.LightPink, ColourName = "LightPink" },
                    new ColourModel() { ColourHex = Color.LightBlue, ColourName = "LightBlue" },
                    new ColourModel() { ColourHex = Color.LightGreen, ColourName = "LightGreen" },
                    new ColourModel() { ColourHex = Color.Coral, ColourName = "Coral" },
                    new ColourModel() { ColourHex = Color.Orchid, ColourName = "Orchid" },
                    new ColourModel() { ColourHex = Color.LightGray, ColourName = "LightGray" },
                    //new ColourModel() { ColourHex = Color.Gray, ColourName = "Gray" },
                    //new ColourModel() { ColourHex = Color.Gold, ColourName = "Gold" },
                };
            }
            return colours;
        }
    }
}
