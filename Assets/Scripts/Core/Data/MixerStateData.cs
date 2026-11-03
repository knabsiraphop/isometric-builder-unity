using System;

namespace IsoBuilder.Core
{
    [Serializable]
    public class MixerStateData
    {
        public int MixerRecipeId = -1;
        public long StartTimeStamp = 0;

        public void SetStateData(int recipeId, long timestamp)
        {
            MixerRecipeId = recipeId;
            StartTimeStamp = timestamp;
        }

        public bool IsActive() 
        {
            return StartTimeStamp != 0;
        }

        public void Clear()
        {
            MixerRecipeId = -1;
            StartTimeStamp = 0;
        }
    }
}
