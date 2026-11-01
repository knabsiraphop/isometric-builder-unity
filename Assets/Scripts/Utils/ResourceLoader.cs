using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsoBuilder.Utils
{
    public static class ResourceLoader
    {
        public static async UniTask<T> LoadResourceAsync<T>(string resourcePath) where T : Object
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(resourcePath);
            await resourceRequest.ToUniTask();
            if (resourceRequest.asset == null)
            {
                Debug.LogError($"Resource not found at path: {resourcePath}");
                return null;
            }
            return resourceRequest.asset as T;
        }
    }
}
