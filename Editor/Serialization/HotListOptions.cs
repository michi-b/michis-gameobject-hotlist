using System;
using UnityEngine;

namespace Michis.GameObjectHotlist.Editor.Serialization
{
    [Serializable]
    public class HotListOptions
    {
        [SerializeField] private bool _display = false;
        [SerializeField] private bool _displayAutoLoadToggles = true;
        [SerializeField] private bool _displayRemoveEntryButtons = true;
        [SerializeField] private bool _isInLabelMode = false;

        public bool Display
        {
            get => _display;
            set => _display = value;
        }

        public bool DisplayAutoLoadToggles
        {
            get => _displayAutoLoadToggles;
            set => _displayAutoLoadToggles = value;
        }

        public bool DisplayRemoveEntryButtons
        {
            get => _displayRemoveEntryButtons;
            set => _displayRemoveEntryButtons = value;
        }

        public bool IsInLabelReorderMode
        {
            get => _isInLabelMode;
            set => _isInLabelMode = value;
        }
    }
}