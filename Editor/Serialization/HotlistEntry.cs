using System;
using System.Text;
using Michis.GameObjectHotlist.Editor.Utility;
using UnityEngine;

namespace Michis.GameObjectHotlist.Editor.Serialization
{
    [Serializable]
    public class HotlistEntry : IEquatable<HotlistEntry>
    {
        [SerializeField] private string _scene; // scene name
        [SerializeField] private string[] _gameObjectHierarchyPath; // game object hierarchy path starting at a scene root game object
        [SerializeField] private bool _autoLoad; // on play mode scene load

        public bool AutoLoad
        {
            get => _autoLoad;
            set => _autoLoad = value;
        }

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

        public bool Equals(HotlistEntry other)
        {
            return other != null && _scene == other._scene && Equals(_gameObjectHierarchyPath, other._gameObjectHierarchyPath);
        }

        public override bool Equals(object obj)
        {
            return obj is HotlistEntry other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_scene != null ? _scene.GetHashCode() : 0) * 397) ^ (_gameObjectHierarchyPath != null ? _gameObjectHierarchyPath.GetHashCode() : 0);
            }
        }
    }
}