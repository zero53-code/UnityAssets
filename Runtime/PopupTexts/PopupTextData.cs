using UnityEngine;

namespace Zero53.PopupTexts
{
    [CreateAssetMenu(menuName = "Zero53/PopupText/PopupTextData", fileName = "New PopupTextData")]
    public class PopupTextData : ScriptableObject
    {
        /// <summary>
        /// 跳字显示的时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 跳字移动的方向
        /// </summary>
        public Vector3 dir;
        
        /// <summary>
        /// 随机弹出结束位置范围
        /// </summary>
        public Vector3 dirRange; 
        
        /// <summary>
        /// 随机弹出位置范围
        /// </summary>
        public Vector3 posRange; 
        
        /// <summary>
        /// 跳字移动的基础速度
        /// </summary>
        public float speed;

        /// <summary>
        /// 跳字的字体颜色
        /// </summary>
        public Color fontColor = Color.white;

        /// <summary>
        /// 跳字的字体大小
        /// </summary>
        public float fontSize = 5f;

        /// <summary>
        /// 跳字透明度变化曲线
        /// </summary>
        public AnimationCurve alphaCurve = AnimationCurve.Linear(0, 0, 1, 1);

        /// <summary>
        /// 跳字速度的变化曲线
        /// </summary>
        public AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1);

        /// <summary>
        /// 跳字大小的变化曲线
        /// </summary>
        public AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}