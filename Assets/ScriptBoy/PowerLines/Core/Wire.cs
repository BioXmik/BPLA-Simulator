using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptBoy.PowerLines
{
	[CreateAssetMenu(fileName = "Wire" , menuName = "PowerLines/Wire")]
	public class Wire : ScriptableObject 
	{
		public Material material;
		public bool hasAnimatedShader;
		[HideInInspector]
		public Vector3[] Base;
		public int segments = 10;
		public float radius = 0.01f;
		public float h = 1;
		public float angleDelta = 120;
		public float angleOffset = 30;

		public void UpdateBase ()
		{
			if (radius <= 0) radius = 0.01f;
			if (angleDelta < 10) angleDelta = 10;
			if (angleDelta > 175) angleDelta = 175;
			if (segments < 1) segments = 1;
			if (h < 0.1f) h = 0.1f;

			List<Vector3> r = new List<Vector3> ();
			float A = 0 + angleOffset;
			while (A < 360 + angleOffset) 
			{
				r.Add (Quaternion.AngleAxis(A, Vector3.forward) * Vector3.right);
				A += angleDelta;
			}
			r.Add (r[0]);
			Base = r.ToArray ();
		}


		public float AngleXZ(Vector3 v)
		{
			return Mathf.Atan2 (v.z, v.x) * Mathf.Rad2Deg;
		}

		public int GetIndex(int i , int min , int max , int L , bool reverse)
		{
			int index = i;
			if (reverse) 
			{
				index = min - 1 - i;
			}

			if (L == max) 
			{
				if (min == 2) 
				{
					float t = (float) i / (min - 1);
					if (reverse) t = 1 - t;
					index = (int)((float) t * (max - 1));
				}
			}

			return index;
		}

		public Mesh GenerateMesh(Vector3 a , Vector3 b)
		{
			Mesh mesh = new Mesh ();

			int L = Base.Length;

			Vector3[] vertices = new Vector3[L * (segments + 1)];
			Vector2[] uv = new Vector2[vertices.Length];
			int[] triangles = new int[L * 6 * segments];

			//...................................................................UV
			if (hasAnimatedShader) 
			{
				for (int i = 0; i <= segments; i++) 
				{
					for (int ii = 0; ii < L; ii++) 
					{
						uv [(i * L) + ii] = new Vector2 (((float)i / segments), (float)ii / (L - 1));
					}
				}
			} 
			else 
			{
				float dis = (a - b).magnitude;
				for (int i = 0; i <= segments; i++)
				{
					for (int ii = 0; ii < L; ii++) 
					{
						uv [(i * L) + ii] = new Vector2 (((float)i / segments) * dis, (float)ii / (L - 1));
					}
				}
			}

			//...................................................................Triangles
			for (int i = 0; i < segments; i++) 
			{
				int p = i * L * 6;
				int p2 = i * L;
				for (int ii = 0; ii < L - 1; ii++) 
				{
					triangles [ii * 6 + p] = ii + p2;
					triangles [ii * 6 + 1 + p] = ii + p2 + L + 1;
					triangles [ii * 6 + 2 + p] = ii + p2 + L;

					triangles [ii * 6 + 3 + p] = ii + p2;
					triangles [ii * 6 + 4 + p] = ii + p2 + 1;
					triangles [ii * 6 + 5 + p] = ii + p2 + L + 1;
				}
			}

			//...................................................................Vertices
			Vector3 preP = 2 * a - Vector3.Lerp (a, b, (float)1 / segments);
			Vector3 c = (a + b) / 2 + Vector3.down * h;
			for (int i = 0; i <= segments; i++) 
			{
				float t = (float)i / segments;
				Vector3 offset = Vector3.Lerp (Vector3.Lerp(a,c,t),Vector3.Lerp(c,b,t),t);
				Vector3 dir = offset - preP;
				Quaternion q = Quaternion.LookRotation (dir);
				for (int ii = 0; ii < L; ii++) 
				{
					vertices [(i * L) + ii] = q * (Base [ii] * radius) + offset;
				}
				preP = offset;
			}

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uv;

			mesh.RecalculateNormals ();
			Vector3[] normals = mesh.normals;
			for (int i = 0; i <= segments; i++) 
			{
				normals [(i * L)] = normals [(i * L)  + L - 1];
			}
			mesh.normals = normals;

			return mesh;
		}

		public Mesh GenerateMesh(Serviceline sline)
		{
			return GenerateMesh (sline.aWorldPosition, sline.bWorldPosition);
		}

		public Mesh GenerateMesh(Connection connection)
		{
			Mesh mesh = new Mesh ();

			int wireCount = Mathf.Min (connection.a.connectors.Length,connection.b.connectors.Length);


			int aL = connection.a.connectors.Length;
			int bL = connection.b.connectors.Length;

			int LMin = Mathf.Min(aL,bL);
			int LMax = Mathf.Max(aL,bL);

			float Aa = AngleXZ (connection.a.transform.forward);
			float Ab = AngleXZ (connection.b.transform.forward);
			float Ac = AngleXZ (connection.b.transform.position - connection.a.transform.position);

			float Dab = Mathf.Abs(Mathf.DeltaAngle (Aa , Ab));
			float Dca = Mathf.Abs(Mathf.DeltaAngle (Ac , Aa));
			float Dcb = Mathf.Abs(Mathf.DeltaAngle (Ac , Ab));
			float D = Mathf.Abs(Mathf.DeltaAngle (Dca , Dcb));

			bool turn = (Dab - D > 90 || Dca > 90 && Dcb < 90 || Dcb > 90 && Dca < 90);



			int L = Base.Length;

			int VerticesL = L * (segments + 1);
			int TrianglesL = L * 6 * segments;

			Vector3[] vertices = new Vector3[VerticesL * wireCount];
			Vector2[] uv = new Vector2[vertices.Length];
			int[] triangles = new int[TrianglesL * wireCount];

			//...................................................................UV
			if (hasAnimatedShader)
			{
				for (int w = 0; w < wireCount; w++) 
				{
					int iw = w * VerticesL;
					for (int i = 0; i <= segments; i++) 
					{
						for (int ii = 0; ii < L; ii++) 
						{
							uv [(i * L) + ii + iw] = new Vector2 (((float)i / segments), (float)ii / (L - 1));
						}
					}
				}
			} 
			else 
			{
				for (int w = 0; w < wireCount; w++) 
				{
					int iw = w * VerticesL;

					int ia = GetIndex (w, LMin, LMax, aL, turn);
					int ib = GetIndex (w, LMin, LMax, bL, false);

					Vector3 a = (Dca < 90) ? connection.a.GetOutPoint (ia) : connection.a.GetInPoint (ia);
					Vector3 b = (Dcb < 90) ? connection.b.GetInPoint (ib) : connection.b.GetOutPoint (ib);
					float dis = (a - b).magnitude;

					for (int i = 0; i <= segments; i++) 
					{
						for (int ii = 0; ii < L; ii++) 
						{
							uv [(i * L) + ii + iw] = new Vector2 (((float)i / segments) * dis, (float)ii / (L - 1));
						}
					}
				}
			}

			//...................................................................Triangles

			for (int w = 0; w < wireCount; w++) 
			{
				int iw = w * TrianglesL;
				int iw2 = w * VerticesL;

				for (int i = 0; i < segments; i++) 
				{
					int p = i * L * 6;
					int p2 = i * L;

					for (int ii = 0; ii < L - 1; ii++) 
					{
						triangles [ii * 6 + p + iw] = ii + p2 + iw2;
						triangles [ii * 6 + 1 + p + iw] = ii + p2 + L + 1 + iw2;
						triangles [ii * 6 + 2 + p + iw] = ii + p2 + L + iw2;

						triangles [ii * 6 + 3 + p + iw] = ii + p2 + iw2;
						triangles [ii * 6 + 4 + p + iw] = ii + p2 + 1 + iw2;
						triangles [ii * 6 + 5 + p + iw] = ii + p2 + L + 1 + iw2;
					}
				}
			}

			//...................................................................Vertices

			for (int w = 0; w < wireCount; w++) 
			{
				int iw = w * VerticesL;

				int ia = GetIndex (w, LMin, LMax, aL, turn);
				int ib = GetIndex (w, LMin, LMax, bL, false);

				Vector3 a = (Dca < 90) ? connection.a.GetOutPoint (ia) : connection.a.GetInPoint (ia);
				Vector3 b = (Dcb < 90) ? connection.b.GetInPoint (ib) : connection.b.GetOutPoint (ib);

				Vector3 preP = 2 * a - Vector3.Lerp (a, b, (float)1 / segments);
				Vector3 c = (a + b) / 2 + Vector3.down * h;

				for (int i = 0; i <= segments; i++) 
				{
					float t = (float)i / segments;
					Vector3 offset = Vector3.Lerp (Vector3.Lerp (a, c, t), Vector3.Lerp (c, b, t), t);
					Vector3 dir = offset - preP;
					Quaternion q = Quaternion.LookRotation (dir);
					for (int ii = 0; ii < L; ii++) 
					{
						vertices [(i * L) + ii + iw] = q * (Base [ii] * radius) + offset;
					}

					preP = offset;
				}
			}

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uv;

			mesh.RecalculateNormals ();
			Vector3[] normals = mesh.normals;
			for (int w = 0; w < wireCount; w++) 
			{
				int iw = w * VerticesL;
				for (int i = 0; i <= segments; i++) 
				{
					normals [(i * L) + iw] = normals [(i * L) + L - 1 + iw];
				}
			}
			mesh.normals = normals;

			return mesh;
		}

		public Mesh GenerateMesh(PowerPole pole)
		{
			Mesh mesh = new Mesh ();

			int wireCount = pole.connectors.Length;
			int L = Base.Length;

			int VerticesL = L * (segments + 1);
			int TrianglesL = L * 6 * segments;

			Vector3[] vertices = new Vector3[VerticesL * wireCount];
			Vector2[] uv = new Vector2[vertices.Length];
			int[] triangles = new int[TrianglesL * wireCount];

			//...................................................................UV
			if (hasAnimatedShader) 
			{
				for (int w = 0; w < wireCount; w++) 
				{
					int iw = w * VerticesL;
					for (int i = 0; i <= segments; i++) 
					{
						for (int ii = 0; ii < L; ii++) 
						{
							uv [(i * L) + ii + iw] = new Vector2 (((float)i / segments), (float)ii / (L - 1));
						}
					}
				}
			} 
			else 
			{
				for (int w = 0; w < wireCount; w++) 
				{
					int iw = w * VerticesL;
					Vector3 a = pole.GetInPoint(w);
					Vector3 b = pole.GetOutPoint(w);
					float dis = (a - b).magnitude;
					for (int i = 0; i <= segments; i++) 
					{
						for (int ii = 0; ii < L; ii++) 
						{
							uv [(i * L) + ii + iw] = new Vector2 (((float)i / segments) * dis, (float)ii / (L - 1));
						}
					}
				}
			}

			//...................................................................Triangles

			for (int w = 0; w < wireCount; w++) 
			{
				int iw = w * TrianglesL;
				int iw2 = w * VerticesL;

				for (int i = 0; i < segments; i++) 
				{
					int p = i * L * 6;
					int p2 = i * L;

					for (int ii = 0; ii < L - 1; ii++) 
					{
						triangles [ii * 6 + p + iw] = ii + p2 + iw2;
						triangles [ii * 6 + 1 + p + iw] = ii + p2 + L + 1 + iw2;
						triangles [ii * 6 + 2 + p + iw] = ii + p2 + L + iw2;

						triangles [ii * 6 + 3 + p + iw] = ii + p2 + iw2;
						triangles [ii * 6 + 4 + p + iw] = ii + p2 + 1 + iw2;
						triangles [ii * 6 + 5 + p + iw] = ii + p2 + L + 1 + iw2;
					}
				}
			}

			//...................................................................Vertices

			for (int w = 0; w < wireCount; w++) 
			{
				int iw = w * VerticesL;

				Vector3 a = pole.GetInPoint(w);
				Vector3 b = pole.GetOutPoint(w);

				Vector3 preP = 2 * a - Vector3.Lerp (a, b, (float)1 / segments);
				Vector3 c = (a + b) / 2 + Vector3.down * h;

				for (int i = 0; i <= segments; i++) 
				{
					float t = (float)i / segments;
					Vector3 offset = Vector3.Lerp (Vector3.Lerp (a, c, t), Vector3.Lerp (c, b, t), t);
					Vector3 dir = offset - preP;
					Quaternion q = Quaternion.LookRotation (dir);
					for (int ii = 0; ii < L; ii++) 
					{
						vertices [(i * L) + ii + iw] = q * (Base [ii] * radius) + offset;
					}

					preP = offset;
				}
			}

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uv;

			mesh.RecalculateNormals ();
			Vector3[] normals = mesh.normals;
			for (int w = 0; w < wireCount; w++) 
			{
				int iw = w * VerticesL;
				for (int i = 0; i <= segments; i++) 
				{
					normals [(i * L) + iw] = normals [(i * L) + L - 1 + iw];
				}
			}
			mesh.normals = normals;

			return mesh;
		}
    }
}