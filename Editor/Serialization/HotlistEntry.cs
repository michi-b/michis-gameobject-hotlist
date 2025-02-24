using System;
using System.IO;
using System.Text;
using Michis.GameObjectHotlist.Editor.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Michis.GameObjectHotlist.Editor.Serialization
{
    [Serializable]
    public struct HotlistEntry
    {
        [SerializeField] private string _scene; // scene name
        [SerializeField] private string[] _gameObjectHierarchyPath; // game object hierarchy path starting at a scene root game object
        [SerializeField] private bool _autoLoad; // on play mode scene load

        public bool AutoLoad => _autoLoad;
        public string Scene => _scene;
        public string[] GameObjectHierarchyPath => _gameObjectHierarchyPath;

        public HotlistEntry(GameObject gameObject)
        {
            _scene = gameObject.scene.name;
            _gameObjectHierarchyPath = GameObjectUtility.GetHierarchyPath(gameObject);
            _autoLoad = false;
        }

        public string GenerateFullPath()
        {
            StringBuilder pathStringBuilder = PathStringBuilder.Instance;

            pathStringBuilder.Clear();
            pathStringBuilder.Append(_scene);
            foreach (string path in _gameObjectHierarchyPath)
            {
                pathStringBuilder.Append('/');
                pathStringBuilder.Append(path);
            }

            return pathStringBuilder.ToString();
        }
    }
}