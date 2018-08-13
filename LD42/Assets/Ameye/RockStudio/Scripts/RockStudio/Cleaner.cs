using UnityEngine;

namespace UnityRockStudio
{
    public class Cleaner : MonoBehaviour
    {
        public static void RemoveGameObject(GameObject gameobject)
        {
            DestroyImmediate(gameobject);
        }

        public static void RemoveGameObject(string name)
        {
            if (GameObject.Find(name) != null) DestroyImmediate(GameObject.Find(name));
        }
    }
}
