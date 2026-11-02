using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsoBuilder.Core
{
    public interface ISpriteLoader
    {
        public UniTask<Sprite> GetSprite(string path, string spriteKey);
        public void ClearAllCache();
    }
}
