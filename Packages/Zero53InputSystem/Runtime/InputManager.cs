using System;
using UnityEngine;

namespace Zero53.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        // public string path = "New Input Data";
        public InputData inputData;
        [SerializeField] [HideInInspector] private bool activeSetKey;
        private Action<KeyCode> _setKey;

        private void Awake()
        {
            // _inputData = Resources.Load<InputData>(path);
            if (inputData == null)
            {
                throw new Exception("inputData:无法加载");
            }
        }

        public void OnGUI()
        {
            if (!activeSetKey)
                return;
            var e = Event.current;
            
            if (!e.isKey || _setKey == null) return;
            
            var key = e.keyCode;
            Debug.Log(key);
            _setKey(key);
            _setKey = null;
            activeSetKey = false;

        }

        private void Update()
        {
            inputData.AcceptInput();
        }

        public void StartSetKey(Action<KeyCode> key)
        {
            _setKey = key;
            activeSetKey = true;
        }

        public float GetAxis(string keyName)
        {
            return inputData.Axis(keyName);
        }

        public bool GetKeyDown(string keyName)
        {
            return inputData.GetKeyDown(keyName);
        }

        public float GetValueKey(string keyName)
        {
            return inputData.GetValue(keyName);
        }

        public void SetKey(string keyName, KeyCode key)
        {
            inputData.SetKey(keyName, key);
        }

        public void SetValueKey(string keyName, KeyCode key)
        {
            inputData.SetValueKey(keyName, key);
        }

        public void SetAxisKey(string keyName, KeyCode a, KeyCode b)
        {
            inputData.SetAxisKey(keyName, a, b);
        }
    }
}