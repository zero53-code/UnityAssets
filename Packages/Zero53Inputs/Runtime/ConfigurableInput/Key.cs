using System;
using UnityEngine;

namespace Zero53.Inputs.ConfigurableInput
{
    [Serializable]
    public class Key
    {
        public string name;
        public KeyCode keyCode;
        public KeyCodeType keyType = KeyCodeType.Once;
        [HideInInspector] public bool isDown = false;
        [HideInInspector] public bool enable = true;
    }
}