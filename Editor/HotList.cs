using UnityEditor;

namespace Michis.GameObjectHotList.Editor
{
    public class HotList : EditorWindow
    {
        [MenuItem("Window/Michi's/HotList")]
        public static void ShowWindow()
        {
            GetWindow<HotList>("Hotbar");
        }
        
    }
}
