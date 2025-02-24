using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace Michis.GameObjectHotlist.Editor.Serialization
{
    [Serializable]
    public class SerializedHotlist : IEnumerable<HotlistEntry>
    {
        [SerializeField] private List<HotlistEntry> _items;

        public void Add(HotlistEntry hotlistEntry)
        {
            _items.Add(hotlistEntry);
            Save();
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
            Save();
        }

        public List<HotlistEntry>.Enumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator<HotlistEntry> IEnumerable<HotlistEntry>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _items.Clear();
            Save();
        }

        public static SerializedHotlist Load()
        {
            var configFile = new FileInfo(GetPath());

            if (!configFile.Exists)
            {
                return new SerializedHotlist();
            }

            string json = File.ReadAllText(configFile.FullName);
            return JsonUtility.FromJson<SerializedHotlist>(json);
        }

        private static string GetPath()
        {
            return Application.persistentDataPath + "/MichisGameObjectHotlist.json";
        }

        private void Save()
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(GetPath(), json);
        }
    }
}