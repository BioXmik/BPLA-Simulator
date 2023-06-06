using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptBoy.PowerLines
{
	[ExecuteInEditMode]
	public class PowerPole : MonoBehaviour 
	{
		public Vector3[] connectors;

		public bool mirror;
		public bool connectionBetween;
		public Wire betweenWire;

        public bool HasInPointToOutPointConnection
        {
            get
            {
                return mirror && connectionBetween && betweenWire != null && betweenWire.material != null;
            }
        }

        // -Z
        public Vector3 GetInPoint(int index)
		{
			if (mirror)
			{
				Vector3 p = connectors [index];
				p.z = -p.z;
				return transform.TransformPoint (p);
			}
			return transform.TransformPoint (connectors [index]);
		}

		// +Z
		public Vector3 GetOutPoint(int index)
		{
			return transform.TransformPoint (connectors [index]);
		}

        public Vector3 center
        {
            get
            {
                Vector3 sum = Vector3.zero;
                for (int i = 0; i < connectors.Length; i++)
                {
                    sum += connectors[i];
                }
                return transform.TransformPoint(sum / connectors.Length);
            }
        }
    }
}