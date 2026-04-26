using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Zero53.BehaviorTree.Components;
using Zero53.BehaviorTree.Components.ActionNodes;

#if UNITY_EDITOR
namespace Editor
{
    [InitializeOnLoad]
    public class BtExtendHierarchyView : MonoBehaviour
    {
        private static float _leftX;
        private static Texture2D _successIcon;
        private static Texture2D _failureIcon;
        private static Texture2D _runningIcon;

        static BtExtendHierarchyView()
        {
            _successIcon =
                AssetDatabase.LoadAssetAtPath<Texture2D>(
                    AssetDatabase.GUIDToAssetPath("284dc02443b2433ca7bf6fc095807c34"));
            _failureIcon =
                AssetDatabase.LoadAssetAtPath<Texture2D>(
                    AssetDatabase.GUIDToAssetPath("fbc1259302fa48f79506bde28739a39c"));
            _runningIcon =
                AssetDatabase.LoadAssetAtPath<Texture2D>(
                    AssetDatabase.GUIDToAssetPath("16dfaba0b0ad41878a79f28b636c1247"));
            
            EditorApplication.hierarchyWindowItemOnGUI += ModifyBackgroundColor;
            EditorApplication.hierarchyWindowItemOnGUI += DisplayStatus;
            EditorApplication.hierarchyWindowItemOnGUI += WaitProgressBar;
            EditorApplication.hierarchyWindowItemOnGUI += DisplayIcon;
        }

        private static void DisplayStatus(int instanceID, Rect selectionRect)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject go) return;

            var btNode = go.GetComponent<BtNode>();
            if (btNode == null) return;
            
            var rect = selectionRect;
            var width = EditorGUIUtility.singleLineHeight;
            var height = EditorGUIUtility.singleLineHeight;
            rect.x = selectionRect.x + selectionRect.width - width;
            rect.y = selectionRect.y;
            rect.width = width;
            rect.height = height;
            
            switch (btNode.status)
            {
                case BtNode.Status.Success:
                    GUI.Box(rect, _successIcon);
                    break;
                case BtNode.Status.Failure:
                    GUI.Box(rect, _failureIcon);
                    break;
                case BtNode.Status.Running:
                    GUI.Box(rect, _runningIcon);
                    break;
                case BtNode.Status.None:
                default:
                    break;
            }
        }
        
        private static void ModifyBackgroundColor(int instanceID, Rect selectionRect)
        {
            _leftX = _leftX <= Mathf.Epsilon 
                ? selectionRect.x 
                : Mathf.Min(selectionRect.x, _leftX);
            
            var rect = selectionRect;
            rect.width += selectionRect.x;
            rect.x = _leftX;

            // 获取物体
            var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (go == null) return;
            var btNode = go.GetComponent<BtNode>();
            if (btNode == null) return;
            
            var bgColor = GUI.backgroundColor;

            switch (btNode.status)
            {
                case BtNode.Status.Success:
                    bgColor = new Color(0, 1, 0, 0.1f);
                    break;
                case BtNode.Status.Failure:
                    bgColor = new Color(1, 0, 0, 0.1f);
                    break;
                case BtNode.Status.Running:
                    bgColor = new Color(1, 1, 1, 0.2f);
                    break;
                case BtNode.Status.None:
                default:
                    return;
            }
            EditorGUI.DrawRect(rect, bgColor);
        }

        /// <summary>
        /// 在 Hierarchy 中显示进度条
        /// </summary>
        private static void WaitProgressBar(int instanceID, Rect selectionRect)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject go) return;

            var btNode = go.GetComponent<BtNode>();
            if (btNode == null) return;
            if (btNode.status != BtNode.Status.Running) return;
            var progress = go.GetComponent<IProgress>();
            if (progress == null) return;

            var rect = selectionRect;
            const float progressBarHeight = 2f;
            rect.y = selectionRect.y + selectionRect.height - progressBarHeight;
            rect.height = progressBarHeight;
            
            var bgColor = new Color(0, 0, 0, 0.5f);
            EditorGUI.DrawRect(rect, bgColor);
            
            var fgColor = new Color(1, 1, 1, 0.5f);
            rect.width = rect.width * progress.progress;
            EditorGUI.DrawRect(rect, fgColor);
        }

        /// <summary>
        /// 在 Hierarchy 中显示图标
        /// </summary>
        private static void DisplayIcon(int instanceID, Rect selectionRect)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject go) return;

            var btNode = go.GetComponent<BtNode>();
            if (btNode == null) return;
            
            var rect = selectionRect;
            var width = EditorGUIUtility.singleLineHeight;
            var height = EditorGUIUtility.singleLineHeight;
            rect.x -= 30;
            rect.width = width;
            rect.height = height;
            
            var icon = AssetPreview.GetMiniThumbnail(btNode);
            if (icon == null) return;
            
            GUI.Label(rect, icon);
        }
    }
}
#endif