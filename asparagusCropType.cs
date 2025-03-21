using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;

namespace SkittysAsparagus
{
    public class ConfigurableCrop : Crop
    {
        public string HarvestSeason { get; set; }
        public int HarvestItemID { get; set; }

        private int daysInCurrentStage = 0; // Track days in the current stage
        private int currentHardcodedPhase = 0; // Track the hardcoded growth phase

        public ConfigurableCrop(int seedIndex, int tileX, int tileY, string harvestSeason, int harvestItemID)
            : base(seedIndex, tileX, tileY)
        {
            HarvestSeason = harvestSeason;
            HarvestItemID = harvestItemID;
            UpdateDormantState();
        }

        public override Item harvest(int xTile, int yTile, HoeDirt soil) //creates a harvest item to make sure that the game can't harvest the crop properly if it isn't the given harvest season is active, and the crop is fully grown.
        {
            if (Game1.Date.Season.ToLower() == HarvestSeason.ToLower() && this.currentPhase >= this.phaseDays.Length) // if the given harvestSeason and crop is mature
            {
                return new StardewValley.Object(HarvestItemID, 1); // then receive the correct item for that stage of growth
            }
            else
            {
                return null; // else nothing
            }
        }

        public override void dayUpdate(GameLocation environment, Vector2 tileLocation) // Init the hardcoded stages which do seeds and stuff
        {
            daysInCurrentStage++; // Increment the day counter

            if (currentHardcodedPhase == 0) // Seed stage
            {
                if (daysInCurrentStage >= 3) // More than 3 or 3 days in the current stage.
                {
                    currentHardcodedPhase = 1; // Move to the next stage
                    daysInCurrentStage = 0; // Reset counter for the next stage
                }
            }
            else if (currentHardcodedPhase == 1) // Stage 1
            {
                if (daysInCurrentStage >= 4)
                {
                    currentHardcodedPhase = 2; // Move to the final hardcoded stage
                    daysInCurrentStage = 0; // Reset counter
                }
            }

            base.dayUpdate(environment, tileLocation); // Update the day
            UpdateSpriteIndex(); // Update the index into the spritesheet for today
        }

        private void UpdateSpriteIndex() // This is what actually updates the current sprite
        {
            if (currentHardcodedPhase == 0) // Hardcoded stuff
            {
                this.rowInSpriteSheet = 0; // Seed stage
            }
            else if (currentHardcodedPhase == 1)
            {
                this.rowInSpriteSheet = 1; // Stage 1
            }
            else // After hardcoded phases
            {
                // Use the currentPhase from the Crop class to determine the sprite index
                this.rowInSpriteSheet = this.currentPhase;
            }
        }


        private void UpdateDormantState() // What to do while in dormant state
        {
            if (Game1.Date.Season.ToLower() != HarvestSeason.ToLower()) // Compares if the current season is not the harvest season to see if the crop should be dormant, CurrentPhase = 0, and .ToLower is to make sure both are properly comparable.
            {
                CurrentPhase = 0; // Set to dormant
            }
            else
            {
                // Ensure the crop is past the hardcoded phases if it's the right season
                if (currentHardcodedPhase < 2)
                {
                    currentHardcodedPhase = 2; // Sets to "start using user growth stages"
                }
            }
        }
    }
}