using TMPro;
using UnityEngine;

namespace Zero53.PopupTexts
{
    /// <summary>
    /// 跳字组件
    /// </summary>
    public class PopupText : MonoBehaviour
    {
        /// <summary>
        /// 数据
        /// </summary>
        public PopupTextData data;

        /// <summary>
        /// PopupText 管理器引用
        /// </summary>
        public PopupTextManager popupTextManager;
        
        /// <summary>
        /// 弹出方向
        /// </summary>
        private Vector2 _popupDir;

        /// <summary>
        /// TextMeshProUGUI 组件
        /// </summary>
        private TextMeshProUGUI _text;

        /// <summary>
        /// 开始时间
        /// </summary>
        private float _startTime;

        /// <summary>
        /// 获取和设置显示的文本
        /// </summary>
        public string text
        {
            get => _text.text;
            set => _text.text = value;
        }

        /// <summary>
        /// 初始位置
        /// </summary>
        private Vector3 _originalPos;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        private void OnEnable()
        {
            Init();
        }

        /// <summary>
        /// 初始化跳字
        /// </summary>
        private void Init()
        {
            if (data == null)
            {
                gameObject.SetActive(false);
                return;
            }

            _text.color = data.fontColor;
            _text.fontSize = data.fontSize;

            var pos = transform.position;
            pos.x += Random.Range(-data.posRange, data.posRange);
            pos.y += Random.Range(-data.posRange, data.posRange);
            _originalPos = pos; //随机位置

            _popupDir = new Vector2(
                Random.Range(-data.dirRange.x, data.dirRange.x),
                Random.Range(0f, data.dirRange.y)); //随机方向

            _startTime = Time.time;
        }

        private void Update()
        {
            // 获取跳字从初始化(调用 Init 函数)开始到现在所过去的时间
            var passed = Time.time - _startTime;
            if (passed >= data.duration)
            {
                // 销毁(回收)跳字
                popupTextManager.Release(this);
                return;
            }

            // 获取时间比例
            var ratio = passed / data.duration;

            var color = data.fontColor;
            // 颜色不透明度随时间变化
            color.a = data.fontColor.a * data.alphaCurve.Evaluate(ratio);
            _text.color = color;

            // 位置随时间变化
            _text.transform.position =
                _originalPos + data.speedCurve.Evaluate(ratio) * passed * data.speed * (Vector3)_popupDir;

            // 大小随时间变化
            var scale = _text.transform.localScale;
            scale *= data.scaleCurve.Evaluate(ratio);
            _text.transform.localScale = scale;
        }
    }
}