using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace SkittysAsparagus
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += (s, e) => // Checking game launch
            {
                InjectConfigurableCrop(); // "Injecting" the custom croptype code
            };
        }

        private void InjectConfigurableCrop() // Injecting the crop code
        {
            var cropData = Helper.GameContent.Load<Dictionary<int, string>>("Data/Crops"); // Load the aforemention Data/Crops
            if (cropData.ContainsKey("SkittysAsparagus.AsparagusSeeds")) // Replace SkittysAsparagus.AsparagusSeeds with your actual seed ID
            {
                string[] cropInfo = cropData["SkittysAsparagus.AsparagusSeeds"].Split('/'); // Parsing and going through all the the data for the asparagus seeds
                string harvestSeason = cropInfo[4];
                int harvestItemID = int.Parse(cropInfo[5]);

                var cropDataReflection = Helper.Reflection.GetField<Dictionary<int, string>>(typeof(Crop), "cropData").GetValue(); // Gets value of the seeds cropdata for use later
                cropDataReflection["SkittysAsparagus.AsparagusSeeds"] = 
                cropDataReflection["SkittysAsparagus.AsparagusSeeds"].Replace("StardewValley.Crop",
                 $"SkittysAsparagus.ConfigurableCrop/{harvestSeason}/{harvestItemID}"); // Puts our custom croptype script in place of the normal one, for this case
                Helper.Reflection.GetMethod(typeof(Crop), "cropData").SetValue(cropDataReflection); // Setting that back into the code
            }
        }
    }
}