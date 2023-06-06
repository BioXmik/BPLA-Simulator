using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ScriptBoy.PowerLines
{
	[CustomEditor(typeof(PowerlinesRenderer))]
	public class PowerlinesRendererEditor : Editor 
	{
		private PowerlinesRenderer renderer;

        private SerializedProperty castShadowsProperty;
        private SerializedProperty receiveShadowsProperty;
        private SerializedProperty drawGizmosProperty;

        private SerializedProperty distance1Property;
        private SerializedProperty distance2Property;

        private void OnEnable()
		{
			renderer = target as PowerlinesRenderer;

            castShadowsProperty = serializedObject.FindProperty("castShadows");
            receiveShadowsProperty = serializedObject.FindProperty("receiveShadows");
            drawGizmosProperty = serializedObject.FindProperty("drawGizmos");

            distance1Property = serializedObject.FindProperty("distance1");
            distance2Property = serializedObject.FindProperty("distance2");
        }
		public override void OnInspectorGUI ()
		{
            EditorGUILayout.PropertyField(castShadowsProperty);
            EditorGUILayout.PropertyField(receiveShadowsProperty);

			if (renderer.useLODTechnique) 
			{
                EditorGUILayout.PropertyField(drawGizmosProperty);

                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(distance1Property);
                EditorGUILayout.PropertyField(distance2Property);

                DrawDistanceRatioBar();

                if (renderer.meshGroups != null) 
				{
					for (int row = 0; row < 3; row++) 
					{
						int vertices = 0;
						for (int i = 0; i < renderer.meshCount; i++) 
						{
							Mesh mesh = renderer.meshGroups [row].Meshes [i];
							if (mesh != null) 
							{
								vertices += mesh.vertices.Length;
							}
						}
							
						GUILayout.Label ("LOD"+ row + " vertices count : " + vertices);
					}
				}	

				SceneView.RepaintAll ();
			}
            serializedObject.ApplyModifiedProperties();
		}

        private void DrawDistanceRatioBar()
        {
            Rect rect = GUILayoutUtility.GetRect(50, 50);
            rect.width += 2;
            rect.xMin = 0;
            GUI.Box(rect, "");

            rect.x += 2;
            rect.y += 30;

            Handles.DrawLine(rect.position, rect.position + Vector2.right * rect.width);
            rect.y -= 20;

            GUI.Label(rect, "LOD0");

            rect.x = (rect.width - 40) * (renderer.distance1 / renderer.distance2);
            Handles.DrawLine(rect.position, rect.position + Vector2.up * 20);
            GUI.Label(rect, "LOD1");


            rect.x = rect.width - 40;
            Handles.DrawLine(rect.position, rect.position + Vector2.up * 20);
            GUI.Label(rect, "LOD2");
        }
	}
}