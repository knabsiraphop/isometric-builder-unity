using System;

namespace IsoBuilder.Core
{
    [Serializable]
    public struct MixerRecipeData
    {
        public int MixerRecipeId;
        public int ResultId;
        public int Ingredient1Id;
        public int Ingredient2Id;
        public long duration;
    }
}
