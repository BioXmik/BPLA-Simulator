using UnityEngine;

namespace ScriptBoy.PowerLines
{

    [ExecuteInEditMode]
	public class PowerlinesRenderer : MonoBehaviour 
	{
        public MeshGroup[] meshGroups;
        public Material[] materials;
        private MaterialPropertyBlock materialPropertyBlock;
        public Line[] meshLines;
        public int[] materialIndex;
        public int meshCount;

        public bool castShadows = true;
        public bool receiveShadows = true;
      
        public bool useLODTechnique;
        public float distance1 = 7, distance2 = 14;


        private Camera mainCamera;

        private Vector3 cameraPosition;
		private Vector3 vZero = Vector3.zero;
		private Quaternion qIdentity = Quaternion.identity;

        private void Awake ()
		{
			materialPropertyBlock = new MaterialPropertyBlock ();
		}

		public void Update ()
		{
            if (meshCount == 0) return;
            if (meshGroups == null) return;
            if (materials == null) return;
            if (materialIndex == null) return;
            if (useLODTechnique && meshLines == null) return;
            if (mainCamera == null || !mainCamera.isActiveAndEnabled) 
			{
				mainCamera = Camera.main;
			}

			if (useLODTechnique) 
			{
                if (mainCamera != null)
                {
                    cameraPosition = mainCamera.transform.position;
                }
			}
				
			if (castShadows && receiveShadows) 
			{
				if (useLODTechnique) 
				{
					for (int i = 0; i < meshCount; i++) 
					{
                        Mesh mesh = meshGroups[GetLODIndex(i)].Meshes[i];
                        Material mat = materials[materialIndex[i]];
                        Graphics.DrawMesh (mesh, vZero, qIdentity, mat, 0);
					}
				} 
				else 
				{
					for (int i = 0; i < meshCount; i++) 
					{
                        Mesh mesh = meshGroups[0].Meshes[i];
                        Material mat = materials[materialIndex[i]];
                        Graphics.DrawMesh (meshGroups [0].Meshes[i], vZero, qIdentity, mat, 0);
					}
				}
			}
			else 
			{
				#if UNITY_EDITOR
				mainCamera = null;
				#endif

				if (useLODTechnique) 
				{
					for (int i = 0; i < meshCount; i++) 
					{
                        Mesh mesh = meshGroups[GetLODIndex(i)].Meshes[i];
                        Material mat = materials[materialIndex[i]];
                        Graphics.DrawMesh(mesh, vZero, qIdentity, mat, 0, mainCamera, 0, materialPropertyBlock, castShadows, receiveShadows);
                    }
				} 
				else 
				{
					for (int i = 0; i < meshCount; i++) 
					{
                        Mesh mesh = meshGroups [0].Meshes [i];
                        Material mat = materials[materialIndex[i]];
                        Graphics.DrawMesh (mesh, vZero , qIdentity, mat, 0, mainCamera, 0, materialPropertyBlock, castShadows, receiveShadows);
					}
				}
			}
		}

        private int GetLODIndex(int meshIndex)
        {
            float Dis = meshLines[meshIndex].Distance(cameraPosition);
            if (Dis > distance1)
            {
                if (Dis > distance2)
                {
                    return 2;
                }
                return 1;
            }
            return 0;
        }

        public void Clear()
        {
            meshCount = 0;
            materialIndex = null;
            materials = null;
            meshLines = null;
            meshGroups = null;
        }

#if UNITY_EDITOR


        [SerializeField] private bool drawGizmos = false;
        private void OnDrawGizmos()
		{	
			if (drawGizmos && useLODTechnique) 
			{
                if (meshGroups == null || meshGroups.Length == 0 || meshCount == 0)
                {
                    return;
                }

                if (mainCamera == null) mainCamera = Camera.main;
                if (mainCamera == null) return;



                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.yellow;
                for (int i = 0; i < meshCount; i++)
                {
                    int Index = GetLODIndex(i);
                    Mesh mesh = meshGroups[Index].Meshes[i];
                    Vector3 v = (mesh.vertices[0] + mesh.vertices[mesh.vertices.Length - 1]) / 2;
                    if (IsPositionInsideView(v))
                    {
                        UnityEditor.Handles.Label(v, "LOD" + Index, style);
                    }
                }
            } 
		}

        private bool IsPositionInsideView(Vector3 worldPosition)
		{
			Vector2 screenPo = mainCamera.WorldToScreenPoint (worldPosition);
			return !(screenPo.x < 0 || screenPo.y < 0 || screenPo.x > Screen.width || screenPo.y > Screen.height);
		}
#endif
	}

	[System.Serializable]
	public struct Line
	{
		public Vector3 a , b;

		public Line(Vector3 a , Vector3 b)
		{
			this.a = a;
			this.b = b;
		}

        public float Distance(Vector3 position)
        {
            Vector3 AB = b - a;
            Vector3 AC = position - a;
            float magnitudeAB = AB.magnitude;
            float dot = Vector3.Dot(AC, AB);
            if (dot < 0)
            {
                return AC.magnitude;
            }
            if (dot / (magnitudeAB * magnitudeAB) > 1)
            {
                return (position - b).magnitude;
            }
            return Vector3.Cross(AC, AB).magnitude * (1 / magnitudeAB);
        }
    }

	[System.Serializable]
	public class MeshGroup
	{
		public Mesh[] Meshes;
	}
}