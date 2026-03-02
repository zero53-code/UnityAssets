using UnityEngine;

// 弹出文本数据
namespace Zero53.PopupTexts
{
    [CreateAssetMenu(menuName = "BaseBuild/PopupText/PopupTextData", fileName = "New PopupTextData")]
    public class PopupTextData : ScriptableObject
    {
        // 跳字显示的时间
        public float duration;

        // 跳字移动的方向
        public Vector2 dir;
        
        // 随机弹出位置范围
        public float posRange; 
        
        // 随机弹出结束位置范围
        public Vector2 dirRange; 
        
        // 跳字移动的基础速度
        public float speed;

        // 跳字的字体颜色
        public Color fontColor;

        // 跳字的字体大小
        public float fontSize;

        // 跳字透明度变化曲线
        public AnimationCurve alphaCurve;

        // 跳字速度的变化曲线
        public AnimationCurve speedCurve;

        // 跳字大小的变化曲线
        public AnimationCurve scaleCurve;
    }
}