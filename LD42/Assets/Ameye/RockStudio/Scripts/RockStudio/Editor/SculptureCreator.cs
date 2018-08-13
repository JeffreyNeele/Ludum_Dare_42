using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityRockStudio
{
  public class SculptureCreator : MonoBehaviour
  {

    public static void GenerateComposition(int NumberOfVertices,Vector3 pos,Vector3 nor,Material rockmat)
    {
      if(GameObject.Find("_Sculpture_") != null)
      {
        GameObject rockstructure = new GameObject();
                Undo.RegisterCreatedObjectUndo(rockstructure, "Create");
                rockstructure.name = "_Structure_";

        if(RockStudio.SeedMethod == RockStudio.MethodOfSeed.Random)
        {
          Random.state = Random.state;
          RockStudio.Seed = Random.Range(0,RockStudio.SeedSize);
          Random.InitState(RockStudio.Seed);
          if(RockStudio.PrintSeedValue == true) Debug.Log("<color=green><b>Used seed: </b></color>" + RockStudio.Seed);
        }
        else Random.InitState(RockStudio.Seed);

        foreach(Transform child in GameObject.Find("_Sculpture_").transform)
        {
          List<Vector3> VertexList = new List<Vector3>(NumberOfVertices);
          pos = child.localPosition;
          Vector3 scale = child.localScale;
          Vector3 rot = child.localEulerAngles;
          for(int i = 0; i< NumberOfVertices; i++) VertexList.Add((new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 0.5f));
          CreateSculpture(VertexList,pos,Vector3.zero,rot,scale,rockmat);
        }
      }
    }

    public static void CreateSculpture(IEnumerable<Vector3> Vertices, Vector3 pos, Vector3 nor,Vector3 rot,Vector3 scale,Material rockmat)
    {
      GameObject rock = new GameObject();
            Undo.RegisterCreatedObjectUndo(rock, "Create");
            rock.name = RockStudio.StructureElementName;
      rock.transform.position = pos;
      rock.transform.eulerAngles = rot;
      rock.transform.localScale = scale;
      rock.transform.LookAt(pos-nor);
      rock.transform.parent = GameObject.Find("_Structure_").transform;
      Selection.activeGameObject = rock;

      MeshFilter meshFilter = (MeshFilter)rock.AddComponent(typeof(MeshFilter));  //Add a mesh filter.
      MeshRenderer renderer = rock.AddComponent(typeof(MeshRenderer)) as MeshRenderer; //Add a mesh renderer.

      renderer.material = rockmat;
      meshFilter.sharedMesh = MeshCreator.CreateMesh(Vertices); //Create a mesh.

      if(RockStudio.TypeOfShading == RockStudio.ShadingType.Flat) MakeLowPoly.LowPoly(rock.transform,RockStudio.toolBar,RockStudio.StructureElementName, RockStudio.GenerateUV);
            else
            {
                if (RockStudio.GenerateUV)
                    UVMapper.CreateUVMap(rock.GetComponent<MeshFilter>().sharedMesh, rock.transform);
            }
        }

    public static void GenerateCompositionFill(int NumberOfVertices,Vector3 pos,Vector3 nor,Material rockmat)
    {
      List<Vector3> VertexList = new List<Vector3>(NumberOfVertices);
      VertexList.Clear();

      if(RockStudio.SeedMethod == RockStudio.MethodOfSeed.Random)
      {
        Random.state = Random.state;
        RockStudio.Seed = Random.Range(0,RockStudio.SeedSize);
        Random.InitState(RockStudio.Seed);
        if(RockStudio.PrintSeedValue == true) Debug.Log("<color=green><b>Used seed: </b></color>" + RockStudio.Seed);
      }
      else Random.InitState(RockStudio.Seed);

      if(GameObject.Find("_Sculpture_") != null)
      {
        GameObject rockstructure = new GameObject();
                Undo.RegisterCreatedObjectUndo(rockstructure, "Create");
                rockstructure.name = "_Structure_";

        foreach(Transform child in GameObject.Find("_Sculpture_").transform)
        {
          Vector3 pos1 = child.localPosition;
          Vector3 scale = child.localScale;
          Vector3 rot = child.localEulerAngles;

          for(int i = 0; i< NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-scale.x*0.5f ,scale.x*0.5f )+pos1.x, Random.Range(-scale.y*0.5f ,scale.y*0.5f ) + pos1.y,Random.Range(-scale.z*0.5f ,scale.z*0.5f )+pos1.z));
        }
      }
      CreateSculptureFill(VertexList, pos,nor,rockmat);	//Create the rock.
    }

    public static void CreateSculptureFill(IEnumerable<Vector3> Vertices, Vector3 pos, Vector3 nor,Material rockmat)
    {
      if (RockStudio.PlacingRock)
      {
        pos += nor * (0.01f * 0.1f);
      }

      if (RockStudio.InstantMode)
      {
        pos += nor * (0.01f * 0.1f);
      }

      GameObject rock = new GameObject(RockStudio.StructureElementName);
            Undo.RegisterCreatedObjectUndo(rock, "Create");
            rock.transform.position = pos;
      rock.transform.LookAt(pos-nor);
      rock.transform.parent = GameObject.Find("_Structure_").transform;
      Selection.activeGameObject = rock;

      MeshFilter meshFilter = (MeshFilter)rock.AddComponent(typeof(MeshFilter));  //Add a mesh filter.
      MeshRenderer renderer = rock.AddComponent(typeof(MeshRenderer)) as MeshRenderer; //Add a mesh renderer.

      renderer.material = rockmat;
      meshFilter.sharedMesh = MeshCreator.CreateMesh(Vertices); //Create a mesh.
            meshFilter.sharedMesh.name = RockStudio.StructureElementName;

      if(RockStudio.TypeOfShading == RockStudio.ShadingType.Flat) MakeLowPoly.LowPoly(rock.transform,RockStudio.toolBar,RockStudio.StructureElementName, RockStudio.GenerateUV);
            else
            {
                if (RockStudio.GenerateUV)
                    UVMapper.CreateUVMap(rock.GetComponent<MeshFilter>().sharedMesh, rock.transform);
            }
        }
  }
}
