using System;
using UnityEngine.SceneManagement;

namespace Michis.GameObjectHotlist.Editor.Exceptions
{
    internal class SceneNotLoadedException : Exception
    {
        public SceneNotLoadedException(Scene scene)
            : base($"Scene {scene.name} is not loaded.")
        {
        }
    }
}