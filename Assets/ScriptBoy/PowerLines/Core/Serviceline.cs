using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptBoy.PowerLines
{
	[System.Serializable]
	public class Serviceline
	{
		public Wire wire;

        public Transform aTran;
        public Transform bTran;

        //a localPosition
        public Vector3 aP;
        //b localPosition
        public Vector3 bP;

        public Vector3 aWorldPosition
		{
			get
			{
				if (aTran == null)
				{
					return aP;
				}
				return aTran.TransformPoint (aP);
			}
			set
			{ 
				aP = aTran.InverseTransformPoint (value);
			}
		}

        public Vector3 bWorldPosition
		{
			get
			{
				if (bTran == null)
				{
					return bP;
				}

				return bTran.TransformPoint (bP);
			}
			set
			{ 
				bP = bTran.InverseTransformPoint (value);
			}
		}

        
		public Vector3 abCenterWorldPosition
		{
			get
			{
				Vector3 c = (aWorldPosition + bWorldPosition) / 2 + Vector3.down * wire.h;

				return Vector3.Lerp(Vector3.Lerp(aWorldPosition,c,0.5f),Vector3.Lerp(c,bWorldPosition,0.5f),0.5f);
			}
		}
	}
}