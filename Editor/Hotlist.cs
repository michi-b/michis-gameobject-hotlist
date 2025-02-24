using System;
using System.Collections.Generic;
using System.Linq;
using Michis.GameObjectHotlist.Editor.Exceptions;
using Michis.GameObjectHotlist.Editor.Extensions;
using Michis.GameObjectHotlist.Editor.Serialization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using GameObjectUtility = Michis.GameObjectHotlist.Editor.Utility.GameObjectUtility;
using Scene = UnityEngine.SceneManagement.Scene;
using Toggle = UnityEngine.UIElements.Toggle;

namespace Michis.GameObjectHotlist.Editor
{
    public class Hotlist : EditorWindow
    {
        private SerializedHotlist _hotlist;

        [MenuItem("Window/Michi's/GameObject Hotlist")]
        public static void ShowWindow()
        {
            GetWindow();
        }

        private static Hotlist GetWindow()
        {
            return GetWindow<Hotlist>("GameObject Hotlist");
        }

        [MenuItem("GameObject/Wow")]
        public static void Wow()
        {
            Debug.Log("Wow");
        }

        [MenuItem("GameObject/Wow", true)]
        private static bool ValidateWow()
        {
            return false;
        }

        [MenuItem("GameObject/Add to Hotlist %h", false, 1)]
        public static void AddToHotlist()
        {
            var gameObject = Selection.activeObject as GameObject;
            Debug.Assert(gameObject != null);
            Hotlist window = GetWindow();
            window._hotlist.Add(new HotlistEntry(gameObject));
        }

        [MenuItem("GameObject/Add to Hotlist %h", true, 1)]
        private static bool ValidateAddToHotlist()
        {
            var gameObject = Selection.activeObject as GameObject;
            return gameObject != null;
        }

        protected virtual void Awake()
        {
            _hotlist = SerializedHotlist.Load();
        }

        protected void OnGUI()
        {
            foreach (HotlistEntry hotlistEntry in _hotlist) Draw(hotlistEntry);

            if (GUILayout.Button("Clear"))
            {
                _hotlist.Clear();
            }
        }

        private static void Draw(HotlistEntry hotlistEntry)
        {
            bool canFindGameObject = GameObjectUtility.CanFindGameObject(hotlistEntry);

            using var horizontalScope = new EditorGUILayout.HorizontalScope();

            string label = hotlistEntry.GenerateFullPath();

            if (canFindGameObject)
            {
                if (GUILayout.Button(label))
                {
                    Select(hotlistEntry);
                }
            }
            else
            {
                Color guiColor = GUI.color;
                GUI.color = Color.red;
                EditorGUILayout.LabelField(label);
                GUI.color = guiColor;
            }
        }

        private static void Select(HotlistEntry entry)
        {
            GameObject target = GameObjectUtility.FindGameObject(entry);
            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
        }
    }
}