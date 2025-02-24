using System;
using System.Text;
using Michis.GameObjectHotlist.Editor.Serialization;
using Michis.GameObjectHotlist.Editor.Utility;

namespace Michis.GameObjectHotlist.Editor.Exceptions
{
    public class GameObjectInHierarchyPathNotFoundException : Exception
    {
        public GameObjectInHierarchyPathNotFoundException(HotlistEntry entry, int depth)
            : base($"GameObject in hierarchy path " +
                   $"{CreateGameObjectHierarchyPathSubString(entry.GameObjectHierarchyPath, depth)}" +
                   $" of scene {entry.Scene} not found.")
        {
        }

        private static string CreateGameObjectHierarchyPathSubString(string[] gameObjectHierarchyPath, int depth)
        {
            StringBuilder pathStringBuilder = PathStringBuilder.Instance;

            pathStringBuilder.Clear();

            pathStringBuilder.Append(gameObjectHierarchyPath[0]);

            for (int i = 1; i < depth; i++)
            {
                pathStringBuilder.Append('/');
                pathStringBuilder.Append(gameObjectHierarchyPath[i]);
            }

            return pathStringBuilder.ToString();
        }
    }
}