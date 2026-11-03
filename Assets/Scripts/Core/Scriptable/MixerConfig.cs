using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "MixerConfig", menuName = "Scriptable Objects/MixerConfig")]
    public class MixerConfig : ScriptableObject
    {
        public MixerRecipeData[] MixerRecipeDatas;

        public bool TryFindRecipe(int id1, int id2, out MixerRecipeData recipe)
        {
            foreach (MixerRecipeData data in MixerRecipeDatas)
            {
                if (data.Ingredient1Id == id1 && data.Ingredient2Id == id2 || data.Ingredient1Id == id2 && data.Ingredient2Id == id1)
                {
                    recipe = data;
                    return true;
                }
            }
            recipe = default;
            return false;
        }

        public bool TryGetRecipeData(int recipeId, out MixerRecipeData result)
        {
            foreach (MixerRecipeData data in MixerRecipeDatas)
            {
                if (data.MixerRecipeId == recipeId)
                {
                    result = data;
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}
