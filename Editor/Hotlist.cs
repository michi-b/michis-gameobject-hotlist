using Michis.GameObjectHotlist.Editor.Serialization;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using GameObjectUtility = Michis.GameObjectHotlist.Editor.Utility.GameObjectUtility;

namespace Michis.GameObjectHotlist.Editor
{
    public class Hotlist : EditorWindow
    {
        private SerializedHotlist _hotlist;
        private ReorderableList _reorderableHotlist;

        private bool _isInitialized = false;
        private float _toggleWidth;

        [MenuItem("Window/Michi's/GameObject Hotlist")]
        public static void ShowWindow()
        {
            GetWindow();
        }

        private static Hotlist GetWindow()
        {
            var window = GetWindow<Hotlist>("GameObject Hotlist");
            window.Initialize();
            return window;
        }

        [MenuItem("GameObject/Add to Hotlist %h", false, 1)]
        public static void AddToHotlist()
        {
            var gameObject = Selection.activeObject as GameObject;
            Debug.Assert(gameObject);
            Hotlist window = GetWindow();
            window._hotlist.Add(new HotlistEntry(gameObject));
        }

        [MenuItem("GameObject/Add to Hotlist %h", true, 1)]
        private static bool ValidateAddToHotlist()
        {
            var gameObject = Selection.activeObject as GameObject;
            return gameObject;
        }

        protected void OnGUI()
        {
            Initialize();

            _reorderableHotlist.DoLayoutList();

            if (GUILayout.Button("Clear"))
            {
                _hotlist.Clear();
            }
        }

        private void Initialize()
        {
            if (!_isInitialized)
            {
                _toggleWidth = GUI.skin.toggle.CalcSize(AutoLoadLabel).x;

                _hotlist = SerializedHotlist.Load();

                _isInitialized = true;
            }

            _reorderableHotlist ??= new ReorderableList(_hotlist, typeof(HotlistEntry), true, false, false, true)
            {
                elementHeight = EditorGUIUtility.singleLineHeight,
                drawElementCallback = Draw,
                onRemoveCallback = list =>
                {
                    _hotlist.RemoveAt(list.index);
                    _hotlist.Save();
                }
            };
        }

        private void Draw(Rect rect, int index, bool isActive, bool isFocused)
        {
            // GUILayout.BeginArea(rect);
            try
            {
                HotlistEntry entry = _hotlist[index];

                bool canFindGameObject = GameObjectUtility.CanFindGameObject(entry);

                // using var horizontalScope = new EditorGUILayout.HorizontalScope();

                string label = entry.GenerateFullPath();

                if (canFindGameObject)
                {
                    if (GUI.Button(rect, label))
                    {
                        Select(entry);
                    }
                    // if (GUILayout.Button(label))
                    // {
                    //     Select(entry);
                    // }
                }
                else
                {
                    Color guiColor = GUI.color;
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField(label);
                    GUI.color = guiColor;
                }

                // EditorGUI.BeginChangeCheck();
                // entry.AutoLoad = GUILayout.Toggle(entry.AutoLoad, AutoLoadLabel, GUILayout.Width(_toggleWidth));
                // if (EditorGUI.EndChangeCheck())
                // {
                //     _hotlist.Save();
                // }
            }
            finally
            {
                // GUILayout.EndArea();
            }
        }

        private static void Select(HotlistEntry entry)
        {
            GameObject target = GameObjectUtility.FindGameObject(entry);
            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
        }

        private static readonly GUIContent AutoLoadLabel = new GUIContent(string.Empty, "Auto load this GameObject on play mode scene load");
    }
}