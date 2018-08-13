using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnityRockStudio
{
	public class MeshCreator : MonoBehaviour
	{
		public static Mesh CreateMesh(IEnumerable<Vector3> Vertices)
		{
			Mesh mesh = new Mesh();
			mesh.name = "RockMesh";
			List<int> Triangles = new List<int>();

			var vertices = Vertices.Select(x => new Vertex(x)).ToList();

			var result = MIConvexHull.ConvexHull.Create(vertices);
			mesh.vertices = result.Points.Select(x => x.ToVec()).ToArray();
			var xxx = result.Points.ToList();

			foreach(var face in result.Faces)
			{
				Triangles.Add(xxx.IndexOf(face.Vertices[0]));
				Triangles.Add(xxx.IndexOf(face.Vertices[1]));
				Triangles.Add(xxx.IndexOf(face.Vertices[2]));
			}
			mesh.triangles = Triangles.ToArray();

			mesh.RecalculateNormals();
			return mesh;
		}
	}
}
