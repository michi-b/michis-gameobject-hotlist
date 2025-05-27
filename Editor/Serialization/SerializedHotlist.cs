using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Michis.GameObjectHotlist.Editor.Serialization
{
    [Serializable]
    public class SerializedHotlist : IList<HotlistEntry>, IList
    {
        [SerializeField] private List<HotlistEntry> _items = new List<HotlistEntry>();

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
            var hotlist = JsonUtility.FromJson<SerializedHotlist>(json);

            if (!hotlist.GetIsValid())
            {
                Debug.LogWarning("Hotlist did not pass validation. Some entries may be invalid or missing. A new hotlist will be created.");
                return new SerializedHotlist();
            }

            return hotlist;
        }

        private bool GetIsValid()
        {
            if (_items == null)
            {
                return false;
            }

            foreach (HotlistEntry entry in _items)
            {
                if (entry == null || string.IsNullOrEmpty(entry.Scene) || entry.GameObjectHierarchyPath == null || entry.GameObjectHierarchyPath.Length == 0)
                {
                    return false;
                }

                if (entry.GameObjectHierarchyPath.Any(string.IsNullOrEmpty))
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetPath()
        {
            return Application.persistentDataPath + "/MichisGameObjectHotlist.json";
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(GetPath(), json);
        }

        #region List<HotlistEntry> Implementation

        public void Add(HotlistEntry hotlistEntry)
        {
            _items.Add(hotlistEntry);
            Save();
        }

        public int IndexOf(HotlistEntry item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, HotlistEntry item)
        {
            _items.Insert(index, item);
            Save();
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
            Save();
        }

        public HotlistEntry this[int index]
        {
            get => _items[index];
            set
            {
                _items[index] = value;
                Save();
            }
        }

        public List<HotlistEntry>.Enumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator<HotlistEntry> IEnumerable<HotlistEntry>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(HotlistEntry item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(HotlistEntry[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
            Save();
        }

        public bool Remove(HotlistEntry item)
        {
            bool removed = _items.Remove(item);
            if (removed)
            {
                Save();
            }

            return removed;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        #endregion

        #region IList Implementation

        void IList.Remove(object value)
        {
            Remove((HotlistEntry)value);
        }

        bool IList.IsFixedSize => false;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.Add(object value)
        {
            Add((HotlistEntry)value);
            return _items.Count - 1;
        }

        bool IList.Contains(object value)
        {
            return Contains((HotlistEntry)value);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((HotlistEntry)value);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (HotlistEntry)value);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo(array.Cast<HotlistEntry>().ToArray(), index);
        }

        bool ICollection.IsSynchronized => ((IList)_items).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)_items).SyncRoot;

        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = (HotlistEntry)value;
        }

        #endregion
    }
}