using System.Text;

namespace Michis.GameObjectHotlist.Editor.Utility
{
    public static class PathStringBuilder
    {
        public static StringBuilder Instance { get; } = new StringBuilder(256);
    }
}