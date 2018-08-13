using UnityEngine;

namespace UnityRockStudio
{
    public class UVMapper : MonoBehaviour
    {
        public static void CreateUVMap(Mesh mesh, Transform transform)
        {
            //Mesh tempMesh = Instantiate(mesh);
            
                UVAlgorithm.BoxUV(mesh, transform);

              //  transform.GetComponent<MeshFilter>().sharedMesh = tempMesh;

           // DestroyImmediate(tempMesh);
            
        }
    }
}