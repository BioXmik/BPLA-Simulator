using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ScriptBoy.PowerLines
{
	[CustomEditor(typeof(Powerlines))]
	public class PowerlinesEditor : Editor 
	{
        #region Variables
        private Texture iconMainTool , iconMoveTool , iconRotateTool;
		private const string powerlinesFolder = "Assets/ScriptBoy/PowerLines/";
        private const string wiresFolder = powerlinesFolder +"src/Wires";
        private const string polesFolder = powerlinesFolder + "src/Poles";
        private const string LODFolder = powerlinesFolder + "src/Wires/LODs";
        private const string iconsFolder = powerlinesFolder + "Core/Icons/";

        [SerializeField] private int toolTab = -1;
        [SerializeField] private bool verticalPole;

		private Vector2 scrollPosition;
		private Vector3 pivot;		
		private Quaternion Qx90 = Quaternion.Euler (90, 0, 0);


		private Powerlines powerlines;
		private PowerPole activePole;
		private Connection activeConnection;

        [SerializeField] private List<PowerPole> poles;
        [SerializeField] private List<Connection> connections;
        [SerializeField] private List<Serviceline> servicelines;
        private List<Wire> wires = new List<Wire>();
        private Color[] colors = new Color[] { Color.yellow, Color.red, Color.blue, Color.magenta, Color.green, Color.white, Color.black };
 
        private PowerPole currentPrefab;
		private int currentPrefabIndex;

		private Wire currentWire;
		private int currentWireIndex;

		private bool ServicelineEditor;
		private Serviceline currentServiceline;
		private Serviceline activeServiceline;

		private Vector3[] _circle;
        private Vector3[] circle
        {
            get
            {
                if (_circle == null || _circle.Length == 0)
                {
                    List<Vector3> list = new List<Vector3>();
                    float a = 0;
                    while (a < 360)
                    {
                        list.Add(Quaternion.AngleAxis(a, Vector3.forward) * Vector3.right);
                        a += 20;
                    }
                    _circle = list.ToArray();
                }
                return _circle;
            }
        }

        #endregion

        #region OnEnable & OnDestroy

        private void OnEnable()
		{
			powerlines = target as Powerlines;
			powerlines.poles = FindObjectsOfType<PowerPole> ();
			if (powerlines.name == "GameObject") powerlines.name = "Powerlines";
			if (powerlines._Renderer == null) 
			{
				PowerlinesRenderer ren = FindObjectOfType<PowerlinesRenderer> ();
				if (ren == null) 
				{
					GameObject g = new GameObject ("Powerlines Renderer",typeof(PowerlinesRenderer));
					powerlines._Renderer = g.GetComponent<PowerlinesRenderer> ();
				}
				else 
				{
					powerlines._Renderer = ren;
				}
			}

			iconMainTool = LoadIcon ("MainTool");
			iconMoveTool = LoadIcon ("MoveTool");
			iconRotateTool = LoadIcon ("RotateTool");

			LoadAssets ();

			poles = (powerlines.poles == null) ? new List<PowerPole> () : new List<PowerPole> (powerlines.poles) ;

			if (powerlines.connections == null) 
			{
				powerlines.connections = new Connection[0];
			}
			if (powerlines.servicelines == null) 
			{
				powerlines.servicelines = new Serviceline[0];
			}

			connections =  new List<Connection> (powerlines.connections) ;
			servicelines =  new List<Serviceline> (powerlines.servicelines) ;

			currentPrefab = (powerlines.polePrefabs.Length > 0) ? powerlines.polePrefabs [0] : null;
			currentPrefabIndex = 0;
			currentWire = (powerlines.wires.Length > 0) ? powerlines.wires [0] : null;
			currentWireIndex = 0;

			activePole = null ;
			activeConnection = null;

			currentServiceline = null;
			activeServiceline = null;

			ServicelineEditor = false;

			#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui -= this._OnSceneGUI;
			SceneView.duringSceneGui += this._OnSceneGUI;
			#else
			SceneView.onSceneGUIDelegate -= this._OnSceneGUI;
			SceneView.onSceneGUIDelegate += this._OnSceneGUI;
			#endif

		}

        private void OnDestroy ()
		{
			
			#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui -= this._OnSceneGUI;
			#else
			SceneView.onSceneGUIDelegate -= this._OnSceneGUI;
			#endif
		}
        #endregion

        #region OnSceneGUI
        private void _OnSceneGUI(SceneView sceneView)
		{
			if (toolTab != -1) 
			{		
				HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

				Event e = Event.current;

				if (e.isKey && e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete) 
				{
					if (activePole != null)Menu_RemovePole (activePole);
					e.Use ();
					return;
				}

				ArraysNullCheck ();

				if (toolTab == 0) 
				{
					if (e.shift || ServicelineEditor) 
					{
						DoServicelineEditor ();
					}
					else 
					{
						DoMainToolEditor ();
					}
				}
				else if (toolTab == 1) 
				{
					DoMoveToolEditor ();
				}
				else if (toolTab == 2)
				{
					DoRotateToolEditor ();
				}

				DoWirePreview ();
			}
		}

        private void DoMainToolEditor()
		{
			Event e = Event.current;

			for (int i = 0; i < poles.Count; i++) 
			{
				PowerPole pole = poles [i];
				Transform poleTran = pole.transform;
				Vector3 v = poleTran.position;
				float handleSize = HandleUtility.GetHandleSize (v);

				Handles.color = Color.green;

				if (IsPointInsideView (v))
				{
					Handles.ArrowHandleCap (0, v, poleTran.rotation, handleSize * 0.5f, EventType.Repaint);
					if (Handles.Button (v, poleTran.rotation * Quaternion.Euler (90, 0, 0), handleSize * 0.2f, 1, Handles.CircleHandleCap)) {
						if (activePole != null && activePole != pole) {
							GenericMenu menu = new GenericMenu ();
							menu.AddItem (new GUIContent ("Do nothing"), true, () => {});
							menu.AddItem (new GUIContent ("Select"), true, Menu_Select, pole);
							Connection newConnection = new Connection (activePole, pole , null);
							if (IsNewConnection (newConnection)) 
							{
								menu.AddItem (new GUIContent ("Connect"), true, Menu_ConnectPoles, newConnection);
								activeConnection = null;
							} 
							else 
							{
								menu.AddItem (new GUIContent ("Disconnect"), true, Menu_RemoveActiveConnection, pole);
							}
							menu.ShowAsContext ();
							e.Use ();
						
							return;
						} else {
							Undo.RecordObject (this, "Selection");
							activePole = (activePole == poles [i]) ? null : pole;
						}
					}
				}
			}

			Handles.color = Color.red;

			RaycastHit hit;
			if (Physics.Raycast (HandleUtility.GUIPointToWorldRay (e.mousePosition), out hit, 1000)) 
			{
				Vector3 p = hit.point;
				float handleSize = HandleUtility.GetHandleSize (p) / 5;
				Handles.CircleHandleCap (0, p, Quaternion.LookRotation (hit.normal), handleSize * 0.4f, EventType.Repaint);
				pivot = p;

				if (activePole != null) {
					Handles.DrawLine (activePole.transform.position, p);
				}

				if (e.isMouse && e.type == EventType.MouseDown && e.button == 0) {
					GenericMenu menu = new GenericMenu ();
					menu.AddItem (new GUIContent ("Do nothing"), true, () => {
					});
					if (activePole != null)
						menu.AddItem (new GUIContent ("Deselect"), true, Menu_Deselect);
					menu.AddItem (new GUIContent ("New Pole"), true, Menu_NewPole);
					menu.ShowAsContext ();
					e.Use ();
					return;
				}
			}

			if (activePole != null)
			{
				Transform poleTran = activePole.transform;
				Vector3 v = poleTran.position;
				float handleSize = HandleUtility.GetHandleSize (v) * 0.2f;
				Handles.CircleHandleCap (0, v, poleTran.rotation * Quaternion.Euler (90, 0, 0), handleSize, EventType.Repaint);
			}

			SceneView.RepaintAll ();
		}

        private void DoMoveToolEditor()
		{
			Handles.color = Color.green;
			for (int i = 0; i < poles.Count; i++) 
			{
				PowerPole pole = poles [i];
				Transform poleTran = pole.transform;
				Vector3 v = poleTran.position;
				float handleSize = HandleUtility.GetHandleSize (v);
				if (IsPointInsideView (v))
				{
					Handles.ArrowHandleCap (0, v, poleTran.rotation, handleSize * 0.5f, EventType.Repaint);
					if (Handles.Button (v, poleTran.rotation * Quaternion.Euler (90, 0, 0), handleSize * 0.2f, 1, Handles.CircleHandleCap)) 
					{
						Undo.RecordObject (this,"Selection");
						activePole = pole;
						break;
					}
				}
			}

			if (activePole != null) 
			{
				Transform poleTran = activePole.transform;
				Vector3 p = poleTran.position;
				Quaternion q = poleTran.rotation;
				float handleSize = HandleUtility.GetHandleSize (p) * 0.2f;
				Handles.color = Color.red;
				Handles.CircleHandleCap (0, p, poleTran.rotation * Quaternion.Euler (90, 0, 0), handleSize, EventType.Repaint);

				EditorGUI.BeginChangeCheck ();
				p = Handles.FreeMoveHandle (p, poleTran.rotation * Qx90, handleSize, Vector3.one, Handles.CircleHandleCap);
				if (EditorGUI.EndChangeCheck ()) 
				{
					Undo.RecordObject (poleTran, "Move");
					RaycastHit hit;
					if (Physics.Raycast (p + Vector3.up * 2000, Vector3.down, out hit, 3000)) 
					{
						p = hit.point;
						if (verticalPole)
						{
							poleTran.rotation = Quaternion.LookRotation ((Event.current.shift) ? GetZoneNormal (p, handleSize) : hit.normal, -poleTran.forward) * Qx90;
						}
					} 
					else 
					{
						p.y = 0;
					}
					poleTran.position = p + new Vector3(0,powerlines.transform.position.y);
					powerlines.Update ();
				}
			}
		}

        private void DoRotateToolEditor()
		{
			Handles.color = Color.green;
			for (int i = 0; i < poles.Count; i++) 
			{
				PowerPole pole = poles [i];
				Transform poleTran = pole.transform;
				Vector3 v = poleTran.position;
				float handleSize = HandleUtility.GetHandleSize (v);
				if (IsPointInsideView (v))
				{
					Handles.ArrowHandleCap (0, v, poleTran.rotation, handleSize * 0.5f, EventType.Repaint);
					if (Handles.Button (v, poleTran.rotation * Quaternion.Euler (90, 0, 0), handleSize * 0.2f, 1, Handles.CircleHandleCap)) 
					{
						Undo.RecordObject (this,"Selection");
						activePole = pole;
						break;
					}
				}
			}

			if (activePole != null) 
			{
				Transform poleTran = activePole.transform;
				Vector3 p = poleTran.position;
				Quaternion q = poleTran.rotation;
				float handleSize = HandleUtility.GetHandleSize (p) * 0.2f;
				Handles.color = Color.red;
				Handles.CircleHandleCap (0, p, poleTran.rotation * Quaternion.Euler (90, 0, 0), handleSize, EventType.Repaint);

				EditorGUI.BeginChangeCheck ();
				q = Handles.DoRotationHandle (q, p);
				if (EditorGUI.EndChangeCheck ()) 
				{
					Undo.RecordObject (poleTran, "Rotate");
					poleTran.rotation = q;
				}
			}
		}

        private void DoServicelineEditor()
		{
			Event e = Event.current;
			if(activeConnection != null)
			{
		
				Vector3 a = activeConnection.a.transform.position;
				Vector3 b = activeConnection.b.transform.position;
				Vector3 center = (a + b) / 2;
				float handleSize = HandleUtility.GetHandleSize (center)*0.2f;
				Handles.color = Color.green;
				Handles.DrawLine (a, b);
				Handles.SphereHandleCap (0, center, Qx90, handleSize, EventType.Repaint);

				if (e.isMouse && e.button == 0 && e.type == EventType.MouseDown) 
				{
					activeConnection = null; 
					activeServiceline = null;
					ServicelineEditor = false;
					return;
				}
			}
			else if (activeServiceline != null) 
			{
				Handles.color = Color.green;
				EditorGUI.BeginChangeCheck ();
				Vector3 a = Handles.DoPositionHandle (activeServiceline.aWorldPosition, Quaternion.identity);

				Handles.DrawLine (a, activeServiceline.aTran.position);

				if (EditorGUI.EndChangeCheck ()) {
					activeServiceline.aWorldPosition = a;
					powerlines.Update ();
				}

				EditorGUI.BeginChangeCheck ();
				Vector3 b = Handles.DoPositionHandle (activeServiceline.bWorldPosition, Quaternion.identity);

				Handles.DrawLine (b, activeServiceline.bTran.position);
				if (EditorGUI.EndChangeCheck ()) {
					activeServiceline.bWorldPosition = b;
					powerlines.Update ();
				}

				if (e.isMouse && e.button == 0 && e.type == EventType.MouseDown) 
				{
					activeServiceline = null; 
					ServicelineEditor = false;
					return;
				}

			} 
			else
			{

				bool buttonClicked = false;
				Transform t = null;
				Vector3 p = Vector3.zero;

				Handles.color = Color.yellow;

				for (int i = 0; i < poles.Count && !buttonClicked; i++) 
				{
					PowerPole pole = poles [i];
					t = pole.transform;
					float handleSize = HandleUtility.GetHandleSize (pole.transform.position);
					for (int ii = 0; ii < pole.connectors.Length; ii++) 
					{
						p = pole.GetInPoint (ii);
						if (IsPointInsideView (p)) {
							if (Handles.Button (p, Quaternion.identity, handleSize * 0.1f, 0, Handles.CubeHandleCap)) 
							{
								buttonClicked = true;
								break;
							}
						}
						if (pole.mirror) {
							p = pole.GetOutPoint (ii);
							if (IsPointInsideView (p)) {
								if (Handles.Button (p, Quaternion.identity, handleSize * 0.1f, 0, Handles.CubeHandleCap)) 
								{
									buttonClicked = true;
									break;
								}
							}
						} 
					}
				}

				Handles.color = Color.green;

				if (!ServicelineEditor) 
				{
					for (int i = 0; i < servicelines.Count && !buttonClicked; i++) 
					{
						var s = servicelines [i];
						Vector3 c = s.abCenterWorldPosition;
						float handleSize = HandleUtility.GetHandleSize (c);

						if (IsPointInsideView (c)) {
							if (Handles.Button (c, Quaternion.identity, handleSize * 0.2f, 0, Handles.SphereHandleCap)) {
								GenericMenu menu = new GenericMenu ();
								menu.AddItem (new GUIContent ("Do nothing"), true, () => {
								});
								menu.AddItem (new GUIContent ("Edit Points"), true, Menu_EditServiceline, s);
								menu.AddItem (new GUIContent ("Change Wire"), true, Menu_ChangeServicelineWire, s);
								menu.AddItem (new GUIContent ("Remove"), true, Menu_RemoveServiceline, s);
								menu.ShowAsContext ();

								return;
							}
						}
					}

					for (int i = 0; i < connections.Count && !buttonClicked; i++) 
					{
						var c = connections [i];

						Vector3 a = c.a.transform.position;
						Vector3 b = c.b.transform.position;
						Vector3 center = (a + b) / 2;

						float handleSize = HandleUtility.GetHandleSize (center);
						Handles.DrawLine (a, b);

						if (IsPointInsideView (center)) {
							if (Handles.Button (center, Quaternion.identity, handleSize * 0.2f, 0, Handles.SphereHandleCap)) {
								GenericMenu menu = new GenericMenu ();
								menu.AddItem (new GUIContent ("Do nothing"), true, () => {
								});
								menu.AddItem (new GUIContent ("Change Wire"), true, Menu_ChangeConnectionWire, c);
								menu.AddItem (new GUIContent ("Remove"), true, Menu_RemoveConnection, c);
								menu.ShowAsContext ();

								return;
							}
						}
					}
				}

				RaycastHit hit;
				if (Physics.Raycast (HandleUtility.GUIPointToWorldRay (e.mousePosition), out hit, 1000)) {	
					if (e.isMouse && e.button == 0 && e.type == EventType.MouseDown) 
					{
						p = hit.point;
						t = hit.transform;
						buttonClicked = true;
						e.Use ();
					}

					if (ServicelineEditor) {
						DrawWireLine (currentServiceline.aWorldPosition, hit.point, currentWire.h, GetWireEditorColor (currentWire));
					}
				}


				if (buttonClicked) 
				{
					pivot = p;

					if (ServicelineEditor) 
					{
						GenericMenu menu = new GenericMenu ();
						menu.AddItem (new GUIContent ("Do nothing"), true, () => {
						});
						menu.AddItem (new GUIContent ("Discard"), true, Menu_DiscardServiceline);
						menu.AddItem (new GUIContent ("Restart"), true, Menu_RestartServiceline, t);
						menu.AddItem (new GUIContent ("Save Serviceline"), true, Menu_SaveServiceline, t);
						menu.ShowAsContext ();
						e.Use ();
					} 
					else 
					{
						ServicelineEditor = true;
						currentServiceline = new Serviceline ();
						currentServiceline.aTran = t.transform;
						currentServiceline.aP = currentServiceline.aTran.InverseTransformPoint (pivot);
					}
				}
			}

			SceneView.RepaintAll ();
		}

        private void DoWirePreview()
		{
			if (connections != null) 
			{
				for (int i = 0; i < connections.Count; i++) 
				{
					Connection c = connections [i];

					int aL = c.a.connectors.Length;
					int bL = c.b.connectors.Length;

					int LMin = Mathf.Min(aL,bL);
					int LMax = Mathf.Max(aL,bL);

					float Aa = GetAngleXZ (c.a.transform.forward);
					float Ab = GetAngleXZ (c.b.transform.forward);
					float Ac = GetAngleXZ (c.b.transform.position - c.a.transform.position);

					float Dab = Mathf.Abs(Mathf.DeltaAngle (Aa , Ab));
					float Dca = Mathf.Abs(Mathf.DeltaAngle (Ac , Aa));
					float Dcb = Mathf.Abs(Mathf.DeltaAngle (Ac , Ab));
					float D = Mathf.Abs(Mathf.DeltaAngle (Dca , Dcb));

					//Debug.Log (string.Format("Fa{0} Fb{1} Fc{2} ||Dab{3} Dca{4} Dcb{5} D{6}",Fa,Fb,Fc,Dab,Dca,Dcb,D));

					float h = c.wire.h;

					Color color = GetWireEditorColor (c.wire);


					if (Dab - D > 90 || Dca > 90 && Dcb < 90 || Dcb > 90 && Dca < 90) 
					{
						for (int ii = 0; ii < LMin; ii++) 
						{
							//DrawWireLine ((Dca < 90)? c.a.GetOutPoint (LMin - 1 - ii) : c.a.GetInPoint (LMin - 1 - ii),(Dcb < 90)? c.b.GetInPoint (ii):c.b.GetOutPoint (ii));

							int ia = GetIndex(ii,LMin,LMax,aL , true);
							int ib = GetIndex(ii,LMin,LMax,bL , false);
							DrawWireLine ((Dca < 90)? c.a.GetOutPoint (ia) : c.a.GetInPoint (ia),(Dcb < 90)? c.b.GetInPoint (ib):c.b.GetOutPoint (ib),h,color);

						}
					} 
					else 
					{
						for (int ii = 0; ii < LMin; ii++) 
						{
							//DrawWireLine ((Dca < 90)? c.a.GetOutPoint (ii) : c.a.GetInPoint (ii) ,(Dcb < 90)? c.b.GetInPoint (ii) : c.b.GetOutPoint (ii));

							int ia = GetIndex(ii,LMin,LMax,aL , false);
							int ib = GetIndex(ii,LMin,LMax,bL , false);
							DrawWireLine ((Dca < 90)? c.a.GetOutPoint (ia) : c.a.GetInPoint (ia) ,(Dcb < 90)? c.b.GetInPoint (ib) : c.b.GetOutPoint (ib),h,color);

						}
					}
				}
			}

			if (servicelines != null) 
			{
				for (int i = 0; i < servicelines.Count; i++) 
				{
					var c = servicelines [i];
					DrawWireLine (c.aWorldPosition, c.bWorldPosition, c.wire.h, GetWireEditorColor (c.wire));
				}
			}
		}

        private void DrawWireLine(Vector3 a, Vector3 b, float h, Color color)
        {
            Vector3 c = (a + b) / 2 + Vector3.down * h;
            Handles.DrawBezier(a, b, Vector3.Lerp(a, c, (float)2 / 3), Vector3.Lerp(b, c, (float)2 / 3), color, null, 2);
        }
        #endregion

        #region OnInspectorGUI
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("useLODTechnique"));
            serializedObject.ApplyModifiedProperties();

            OnToolbarGUI();
            if (toolTab == 0)
            {
                OnMainToolGUI();
            }
            else if (toolTab == 1)
            {
                OnMoveToolGUI();
            }
            else if (toolTab == 2)
            {

            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            OnBottombarGUI();
        }

        private void OnToolbarGUI()
        {
            EditorGUI.BeginChangeCheck();
            int _tab = GUILayout.Toolbar(toolTab, new Texture[] { iconMainTool, iconMoveTool, iconRotateTool });
            if (toolTab != _tab)
            {
                Undo.RecordObject(this, "Toolbar changed");
                toolTab = _tab;
            }
            else if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Toolbar changed");
                toolTab = -1;
            }
        }

        private void OnMainToolGUI()
        {
            PowerPole[] prefabs = powerlines.polePrefabs;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUI.skin.window, GUILayout.Height(140));
            GUILayout.BeginHorizontal();
            for (int i = 0; i < prefabs.Length; i++)
            {
                PowerPole p = prefabs[i];
                Texture2D icon = AssetPreview.GetAssetPreview(p.gameObject);
                int width = (currentPrefabIndex == i) ? 100 : 68;

                if (GUILayout.Button(icon, GUILayout.Width(width), GUILayout.Height(width)))
                {
                    Undo.RecordObject(this, "Select prefab");
                    currentPrefab = p;
                    currentPrefabIndex = i;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();


            Wire[] wires = powerlines.wires;
            string[] _wires = new string[wires.Length];

            for (int i = 0; i < wires.Length; i++)
            {
                Wire c = wires[i];
                _wires[i] = c.name + " Seg:" + c.segments + " R:" + c.radius + " H:" + c.h;
            }

            EditorGUI.BeginChangeCheck();
            int _currentWireIndex = EditorGUILayout.Popup("Current Wire", currentWireIndex, _wires);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.SetCurrentGroupName("Wire changed");
                int undoGroupIndex = Undo.GetCurrentGroup();

                Undo.RecordObject(this, "");
                currentWireIndex = _currentWireIndex;
                currentWire = wires[currentWireIndex];

                if (ServicelineEditor)
                {
                    if (activeConnection != null)
                    {
                        activeConnection.wire = currentWire;
                        Undo.RecordObject(powerlines._Renderer, "");
                        powerlines.Update();
                    }
                    else if (activeServiceline != null)
                    {
                        activeServiceline.wire = currentWire;
                        Undo.RecordObject(powerlines._Renderer, "");
                        powerlines.Update();
                    }
                }

                Undo.CollapseUndoOperations(undoGroupIndex);
            }

            Rect rect = GUILayoutUtility.GetRect(50, 50);
            rect.width += 2;
            rect.xMin = 0;
            if (GUI.Button(rect, AssetPreview.GetAssetPreview(currentWire.material)))
            {
                WireEditor.pl = powerlines;
                Selection.activeObject = currentWire;
            }

        }

        private void OnMoveToolGUI()
        {
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            bool _beVertical = GUILayout.Toggle(verticalPole, "Vertical");
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Vertical");
                verticalPole = _beVertical;
            }

            if (verticalPole)
            {
                EditorGUILayout.HelpBox("Hold Shift for rough surface ", MessageType.Info);
            }
        }

        private void OnBottombarGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Update"))
            {
                ArraysNullCheck();
                if (powerlines.poles != null)
                {
                    Undo.SetCurrentGroupName("Update");
                    int undoGroupIndex = Undo.GetCurrentGroup();

                    foreach (var e in powerlines.poles)
                    {
                        Vector3 v = e.transform.position;
                        RaycastHit hit;
                        if (Physics.Raycast(v + Vector3.up * 2000, Vector3.down, out hit, 3000))
                        {
                            Undo.RecordObject(e.transform, "");
                            e.transform.position = hit.point + new Vector3(0, powerlines.transform.position.y);
                        }
                    }
                    Undo.CollapseUndoOperations(undoGroupIndex);
                }
                LoadAssets();
                powerlines.Update();
            }
            if (GUILayout.Button("Clear"))
            {
                if (powerlines.poles != null)
                {
                    Undo.SetCurrentGroupName("Clear");
                    int undoGroupIndex = Undo.GetCurrentGroup();

                    PowerPole[] array = powerlines.poles;

                    Undo.RecordObject(this, "");

                    poles = new List<PowerPole>();
                    connections = new List<Connection>();
                    servicelines = new List<Serviceline>();

                    activeConnection = null;
                    activePole = null;

                    Undo.RecordObject(powerlines, "");
                    powerlines.connections = new Connection[0];
                    powerlines.servicelines = new Serviceline[0];
                    powerlines.poles = new PowerPole[0];

                    foreach (var e in array)
                    {
                        Undo.DestroyObjectImmediate(e.gameObject);
                    }

                    Undo.RecordObject(powerlines._Renderer, "");
                    powerlines._Renderer.Clear();

                    Undo.CollapseUndoOperations(undoGroupIndex);
                }
            }
            GUILayout.EndHorizontal();
        }
        #endregion

        #region Tools/ScriptBoy/PowerLines
        [UnityEditor.MenuItem("Tools/ScriptBoy/PowerLines/Start", false, 0)]
        private static void CreatePowerLines()
        {
            GameObject g = new GameObject("Powerlines", typeof(Powerlines));
            g.transform.position = new Vector3(2000, 0, 0);
            Undo.RegisterCreatedObjectUndo(g, "Powerlines Created");
        }

        [UnityEditor.MenuItem("Tools/ScriptBoy/PowerLines/Start", true, 0)]
        private static bool _CreatePowerLines()
        {
            return FindObjectOfType<Powerlines>() == null;
        }

        [UnityEditor.MenuItem("Tools/ScriptBoy/PowerLines/Connect", false, 1)]
        private static void CreateServiceline()
        {
            Transform[] ab = Selection.transforms;
            Powerlines pl = FindObjectOfType<Powerlines>();
            if (pl != null && pl.wires != null && pl.wires.Length > 0)
            {

                Serviceline s = new Serviceline();
                s.aTran = ab[0];
                s.bTran = ab[1];
                s.wire = pl.wires[0];
                List<Serviceline> servicelines = (pl.servicelines == null) ? new List<Serviceline>() : new List<Serviceline>(pl.servicelines);
                servicelines.Add(s);

                Undo.SetCurrentGroupName("New Serviceline");
                int undoGroupIndex = Undo.GetCurrentGroup();

                Undo.RecordObject(pl, "");
                pl.servicelines = servicelines.ToArray();
                Undo.RecordObject(pl._Renderer, "");
                pl.Update();

                Undo.CollapseUndoOperations(undoGroupIndex);
            }
        }

        [UnityEditor.MenuItem("Tools/ScriptBoy/PowerLines/Connect", true, 1)]
        private static bool _CreateServiceline()
        {
            return FindObjectOfType<Powerlines>() != null && Selection.gameObjects != null && Selection.gameObjects.Length == 2;
        }

        [UnityEditor.MenuItem("Tools/ScriptBoy/PowerLines/Components/PowerPole", false, 0)]
        private static void AddPowerPoleComponent()
        {
            Selection.activeGameObject.AddComponent<PowerPole>();
        }

        [UnityEditor.MenuItem("Tools/ScriptBoy/PowerLines/Components/PowerPole", true, 0)]
        private static bool _AddPowerPoleComponent()
        {
            return Selection.activeGameObject != null;
        }

        [UnityEditor.MenuItem("Tools/ScriptBoy/PowerLines/Components/Wind", false, 1)]
        private static void AddWindComponent()
        {
            Selection.activeGameObject.AddComponent<Wind>();
        }

        [UnityEditor.MenuItem("Tools/ScriptBoy/PowerLines/Components/Wind", true, 1)]
        private static bool _AddWindComponent()
        {
            return Selection.activeGameObject != null;
        }
        #endregion

        #region Menu Functions
        public void Menu_SaveServiceline(object transform)
		{
			Undo.SetCurrentGroupName ("SaveServiceline");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			Undo.RecordObject (this, "");
			currentServiceline.bTran = (Transform)transform;
			currentServiceline.bP = currentServiceline.bTran.InverseTransformPoint (pivot);
			currentServiceline.wire = currentWire;
			servicelines.Add (currentServiceline);
			currentServiceline = null;
			ServicelineEditor = false;
			Undo.RecordObject (powerlines, "");
			powerlines.servicelines = servicelines.ToArray ();
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}

		public void Menu_RestartServiceline(object transform)
		{
			Undo.RecordObject (this,"RestartServiceline");
			currentServiceline.aTran = (Transform)transform;
			currentServiceline.aP = currentServiceline.aTran.InverseTransformPoint (pivot);
		}

		public void Menu_ChangeServicelineWire(object serviceline)
		{
			Undo.SetCurrentGroupName ("ChangeServicelineWire");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			Undo.RecordObject (this,"");
			activeServiceline = (Serviceline)serviceline;
			ServicelineEditor = true;
			activeServiceline.wire = currentWire;
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}

		public void Menu_EditServiceline(object serviceline)
		{
			Undo.RecordObject (this,"EditServiceline");
			activeServiceline = (Serviceline)serviceline;
			ServicelineEditor = true;
			currentWire = activeServiceline.wire;

		}

		public void Menu_RemoveServiceline(object serviceline)
		{
			Undo.SetCurrentGroupName ("RemoveServiceline");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			Undo.RecordObject (this,"");
			servicelines.Remove ((Serviceline)serviceline);
			Undo.RecordObject (powerlines,"");
			powerlines.servicelines = servicelines.ToArray ();
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}

		public void Menu_DiscardServiceline()
		{
			Undo.RecordObject (this,"DiscardServiceline");
			currentServiceline = null;
			ServicelineEditor = false;
		}

		public void Menu_ConnectPoles(object connection)
		{
			Undo.SetCurrentGroupName ("New Connection");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			Connection c = (Connection)connection;
			c.wire = currentWire;
			Undo.RecordObject (this,"");
			activePole = c.b;
			connections.Add (c);
			Undo.RecordObject (powerlines,"");
			powerlines.connections = connections.ToArray ();
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}
			
		public void Menu_ChangeConnectionWire(object connection)
		{
			Undo.SetCurrentGroupName ("ChangeConnectionWire");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			Undo.RecordObject (this , "");
			activeConnection = (Connection)connection;
			activeConnection.wire = currentWire;
			ServicelineEditor = true;
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}

		public void Menu_RemoveConnection(object connection)
		{
			Undo.SetCurrentGroupName ("Remove Connection");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			Undo.RecordObject (this,"");
			connections.Remove ((Connection)connection);
			Undo.RecordObject (powerlines,"");
			powerlines.connections = connections.ToArray ();
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}

		public void Menu_RemoveActiveConnection(object nextActivePole)
		{
			Undo.SetCurrentGroupName ("Remove Connection");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			Undo.RecordObject (this,"");
			connections.Remove (activeConnection);
			activePole = (PowerPole)nextActivePole;
			Undo.RecordObject (powerlines,"");
			powerlines.connections = connections.ToArray ();
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}

		public void Menu_Select(object pole)
		{
			Undo.RecordObject (this,"Pole Select");
			activePole = (PowerPole)pole;
		}

		public void Menu_Deselect()
		{
			Undo.RecordObject (this,"Pole Deselect");
			activePole = null;
		}

		public void Menu_NewPole()
		{
			if (currentPrefab == null || currentPrefabIndex > powerlines.polePrefabs.Length - 1) 
			{
				Debug.LogWarning ("No powerploe selected");
				return;
			}

			Undo.SetCurrentGroupName ("New Pole");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			PowerPole pole = PrefabUtility.InstantiatePrefab (currentPrefab) as PowerPole;
			pole.transform.position = pivot + new Vector3(0,powerlines.transform.position.y);
			Undo.RegisterCreatedObjectUndo (pole.gameObject,"");
			Undo.RecordObject (this,"");
			poles.Add(pole);
			if (activePole != null)
			{
				Vector3 q = Quaternion.LookRotation (pivot - activePole.transform.position).eulerAngles;
				q = new Vector3 (Mathf.LerpAngle(q.x,0,0.85f),q.y,Mathf.LerpAngle(q.z,0,0.85f));
				pole.transform.eulerAngles = q;

				if (connections.Find ((x) => x.a == activePole || x.b == activePole) == null)
				{
					Undo.RecordObject (activePole,"");
					activePole.transform.eulerAngles = q;
				}
				Undo.RecordObject (this,"");
				connections.Add (new Connection(activePole,pole,currentWire));
			}
			activePole = pole;
			Undo.RecordObject (powerlines,"");
			powerlines.connections = connections.ToArray ();
			powerlines.poles = poles.ToArray ();
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}

		void Menu_RemovePole(PowerPole pole)
		{
			Undo.SetCurrentGroupName ("Remove Pole");
			int undoGroupIndex = Undo.GetCurrentGroup ();

			Undo.RecordObject(this,"");
			poles.Remove (pole);
			if (connections.RemoveAll ((x) => x.a == pole || x.b == pole) > 0) 
			{
				Undo.RecordObject(powerlines,"");
				powerlines.connections = connections.ToArray ();
			}
			Undo.RecordObject(powerlines,"");
			powerlines.poles = poles.ToArray ();
			Undo.DestroyObjectImmediate (pole.gameObject);
			Undo.RecordObject (powerlines._Renderer, "");
			powerlines.Update ();

			Undo.CollapseUndoOperations (undoGroupIndex);
		}
        #endregion

        #region Load Assets

        private Texture LoadIcon(string name)
        {
            return EditorGUIUtility.Load(iconsFolder + name + ".png") as Texture;
        }

        private void LoadAssets()
		{
			powerlines.wires = LoadAssetsAtPath<Wire>(wiresFolder);
			if (powerlines.wires.Length > 0) 
			{
				currentWire = powerlines.wires [0];
				currentWireIndex = 0;;
			}

			powerlines.polePrefabs = LoadAssetsAtPath<PowerPole>(polesFolder);
			if (powerlines.polePrefabs.Length > 0) 
			{
				currentPrefab = powerlines.polePrefabs [0];
				currentPrefabIndex = 0;
			}

			if(powerlines.LODWires == null || powerlines.LODWires.Length == 0)
			{
				powerlines.LODWires = LoadAssetsAtPath<Wire>(LODFolder);
			}
		}

        private T[] LoadAssetsAtPath<T>(string path) where T : UnityEngine.Object
		{
			List<T> assetsFound = new List<T> ();
			string[] filePaths = System.IO.Directory.GetFiles(path);
			if (filePaths != null && filePaths.Length > 0)
			{
				for (int i = 0; i < filePaths.Length; i++)
				{
					UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(T));
					if (obj is T)
					{
						assetsFound.Add((T)obj);
					}
				}
			}
			return assetsFound.ToArray ();
		}

        #endregion

        #region Other Functions
        private Vector3 GetZoneNormal(Vector3 point, float radius)
        {
            float scale = radius * 1.5f;
            Vector3 sumNormal = Vector3.zero;
            int hitCount = 1;
            for (int i = 0; i < circle.Length - 1; i++)
            {
                Vector3 rayOrigin = point + Vector3.up * 100 + (Qx90 * (circle[i] * scale));
                RaycastHit hit2;
                if (Physics.Raycast(rayOrigin, Vector3.down, out hit2, 200))
                {
                    hitCount++;
                    sumNormal += hit2.normal;
                }
            }
            scale *= 0.5f;
            for (int i = 0; i < circle.Length - 1; i++)
            {
                Vector3 rayOrigin = point + Vector3.up * 100 + (Qx90 * (circle[i] * scale));
                RaycastHit hit2;
                if (Physics.Raycast(rayOrigin, Vector3.down, out hit2, 200))
                {
                    hitCount++;
                    sumNormal += hit2.normal;
                }
            }
            sumNormal /= hitCount;
            return sumNormal;
        }

        private Color GetWireEditorColor(Wire c)
		{
			if (!wires.Contains (c)) 
			{
				wires.Add (c);
			} 
			return colors [Mathf.Clamp (wires.IndexOf (c), 0, 7)];
		}

        private int GetIndex(int i , int min , int max , int L , bool reverse)
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

        private float GetAngleXZ(Vector3 v)
		{
			return Mathf.Atan2 (v.z, v.x) * Mathf.Rad2Deg;
		}

        private bool IsNewConnection(Connection c)
		{
			for (int i = 0; i < connections.Count; i++) 
			{
				Connection e = connections [i];
				if (e.a == c.a && e.b == c.b || e.a == c.b && e.b == c.a) 
				{
					activeConnection = e;
					return false;
				}
			}
			return true;
		}

        private void ArraysNullCheck()
		{
			if (poles == null) poles = (powerlines.poles == null) ? new List<PowerPole> () : new List<PowerPole> (powerlines.poles);
			if (connections == null) connections = (powerlines.connections == null) ? new List<Connection> () : new List<Connection> (powerlines.connections);
			if (servicelines == null) servicelines = (powerlines.servicelines == null) ? new List<Serviceline> () : new List<Serviceline> (powerlines.servicelines);

			bool needUpdate = false;
			if (poles.RemoveAll ((x)=> x == null) > 0) 
			{
				powerlines.poles = poles.ToArray ();needUpdate = true;
			}
			if (connections.RemoveAll ((x) => x == null || x.a == null || x.b == null || x.wire == null) > 0) 
			{
				powerlines.connections = connections.ToArray ();needUpdate = true;
			}
			if (servicelines.RemoveAll ((x)=> x == null || x.aTran == null || x.bTran == null || x.wire == null) > 0) 
			{
				powerlines.servicelines = servicelines.ToArray ();needUpdate = true;
			}
			if (needUpdate) 
			{
				powerlines.Update ();
			}
		}

        private bool IsPointInsideView(Vector3 point)
		{
			Vector2 p = HandleUtility.WorldToGUIPoint (point);
			return !(p.x < 0 || p.y < 0 || p.x > Screen.width || p.y > Screen.height);
        }
        #endregion
    }
}