using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ScriptBoy.PowerLines
{
	[CustomEditor(typeof(Wire))]
	public class WireEditor : Editor
	{
		Wire Wire;
		public static Powerlines pl;

		void OnEnable()
		{
			Wire = target as Wire;
			Wire.UpdateBase ();
			//setDirty ();
		}

		void OnDisable()
		{
			//setDirty ();
			pl = null;
		}

		void setDirty()
		{
			AssetDatabase.Refresh ();
			EditorUtility.SetDirty (Wire);
			AssetDatabase.SaveAssets ();
		}
			
		public override void OnInspectorGUI ()
		{
			if (pl != null) 
			{
				if (GUILayout.Button ("<<Back" , GUILayout.ExpandWidth(false))) 
				{
					Selection.activeObject = pl;
				}
			}

			base.OnInspectorGUI ();

			if (Wire.radius <= 0) Wire.radius = 0.01f;
			if (Wire.angleDelta < 10) Wire.angleDelta = 10;
			if (Wire.angleDelta > 175) Wire.angleDelta = 175;
			if (Wire.segments < 1) Wire.segments = 1;
			if (Wire.h < 0.1f) Wire.h = 0.1f;

			WirePreview ();

			int wireV = Wire.Base.Length * (Wire.segments + 1);
			GUILayout.Label ("Vertices : " + wireV);
			GUILayout.Label ("65000 / Vertices: " + (65000 / wireV));

			if (GUI.changed) 
			{
				Wire.UpdateBase ();
				if (!Application.isPlaying) 
				{
					foreach (var e in FindObjectsOfType<Powerlines>()) 
					{
						e.Update ();
					}
				}

				foreach (var e in FindObjectsOfType<PowerlinesRenderer>())
				{
					e.Update ();
				}
			}
		}

		void WirePreview()
		{
			Vector3[] Base = Wire.Base;
			float scale = 50;
			Rect rect = GUILayoutUtility.GetRect (50, 120);
			rect.width += 2 ;
			rect.xMin = 0;

			Vector3 prePoint = Base[Base.Length-2] * scale;
			GUI.Box (rect,"");
			DrawGrid (rect);

			for (int i = 0; i < Base.Length-1; i++) 
			{
				Vector3 point = Base[i] * scale;
				DrawLineInRect(prePoint, point , rect);
				prePoint = point;
			}

			Vector3 a = new Vector3 (0 , rect.y + rect.height * 0.5f);
			Vector3 b = new Vector3 (rect.width , rect.y + rect.height * 0.5f);
			Vector3 c =  new Vector3 (rect.width * 0.5f , rect.y + rect.height/2  + rect.height/2 * Mathf.Clamp(Wire.h,0,2));

			prePoint = a;
			for (int i = 1; i <= Wire.segments; i++) 
			{
				float t = (float)i / Wire.segments;
				Vector3 point = Vector3.Lerp (Vector3.Lerp (a,c,t),Vector3.Lerp (c,b,t),t);
				DrawLine(point,prePoint);
				prePoint = point;
			}

			if (Wire.material != null) 
			{
				rect.width = 40 ;
				rect.height = 40;
				GUI.Box (rect,AssetPreview.GetAssetPreview(Wire.material));
			}
		}

		void DrawGrid(Rect rect)
		{
			Vector3 a = new Vector3 (rect.x , rect.y + rect.height * 0.5f);
			Vector3 b = new Vector3 (rect.x + rect.width , rect.y + rect.height * 0.5f);
			Handles.DrawBezier (a,b,b,a,Color.white,null,2);

			a = new Vector3 (rect.x + rect.width * 0.5f , rect.y );
			b = new Vector3 (rect.x + rect.width * 0.5f , rect.y + rect.height);
			Handles.DrawBezier (a,b,b,a,Color.white,null,2);
		}

		void DrawLineInRect(Vector3 a , Vector3 b , Rect rect)
		{
			a.y *= -1;
			b.y *= -1;
			a.y += rect.yMin + rect.height * 0.5f;
			b.y += rect.yMin + rect.height * 0.5f;
			a.x += rect.xMin + rect.width * 0.5f;
			b.x += rect.xMin + rect.width * 0.5f;
			Handles.DrawBezier (a,b,b,a,Color.white,null,2);
		}

		void DrawLine(Vector3 a , Vector3 b)
		{
			Handles.DrawBezier (a,b,b,a,Color.white,null,2);
			Handles.DrawLine (b, b + Vector3.up*3);
		}
	}
}