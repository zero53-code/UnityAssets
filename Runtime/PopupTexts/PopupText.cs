using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
        /// 显示的文本
        /// </summary>
        public string text;

        [Min(0)] public float lodDistance = 50f;
        [Min(0)] public float showDistance = 100f;
        
        public Vector3 worldPosition { get; set; }
        
        private Canvas _popupTextCanvas;
        public Canvas popupTextCanvas
        {
            get => _popupTextCanvas;
            set
            {
                _popupTextCanvas = value;
                _canvasRect = _popupTextCanvas.GetComponent<RectTransform>();
            }
        }

        private RectTransform _canvasRect;
                
        /// <summary>
        /// 弹出方向
        /// </summary>
        private Vector3 _popupDir;

        // TextMeshProUGUI 组件
        private TextMeshProUGUI _text;

        private RectTransform _rect;

        private float _timer;

        private void OnEnable()
        {
            Setup();
        }
        
        private const int LODFrame = 10;
        
        private void Update()
        {
            // 获取跳字从初始化(调用 Setup 函数)开始到现在所过去的时间
            _timer += Time.deltaTime;
            
            if (_timer >= data.duration)
            {
                // 销毁(回收)跳字
                popupTextManager.Release(this);
                return;
            }
            
            var mainCamera = Camera.main;
            if (mainCamera == null) return;
            var distance = Vector3.Distance(worldPosition, mainCamera.transform.position);
            
            // 超过显示距离禁用 TextMeshProUGUI 组件
            _text.enabled = distance < showDistance;
            
            if (distance >= lodDistance)
            {
                if (Time.frameCount % LODFrame != 0) return;
            }

            _text.text = text;
            // 获取时间比例
            var normalizedTime = _timer / data.duration;

            var color = data.fontColor;
            // 颜色不透明度随时间变化
            color.a = data.fontColor.a * data.alphaCurve.Evaluate(normalizedTime);
            _text.color = color;

            // 位置随时间变化
            var speed = data.speedCurve.Evaluate(normalizedTime) * data.speed * _popupDir;
            worldPosition += speed * Time.deltaTime;
            
            WorldToCanvasPosition(mainCamera, _popupTextCanvas, worldPosition, out var uiPos);
            _rect.anchoredPosition = uiPos;
            
            var uiScale = CalculateProjectionScale(mainCamera, worldPosition);
            // 大小随时间变化
            _text.fontSize = data.fontSize * data.scaleCurve.Evaluate(normalizedTime) * uiScale;
        }

        /// <summary>
        /// 初始化跳字
        /// </summary>
        private void Setup()
        {
            if (data == null)
            {
                gameObject.SetActive(false);
                return;
            }

            if (_text == null) _text = GetComponent<TextMeshProUGUI>();
            if (_text == null) _text = gameObject.AddComponent<TextMeshProUGUI>();
            _rect = GetComponent<RectTransform>();
            
            _text.color = Color.clear;
            _text.fontSize = 0;

            // 随机位置
            var pos = transform.position;
            pos.x += Random.Range(-data.posRange.x, data.posRange.x);
            pos.y += Random.Range(-data.posRange.y, data.posRange.y);
            pos.z += Random.Range(-data.posRange.z, data.posRange.z);

            // 随机方向
            _popupDir = data.dir + new Vector3(
                Random.Range(-data.dirRange.x, data.dirRange.x),
                Random.Range(-data.dirRange.y, data.dirRange.y),
                Random.Range(-data.dirRange.z, data.dirRange.z));

            _timer = 0;
        }
        
        private static bool WorldToCanvasPosition(Camera camera, Canvas canvas, Vector3 worldPos, out Vector2 uiPos)
        {
            // 世界坐标转屏幕坐标
            var screenPos = camera.WorldToScreenPoint(worldPos);

            // 转 RectTransform/Canvas 坐标（适配 UI）
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                screenPos,
                canvas.worldCamera,
                out uiPos
            );
        }
        
        private static float CalculateProjectionScale(Camera camera, Vector3 worldPos)
        {
            var viewPos = camera.WorldToViewportPoint(worldPos);
            // 深度
            var depth = Mathf.Abs(viewPos.z);
            return 1f / depth;
        }
    }
}