using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class AddressableManager : Manager
{
    public override void Init()
    {
    }

    public override void Begin()
    {
        Resolve();
    }
    
    public async Task<T> LoadAsset<T>(string label)
    {
        return await Addressables.LoadAssetAsync<T>(label).Task;
    }
}