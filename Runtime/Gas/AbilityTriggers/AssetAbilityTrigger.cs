using System;
using Object = UnityEngine.Object;

namespace Zero53.Gas.AbilityTriggers
{
    [Serializable]
    public class AssetAbilityTrigger : IAbilityTrigger
    {
        public AbilityTriggerAsset asset;

        private AbilityTriggerAsset _assetInstance;
        public bool Check(float deltaTime)
        {
            if (_assetInstance == null) _assetInstance = Object.Instantiate(asset);
            
            return _assetInstance.trigger.Check(deltaTime);
        }
    }
}