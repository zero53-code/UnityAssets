using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR && ODIN_INSPECTOR

namespace Zero53.Editor
{
    public static class TypeGetter
    {
        private static readonly Dictionary<Type, Dictionary<string, Type>> _typeMap = new();

        public static Dictionary<string, Type> Get(Type baseType)
        {
            if (_typeMap.TryGetValue(baseType, out var map)) return map;

            map = AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .SelectMany(asm =>
                    {
                        try
                        {
                            return asm.GetTypes();
                        }
                        catch
                        {
                            return Type.EmptyTypes;
                        }
                    })
                    .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t))
                    .ToDictionary(t => ObjectNames.NicifyVariableName(t.Name), t => t);
            
            _typeMap[baseType] = map;
            return map;
        }
    }
}

#endif