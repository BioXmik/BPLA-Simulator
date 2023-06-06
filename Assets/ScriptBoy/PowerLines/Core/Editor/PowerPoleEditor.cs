using UnityEngine;
using UnityEditor;

namespace ScriptBoy.PowerLines
{
	[CustomEditor(typeof(PowerPole))]
	public class PowerPoleEditor : Editor 
	{
        private PowerPole powerPole;
        private Transform transform;
        private bool editMode;
        private GUIStyle _lableStyle;
        private GUIStyle lableStyle
        {
            get
            {
                if (_lableStyle == null)
                {
                    _lableStyle = new GUIStyle(EditorStyles.boldLabel);
                    _lableStyle.normal.textColor = Color.yellow;
                }

                return _lableStyle;
            }
        }

        private GUIStyle _errorlableStyle;
        private GUIStyle errorLableStyle
        {
            get
            {
                if (_errorlableStyle == null)
                {
                    _errorlableStyle = new GUIStyle(EditorStyles.boldLabel);
                    _errorlableStyle.normal.textColor = Color.red;
                }

                return _errorlableStyle;
            }
        }

        private SerializedProperty property_connectors;
        private SerializedProperty property_mirror;
        private SerializedProperty property_connectionBetween;
        private SerializedProperty property_betweenWire;


        private void OnEnable()
        {
            powerPole = target as PowerPole;
            transform = powerPole.transform;

            if (powerPole.connectors == null) powerPole.connectors = new Vector3[0];


            property_connectors = serializedObject.FindProperty("connectors");
            property_mirror = serializedObject.FindProperty("mirror");
            property_connectionBetween = serializedObject.FindProperty("connectionBetween");
            property_betweenWire = serializedObject.FindProperty("betweenWire");

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -=  SceneView_duringSceneGui;
            SceneView.duringSceneGui +=  SceneView_duringSceneGui;
#else
            SceneView.onSceneGUIDelegate -= SceneView_duringSceneGui;
            SceneView.onSceneGUIDelegate += SceneView_duringSceneGui;
#endif
        }

        private void OnDestroy()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -=  SceneView_duringSceneGui;
#else
            SceneView.onSceneGUIDelegate -= SceneView_duringSceneGui;
#endif
        }

        private void SceneView_duringSceneGui(SceneView sceneView)
        {
            DrawConnectors();
            if (editMode)
            {
                DrawConnectorsPath();
                DoConnectorHandles();
            }

            if (powerPole.HasInPointToOutPointConnection)
            {
                PreviewInPointToOutPointWire();
            }
        }

        private void DrawConnectors()
        {
            if (powerPole.mirror)
            {
                for (int i = 0; i < powerPole.connectors.Length; i++)
                {
                    Vector3 inPoint = powerPole.GetInPoint(i);
                    Vector3 outPoint = powerPole.GetOutPoint(i);
          
                    float inPointSize = HandleUtility.GetHandleSize(inPoint) * 0.1f;
                    float outPointSize = HandleUtility.GetHandleSize(outPoint) * 0.1f;

                    Handles.SphereHandleCap(i, inPoint, Quaternion.identity, inPointSize, Event.current.type);
                    Handles.SphereHandleCap(i, outPoint, Quaternion.identity, outPointSize, Event.current.type);

                    Handles.DrawLine(inPoint, outPoint);
                    bool error = Vector3.Distance(inPoint, outPoint) <= 0.001f;
                    if (error)
                    {
                        if (powerPole.connectionBetween)
                        {
                            string errorMsg = string.Format("Warning: InPoint_{0} & OutPoint_{0} Are Overplaing!", i);
                            Handles.Label(inPoint, errorMsg, errorLableStyle);
                        }
                        else
                        {
                            Handles.Label(inPoint, " Point_" + i, lableStyle);
                        }
                    }
                    else
                    {
                        Handles.Label(inPoint, " InPoint_" + i, lableStyle);
                        Handles.Label(outPoint, " OutPoint_" + i, lableStyle);
                    }
                }
            }
            else
            {
                for (int i = 0; i < powerPole.connectors.Length; i++)
                {
                    Vector3 position = powerPole.connectors[i];
                    position = transform.TransformPoint(position);//Local To World
                    Handles.Label(position, " Point_" + i, lableStyle);

                    float handleSize = HandleUtility.GetHandleSize(position) * 0.1f;
                    Handles.SphereHandleCap(i, position, Quaternion.identity, handleSize, Event.current.type);
                }
            }
        }

        private void DrawConnectorsPath()
        {
            Vector3[] connectorsWorld = new Vector3[powerPole.connectors.Length];
            for (int i = 0; i < powerPole.connectors.Length; i++)
            {
                Vector3 position = powerPole.connectors[i];
                position = transform.TransformPoint(position);//Local To World
                connectorsWorld[i] = position;
            }

            Handles.DrawAAPolyLine(3,connectorsWorld);
        }

        private void DoConnectorHandles()
        {
            for (int i = 0; i < powerPole.connectors.Length; i++)
            {
                Vector3 position = powerPole.connectors[i];
                position = transform.TransformPoint(position);//Local To World
                EditorGUI.BeginChangeCheck();
                position = Handles.DoPositionHandle(position, transform.rotation);
                if (EditorGUI.EndChangeCheck())
                {
                    position = transform.InverseTransformPoint(position);//World To Local
                    Undo.RecordObject(powerPole, "Move Connector");
                    powerPole.connectors[i] = position;
                }
            }
        }

        private void PreviewInPointToOutPointWire()
        {
            Wire wire = powerPole.betweenWire;
            Vector3[] wireSegments = new Vector3[wire.segments + 1];

            int wireCount = powerPole.connectors.Length;
            for (int w = 0; w < wireCount; w++)
            {
                Vector3 a = powerPole.GetInPoint(w);
                Vector3 b = powerPole.GetOutPoint(w);

                Vector3 prePoint = 2 * a - Vector3.Lerp(a, b, (float)1 / wire.segments);
                Vector3 c = (a + b) / 2 + Vector3.down * wire.h;

                for (int s = 0; s <= wire.segments; s++)
                {
                    float t = (float)s / wire.segments;
                    Vector3 currentPoint = Vector3.Lerp(Vector3.Lerp(a, c, t), Vector3.Lerp(c, b, t), t);
                    wireSegments[s] = currentPoint;
                    prePoint = currentPoint;
                }

                Handles.DrawAAPolyLine(2, wireSegments);
            }
        }

        public override void OnInspectorGUI ()
		{
           // base.OnInspectorGUI ();
            DrawEditModeButton("Edit Connectors");

            EditorGUILayout.PropertyField(property_connectors);
            EditorGUILayout.PropertyField(property_mirror,new GUIContent("Two Sided (InPoint & OutPoint)"));
            EditorGUILayout.PropertyField(property_connectionBetween, new GUIContent("Connect InPoint To OutPoint"));
            EditorGUILayout.PropertyField(property_betweenWire, new GUIContent("InPoint To OutPoint Wire"));

            serializedObject.ApplyModifiedProperties();

            if (powerPole.mirror)
            {
                if (powerPole.connectionBetween)
                {
                    if (powerPole.betweenWire == null)
                    {
                        string msg = "The Wire is null!";
                        EditorGUILayout.HelpBox(msg, MessageType.Error);
                    }
                    else
                    {
                        if (powerPole.betweenWire.material == null)
                        {
                            string msg = (powerPole.betweenWire.name.ToLower().Contains("lod")) ?
                                "You can't use LOD wire" : "The wire material is null!";
                            EditorGUILayout.HelpBox(msg, MessageType.Error);
                        }
                    }
                }
            }
		}

        private void DrawEditModeButton(string text)
        {
            GUILayout.BeginHorizontal();
            editMode = GUILayout.Toggle(editMode, EditorGUIUtility.IconContent("d_EditCollider"), GUI.skin.button, GUILayout.Width(35), GUILayout.Height(25));
            GUILayout.Label(text, new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft }, GUILayout.Height(28));
            GUILayout.EndHorizontal();
        }
	}
}