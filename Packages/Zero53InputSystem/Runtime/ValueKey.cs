using System;
using UnityEngine;

namespace Zero53.InputSystem
{
    [Serializable]
    public class ValueKey
    {
        public string name;
        public Vector2 range = new Vector2(0, 1);
        public float currValue = 0;
        public float addSpeed = 3f;
        public KeyCode keyCode;
        [HideInInspector] public bool enable = true;
    }
}