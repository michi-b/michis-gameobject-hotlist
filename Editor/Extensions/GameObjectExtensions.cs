using UnityEngine;

namespace Michis.GameObjectHotlist.Editor.Extensions
{
    public static class GameObjectExtensions
    {
        public static string GetFullPath(this GameObject child)
        {
            string path = child.name;
            Transform transform = child.transform;
            while (transform.parent != null)
            {
                transform = transform.parent;
                GameObject gameObject = transform.gameObject;
                path = gameObject.name + "/" + path;
            }

            return path;
        }
    }
}