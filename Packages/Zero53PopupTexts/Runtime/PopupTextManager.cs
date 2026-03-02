using UnityEngine;
using UnityEngine.Pool;

namespace Zero53.PopupTexts
{
    /// <summary>
    /// 跳字管理器
    /// 跳字将在该组件挂在对象的 Canvas 上显示
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class PopupTextManager : MonoBehaviour
    {
        /// <summary>
        /// 跳字的游戏对象的预设体
        /// </summary>
        public GameObject popupTextPrefab;
        public GameObject popupTextContainer;
    
        /// <summary>
        /// 跳字对象池的初始数量
        /// </summary>
        [SerializeField] [Min(1)] private int poolInitSize = 5;

        [SerializeField] [Min(1)] private int poolMaxSize = 20;

        private ObjectPool<PopupText> _pool;

        private void Start()
        {
            _pool = new ObjectPool<PopupText>(
                Create,
                OnGet,
                OnRelease,
                null,
                true,
                poolInitSize,
                poolMaxSize);
        }

        /// <summary>
        /// 创建一个跳字对象
        /// </summary>
        /// <returns></returns>
        private PopupText Create()
        {
            var ptGo = new GameObject();
            
            var pt = ptGo.AddComponent<PopupText>();

            pt.popupTextManager = this;

            return pt;
        }

        private void OnGet(PopupText popupText)
        {
            popupText.popupTextManager = this;
        }

        private void OnRelease(PopupText popupText)
        {
            popupText.gameObject.SetActive(false);
        }

        /// <summary>
        /// 生成一个跳字对象
        /// </summary>
        /// <param name="data">跳字数据</param>
        /// <param name="text">跳字显示的文本</param>
        /// <param name="pos">跳字显示的位置</param>
        /// <returns>跳字组件对象</returns>
        public PopupText SpawnPopupText(PopupTextData data, string text, Vector3 pos)
        {
            var pt = _pool.Get();

            pt.data = data;
            pt.text = text;
            pt.transform.position = pos;

            pt.gameObject.SetActive(true);

            return pt;
        }

        /// <summary>
        /// 回收跳字对象
        /// </summary>
        /// <param name="pt">需要回收的跳字组件对象</param>
        public void Release(PopupText pt)
        {
            _pool.Release(pt);
        }
    }
}