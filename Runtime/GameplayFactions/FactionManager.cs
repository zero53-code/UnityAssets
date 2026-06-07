using System.Collections.Generic;
using UnityEngine;
using Zero53.GameplayTags;

namespace Zero53.GameplayFactions
{
    [DisallowMultipleComponent]
    public class FactionManager : MonoBehaviour
    {
        private Dictionary<Tag, Dictionary<Tag, bool>> _hostileMatrix;

        public void SetHostile(Tag sourceFaction, Tag targetFaction)
        {
            if (_hostileMatrix.TryGetValue(sourceFaction, out var dict))
            {
                dict[targetFaction] = true;
            }
            
            dict = new Dictionary<Tag, bool>
            {
                [targetFaction] = true
            };
            
            _hostileMatrix[sourceFaction] = dict;
        }

        public void SetNonHostile(Tag sourceFaction, Tag targetFaction)
        {
            if (_hostileMatrix.TryGetValue(sourceFaction, out var dict))
            {
                dict[targetFaction] = false;
            }
            
            dict = new Dictionary<Tag, bool>
            {
                [targetFaction] = false
            };
            
            _hostileMatrix[sourceFaction] = dict;
        }
        
        public bool IsHostile(Tag sourceFaction, Tag targetFaction)
        {
            if (!_hostileMatrix.TryGetValue(sourceFaction, out var dict)) return false;

            return dict.TryGetValue(targetFaction, out var isHostile) && isHostile;
        }
    }
}