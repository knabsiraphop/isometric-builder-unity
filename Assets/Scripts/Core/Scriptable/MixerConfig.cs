using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "MixerConfig", menuName = "Scriptable Objects/MixerConfig")]
    public class MixerConfig : ScriptableObject
    {
        public MixerRecipeData[] MixerRecipeDatas;
    }
}
