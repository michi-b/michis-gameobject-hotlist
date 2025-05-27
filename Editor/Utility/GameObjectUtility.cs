using System;
using System.Collections.Generic;
using System.Linq;
using Michis.GameObjectHotlist.Editor.Exceptions;
using Michis.GameObjectHotlist.Editor.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michis.GameObjectHotlist.Editor.Utility
{
    public static class GameObjectUtility
    {
        public static bool CanFindGameObject(HotlistEntry entry)
        {
            Scene scene = SceneManager.GetSceneByName(entry.Scene);

            if (!scene.isLoaded)
            {
                return false;
            }

            string[] gameObjectHierarchyPath = entry.GameObjectHierarchyPath;

            Debug.Assert(gameObjectHierarchyPath != null);
            Debug.Assert(gameObjectHierarchyPath.Length > 0);

            GameObject root = scene.GetRootGameObjects().FirstOrDefault(go => go.name == gameObjectHierarchyPath[0]);

            if (root == null)
            {
                return false;
            }

            Transform current = root.transform;

            for (int gameObjectHierarchyPathIndex = 1; gameObjectHierarchyPathIndex < gameObjectHierarchyPath.Length; gameObjectHierarchyPathIndex++)
            {
                string currentGameObjectName = gameObjectHierarchyPath[gameObjectHierarchyPathIndex];
                bool gameObjectFoundAtCurrentDepth = false;
                for (int childIndex = 0; childIndex < current.childCount; childIndex++)
                {
                    GameObject currentGameObject = current.GetChild(childIndex).gameObject;
                    if (currentGameObject.name == currentGameObjectName)
                    {
                        current = currentGameObject.transform;
                        gameObjectFoundAtCurrentDepth = true;
                        break;
                    }
                }

                if (!gameObjectFoundAtCurrentDepth)
                {
                    return false;
                }
            }

            return true;
        }

        public static GameObject FindGameObject(HotlistEntry entry)
        {
            Scene scene = SceneManager.GetSceneByName(entry.Scene);

            if (!scene.isLoaded)
            {
                throw new SceneNotLoadedException(scene);
            }

            string[] gameObjectHierarchyPath = entry.GameObjectHierarchyPath;

            Debug.Assert(gameObjectHierarchyPath != null);
            Debug.Assert(gameObjectHierarchyPath.Length > 0);

            GameObject root = scene.GetRootGameObjects().First(go => go.name == gameObjectHierarchyPath[0]);

            if (!root)
            {
                throw new GameObjectInHierarchyPathNotFoundException(entry, 0);
            }

            Transform current = root.transform;

            for (int depth = 1; depth < gameObjectHierarchyPath.Length; depth++)
            {
                string currentGameObjectName = gameObjectHierarchyPath[depth];
                bool gameObjectFoundAtCurrentDepth = false;
                for (int childIndex = 0; childIndex < current.childCount; childIndex++)
                {
                    GameObject currentGameObject = current.GetChild(childIndex).gameObject;
                    if (currentGameObject.name == currentGameObjectName)
                    {
                        current = currentGameObject.transform;
                        gameObjectFoundAtCurrentDepth = true;
                        break;
                    }
                }

                if (!gameObjectFoundAtCurrentDepth)
                {
                    throw new GameObjectInHierarchyPathNotFoundException(entry, depth);
                }
            }

            return current.gameObject;
        }

        public static string[] GetHierarchyPath(GameObject gameObject)
        {
            var stack = new Stack<string>(256);

            stack.Push(gameObject.name);

            Transform current = gameObject.transform;

            while (current.parent != null)
            {
                current = current.parent;
                stack.Push(current.gameObject.name);
            }

            return stack.ToArray();
        }
    }
}