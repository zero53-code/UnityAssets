using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zero53.InputSystem
{
    [CreateAssetMenu(fileName = "New InputData", menuName = "BaseBuild/InputSystem/InputData")]
    public class InputData : ScriptableObject
    {
        public List<Key> keys = new List<Key>() { new Key() };
        public List<ValueKey> valueKeys = new List<ValueKey>() { new ValueKey() };
        public List<AxisKey> axisKeys = new List<AxisKey>() { new AxisKey() };

        public void SetKey(string keyName, KeyCode key)
        {
            var key1 = GetKey(keyName);
            if (key1 != null)
            {
                key1.keyCode = key;
            }
        }

        public void SetAxisKey(string keyName, KeyCode a, KeyCode b)
        {
            var axisKey = GetAxisKey(keyName);
            if (axisKey != null)
            {
                axisKey.SetKey(a, b);
            }
        }

        public void SetValueKey(string keyName, KeyCode key)
        {
            var valueKey = GetValueKey(keyName);
            if (valueKey != null)
            {
                valueKey.keyCode = key;
            }
        }

        public ValueKey GetValueKey(string keyName)
        {
            var len = valueKeys.Count;
            for (var i = 0; i < len; i++)
            {
                if (valueKeys[i].name == keyName)
                {
                    return valueKeys[i];
                }
            }

            Debug.LogError("ValueKey:" + keyName + "不存在");
            return null;
        }

        public AxisKey GetAxisKey(string keyName)
        {
            var len = axisKeys.Count;
            for (var i = 0; i < len; i++)
            {
                if (axisKeys[i].name == keyName)
                {

                    return axisKeys[i];
                }
            }

            Debug.LogError("AxisKey:" + keyName + "不存在");
            return null;
        }

        public Key GetKey(string keyName)
        {
            var len = keys.Count;
            for (var i = 0; i < len; i++)
            {
                if (keys[i].name == keyName)
                {
                    return keys[i];
                }
            }

            Debug.LogError("Key:" + keyName + "不存在");
            return null;
        }

        public float Axis(string keyName)
        {
            var axisKey = GetAxisKey(keyName);
            if (axisKey != null)
            {
                return axisKey.value;
            }

            return 0;
        }

        public bool GetKeyDown(string keyName)
        {
            var key = GetKey(keyName);
            if (key != null)
            {
                return key.isDown;
            }

            return false;
        }

        public float GetValue(string keyName)
        {
            var valueKey = GetValueKey(keyName);
            if (valueKey != null)
            {
                return valueKey.currValue;
            }

            return 0;
        }

        public void SetKeyEnable(string keyName, bool enable)
        {
            var key = GetKey(keyName);
            if (key != null)
            {
                key.enable = enable;
                key.isDown = false;
            }
        }

        public void SetValueKeyEnable(string keyName, bool enable)
        {
            var valueKey = GetValueKey(keyName);
            if (valueKey != null)
            {
                valueKey.enable = enable;
                valueKey.currValue = 0;
            }
        }

        public void SetAxisKeyEnable(string keyName, bool enable)
        {
            var axisKey = GetAxisKey(keyName);
            if (axisKey != null)
            {
                axisKey.enable = enable;
                axisKey.value = 0;
            }
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void AcceptInput()
        {
            UpdateKeys();
            UpdateValueKey();
            UpdateAllAxisKey();
        }

        private void UpdateKeys()
        {
            foreach (var t in keys.Where(t => t.enable))
            {
                t.isDown = false;
                switch (t.keyType)
                {
                    case KeyCodeType.Once:
                        if (Input.GetKeyDown(t.keyCode))
                        {
                            t.isDown = true;
                        }

                        break;
                    case KeyCodeType.Continuity:
                        if (Input.GetKey(t.keyCode))
                        {
                            t.isDown = true;
                        }

                        break;
                }
            }
        }

        private void UpdateValueKey()
        {
            var len = valueKeys.Count;
            for (var i = 0; i < len; i++)
            {
                if (valueKeys[i].enable)
                {
                    if (Input.GetKey(valueKeys[i].keyCode))
                    {
                        valueKeys[i].currValue =
                            Mathf.Clamp(valueKeys[i].currValue + valueKeys[i].addSpeed * Time.deltaTime,
                                valueKeys[i].range.x, valueKeys[i].range.y);
                    }
                    else
                    {
                        valueKeys[i].currValue =
                            Mathf.Clamp(valueKeys[i].currValue - valueKeys[i].addSpeed * Time.deltaTime,
                                valueKeys[i].range.x, valueKeys[i].range.y);
                    }

                }

            }
        }

        private void UpdateAllAxisKey()
        {
            var len = axisKeys.Count;
            for (var i = 0; i < len; i++)
            {
                UpdateAxisKey(axisKeys[i]);

            }
        }

        private void UpdateAxisKey(AxisKey axisKey)
        {
            if (!axisKey.enable)
                return;
            if (Input.GetKey(axisKey.min) || Input.GetKey(axisKey.max))
            {
                if (Input.GetKey(axisKey.min))
                    axisKey.value = Mathf.Clamp(axisKey.value - axisKey.addSpeed * Time.deltaTime, axisKey.range.x,
                        axisKey.range.y);
                if (Input.GetKey(axisKey.max))
                    axisKey.value = Mathf.Clamp(axisKey.value + axisKey.addSpeed * Time.deltaTime, axisKey.range.x,
                        axisKey.range.y);
            }
            else if (Input.GetKey(axisKey.min) && Input.GetKey(axisKey.max))
            {
                axisKey.value = 0;
            }
            else
            {
                axisKey.value = Mathf.Lerp(axisKey.value, 0, Time.deltaTime * axisKey.addSpeed);
            }
        }
    }
}