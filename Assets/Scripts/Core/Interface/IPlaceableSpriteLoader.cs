using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsoBuilder.Core
{
    public interface IPlaceableSpriteLoader
    {
        public UniTask<Sprite> GetSprite(PlaceableData placeableData);
    }
}
