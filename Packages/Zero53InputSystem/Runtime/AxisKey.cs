using System;
using UnityEngine;

namespace Zero53.InputSystem
{
    [Serializable]
    public class AxisKey
    {
        public string name;
        public float value = 0;
        public float addSpeed = 5;
        public Vector2 range = new Vector2(-1, 1);
        public KeyCode min, max;
        [HideInInspector] public bool enable = true;

        public void SetKey(KeyCode a, KeyCode b)
        {
            min = a;
            max = b;
        }
    }
}