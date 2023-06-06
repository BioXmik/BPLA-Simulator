using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptBoy.PowerLines
{

	[ExecuteInEditMode]
	public class Powerlines : MonoBehaviour 
	{
		public PowerlinesRenderer _Renderer;

		public bool useLODTechnique;
		public Wire[] wires;
		public Wire[] LODWires;
		public PowerPole[] polePrefabs;
		public PowerPole[] poles;
		public Connection[] connections;
		public Serviceline[] servicelines;
		private int connectionsBetweenCount;

		public void GenerateMeshs(MeshGroup e)
		{
			int cLength = connections.Length;
			int sLength = servicelines.Length; 
			int wLength = wires.Length;
			if (wLength > 0) 
			{
				e.Meshes = new Mesh[cLength + sLength + connectionsBetweenCount];
				List<Material> materials = new List<Material> ();
				for (int i = 0; i < wLength; i++) 
				{
					Material m = wires [i].material;
					if (!materials.Contains (m)) 
					{
						materials.Add (m);
					}
				}


				_Renderer.materialIndex = new int[cLength + sLength + connectionsBetweenCount];
				_Renderer.materials = materials.ToArray ();

				for (int i = 0; i < cLength; i++) 
				{
					var c = connections [i];
					_Renderer.materialIndex [i] = materials.IndexOf (c.wire.material);
					e.Meshes[i] = c.wire.GenerateMesh (c);
				}

				for (int i = 0; i < sLength; i++) 
				{
					var c = servicelines [i];
					_Renderer.materialIndex [i+cLength] = materials.IndexOf (c.wire.material);
					e.Meshes[i+cLength] = c.wire.GenerateMesh (c);
				}

				int mesheIndex = cLength + sLength;
				for (int i = 0; i < poles.Length; i++) 
				{
					PowerPole pole = poles [i];
					if (pole.HasInPointToOutPointConnection) 
					{
						_Renderer.materialIndex [mesheIndex] = materials.IndexOf (pole.betweenWire.material);
						e.Meshes[mesheIndex] = pole.betweenWire.GenerateMesh (pole);
						mesheIndex++;
					}
				}
                
			}

		}

		public void GenerateMeshs(MeshGroup e , Wire wire)
		{
			int cLength = connections.Length;
			int sLength = servicelines.Length; 
			int wLength = wires.Length;

			if (wLength > 0) 
			{
				e.Meshes = new Mesh[cLength + sLength + connectionsBetweenCount];
				for (int i = 0; i < cLength; i++) 
				{
					var c = connections [i];
					int seg = wire.segments;
					Vector3[] Base = wire.Base;
					wire.hasAnimatedShader = c.wire.hasAnimatedShader;
					wire.segments = Mathf.Min (seg , c.wire.segments);
					if (Base.Length > c.wire.Base.Length) 
					{
						wire.Base = c.wire.Base;
					}
					wire.h = c.wire.h;
					wire.radius = c.wire.radius;
					e.Meshes[i] = wire.GenerateMesh (c);
					wire.segments = seg;
					wire.Base = Base;
					wire.hasAnimatedShader = false;
				}

				for (int i = 0; i < sLength; i++) 
				{
					var c = servicelines [i];
					int seg = wire.segments;
					Vector3[] Base = wire.Base;
					wire.hasAnimatedShader = c.wire.hasAnimatedShader;
					wire.segments = Mathf.Min (seg , c.wire.segments);
					if (Base.Length > c.wire.Base.Length) 
					{
						wire.Base = c.wire.Base;
					}
					wire.h = c.wire.h;
					wire.radius = c.wire.radius;
					e.Meshes[i+cLength] = wire.GenerateMesh (c);
					wire.segments = seg;
					wire.Base = Base;
					wire.hasAnimatedShader = false;
				}

				int mesheIndex = cLength + sLength;
				for (int i = 0; i < poles.Length; i++) 
				{
					PowerPole pole = poles [i];
					if (pole.HasInPointToOutPointConnection) 
					{
						int seg = wire.segments;
						Vector3[] Base = wire.Base;
						wire.hasAnimatedShader = pole.betweenWire.hasAnimatedShader;
						wire.segments = Mathf.Min (seg , pole.betweenWire.segments);
						if (Base.Length > pole.betweenWire.Base.Length) 
						{
							wire.Base = pole.betweenWire.Base;
						}
						wire.h = pole.betweenWire.h;
						wire.radius = pole.betweenWire.radius;
						e.Meshes[mesheIndex] = wire.GenerateMesh (pole);
						wire.segments = seg;
						wire.Base = Base;
						wire.hasAnimatedShader = false;
						mesheIndex++;
					}
				}
			}

		}

#if UNITY_EDITOR
        private void Awake()
        {
            Update();
        }

        public void Update ()
		{	
			if (!Application.isPlaying) 
			{
				if(poles != null)
				{
					for (int i = 0; i < poles.Length; i++) 
					{
						PowerPole e = poles [i];
						if (e == null && !ReferenceEquals (e, null)) 
						{
							GameObject g = Instantiate<GameObject> (gameObject);
							g.name = name;
							DestroyImmediate (gameObject);
							return;
						}
					}
				}


				if (_Renderer != null) 
				{
					_Renderer.useLODTechnique = useLODTechnique;
					if (connections != null && servicelines != null && wires != null) 
					{

						connectionsBetweenCount = 0;
						for (int i = 0; i < poles.Length; i++) 
						{
							if (poles [i].HasInPointToOutPointConnection) 
							{
								connectionsBetweenCount++;
							}
						}

						int cLength = connections.Length;
						int sLength = servicelines.Length; 
						int meshCount = cLength + sLength + connectionsBetweenCount;

						_Renderer.meshCount = meshCount;

						if (useLODTechnique) 
						{
							_Renderer.meshLines = new Line[_Renderer.meshCount];
							for (int i = 0; i < cLength; i++) {
								Connection c = connections [i];
								_Renderer.meshLines [i] = new Line (c.a.center, c.b.center);
							}

							for (int i = 0; i < sLength; i++) {
								Serviceline c = servicelines [i];
								_Renderer.meshLines [i + cLength] = new Line (c.aWorldPosition, c.bWorldPosition);
							}
								
							int LIndex = cLength + sLength;
							for (int i = 0; i < poles.Length; i++) 
							{
								PowerPole pole = poles [i];
								int pointIndex = pole.connectors.Length / 2;
								if (pole.HasInPointToOutPointConnection) 
								{
									_Renderer.meshLines [LIndex] = new Line (pole.GetInPoint(pointIndex), pole.GetOutPoint(pointIndex));
									LIndex++;
								}
							}


							_Renderer.meshGroups = new MeshGroup[3]{new MeshGroup(),new MeshGroup(),new MeshGroup()};

							GenerateMeshs (_Renderer.meshGroups[0]);
							GenerateMeshs (_Renderer.meshGroups[1], LODWires [0]);
							GenerateMeshs (_Renderer.meshGroups[2], LODWires [1]);
						} 
						else if (_Renderer.meshCount > 0)
						{	
							_Renderer.meshGroups = new MeshGroup[1]{new MeshGroup()};
							GenerateMeshs (_Renderer.meshGroups[0]);
						} 
						else 
						{
							_Renderer.Clear ();
						}
					}

					//Force update :)
					_Renderer.transform.Translate (0,+1, 0);
					_Renderer.transform.Translate (0,-1, 0);
				}
			}
		}
#endif
	}
}