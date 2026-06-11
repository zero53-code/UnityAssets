using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Zero53.GameplayTags;
using Zero53.Utils;

namespace Zero53.GameplayFactions
{
    public class FactionSetting : ScriptableObject
    {
        [OnValueChanged("Setup")]
        public Tag factionParentTag;
        
        [SerializeField, ReadOnly, HideInInspector]
        private List<HostileList> factions;

#if UNITY_EDITOR
        
        [ShowInInspector] 
        [TableMatrix(SquareCells = true, Labels = "GetLabel", VerticalTitle = "Source", HorizontalTitle = "Target")] 
        [OnValueChanged("OnMatrixChanged", includeChildren: true)]
        internal bool[,] factionMatrix;

        [Button]
        [OnInspectorInit]
        internal void Setup()
        {
            factions ??= new List<HostileList>();
            var factionTags = GetFactionTags();

            if (factions.Count == factionTags.Length)
            {
                UpdateMatrix();
                return;
            }

            var tempFactions = new List<HostileList>();
            
            foreach (var hostileList in factions)
            {
                if (factionTags.Contains(hostileList.factionTag))
                {
                    tempFactions.Add(hostileList);
                }
            }

            foreach (var tag in factionTags)
            {
                if (factions.Count(hostileList => hostileList.factionTag == tag) == 0)
                {
                    var hostileList = new HostileList { factionTag = tag };
                    hostileList.Setup(factionTags);
                    tempFactions.Add(hostileList);
                }
            }
            
            factions = tempFactions;

            UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            if (factionMatrix == null || 
                factionMatrix.GetLength(0) != factions.Count ||
                factionMatrix.GetLength(1) != factions.Count)
            {
                factionMatrix = new bool[factions.Count, factions.Count];
            }
            
            for (var i = 0; i < factions.Count; i++)
            {
                for (var j = 0; j < factions.Count; j++)
                {
                    factionMatrix[i, j] = factions[i].list[j].isHostile;
                }
            }
        }

        private (string, LabelDirection) GetLabel(bool[,] array, TableAxis axis, int index)
        {
            var labelDirection = axis switch
            {
                TableAxis.X => LabelDirection.TopToBottom,
                TableAxis.Y => LabelDirection.BottomToTop,
                _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
            };
            
            
            return (GetFactionName(factions[index].factionTag), labelDirection);
        }

        private void OnMatrixChanged()
        {
            for (var i = 0; i < factions.Count; i++)
            {
                for (var j = 0; j < factions.Count; j++)
                {
                    if (factions[i].list[j].isHostile != factionMatrix[i, j])
                    {
                        Undo.RecordObject(this, "Change Matrix");
                    }
                    
                    factions[i].list[j].isHostile = factionMatrix[i, j];
                }
            }
        }
        
#endif

        private string GetFactionName(Tag factionTag)
        {
            if (!factionTag.IsChildOf(factionParentTag)) return string.Empty;

            return factionTag.ToString()[(factionParentTag.ToString().Length + 1)..];
        }

        private Tag[] GetFactionTags()
        {
            return Tag
                .tagLibrary
                .Query(t => t.IsChildOf(factionParentTag))
                .ToArray();
        }
        
        private int GetTagIndex(Tag tag)
        {
            return factions.FindIndex(x => x.factionTag == tag);
        }
        
        [Serializable]
        internal class HostileList
        {
            public Tag factionTag;
            public List<Hostile> list;

            public void Setup(Tag[] factionTags)
            {
                list ??= new List<Hostile>();
                var newList = new List<Hostile>();
                foreach (var hostile in list)
                {
                    if (factionTags.Contains(hostile.factionTag))
                    {
                        newList.Add(hostile);
                    }
                }

                foreach (var tag in factionTags)
                {
                    if (newList.Count(hostile => hostile.factionTag == tag) == 0)
                    {
                        newList.Add(new Hostile { factionTag = tag, isHostile = false });
                    }
                }
                
                newList.Sort((x, y) => x.factionTag.CompareTo(y.factionTag));
                
                list = newList;
            }
        }
        
        [Serializable]
        internal class Hostile
        {
            public Tag factionTag;
            public bool isHostile;
        }
    }
    
#if UNITY_EDITOR

    internal static class FactionSettingSettingProvider
    {
        private static FactionSetting _factionSetting;
        private static PropertyTree _propertyTree;

        [SettingsProvider]
        public static SettingsProvider CreateMyPluginSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Faction Setting", SettingsScope.Project)
            {
                label = "Faction Setting",
                guiHandler = _ =>
                {
                    var newFactionSetting = ScriptableObjectUtils.FindOnEditor<FactionSetting>();
                    
                    if (newFactionSetting != _factionSetting)
                    {
                        _factionSetting = newFactionSetting;
                        _propertyTree?.Dispose();
                        _propertyTree = PropertyTree.Create(_factionSetting);
                    }

                    _propertyTree.Draw();
                },
                deactivateHandler = () =>
                {
                    _propertyTree?.Dispose();
                }
            };
            
            return provider;
        }
    }

#endif
}