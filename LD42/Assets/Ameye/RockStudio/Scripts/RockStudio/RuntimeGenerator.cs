using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityRockStudio
{

    public class RuntimeGenerator : MonoBehaviour
    {
        //TODO: move all rock settings to here so you have all the variables you can set, and then
        //In another script, you call Generate() and everything gets done, for frequent spawning, that has
        //to happen in the other script, this script just functions as a 'spawner'.
        public bool includeRigidBody = true;
        GameObject rock;
        string TemporaryRockName = "TempRock";
        public bool GenerateUV;
        public List<Vector3> VertexList;

        public enum MethodOfSeed { Random, Custom }
        public static MethodOfSeed SeedMethod;

        public static int Seed;
        public static int SeedSize = 5000;
        public enum ColliderType { Mesh, Box, None }
        public ColliderType TypeOfCollider;
        public Vector3 pos = new Vector3(0, 0, 0);

        public enum RockType
        {
            Cubic,
            Tetragonal,
            TriclinicAndObelisk,
            Orthorombic,
            Hexagonal,
            MonoclinicAndTrigonal,
            Octahedral,
            Spherical,
            Quartz,
            Pyramidal,
        }
        public RockType TypeOfRock;

        public float Length = 1;

        public Vector3 TetragonalScale = new Vector3(1, 1, 1);
        public float PointSmoothing = 0.1f;
        public float TetragonalPointLength = 0.5f;

        public Vector3 TriclinicScale = new Vector3(1, 1, 1);
        public float TriclinicEdgeHeigth = 0.5f;
        public float TriclinicEdgeLength = 0.1f;
        public float TriclinicEdgeSmoothing = 0.1f;

        public Vector3 Scale = new Vector3(1, 1, 1);

        public Vector3 HexagonalScale = new Vector3(1, 1, 1);
        public float HexagonalEdgeWidth = 0.1f;

        public Vector3 TrigonalScale = new Vector3(1, 1, 1);
        public float TrigonalEdgeSmoothing = 0.1f;
        public float TrigonalEdgePosition = 0.75f;

        public Vector3 OctahedralScale = new Vector3(1, 1, 1);
        public float OctahedralPointPosition = 1f;
        public float OctahedralPointSmoothing = 0.1f;

        public Vector3 QuartzScale = new Vector3(1, 1, 1);
        public float QuartzEdgeWidth = 0.1f;
        public float QuartzPointPosition = 2f;

        public float Radius = 1;

        public float PyramidalBaseScale = 1;
        public float PyramidalVertexPortrusion = 2;
        public float PyramidalVertexSmoothing = 0.1f;
        public enum RockOrientation { X, Y, Z }
        public RockOrientation OrientationOfRock;
        public enum LowPolyGrade
        {
            LowPoly,
            MediumPoly,
            HighPoly,
            Custom
        }
        public LowPolyGrade LowPoly;
        public enum ShadingType { Flat, Smooth }
        public ShadingType TypeOfShading;
        public int NumberOfVertices = 250;

        public enum TypeOfMaterial { SingleMaterial, RandomFromCollection }
        public TypeOfMaterial MaterialType;
        public Material[] Materials;
        public static Material RockMaterial;
        public Material rockmat;

        public void Generate()
        {
            GenerateRock(NumberOfVertices, pos);
        }

        public void GenerateRock(int NumberOfVertices, Vector3 pos)
        {
            VertexList = new List<Vector3>(NumberOfVertices);
            VertexList.Clear();

            if (SeedMethod == MethodOfSeed.Random)
            {
                Random.state = Random.state;
                Seed = Random.Range(0, SeedSize);
                Random.InitState(Seed);
               
            }
            else Random.InitState(Seed);


            if (TypeOfRock == RockType.Cubic)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-Length * 0.5f, Length * 0.5f), Random.Range(-Length * 0.5f, Length * 0.5f), Random.Range(-Length * 0.5f, Length * 0.5f)));
            }

            if (TypeOfRock == RockType.Tetragonal)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TetragonalScale.x * 0.5f, TetragonalScale.x * 0.5f), Random.Range(-TetragonalScale.y * 0.5f, TetragonalScale.y * 0.5f), Random.Range(-TetragonalScale.z * 0.5f, TetragonalScale.z * 0.5f)));
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TetragonalScale.x * 0.5f * PointSmoothing, TetragonalScale.x * 0.5f * PointSmoothing), Random.Range(-TetragonalScale.y * 0.5f * PointSmoothing, TetragonalScale.y * 0.5f * PointSmoothing), Random.Range(-TetragonalScale.z * 0.5f - TetragonalPointLength, TetragonalScale.z * 0.5f + TetragonalPointLength)));
            }

            if (TypeOfRock == RockType.TriclinicAndObelisk)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TriclinicScale.x * 0.5f, TriclinicScale.x * 0.5f), Random.Range(-TriclinicScale.y * 0.5f, TriclinicScale.y * 0.5f), Random.Range(-TriclinicScale.z * 0.5f, TriclinicScale.z * 0.5f)));
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TriclinicScale.x * 0.5f * TriclinicEdgeSmoothing, TriclinicScale.x * 0.5f * TriclinicEdgeSmoothing), Random.Range(-TriclinicScale.y * 0.5f * TriclinicEdgeLength, TriclinicScale.y * 0.5f * TriclinicEdgeLength), Random.Range(0, TriclinicScale.z * 0.5f + TriclinicEdgeHeigth)));
            }

            if (TypeOfRock == RockType.Orthorombic) for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-Scale.z * 0.5f, Scale.z * 0.5f), Random.Range(-Scale.y * 0.5f, Scale.y * 0.5f), Random.Range(-Scale.x * 0.5f, Scale.x * 0.5f)));

            if (TypeOfRock == RockType.Hexagonal)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-HexagonalScale.x * 0.5f, HexagonalScale.x * 0.5f), Random.Range(-HexagonalScale.y * 0.5f, HexagonalScale.y * 0.5f), Random.Range(-HexagonalScale.z * 0.5f, HexagonalScale.z * 0.5f)));
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-HexagonalScale.x * 0.5f * HexagonalEdgeWidth, HexagonalScale.x * 0.5f * HexagonalEdgeWidth), 0, Random.Range(-HexagonalScale.z * 0.5f, HexagonalScale.z * 0.5f)));
            }

            if (TypeOfRock == RockType.MonoclinicAndTrigonal)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TrigonalScale.x * 0.5f, TrigonalScale.x * 0.5f), Random.Range(-TrigonalScale.y * 0.5f, TrigonalScale.y * 0.5f), Random.Range(-TrigonalScale.z * 0.5f, TrigonalScale.z * 0.5f)));
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TrigonalScale.x * 0.5f * TrigonalEdgeSmoothing, TrigonalScale.x * 0.5f * TrigonalEdgeSmoothing), Random.Range(-TrigonalScale.y * 0.5f, TrigonalScale.y * 0.5f), Random.Range(-TrigonalScale.z * 0.5f - TrigonalEdgePosition, TrigonalScale.z * 0.5f + TrigonalEdgePosition)));
            }

            if (TypeOfRock == RockType.Octahedral)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-OctahedralScale.x * 0.5f, OctahedralScale.x * 0.5f), Random.Range(-OctahedralScale.y * 0.5f, OctahedralScale.y * 0.5f), 0));
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-OctahedralScale.x * 0.5f * OctahedralPointSmoothing, OctahedralScale.x * 0.5f * OctahedralPointSmoothing), Random.Range(-OctahedralScale.y * 0.5f * OctahedralPointSmoothing, OctahedralScale.y * 0.5f * OctahedralPointSmoothing), Random.Range(-OctahedralPointPosition, OctahedralPointPosition)));
            }

            if (TypeOfRock == RockType.Quartz)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-QuartzScale.x * 0.5f, QuartzScale.x * 0.5f), Random.Range(-QuartzScale.y * 0.5f, QuartzScale.y * 0.5f), Random.Range(-QuartzScale.z * 0.5f, QuartzScale.z * 0.5f)));
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-QuartzScale.x * 0.5f * QuartzEdgeWidth, QuartzScale.x * 0.5f * QuartzEdgeWidth), 0, Random.Range(-QuartzScale.z * 0.5f, QuartzScale.z * 0.5f)));
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-QuartzScale.x * 0.5f * 0.01f, QuartzScale.x * 0.5f * 0.01f), Random.Range(-QuartzScale.y * 0.5f * 0.01f, QuartzScale.y * 0.5f * 0.01f), Random.Range(-QuartzScale.z * 0.5f - QuartzPointPosition, QuartzScale.z * 0.5f + QuartzPointPosition)));
            }

            if (TypeOfRock == RockType.Spherical)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(Random.insideUnitSphere * Radius);
            }

            if (TypeOfRock == RockType.Pyramidal)
            {
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.insideUnitSphere.x * PyramidalBaseScale, Random.insideUnitSphere.y * PyramidalBaseScale, Random.Range(0, 0.1f)));
                for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-PyramidalBaseScale * PyramidalVertexSmoothing, PyramidalBaseScale * PyramidalVertexSmoothing), Random.Range(-PyramidalBaseScale * PyramidalVertexSmoothing, PyramidalBaseScale * PyramidalVertexSmoothing), Random.Range(0, PyramidalVertexPortrusion)));
            }

            CreateRock(VertexList, pos);
        }

        public void CreateRock(IEnumerable<Vector3> Vertices, Vector3 pos)
        {

            rock = new GameObject(TemporaryRockName);
            rock.transform.position = pos;
            //Selection.activeGameObject = rock;

            MeshFilter meshFilter = (MeshFilter)rock.AddComponent(typeof(MeshFilter));  //Add a mesh filter.
            MeshRenderer renderer = rock.AddComponent(typeof(MeshRenderer)) as MeshRenderer; //Add a mesh renderer.

            
             if (MaterialType == TypeOfMaterial.SingleMaterial) renderer.material = RockMaterial; //Assign single material.
             else renderer.material = Materials[Random.Range(0, Materials.Length)]; //Assigne random material from a collection of materials.

            meshFilter.sharedMesh = CreateMesh(Vertices); //Create a mesh.

             if (TypeOfShading == ShadingType.Flat) MakeLowPoly(rock.transform, TemporaryRockName, GenerateUV);
            else
            {
                if (GenerateUV)
                    UVMapper.CreateUVMap(rock.GetComponent<MeshFilter>().sharedMesh, rock.transform);
            }


            if (OrientationOfRock == RockOrientation.X) rock.transform.localEulerAngles = new Vector3(0, 90, 0);


             if (OrientationOfRock == RockOrientation.Y)
             {
                 if (TypeOfRock == RockType.Orthorombic) rock.transform.localEulerAngles = new Vector3(0, 90, 0);
                 if (TypeOfRock == RockType.TriclinicAndObelisk || TypeOfRock == RockType.Pyramidal) rock.transform.localEulerAngles = new Vector3(-90, 0, 0);
                 else rock.transform.localEulerAngles = new Vector3(90, 0, 0);
             }

             if (OrientationOfRock == RockOrientation.Z)
             {
                 if (TypeOfRock == RockType.Orthorombic) rock.transform.localEulerAngles = new Vector3(0, 90, 0);
                 else rock.transform.localEulerAngles = new Vector3(0, 0, 0);
             }
        }

        public static Mesh CreateMesh(IEnumerable<Vector3> Vertices)
        {
            Mesh mesh = new Mesh();
            mesh.name = "RockMesh";
            List<int> Triangles = new List<int>();

            var vertices = Vertices.Select(x => new Vertex(x)).ToList();

            var result = MIConvexHull.ConvexHull.Create(vertices);
            mesh.vertices = result.Points.Select(x => x.ToVec()).ToArray();
            var xxx = result.Points.ToList();

            foreach (var face in result.Faces)
            {
                Triangles.Add(xxx.IndexOf(face.Vertices[0]));
                Triangles.Add(xxx.IndexOf(face.Vertices[1]));
                Triangles.Add(xxx.IndexOf(face.Vertices[2]));
            }
            mesh.triangles = Triangles.ToArray();

            mesh.RecalculateNormals();
            return mesh;
        }

        public static void MakeLowPoly(Transform transform, string name, bool GenerateUV)
        {
            if (transform == null)
            {
                Debug.Log("No appropriate object selected.");
                return;
            }

            MeshFilter meshfilter = transform.GetComponent<MeshFilter>();
            if (meshfilter == null || meshfilter.sharedMesh == null)
            {
                Debug.Log("No mesh found on the selected object");
                return;
            }

            GameObject gameobject = transform.gameObject;
            gameobject.name = name;

            meshfilter = gameobject.GetComponent<MeshFilter>();

            Mesh mesh = meshfilter.sharedMesh;
            Vector3[] oldVerts = mesh.vertices;
            int[] triangles = mesh.triangles;
            Vector3[] vertices = new Vector3[triangles.Length];

            for (int i = 0; i < triangles.Length; i++)
            {
                vertices[i] = oldVerts[triangles[i]];
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            if (GenerateUV)
                UVMapper.CreateUVMap(mesh, gameobject.transform);
        }
    }
}