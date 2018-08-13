using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityRockStudio;
using System.Collections;

public class PaintObject
{
    public GameObject prefab = null;
    public Vector2 scale = Vector2.one;
    public bool randomRotationX = false;
    public bool randomRotationY = false;
    public bool randomRotationZ = false;
    public Editor gameObjectEditor;

    public static void display(PaintObject obj)
    {
        if (obj == null) return;
        EditorGUILayout.BeginVertical(RockStudio.boxStyle);
        GUILayout.Space(3);

        EditorGUI.BeginChangeCheck();
        GameObject gameObject = obj.prefab;
        if (EditorGUI.EndChangeCheck())
        {
            if (obj.gameObjectEditor != null) Object.DestroyImmediate(obj.gameObjectEditor);
        }

        GUIStyle bgColor = new GUIStyle();
        bgColor.normal.background = RockStudio.previewBackgroundTexture;

        if (gameObject != null)
        {
            if (obj.gameObjectEditor == null)
                obj.gameObjectEditor = Editor.CreateEditor(gameObject);
            obj.gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(50, 50), bgColor);
        }

        EditorGUI.BeginChangeCheck();
        gameObject = (GameObject)EditorGUILayout.ObjectField("", obj.prefab, typeof(GameObject), true);
        obj.prefab = gameObject;
        if (EditorGUI.EndChangeCheck())
        {
            if (obj.gameObjectEditor != null) Object.DestroyImmediate(obj.gameObjectEditor);
        }

        if (obj.prefab != null)
        {
            obj.scale.x = EditorGUILayout.FloatField("Min Size", obj.scale.x);
            obj.scale.y = EditorGUILayout.FloatField("Max Size", obj.scale.y);
            GUILayout.Label("Random Rotation :");
            EditorGUILayout.BeginHorizontal();
            obj.randomRotationX = GUILayout.Toggle(obj.randomRotationX, "X");
            obj.randomRotationY = GUILayout.Toggle(obj.randomRotationY, "Y");
            obj.randomRotationZ = GUILayout.Toggle(obj.randomRotationZ, "Z");
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(3);
        EditorGUILayout.EndVertical();
        GUILayout.Space(0);
    }
}

public class RockStudio : EditorWindow
{
    public static Editor gameObjectEditor;
    GameObject gameObject;
    readonly Rect dragWindowRect = new Rect(0, 0, 128, 20);
    Rect windowRect = new Rect(24, 24, 95, 217);


    public static Texture2D previewBackgroundTexture;
    public Texture2D[] sprites;
    public static bool GenerateUV = true;
    Vector2 scrollPos;
    public static int toolBar = 0;
    public static int QualityToolBar = 0;
    public static int ShadingToolBar = 0;
    Color placeModeColor = Color.yellow;
    Color activeColor = Color.blue;
    Color passiveColor = Color.green;

    //bool MultipleMaterials = true;
    public int LowPolyVerts;
    public int MediumPolyVerts;
    public int HighPolyVerts;
    GameObject rock;
    GameObject NewRockShape;
    GameObject MeshCombineParent;
    GameObject ObjectToExport;
    bool includeRigidBody = true;
private Texture[] images;
    bool removeOldMesh = true;
    public static bool createFolder = true;
    public static string defaultPath = "Assets/";
    public enum ExportType { fbx, prefab }
    public ExportType TypeOfExport;
    public Color testColor;
    Vector3 currentMousePos = Vector3.zero;
    Vector3 lastPaintPos = Vector3.zero;
    float gizmoNormalLength = 1;
    RaycastHit mouseHitPoint;
    Event currentEvent;
    public static bool PlacingRock = false;
    public static bool InstantMode = false;
    public Vector3 GenerateAt;
    public enum GenerateOn { Custom, PickLocation }
    public GenerateOn Generate;
    public enum RockOrientation { X, Y, Z }
    public RockOrientation OrientationOfRock;
    Rect windowBounds = new Rect(0, 0, 0, 0);
    string paintGroupName = "Paint";
    string TEMPORARY_OBJECT_NAME = "Gizmo Location";
    public float brushSize = 0.1f;
    public int brushDensity = 2;
    public float maxYPosition = 400;
    public int listSize = 1;
    public LayerMask paintMask = 1;
    public bool showGizmoInfo = true;
    public List<PaintObject> objects;
    GameObject paintGroup = null;
    Transform gizmoLocation;
    bool isPainting = false;
    List<string> layerNames;
    string StructureName = "StructureName_";
    public static string StructureElementName = "StructureElementName_";
    string RockName = "RockName_";
    string CompositionName = "CompositionName_";
    string combinedMeshName = "";
    string ElementName = "ElementName_";
    string TemporaryRockName = "TemporaryRockName_";
    public enum CompositionMode { Fill, Outline }
    public CompositionMode ModeOfComposition;
    public enum MethodOfSeed { Random, Custom }
    public static MethodOfSeed SeedMethod;
    public static bool PrintSeedValue = false;
    public static int Seed;
    public static int SeedSize = 5000;
    public enum RockType
    {
        Cubic,
        Tetragonal,
        TriclinicAndObelisk,
        Orthorombic,
        Hexagonal,
        MonoclinicAndTrigonal,
        Octahedral,
        Spherical,
        Quartz,
        Pyramidal,
    }
    public RockType TypeOfRock;
    public enum ColliderType { Mesh, Box, None }
    public ColliderType TypeOfCollider;
    public float Length = 1;
    public Vector3 TetragonalScale = new Vector3(1, 1, 1);
    public float PointSmoothing = 0.1f;
    public float TetragonalPointLength = 0.5f;
    public float SizeInterval = 0.10f;
    public Vector3 TriclinicScale = new Vector3(1, 1, 1);
    public float TriclinicEdgeHeigth = 0.5f;
    public float TriclinicEdgeLength = 0.1f;
    public float TriclinicEdgeSmoothing = 0.1f;
    public Vector3 Scale = new Vector3(1, 1, 1);
    public Vector3 HexagonalScale = new Vector3(1, 1, 1);
    public float HexagonalEdgeWidth = 0.1f;
    public Vector3 TrigonalScale = new Vector3(1, 1, 1);
    public float TrigonalEdgeSmoothing = 0.1f;
    public float TrigonalEdgePosition = 0.75f;
    public Vector3 OctahedralScale = new Vector3(1, 1, 1);
    public float OctahedralPointPosition = 1f;
    public float OctahedralPointSmoothing = 0.1f;
    GUIContent[] PolyIcons;
    GUIContent[] ShadingIcons;
    public Vector3 QuartzScale = new Vector3(1, 1, 1);
    public float QuartzEdgeWidth = 0.1f;
    public float QuartzPointPosition = 2f;
    public float Radius = 1;
    public float PyramidalBaseScale = 1;
    public float PyramidalVertexPortrusion = 2;
    public float PyramidalVertexSmoothing = 0.1f;
    GUIContent remapUV = new GUIContent("UV", "Re-calculate the UV map of the selected mesh.");
    GUIContent reCalculateNormals = new GUIContent("Normals", "Re-calculate the normals of the selected mesh.");
    GUIContent combineSelected = new GUIContent("Combine", "Combine the selected meshes.");
    GUIContent exportSelected = new GUIContent("Export", "Export the selected object.");
    public enum LowPolyGrade
    {
        LowPoly,
        MediumPoly,
        HighPoly,
        Custom
    }
    public LowPolyGrade LowPoly;
    public enum ShadingType { Flat, Smooth }
    public static ShadingType TypeOfShading;
    public int NumberOfVertices = 250;
    public enum PolyMode
    {
        Low,
        Medium,
        High
    }
    PolyMode polyMode;
    public enum ShadingMode
    {
        Flat,
        Smooth
    }
    ShadingMode shadingMode;
    public enum TypeOfMaterial { SingleMaterial, RandomFromCollection }
    public TypeOfMaterial MaterialType;
    public Material[] Materials;
    public static Material RockMaterial;
    public Material rockmat;
    public List<Vector3> VertexList;
    void OnEnable()
    {
        LowPolyVerts = EditorPrefs.GetInt("lpverts", 50);
        MediumPolyVerts = EditorPrefs.GetInt("mpverts", 500);
        HighPolyVerts = EditorPrefs.GetInt("hpverts", 5000);
        toolBar = 0;
        listSize = 1;
        previewBackgroundTexture = (Texture2D)EditorGUIUtility.Load("Assets/Ameye/RockStudio/Icons/GreySquared.png");
        SceneView.onSceneGUIDelegate += SceneGUI;
        if (objects == null) objects = new List<PaintObject>();
        layerNames = new List<string>();
        for (int i = 0; i <= 10; i++) layerNames.Add(LayerMask.LayerToName(i));
images = new Texture[7];
        images[0] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/rock_colored.png", typeof(Texture2D));
        images[1] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/sculpture_colored.png", typeof(Texture2D));
        images[2] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/compose_colored.png", typeof(Texture2D));
        images[3] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/combine_colored.png", typeof(Texture2D));
        images[4] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/export_colored.png", typeof(Texture2D));
        images[5] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/brush_colored.png", typeof(Texture2D));
        images[6] = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/settings_colored.png", typeof(Texture2D));

        EditorApplication.hierarchyWindowChanged += HierarchyChanged;

        Texture2D flatShading = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/flat.png", typeof(Texture2D));
        Texture2D smoothShading = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/rock_colored.png", typeof(Texture2D));
        Texture2D lowPoly = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/rock_colored.png", typeof(Texture2D));
        Texture2D medPoly = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/rock_colored.png", typeof(Texture2D));
        Texture2D highPoly = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/rock_colored.png", typeof(Texture2D));

        PolyIcons = new GUIContent[3]
                {
                    new GUIContent(lowPoly, "Low Poly"),
                    new GUIContent(medPoly, "Medium Poly"),
                    new GUIContent(highPoly, "High Poly")
                };

        ShadingIcons = new GUIContent[2]
        {
            new GUIContent(flatShading,"Flat Shading"),
            new GUIContent(smoothShading,"Smooth Shading"),
        };
    }

    void OnDisable()
    {
        EditorPrefs.SetInt("lpverts", LowPolyVerts);
        EditorPrefs.SetInt("mpverts", MediumPolyVerts);
        EditorPrefs.SetInt("hpverts", HighPolyVerts);
        SceneView.onSceneGUIDelegate -= SceneGUI;
        if (gizmoLocation) DestroyImmediate(gizmoLocation.gameObject);
        Cleaner.RemoveGameObject("TemporaryRockName");
        EditorApplication.hierarchyWindowChanged -= HierarchyChanged;
    }

    public static void ShowWindow()
    {
        GetWindow(typeof(RockStudio));
    }

    void SceneGUI(SceneView sceneView)
    {
        windowBounds.width = Screen.width;
        windowBounds.height = Screen.height;

        windowRect = ClampRect(GUI.Window(0, windowRect, DrawWindow, "RockStudio"), windowBounds);

        if (toolBar == 0 || toolBar == 2 || toolBar == 5)
        {
            currentEvent = Event.current;
            updateMousePos(sceneView);
            drawGizmo();
            sceneInput();
            if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDrag) SceneView.RepaintAll();
        }

        if (toolBar == 0 || toolBar == 2)
        {
            Event current = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (current.type)
            {
                case EventType.MouseUp:
                    {
                        if (PlacingRock && current.button == 0 && !current.alt)
                        {
                            Cleaner.RemoveGameObject("_Sculpture_");
                            Cleaner.RemoveGameObject("_Structure_");
                            GenerateRock(NumberOfVertices, mouseHitPoint.point, mouseHitPoint.normal);
                            SceneView.lastActiveSceneView.FrameSelected();
                            Place();
                            Repaint();
                            break;
                        }

                        if (InstantMode && current.button == 0 && !current.alt)
                        {
                            GenerateRock(NumberOfVertices, mouseHitPoint.point, mouseHitPoint.normal);
                            GameObject oldRock = GameObject.Find(TemporaryRockName);
                            GameObject copy = new GameObject();
                            Undo.RegisterCreatedObjectUndo(copy, "Combine Meshes");
                            copy.transform.position = oldRock.transform.position;
                            copy.transform.eulerAngles = oldRock.transform.eulerAngles;
                            copy.transform.localScale = new Vector3(1, 1, 1);

                            if (GameObject.Find(CompositionName) == null)
                            {
                                GameObject newRockGroup = new GameObject(CompositionName);
                                Undo.RegisterCreatedObjectUndo(newRockGroup, "Create");
                                newRockGroup.transform.position = oldRock.transform.position;
                                SceneView.lastActiveSceneView.FrameSelected();
                            }

                            copy.transform.parent = GameObject.Find(CompositionName).transform;
                            copy.AddComponent<MeshFilter>();
                            copy.GetComponent<MeshFilter>().sharedMesh = oldRock.GetComponent<MeshFilter>().sharedMesh;
                            copy.name = ElementName;
                            Cleaner.RemoveGameObject(rock);
                            Repaint();

                            MeshRenderer renderer = copy.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                            if (MaterialType == TypeOfMaterial.SingleMaterial) renderer.material = RockMaterial;
                            else renderer.material = Materials[Random.Range(0, Materials.Length)];

                            if (includeRigidBody) copy.AddComponent(typeof(Rigidbody));

                            if (TypeOfCollider == ColliderType.Box) copy.AddComponent(typeof(BoxCollider));
                            if (TypeOfCollider == ColliderType.Mesh)
                            {
                                MeshCollider meshcollider = copy.AddComponent(typeof(MeshCollider)) as MeshCollider;
                                meshcollider.convex = true;
                                meshcollider.sharedMesh = copy.GetComponent<MeshFilter>().sharedMesh;
                            }
                            break;
                        }
                        break;
                    }

                case EventType.Layout:
                    if (PlacingRock)
                    {
                        HandleUtility.AddDefaultControl(controlID);
                        break;
                    }

                    if (InstantMode)
                    {
                        HandleUtility.AddDefaultControl(controlID);
                        break;
                    }
                    break;
            }
        }
    }

    public static Rect ClampRect(Rect rect, Rect bounds)
    {
        if (rect.x + rect.width > bounds.x + bounds.width)
            rect.x = (bounds.x + bounds.width) - rect.width;
        else if (rect.x < bounds.x)
            rect.x = bounds.x;

        if (rect.y + rect.height > bounds.y + bounds.height)
            rect.y = (bounds.y + bounds.height) - rect.height;
        else if (rect.y < bounds.y)
            rect.y = bounds.y;

        return rect;
    }

    void DrawWindow(int id)
    {
        GUI.DragWindow(dragWindowRect);
        GUIStyle style = new GUIStyle();
        style.richText = true;

        polyMode = (PolyMode)GUILayout.Toolbar((int)polyMode, PolyIcons, GUILayout.MinWidth(85), GUILayout.MaxWidth(85), GUILayout.MinHeight(22), GUILayout.MaxHeight(22));
        shadingMode = (ShadingMode)GUILayout.Toolbar((int)shadingMode, ShadingIcons, GUILayout.MinWidth(85), GUILayout.MaxWidth(85), GUILayout.MinHeight(22), GUILayout.MaxHeight(22));

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(remapUV, EditorStyles.miniButtonLeft))
        {

            if (Selection.activeGameObject != null && Selection.activeGameObject.transform.GetComponent<MeshFilter>() != null)
            {
                MeshFilter meshfilter = Selection.activeGameObject.transform.GetComponent<MeshFilter>();
                Mesh mesh = meshfilter.sharedMesh;
                UVMapper.CreateUVMap(mesh, Selection.activeGameObject.transform);
            }
        }

        if (GUILayout.Button(reCalculateNormals, EditorStyles.miniButtonRight))
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject.transform.GetComponent<MeshFilter>() != null)
            {
                Selection.activeGameObject.transform.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button(combineSelected, EditorStyles.miniButton) && Selection.transforms.Length > 1)
        {
            GameObject tempParent = new GameObject("_newParent");
            Undo.RegisterCreatedObjectUndo(tempParent, "Combine");

            Transform[] selectedTransforms = Selection.transforms;

            foreach (Transform child in selectedTransforms)
            {

                Undo.SetTransformParent(child, tempParent.transform, "Combine");

            }

            if (AllGameObjects(tempParent))
            {
                GameObject combinedGameObject = MeshCombiner.Combine(combinedMeshName, tempParent);
                //GameObject combinedGameObject = MeshCombiner.Combine(combinedMeshName, tempParent, MultipleMaterials);

                if (removeOldMesh) Undo.DestroyObjectImmediate(tempParent);

                if (includeRigidBody) combinedGameObject.AddComponent(typeof(Rigidbody));

                if (TypeOfCollider == ColliderType.Box) combinedGameObject.AddComponent(typeof(BoxCollider));
                if (TypeOfCollider == ColliderType.Mesh)
                {
                    MeshCollider meshcollider = combinedGameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
                    meshcollider.convex = true;
                    meshcollider.sharedMesh = combinedGameObject.GetComponent<MeshFilter>().sharedMesh;
                }

                Selection.activeGameObject = combinedGameObject;
            }
        }

        if (GUILayout.Button(exportSelected, EditorStyles.miniButton))
        Export.ExportGameObject(Selection.activeGameObject, false, false);

        EditorGUILayout.BeginVertical(boxStyle);
        GUILayout.Space(3);

        if (polyMode == PolyMode.Low)
        {
            GUI.color = Color.green;
            EditorGUILayout.LabelField("Low Poly", EditorStyles.whiteMiniLabel);
            GUI.color = Color.black;
            EditorGUILayout.LabelField("mode active", EditorStyles.whiteMiniLabel);
            GUI.color = Color.white;
        }

        if (polyMode == PolyMode.Medium)
        {
            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("Medium Poly", EditorStyles.whiteMiniLabel);
            GUI.color = Color.black;
            EditorGUILayout.LabelField("mode active", EditorStyles.whiteMiniLabel);
            GUI.color = Color.white;
        }

        if (polyMode == PolyMode.High)
        {
            GUI.color = Color.red;
            EditorGUILayout.LabelField("High Poly", EditorStyles.whiteMiniLabel);
            GUI.color = Color.black;
            EditorGUILayout.LabelField("mode active", EditorStyles.whiteMiniLabel);
            GUI.color = Color.white;
        }

        EditorGUILayout.EndVertical();
        GUILayout.Space(0);

        EditorGUILayout.BeginVertical(boxStyle);
        GUILayout.Space(3);

        if (shadingMode == ShadingMode.Flat)

        {
            GUI.color = Color.green;
            EditorGUILayout.LabelField("Flat", EditorStyles.whiteMiniLabel);
            GUI.color = Color.black;
            EditorGUILayout.LabelField("shading active", EditorStyles.whiteMiniLabel);
            GUI.color = Color.black;
        }

        if (shadingMode == ShadingMode.Smooth)
        {
            GUI.color = Color.yellow;
            EditorGUILayout.LabelField("Smooth", EditorStyles.whiteMiniLabel);
            GUI.color = Color.black;
            EditorGUILayout.LabelField("shading active", EditorStyles.whiteMiniLabel);
            GUI.color = Color.black;
        }
        EditorGUILayout.EndVertical();
        GUILayout.Space(0);

        Texture2D FlatIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/flat.png", typeof(Texture2D));
        Texture2D SmoothIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/smooth.png", typeof(Texture2D));
        Texture2D LowIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/low.png", typeof(Texture2D));
        Texture2D MedIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/med.png", typeof(Texture2D));
        Texture2D HighIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/high.png", typeof(Texture2D));

        ShadingIcons = new GUIContent[2]
        {
            new GUIContent(FlatIcon,"Flat Shading"),
            new GUIContent(SmoothIcon,"Smooth Shading")

        };

        PolyIcons = new GUIContent[3]
        {
            new GUIContent(LowIcon,"Low Poly"),
            new GUIContent(MedIcon,"Medium Poly"),
            new GUIContent(HighIcon,"High Poly")

        };

        string[] qualitytexts = new string[3];
        qualitytexts[0] = "Low";
        qualitytexts[1] = "Medium";
        qualitytexts[2] = "High";

        string[] shadingTexts = new string[2];
        shadingTexts[0] = "Flat";
        shadingTexts[1] = "Smooth";
    }

    public static bool seedFold
    {
        get { return EditorPrefs.GetBool("SeedFold", false); }
        set { EditorPrefs.SetBool("SeedFold", value); }
    }

    public static bool paintSettings
    {
        get { return EditorPrefs.GetBool("PaintSettings", false); }
        set { EditorPrefs.SetBool("PaintSettings", value); }
    }

    public static bool gizmoFold
    {
        get { return EditorPrefs.GetBool("GizmoFold", false); }
        set { EditorPrefs.SetBool("GizmoFold", value); }
    }

    public static bool paintPrefabs
    {
        get { return EditorPrefs.GetBool("PaintPrefabs", false); }
        set { EditorPrefs.SetBool("PaintPrefabs", value); }
    }

    public static bool qualityFold
    {
        get { return EditorPrefs.GetBool("QualityFold", false); }
        set { EditorPrefs.SetBool("QualityFold", value); }
    }

    public static bool materialFold
    {
        get { return EditorPrefs.GetBool("MaterialFold", false); }
        set { EditorPrefs.SetBool("MaterialFold", value); }
    }

    public static bool rockFold
    {
        get { return EditorPrefs.GetBool("RockFold", false); }
        set { EditorPrefs.SetBool("RockFold", value); }
    }

    public static bool exportFold
    {
        get { return EditorPrefs.GetBool("ExportFold", false); }
        set { EditorPrefs.SetBool("ExportFold", value); }
    }

    public static bool placementFold
    {
        get { return EditorPrefs.GetBool("PlacementFold", false); }
        set { EditorPrefs.SetBool("PlacementFold", value); }
    }

    public static bool generationFold
    {
        get { return EditorPrefs.GetBool("GenerationFold", false); }
        set { EditorPrefs.SetBool("GenerationFold", value); }
    }

    public static bool previewFold
    {
        get { return EditorPrefs.GetBool("PreviewFold", false); }
        set { EditorPrefs.SetBool("PreviewFold", value); }
    }

    public static bool previewBackgroundFold
    {
        get { return EditorPrefs.GetBool("PreviewBackGroundFold", false); }
        set { EditorPrefs.SetBool("PreviewBackGroundFold", value); }
    }

    public static bool workflowFold
    {
        get { return EditorPrefs.GetBool("WorkFlowFold", false); }
        set { EditorPrefs.SetBool("WorkFlowFold", value); }
    }

    //FOLDOUT
    static GUIStyle _foldoutStyle;
    static GUIStyle foldoutStyle
    {
        get
        {
            if (_foldoutStyle == null)
            {
                _foldoutStyle = new GUIStyle(EditorStyles.foldout);
                _foldoutStyle.font = EditorStyles.boldFont;
            }
            return _foldoutStyle;
        }
    }

    //BOX
    static GUIStyle _boxStyle;
    public static GUIStyle boxStyle
    {
        get
        {
            if (_boxStyle == null)
            {
                _boxStyle = new GUIStyle(EditorStyles.helpBox);
            }
            return _boxStyle;
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;

        
        GUILayout.Space(8);

        toolBar = GUILayout.Toolbar(toolBar, images, GUILayout.MinWidth(223), GUILayout.MaxWidth(223), GUILayout.MinHeight(20), GUILayout.MaxHeight(20));
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        switch (toolBar)
        {
            case 0:
                Cleaner.RemoveGameObject("_Structure_");
                Cleaner.RemoveGameObject("_Sculpture_");
                Cleaner.RemoveGameObject(TEMPORARY_OBJECT_NAME);
                if (InstantMode) SwitchInstantMode();
                GUILayout.Label(Styles.Rock, Styles.helpbox);

                rockFold = BeginFold("Rock Type", rockFold);
                if (rockFold) ShowRockTypes();
                EndFold();

                placementFold = BeginFold("Placement", placementFold);
                if (placementFold)
                {
                    OrientationOfRock = (RockOrientation)EditorGUILayout.EnumPopup("Orientation", OrientationOfRock);
                    Generate = (GenerateOn)EditorGUILayout.EnumPopup("Position", Generate);
                    if (Generate == GenerateOn.Custom) GenerateAt = EditorGUILayout.Vector3Field("", GenerateAt);
                }

                GUI.color = Color.green;
                if (Generate == GenerateOn.Custom)
                {
                    if (GUILayout.Button("Generate"))
                    {
                        Cleaner.RemoveGameObject(rock);
                        Cleaner.RemoveGameObject("_Sculpture_");
                        Cleaner.RemoveGameObject("_Structure_");
                        GenerateRock(NumberOfVertices, GenerateAt, Vector3.zero);
                        SceneView.lastActiveSceneView.FrameSelected();
                    }
                }

                else
                {
                    if (!PlacingRock)
                    {
                        GUI.color = Color.green;
                        if (GUILayout.Button("Place and Generate"))
                        {
                            Cleaner.RemoveGameObject(rock);
                            Place();
                        }
                    }

                    else
                    {
                        GUI.color = placeModeColor;
                        if (GUILayout.Button("Cancel"))
                        {
                            Cleaner.RemoveGameObject(rock);
                            Place();
                        }
                    }
                }
                EndFold();
                GUI.color = Color.white;
                EditorGUILayout.BeginVertical(boxStyle);
                GUILayout.Space(3);
                RockName = EditorGUILayout.TextField("Name", RockName);
                TypeOfCollider = (ColliderType)EditorGUILayout.EnumPopup("Collider", TypeOfCollider);
                includeRigidBody = EditorGUILayout.Toggle("Include Rigidbody", includeRigidBody);

                GUI.color = Color.green;
                if (GUILayout.Button("Save Rock"))
                {
                    if (GameObject.Find(TemporaryRockName) != null)
                    {
                        GameObject oldRock = GameObject.Find(TemporaryRockName);
                        GameObject newRock = new GameObject();
                        Undo.RegisterCreatedObjectUndo(newRock, "Create");
                        newRock.transform.position = oldRock.transform.position;
                        newRock.transform.eulerAngles = oldRock.transform.eulerAngles;
                        newRock.transform.localScale = new Vector3(1, 1, 1);
                        newRock.AddComponent<MeshFilter>();
                        newRock.GetComponent<MeshFilter>().sharedMesh = oldRock.GetComponent<MeshFilter>().sharedMesh;
                        newRock.GetComponent<MeshFilter>().sharedMesh.name = RockName;
                        Debug.Log("Mesh saved with " + oldRock.GetComponent<MeshFilter>().sharedMesh.vertexCount + " vertices");
                        Cleaner.RemoveGameObject(rock);
                        newRock.name = RockName;

                        if (includeRigidBody) newRock.AddComponent(typeof(Rigidbody));

                        MeshRenderer renderer = newRock.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
                        if (MaterialType == TypeOfMaterial.SingleMaterial) renderer.material = RockMaterial;
                        else renderer.material = Materials[Random.Range(0, Materials.Length)];

                        if (TypeOfCollider == ColliderType.Box) newRock.AddComponent(typeof(BoxCollider));
                        if (TypeOfCollider == ColliderType.Mesh)
                        {
                            MeshCollider meshcollider = newRock.AddComponent(typeof(MeshCollider)) as MeshCollider;
                            meshcollider.convex = true;
                            meshcollider.sharedMesh = newRock.GetComponent<MeshFilter>().sharedMesh;
                        }
                    }
                    else Debug.Log("<color=black><b>No mesh was found to save. Generate a rock mesh first.</b></color>");
                }
                GUILayout.Space(3);
                EditorGUILayout.EndVertical();
                GUILayout.Space(0);

                GUI.color = Color.white;
                if (GameObject.Find(TemporaryRockName)) EditorGUILayout.HelpBox("Don't forget to save your rock after generating it!", MessageType.Warning);
                if (PlacingRock)
                {
                    EditorGUILayout.HelpBox("Use Ctrl+Scroll to adjust the size of the placement tool.", MessageType.Info);
                }
                break;

            case 1:
                Cleaner.RemoveGameObject(TEMPORARY_OBJECT_NAME);
                Cleaner.RemoveGameObject(TemporaryRockName);
                if (InstantMode) SwitchInstantMode();

                GUILayout.Label(Styles.Sculpting, Styles.helpbox);
                EditorGUILayout.BeginVertical(boxStyle);
                GUILayout.Space(3);
                ModeOfComposition = (CompositionMode)EditorGUILayout.EnumPopup("Mode", ModeOfComposition);
                Cleaner.RemoveGameObject("TemporaryRockName");
                if (GUILayout.Button("New Sculpture"))
                {
                    Cleaner.RemoveGameObject(rock);
                    Cleaner.RemoveGameObject("_Sculpture_");
                    NewSculpture();
                    AddShape();
                    SceneView.lastActiveSceneView.FrameSelected();
                }

                if (GUILayout.Button("Add Shape"))
                {
                    if (GameObject.Find("_Sculpture_") != null)
                    {
                        AddShape();
                        SceneView.lastActiveSceneView.FrameSelected();
                    }
                    else Debug.Log("<color=black><b>No sculpture was found. Start a new sculpture first.</b></color>");
                }

                GUI.color = Color.green;
                if (GUILayout.Button("Generate Mesh"))
                {
                    if (GameObject.Find("_Sculpture_") != null)
                    {
                        Cleaner.RemoveGameObject("_Structure_");
                        rockmat = RockMaterial; 

                        if (ModeOfComposition == CompositionMode.Outline) SculptureCreator.GenerateComposition(NumberOfVertices, Vector3.zero, Vector3.zero, rockmat);
                        else SculptureCreator.GenerateCompositionFill(NumberOfVertices, Vector3.zero, Vector3.zero, rockmat);
                    }
                    else Debug.Log("<color=black><b>No sculpture was found. Start a new sculpture first.</b></color>");
                }
                GUILayout.Space(3);
                EditorGUILayout.EndVertical();
                GUILayout.Space(0);

                GUI.color = Color.white;
                EditorGUILayout.BeginVertical(boxStyle);
                GUILayout.Space(3);
                StructureName = EditorGUILayout.TextField("Name", StructureName);
                TypeOfCollider = (ColliderType)EditorGUILayout.EnumPopup("Collider", TypeOfCollider);
                includeRigidBody = EditorGUILayout.Toggle("Include Rigidbody", includeRigidBody);

                GUI.color = Color.green;
                if (GUILayout.Button("Save Sculpture"))
                {
                    if (GameObject.Find("_Structure_") != null)
                    {
                        GameObject oldStructure = GameObject.Find("_Structure_");
                        GameObject newStructure = MeshCombiner.Combine(StructureName, oldStructure);
                        //GameObject newStructure = MeshCombiner.Combine(StructureName, oldStructure, true);

                        MeshRenderer renderer = newStructure.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
                        renderer.material = RockMaterial;

                        if (includeRigidBody) newStructure.AddComponent(typeof(Rigidbody));

                        if (TypeOfCollider == ColliderType.Box) newStructure.AddComponent(typeof(BoxCollider));
                        if (TypeOfCollider == ColliderType.Mesh)
                        {
                            MeshCollider meshcollider = newStructure.AddComponent(typeof(MeshCollider)) as MeshCollider;
                            meshcollider.convex = true;
                            meshcollider.sharedMesh = newStructure.GetComponent<MeshFilter>().sharedMesh;
                        }
                        Cleaner.RemoveGameObject("_Sculpture_");
                        Cleaner.RemoveGameObject("_Structure_");
                    }
                    else Debug.Log("<color=black><b>No mesh was found to save. Generate a sculpture mesh first.</b></color>");
                }

                GUILayout.Space(3);
                EditorGUILayout.EndVertical();
                GUILayout.Space(0);
                GUI.color = Color.white;
                if (GameObject.Find("_Structure_")) EditorGUILayout.HelpBox("Don't forget to save your sculpture after generating it!", MessageType.Info);
                break;

            case 2:
                Cleaner.RemoveGameObject(TEMPORARY_OBJECT_NAME);
                Cleaner.RemoveGameObject("_Structure_");
                Cleaner.RemoveGameObject("_Sculpture_");
                Cleaner.RemoveGameObject(TemporaryRockName);
                GUILayout.Label(Styles.Compose, Styles.helpbox);
                rockFold = BeginFold("Rock Type", rockFold);
                if (rockFold)
                {
                    Cleaner.RemoveGameObject("TemporaryRockName");
                    ShowRockTypes();
                }
                EndFold();

                placementFold = BeginFold("Placement", placementFold);
                if (placementFold) OrientationOfRock = (RockOrientation)EditorGUILayout.EnumPopup("Orientation", OrientationOfRock);

                EndFold();

                EditorGUILayout.BeginVertical(boxStyle);
                GUILayout.Space(3);
                CompositionName = EditorGUILayout.TextField("Group Name", CompositionName);
                ElementName = EditorGUILayout.TextField("Element Name", ElementName);
                TypeOfCollider = (ColliderType)EditorGUILayout.EnumPopup("Collider", TypeOfCollider);
                includeRigidBody = EditorGUILayout.Toggle("Include Rigidbody", includeRigidBody);

                if (!InstantMode)
                {
                    GUI.color = Color.green;
                    if (GUILayout.Button("Place and Generate")) SwitchInstantMode();
                }

                else
                {
                    GUI.color = placeModeColor;
                    if (GUILayout.Button("Cancel")) SwitchInstantMode();
                }
                GUILayout.Space(3);
                EditorGUILayout.EndVertical();
                GUILayout.Space(0);
                GUI.color = Color.white;
              
                if (InstantMode)
                    EditorGUILayout.HelpBox("Use Ctrl+Scroll to adjust the size of the placement tool.", MessageType.Info);
                
                break;

            case 3:
                Cleaner.RemoveGameObject(TemporaryRockName);
                Cleaner.RemoveGameObject(TEMPORARY_OBJECT_NAME);
                Cleaner.RemoveGameObject("_Structure_");
                Cleaner.RemoveGameObject("_Sculpture_");
                if (InstantMode) SwitchInstantMode();
                GUILayout.Label(Styles.Combine, Styles.helpbox);
                EditorGUILayout.BeginVertical(boxStyle);
                GUILayout.Space(3);
                Cleaner.RemoveGameObject("TemporaryRockName");
                MeshCombineParent = (GameObject)EditorGUILayout.ObjectField("Parent Object", MeshCombineParent, typeof(GameObject), true);
                combinedMeshName = EditorGUILayout.TextField("New Mesh Name", combinedMeshName);
                if (combinedMeshName == "" && MeshCombineParent != null) combinedMeshName = MeshCombineParent.name;
                if (combinedMeshName == "") combinedMeshName = "Combined Mesh";
                TypeOfCollider = (ColliderType)EditorGUILayout.EnumPopup("Collider", TypeOfCollider);
                //MultipleMaterials = EditorGUILayout.Toggle("Multiple Materials", MultipleMaterials);
                includeRigidBody = EditorGUILayout.Toggle("Add Rigidbody", includeRigidBody);
                removeOldMesh = EditorGUILayout.Toggle("Remove Source Mesh", removeOldMesh);

                if (MeshCombineParent && AllGameObjects(MeshCombineParent))
                {
                    if (MeshCombineParent.transform.childCount != 0)
                    {
                        EditorGUILayout.LabelField("Mesh Count", MeshCombineParent.transform.childCount.ToString());
                        EditorGUILayout.LabelField("Vertex Count", GetVerts(MeshCombineParent).ToString());
                    }

                    else
                    {
                        EditorGUILayout.LabelField("Mesh Count", "1");
                        EditorGUILayout.LabelField("Vertex Count", MeshCombineParent.transform.GetComponent<MeshFilter>().sharedMesh.vertexCount.ToString());
                    }
                }

                GUI.color = Color.green;

                if (GUILayout.Button("Combine"))
                {
                    if (MeshCombineParent != null && AllGameObjects(MeshCombineParent))
                    {
                        //GameObject combinedGameObject = MeshCombiner.Combine(combinedMeshName, MeshCombineParent, MultipleMaterials);
                        GameObject combinedGameObject = MeshCombiner.Combine(combinedMeshName, MeshCombineParent);
                        if (removeOldMesh) Undo.DestroyObjectImmediate(MeshCombineParent);

                        if (includeRigidBody) combinedGameObject.AddComponent(typeof(Rigidbody));

                        if (TypeOfCollider == ColliderType.Box) combinedGameObject.AddComponent(typeof(BoxCollider));
                        if (TypeOfCollider == ColliderType.Mesh)
                        {
                            MeshCollider meshcollider = combinedGameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
                            meshcollider.convex = true;
                            meshcollider.sharedMesh = combinedGameObject.GetComponent<MeshFilter>().sharedMesh;
                        }

                        Selection.activeGameObject = combinedGameObject;
                    }

                    else Debug.Log("Parent object field was left empty.");
                }
                GUILayout.Space(3);
                EditorGUILayout.EndVertical();
                GUILayout.Space(0);

                GUI.color = Color.white;
                workflowFold = BeginFold("Workflow", workflowFold);

                if (workflowFold)
                    GUILayout.Label("Add a parent object and its children\nwill be combined into a single mesh.", style);
                EndFold();
                if (MeshCombineParent != null && !AllGameObjects(MeshCombineParent))
                    EditorGUILayout.HelpBox("Not all children have a MeshFilter component.", MessageType.Error);

                break;

            case 4:
                Cleaner.RemoveGameObject(TEMPORARY_OBJECT_NAME);
                Cleaner.RemoveGameObject("_Structure_");
                Cleaner.RemoveGameObject("_Sculpture_");
                if (InstantMode) SwitchInstantMode();
                Cleaner.RemoveGameObject(TemporaryRockName);
                GUILayout.Label(Styles.Export, Styles.helpbox);
                EditorGUILayout.BeginVertical(boxStyle);
                GUILayout.Space(3);
                ObjectToExport = (GameObject)EditorGUILayout.ObjectField("Object", ObjectToExport, typeof(GameObject), true);
                GUI.color = Color.green;

                if (GUILayout.Button("Export"))
                 Export.ExportGameObject(ObjectToExport, false, false);

                GUILayout.Space(3);
                EditorGUILayout.EndVertical();
                GUILayout.Space(0);
                GUI.color = Color.white;
                EditorGUILayout.HelpBox("Mesh will be exported as fbx.\nSubstances will not be included!", MessageType.Warning);
                break;

            case 5:
                if (InstantMode) SwitchInstantMode();
                Cleaner.RemoveGameObject("_Structure_");
                Cleaner.RemoveGameObject("_Sculpture_");
                Cleaner.RemoveGameObject(TemporaryRockName);
                GUILayout.Label(Styles.Brush, Styles.helpbox);
                float tempSize = brushSize;
                int tempDensity = brushDensity;
                paintSettings = BeginFold("Paint Settings", paintSettings);
                if (paintSettings)
                {
                    paintMask = EditorGUILayout.MaskField(new GUIContent("Paint Layer", "On which layer the tool will paint"), paintMask, layerNames.ToArray());
                    brushSize = EditorGUILayout.FloatField("Brush Size", brushSize);
                    brushDensity = EditorGUILayout.IntField("Brush Density", brushDensity);
                    paintGroupName = EditorGUILayout.TextField("Paint Group Name", paintGroupName);
                    maxYPosition = 1f;
                }
                EndFold();

                listSize = Mathf.Max(0, listSize);

                for (int i = 0; i < objects.Count; i++) PaintObject.display(objects[i]);

                EditorGUILayout.BeginVertical(boxStyle);
                GUILayout.Space(3);
                GUI.color = Color.green;
                if (GUILayout.Button("Add")) listSize++;
                GUI.color = Color.yellow;
                if (GUILayout.Button("Remove") && listSize != 0) listSize--;

                CheckForChanges(tempSize, tempDensity);
                GUI.color = Color.white;
                GUILayout.Space(3);
                EditorGUILayout.EndVertical();
                GUILayout.Space(0);

                EditorGUILayout.HelpBox("Press and hold 'Ctrl' to start painting.", MessageType.Warning);
                EditorGUILayout.HelpBox("Add objects you don't want to\npaint over to a seperate layer.", MessageType.Warning);
                break;

            case 6:
                Cleaner.RemoveGameObject(TEMPORARY_OBJECT_NAME);
                Cleaner.RemoveGameObject("_Structure_");
                Cleaner.RemoveGameObject(TemporaryRockName);
                Cleaner.RemoveGameObject("_Sculpture_");
                if (InstantMode) SwitchInstantMode();
                GUILayout.Label(Styles.Settings, Styles.helpbox);
                seedFold = BeginFold("Seeds", seedFold);
                if (seedFold)
                {
                    SeedMethod = (MethodOfSeed)EditorGUILayout.EnumPopup("Generation Seed", SeedMethod);
                    if (SeedMethod == MethodOfSeed.Custom) Seed = EditorGUILayout.IntField("Seed Value", Seed);
                    if (SeedMethod == MethodOfSeed.Random) SeedSize = EditorGUILayout.IntField("Maximum Seed Size", SeedSize);
                    PrintSeedValue = EditorGUILayout.Toggle("Print Seed Value", PrintSeedValue);
                }
                EndFold();

                qualityFold = BeginFold("Vertex Count Presets", qualityFold);
                if (qualityFold)
                {
                    LowPolyVerts = EditorGUILayout.IntField("Low Poly", LowPolyVerts);
                    MediumPolyVerts = EditorGUILayout.IntField("Medium Poly", MediumPolyVerts);
                    HighPolyVerts = EditorGUILayout.IntField("High Poly", HighPolyVerts);

                }
                EndFold();

                materialFold = BeginFold("Materials and Texturing", materialFold);
                if (materialFold)
                {
                    RockMaterial = (Material)EditorGUILayout.ObjectField("Material", RockMaterial, typeof(Material), true);
                    GenerateUV = EditorGUILayout.Toggle("Generate UV Map", GenerateUV);
                }
                EndFold();

                previewBackgroundFold = BeginFold("Prefab Preview", previewBackgroundFold);
                if (previewBackgroundFold) previewBackgroundTexture = (Texture2D)EditorGUILayout.ObjectField("Background", previewBackgroundTexture, typeof(Texture2D), true);
                EndFold();

                exportFold = BeginFold("Exporting", exportFold);
                if (exportFold)
                {
                    EditorGUILayout.LabelField("<size=11><color=black><b>Default Folder: </b></color></size>" + defaultPath, style);
                    if (GUILayout.Button("Change")) defaultPath = SaveToAssets.ChangePath(defaultPath);
                }
                EndFold();

                gizmoFold = BeginFold("Placement Tool", gizmoFold);
                if (gizmoFold)
                {
                    showGizmoInfo = EditorGUILayout.Toggle("Display Gizmo Info", showGizmoInfo);
                    activeColor = EditorGUILayout.ColorField("Active Color", activeColor);
                    passiveColor = EditorGUILayout.ColorField("Passive Color", passiveColor);
                    gizmoNormalLength = EditorGUILayout.FloatField("Arrow Length", gizmoNormalLength);
                    SizeInterval = EditorGUILayout.FloatField("Size Interval", SizeInterval);
                }
                EndFold();
                break;
            default: break;
        }

        GUI.color = Color.white;
        EditorGUILayout.EndScrollView();

        if (RockMaterial == null) RockMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Materials/UV1.mat", typeof(Material));
        if (previewBackgroundTexture == null) previewBackgroundTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ameye/RockStudio/Icons/GreySquared.png", typeof(Texture2D));

        if (polyMode == PolyMode.Low) NumberOfVertices = LowPolyVerts;
        if (polyMode == PolyMode.Medium) NumberOfVertices = MediumPolyVerts;
        if (polyMode == PolyMode.High) NumberOfVertices = HighPolyVerts;

        if (shadingMode == ShadingMode.Flat) TypeOfShading = ShadingType.Flat;
        if (shadingMode == ShadingMode.Smooth) TypeOfShading = ShadingType.Smooth;
    }

    void CheckForChanges(float tempSize, int tempDensity)
    {
        if (tempSize != brushSize)
        {
            brushSize = Mathf.Max(brushSize, 1);
            SceneView.RepaintAll();
        }

        else if (brushDensity != tempDensity)
        {
            brushDensity = Mathf.Max(brushDensity, 1);
            SceneView.RepaintAll();
        }

        else if (objects != null && listSize != objects.Count)
        {
            List<PaintObject> tempObj = new List<PaintObject>(listSize);
            for (int i = 0; i < listSize; i++)
            {
                if (objects.Count > i) tempObj.Add(objects[i]);
                else tempObj.Add(new PaintObject());
            }
            objects = new List<PaintObject>(tempObj);
        }
    }

    void drawGizmo()
    {
        if (isPainting) Handles.color = activeColor;
        else Handles.color = passiveColor;

        if (toolBar == 5)
        {
            if (mouseHitPoint.transform)
            {
                if (gizmoLocation == null) gizmoLocation = new GameObject(TEMPORARY_OBJECT_NAME).transform;
                gizmoLocation.rotation = mouseHitPoint.transform.rotation;
                gizmoLocation.forward = mouseHitPoint.normal;
                Handles.ArrowHandleCap(3, mouseHitPoint.point, gizmoLocation.rotation, gizmoNormalLength * brushSize, EventType.Repaint);
                Handles.CircleHandleCap(2, currentMousePos, gizmoLocation.rotation, brushSize, EventType.Repaint);
                gizmoLocation.up = mouseHitPoint.normal;
            }
        }

        else
        {
            if (PlacingRock || InstantMode)
            {
                if (mouseHitPoint.transform)
                {
                    if (gizmoLocation == null) gizmoLocation = new GameObject(TEMPORARY_OBJECT_NAME).transform;
                    gizmoLocation.rotation = mouseHitPoint.transform.rotation;
                    gizmoLocation.forward = mouseHitPoint.normal;
                    Handles.ArrowHandleCap(3, mouseHitPoint.point, gizmoLocation.rotation, gizmoNormalLength * brushSize, EventType.Repaint);
                    Handles.CircleHandleCap(2, currentMousePos, gizmoLocation.rotation, brushSize, EventType.Repaint);
                    gizmoLocation.up = mouseHitPoint.normal;
                }
            }
        }

        Handles.BeginGUI();
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        GUILayout.BeginArea(new Rect(currentEvent.mousePosition.x + 10, currentEvent.mousePosition.y + 10, 250, 100));
        if (showGizmoInfo && toolBar == 5)
        {
            GUILayout.TextField("Size: " + brushSize, style);
            GUILayout.TextField("Density: " + brushDensity, style);
            GUILayout.TextField("Height: " + currentMousePos.y, style);
            GUILayout.TextField("Surface Name: " + (mouseHitPoint.collider ? mouseHitPoint.collider.name : "none"), style);
            GUILayout.TextField("Position: " + currentMousePos.ToString(), style);
        }

        else if (showGizmoInfo && toolBar != 5)
        {
            if (PlacingRock || InstantMode)
            {
                GUILayout.TextField("Size: " + brushSize, style);
                GUILayout.TextField("Height: " + currentMousePos.y, style);
                GUILayout.TextField("Surface Name: " + (mouseHitPoint.collider ? mouseHitPoint.collider.name : "none"), style);
                GUILayout.TextField("Position: " + currentMousePos.ToString(), style);
            }
        }
        GUILayout.EndArea();
        Handles.EndGUI();
    }

    void updateMousePos(SceneView sceneView)
    {
        if (currentEvent.control) HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        RaycastHit hit;
        Ray ray = sceneView.camera.ScreenPointToRay(new Vector2(currentEvent.mousePosition.x, sceneView.camera.pixelHeight - currentEvent.mousePosition.y));
        if (Physics.Raycast(ray, out hit, 1000, paintMask))
        {
            currentMousePos = hit.point;
            mouseHitPoint = hit;
        }
        else mouseHitPoint = new RaycastHit();
    }

    public bool preventCustomUserHotkey(EventType type, EventModifiers codeModifier, KeyCode hotkey)
    {
        Event currentevent = Event.current;
        if (currentevent.type == type && currentevent.modifiers == codeModifier && currentevent.keyCode == hotkey)
        {
            currentevent.Use();
            return true;
        }
        return false;
    }

    void sceneInput()
    {
        if (preventCustomUserHotkey(EventType.ScrollWheel, EventModifiers.Control, KeyCode.None))
        {
            if (currentEvent.delta.y > 0) brushSize = brushSize + SizeInterval;
            else
            {
                brushSize = brushSize - SizeInterval;
                brushSize = Mathf.Max(SizeInterval, brushSize);
            }
            this.Repaint();
        }

        else if (preventCustomUserHotkey(EventType.ScrollWheel, EventModifiers.Alt, KeyCode.None))
        {
            if (toolBar == 5)
            {
                if (currentEvent.delta.y > 0) brushDensity++;
                else
                {
                    brushDensity--;
                    brushDensity = Mathf.Max(1, brushDensity);
                }
                this.Repaint();
            }
        }

        else if (currentEvent.control && (currentEvent.button == 0 && currentEvent.type == EventType.MouseDown))
        {
            if (toolBar == 5)
            {
                isPainting = true;
                painting();
            }
        }

        else if (isPainting && !currentEvent.control || (currentEvent.button != 0 || currentEvent.type == EventType.MouseUp))
        {
            if (toolBar == 5)
            {
                lastPaintPos = Vector3.zero;
                isPainting = false;
            }
        }

        else if (isPainting && (currentEvent.type == EventType.MouseDrag) && toolBar == 5) painting();
    }

    void painting()
    {
        if (objects != null && objects.Count > 0)
        {
            if (Vector3.Distance(lastPaintPos, currentMousePos) > brushSize)
            {
                lastPaintPos = currentMousePos;
                drawPaint();
            }
        }
        else Debug.LogWarning("Prefab list is empty !");
    }


    void drawPaint()
    {
        if (paintGroup == null)
        {
            if (GameObject.Find(paintGroupName)) paintGroup = GameObject.Find(paintGroupName);
            else paintGroup = new GameObject(paintGroupName);
        }

        int localDensity = brushDensity;
        Vector3 dir = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Vector3.right;
        Vector3[] spawnPoint = new Vector3[localDensity];

        for (int i = 0; i < localDensity; i++)
        {
            dir = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * Vector3.right;
            Vector3 spawnPos = (dir * brushSize * Random.Range(0.1f, 1.1f)) + currentMousePos;

            if (spawnPos != Vector3.zero)
            {
                spawnPoint[i] = spawnPos;
                spawnObject(spawnPoint[i]);
            }
        }
    }

    GameObject spawnObject(Vector3 pos)
    {
        int rndIndex = Random.Range(0, objects.Count);
        GameObject prefabObj = objects[rndIndex].prefab;
        GameObject go = null;
        if (prefabObj != null)
        {
            go = (GameObject)PrefabUtility.InstantiatePrefab(prefabObj);

            if (gizmoLocation)
            {
                go.transform.rotation = gizmoLocation.rotation;
                go.transform.up = gizmoLocation.up;
            }

            else go.transform.rotation = Quaternion.identity;

            bool randomRotationX = objects[rndIndex].randomRotationX;
            bool randomRotationY = objects[rndIndex].randomRotationY;
            bool randomRotationZ = objects[rndIndex].randomRotationZ;

            if (randomRotationX) go.transform.Rotate(Vector3.right, Random.Range(0, 360));
            if (randomRotationY) go.transform.Rotate(Vector3.up, Random.Range(0, 360));
            if (randomRotationZ) go.transform.Rotate(Vector3.forward, Random.Range(0, 360));

            Vector2 scale = objects[rndIndex].scale;
            if (scale != Vector2.one && scale != Vector2.zero) go.transform.localScale *= Random.Range(scale.x, scale.y);
            go.transform.position = pos;
            DoubleRayCast(go, rndIndex);
            if (go) addObjectToGroup(go, rndIndex);
        }
        return go;
    }


    void addObjectToGroup(GameObject obj, int index)
    {
        Transform parent = GameObject.Find(paintGroupName).transform;
        if (parent == null) parent = new GameObject(paintGroupName).transform;
        obj.transform.SetParent(parent);
    }

    public bool layerContain(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    void DoubleRayCast(GameObject obj, int index)
    {
        Vector3 position = obj.transform.position + obj.transform.up * maxYPosition;
        obj.transform.position = position;
        obj.SetActive(false);
        RaycastHit groundHit;

        if (Physics.Raycast(position, -obj.transform.up, out groundHit))
        {
            RaycastHit objectHit;
            if (layerContain(paintMask, groundHit.collider.gameObject.layer))
            {
                obj.SetActive(true);
                if (Physics.Raycast(groundHit.point, obj.transform.up, out objectHit) && obj.layer == objectHit.collider.gameObject.layer)
                {
                    Vector3 newPos;
                    float differencialDistance = Vector3.Distance(objectHit.point, obj.transform.position);
                    newPos = groundHit.point + (obj.transform.up * differencialDistance);
                    obj.transform.position = newPos;
                    return;
                }
            }
        }
        DestroyImmediate(obj);
    }

    public static bool BeginFold(string foldName, bool foldState)
    {
        EditorGUILayout.BeginVertical(boxStyle);
        GUILayout.Space(3);
        foldState = EditorGUI.Foldout(EditorGUILayout.GetControlRect(),
        foldState, foldName, true, foldoutStyle);
        if (foldState) GUILayout.Space(3);
        return foldState;
    }

    public static void EndFold()
    {
        GUILayout.Space(3);
        EditorGUILayout.EndVertical();
        GUILayout.Space(0);
    }

    public void GenerateRock(int NumberOfVertices, Vector3 pos, Vector3 nor)
    {
        VertexList = new List<Vector3>(NumberOfVertices);
        VertexList.Clear();

        if (SeedMethod == MethodOfSeed.Random)
        {
            Random.state = Random.state;
            Seed = Random.Range(0, SeedSize);
            Random.InitState(Seed);
            if (PrintSeedValue == true) Debug.Log("<color=green><b>Used seed: </b></color>" + Seed);
        }
        else Random.InitState(Seed);

        if (TypeOfRock == RockType.Cubic)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-Length * 0.5f, Length * 0.5f), Random.Range(-Length * 0.5f, Length * 0.5f), Random.Range(-Length * 0.5f, Length * 0.5f)));
        }

        if (TypeOfRock == RockType.Tetragonal)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TetragonalScale.x * 0.5f, TetragonalScale.x * 0.5f), Random.Range(-TetragonalScale.y * 0.5f, TetragonalScale.y * 0.5f), Random.Range(-TetragonalScale.z * 0.5f, TetragonalScale.z * 0.5f)));
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TetragonalScale.x * 0.5f * PointSmoothing, TetragonalScale.x * 0.5f * PointSmoothing), Random.Range(-TetragonalScale.y * 0.5f * PointSmoothing, TetragonalScale.y * 0.5f * PointSmoothing), Random.Range(-TetragonalScale.z * 0.5f - TetragonalPointLength, TetragonalScale.z * 0.5f + TetragonalPointLength)));
        }

        if (TypeOfRock == RockType.TriclinicAndObelisk)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TriclinicScale.x * 0.5f, TriclinicScale.x * 0.5f), Random.Range(-TriclinicScale.y * 0.5f, TriclinicScale.y * 0.5f), Random.Range(-TriclinicScale.z * 0.5f, TriclinicScale.z * 0.5f)));
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TriclinicScale.x * 0.5f * TriclinicEdgeSmoothing, TriclinicScale.x * 0.5f * TriclinicEdgeSmoothing), Random.Range(-TriclinicScale.y * 0.5f * TriclinicEdgeLength, TriclinicScale.y * 0.5f * TriclinicEdgeLength), Random.Range(0, TriclinicScale.z * 0.5f + TriclinicEdgeHeigth)));
        }

        if (TypeOfRock == RockType.Orthorombic) for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-Scale.z * 0.5f, Scale.z * 0.5f), Random.Range(-Scale.y * 0.5f, Scale.y * 0.5f), Random.Range(-Scale.x * 0.5f, Scale.x * 0.5f)));

        if (TypeOfRock == RockType.Hexagonal)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-HexagonalScale.x * 0.5f, HexagonalScale.x * 0.5f), Random.Range(-HexagonalScale.y * 0.5f, HexagonalScale.y * 0.5f), Random.Range(-HexagonalScale.z * 0.5f, HexagonalScale.z * 0.5f)));
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-HexagonalScale.x * 0.5f * HexagonalEdgeWidth, HexagonalScale.x * 0.5f * HexagonalEdgeWidth), 0, Random.Range(-HexagonalScale.z * 0.5f, HexagonalScale.z * 0.5f)));
        }

        if (TypeOfRock == RockType.MonoclinicAndTrigonal)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TrigonalScale.x * 0.5f, TrigonalScale.x * 0.5f), Random.Range(-TrigonalScale.y * 0.5f, TrigonalScale.y * 0.5f), Random.Range(-TrigonalScale.z * 0.5f, TrigonalScale.z * 0.5f)));
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-TrigonalScale.x * 0.5f * TrigonalEdgeSmoothing, TrigonalScale.x * 0.5f * TrigonalEdgeSmoothing), Random.Range(-TrigonalScale.y * 0.5f, TrigonalScale.y * 0.5f), Random.Range(-TrigonalScale.z * 0.5f - TrigonalEdgePosition, TrigonalScale.z * 0.5f + TrigonalEdgePosition)));
        }

        if (TypeOfRock == RockType.Octahedral)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-OctahedralScale.x * 0.5f, OctahedralScale.x * 0.5f), Random.Range(-OctahedralScale.y * 0.5f, OctahedralScale.y * 0.5f), 0));
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-OctahedralScale.x * 0.5f * OctahedralPointSmoothing, OctahedralScale.x * 0.5f * OctahedralPointSmoothing), Random.Range(-OctahedralScale.y * 0.5f * OctahedralPointSmoothing, OctahedralScale.y * 0.5f * OctahedralPointSmoothing), Random.Range(-OctahedralPointPosition, OctahedralPointPosition)));
        }

        if (TypeOfRock == RockType.Quartz)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-QuartzScale.x * 0.5f, QuartzScale.x * 0.5f), Random.Range(-QuartzScale.y * 0.5f, QuartzScale.y * 0.5f), Random.Range(-QuartzScale.z * 0.5f, QuartzScale.z * 0.5f)));
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-QuartzScale.x * 0.5f * QuartzEdgeWidth, QuartzScale.x * 0.5f * QuartzEdgeWidth), 0, Random.Range(-QuartzScale.z * 0.5f, QuartzScale.z * 0.5f)));
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-QuartzScale.x * 0.5f * 0.01f, QuartzScale.x * 0.5f * 0.01f), Random.Range(-QuartzScale.y * 0.5f * 0.01f, QuartzScale.y * 0.5f * 0.01f), Random.Range(-QuartzScale.z * 0.5f - QuartzPointPosition, QuartzScale.z * 0.5f + QuartzPointPosition)));
        }

        if (TypeOfRock == RockType.Spherical)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(Random.insideUnitSphere * Radius);
        }

        if (TypeOfRock == RockType.Pyramidal)
        {
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.insideUnitSphere.x * PyramidalBaseScale, Random.insideUnitSphere.y * PyramidalBaseScale, Random.Range(0, 0.1f)));
            for (int i = 0; i < NumberOfVertices; i++) VertexList.Add(new Vector3(Random.Range(-PyramidalBaseScale * PyramidalVertexSmoothing, PyramidalBaseScale * PyramidalVertexSmoothing), Random.Range(-PyramidalBaseScale * PyramidalVertexSmoothing, PyramidalBaseScale * PyramidalVertexSmoothing), Random.Range(0, PyramidalVertexPortrusion)));
        }
        CreateRock(VertexList, pos, nor);
    }

    public void CreateRock(IEnumerable<Vector3> Vertices, Vector3 pos, Vector3 nor)
    {
        if (PlacingRock) pos += nor * (0.01f * 0.1f);
        if (InstantMode) pos += nor * (0.01f * 0.1f);

        if (toolBar == 0)
        {
            rock = new GameObject(TemporaryRockName);
            Undo.RegisterCreatedObjectUndo(rock, "Create");
            rock.transform.position = pos;
            rock.transform.LookAt(pos - nor);
            Selection.activeGameObject = rock;

            MeshFilter meshFilter = (MeshFilter)rock.AddComponent(typeof(MeshFilter));  //Add a mesh filter.
            MeshRenderer renderer = rock.AddComponent(typeof(MeshRenderer)) as MeshRenderer; //Add a mesh renderer.

            if (MaterialType == TypeOfMaterial.SingleMaterial) renderer.material = RockMaterial; //Assign single material.
            else renderer.material = Materials[Random.Range(0, Materials.Length)]; //Assigne random material from a collection of materials.

            meshFilter.sharedMesh = MeshCreator.CreateMesh(Vertices); //Create a mesh.
            meshFilter.sharedMesh.name = TemporaryRockName;

            if (TypeOfShading == ShadingType.Flat) MakeLowPoly.LowPoly(rock.transform, toolBar, TemporaryRockName, GenerateUV);

            else
            {
                if (GenerateUV)
                    UVMapper.CreateUVMap(rock.GetComponent<MeshFilter>().sharedMesh, rock.transform);
            }
        }

        if (toolBar == 2)
        {
            rock = new GameObject(TemporaryRockName);
            Undo.RegisterCreatedObjectUndo(rock, "Create");
            rock.transform.position = pos;
            rock.transform.LookAt(pos - nor);
            Selection.activeGameObject = rock;

            MeshFilter meshFilter = (MeshFilter)rock.AddComponent(typeof(MeshFilter));  //Add a mesh filter.
            MeshRenderer renderer = rock.AddComponent(typeof(MeshRenderer)) as MeshRenderer; //Add a mesh renderer.

            if (MaterialType == TypeOfMaterial.SingleMaterial) renderer.material = RockMaterial; //Assign single material.
            else renderer.material = Materials[Random.Range(0, Materials.Length)]; //Assign random material from a collection of materials.

            meshFilter.sharedMesh = MeshCreator.CreateMesh(Vertices); //Create a mesh.

            if (TypeOfShading == ShadingType.Flat) MakeLowPoly.LowPoly(rock.transform, toolBar, TemporaryRockName, GenerateUV);
            else
            {
                if (GenerateUV)
                    UVMapper.CreateUVMap(rock.GetComponent<MeshFilter>().sharedMesh, rock.transform);
            }
        }

        if (OrientationOfRock == RockOrientation.X) rock.transform.localEulerAngles = new Vector3(0, 90, 0);


        if (OrientationOfRock == RockOrientation.Y)
        {
            if (TypeOfRock == RockType.Orthorombic) rock.transform.localEulerAngles = new Vector3(0, 90, 0);
            if (TypeOfRock == RockType.TriclinicAndObelisk || TypeOfRock == RockType.Pyramidal) rock.transform.localEulerAngles = new Vector3(-90, 0, 0);
            else rock.transform.localEulerAngles = new Vector3(90, 0, 0);
        }

        if (OrientationOfRock == RockOrientation.Z)
        {
            if (TypeOfRock == RockType.Orthorombic) rock.transform.localEulerAngles = new Vector3(0, 90, 0);
            else rock.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void Update()
    {
        SceneView.RepaintAll();
    }

    void Place()
    {
        PlacingRock = !PlacingRock;
    }

    private void HierarchyChanged()
    {
        Repaint();
    }


    public static int GetVerts(GameObject obj)
    {

        int numberOfVerts = 0;
        foreach (Transform child in obj.transform)
        {
            numberOfVerts += child.gameObject.GetComponent<MeshFilter>().sharedMesh.vertexCount;
        }

        return numberOfVerts;
    }

    public static bool AllGameObjects(GameObject obj)
    {

        foreach (Transform child in obj.transform)
        {
            if (child.GetComponent<MeshFilter>() == null)

                return false;
        }

        return true;
    }

    void SwitchInstantMode()
    {
        InstantMode = !InstantMode;
    }

    void NewSculpture()
    {
        GameObject newSculpture = new GameObject("_Sculpture_");
        Undo.RegisterCreatedObjectUndo(newSculpture, "Create");
    }

    void AddShape()
    {
        NewRockShape = new GameObject("Primitive Shape (Cube)");
        Undo.RegisterCreatedObjectUndo(NewRockShape, "Create");

        NewRockShape.transform.parent = GameObject.Find("_Sculpture_").transform;
        NewRockShape.AddComponent<Handle>();
        Selection.activeGameObject = NewRockShape;
    }

    void ShowRockTypes()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;
        TypeOfRock = (RockType)EditorGUILayout.EnumPopup("Type", TypeOfRock);
        EditorGUILayout.Space();

        if (TypeOfRock == RockType.Cubic)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Cubic</b></color></size>", style);
            Length = EditorGUILayout.Slider("Edge Length", Length, 0, 5);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.Tetragonal)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Tetragonal</b></color></size>", style);
            TetragonalScale = EditorGUILayout.Vector3Field("Central Part Scale", TetragonalScale);
            TetragonalPointLength = EditorGUILayout.Slider("Vertex Portrusion", TetragonalPointLength, 0, 2 * TetragonalScale.z);
            PointSmoothing = EditorGUILayout.Slider("Vertex Smoothing", PointSmoothing, 0, 1);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.TriclinicAndObelisk)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Triclinic and Obelisk</b></color></size>", style);
            TriclinicScale = EditorGUILayout.Vector3Field("Central Part Scale", TriclinicScale);
            TriclinicEdgeHeigth = EditorGUILayout.Slider("Edge Portrusion", TriclinicEdgeHeigth, 0, 2 * TriclinicScale.z);
            TriclinicEdgeLength = EditorGUILayout.Slider("Edge Length", TriclinicEdgeLength, 0, 1);
            TriclinicEdgeSmoothing = EditorGUILayout.Slider("Edge Smoothing", TriclinicEdgeSmoothing, 0, 1);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.Orthorombic)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Orthorombic</b></color></size>", style);
            Scale = EditorGUILayout.Vector3Field("Scale", Scale);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.Hexagonal)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Hexagonal</b></color></size>", style);
            HexagonalScale = EditorGUILayout.Vector3Field("Central Part Scale", HexagonalScale);
            HexagonalEdgeWidth = EditorGUILayout.Slider("Edge Portrusion", HexagonalEdgeWidth, 1, 3);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.MonoclinicAndTrigonal)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Monoclinic and Trigonal</b></color></size>", style);
            TrigonalScale = EditorGUILayout.Vector3Field("Central Part Scale", TrigonalScale);
            TrigonalEdgePosition = EditorGUILayout.Slider("Edge Portrusion", TrigonalEdgePosition, 0, 2 * TrigonalScale.z);
            TrigonalEdgeSmoothing = EditorGUILayout.Slider("Edge Smoothing", TrigonalEdgeSmoothing, 0, 1);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.Octahedral)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Octahedral</b></color></size>", style);
            OctahedralScale = EditorGUILayout.Vector3Field("Central Part Scale", OctahedralScale);
            OctahedralPointPosition = EditorGUILayout.Slider("Vertex Portrusion", OctahedralPointPosition, 0.75f, 3);
            OctahedralPointSmoothing = EditorGUILayout.Slider("Vertex Smoothing", OctahedralPointSmoothing, 0, 1);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.Spherical)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Sphere</b></color></size>", style);
            Radius = EditorGUILayout.Slider("Radius", Radius, 0, 5);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.Quartz)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Quartz</b></color></size>", style);
            QuartzScale = EditorGUILayout.Vector3Field("Central Part Scale", QuartzScale);
            QuartzPointPosition = EditorGUILayout.Slider("Vertex Portrusion", QuartzPointPosition, 0, 2 * QuartzScale.z);
            QuartzEdgeWidth = EditorGUILayout.Slider("Edge Portrusion", QuartzEdgeWidth, 1, 3);
            SceneView.RepaintAll();
        }

        if (TypeOfRock == RockType.Pyramidal)
        {
            EditorGUILayout.LabelField("<size=11><color=black><b>Pyramidal</b></color></size>", style);
            PyramidalBaseScale = EditorGUILayout.Slider("Base Scale", PyramidalBaseScale, 0, 5);
            PyramidalVertexPortrusion = EditorGUILayout.Slider("Vertex Portrusion", PyramidalVertexPortrusion, 0, PyramidalBaseScale * 3);
            PyramidalVertexSmoothing = EditorGUILayout.Slider("Vertex Smoothing", PyramidalVertexSmoothing, 0, 1);
            SceneView.RepaintAll();
        }
    }

    static class Styles
    {
        internal static GUIContent Export;
        internal static GUIContent Combine;
        internal static GUIContent Settings;
        internal static GUIContent Rock;
        internal static GUIContent Sculpting;
        internal static GUIContent Compose;
        internal static GUIContent Brush;
        internal static GUIContent Library;
        internal static GUIStyle helpbox;

        static Styles()
        {
            Export = IconContent("export_colored", " <size=11><b> Export</b></size>");
            Combine = IconContent("combine_colored", " <size=11><b> Combine</b></size>");
            Settings = IconContent("settings_colored", " <size=11><b> Settings</b></size>");
            Rock = IconContent("rock_colored", " <size=11><b> Create</b></size>");
            Sculpting = IconContent("sculpture_colored", " <size=11><b> Sculpt</b></size>");
            Compose = IconContent("compose_colored", " <size=11><b> Compose</b></size>");
            Brush = IconContent("brush_colored", " <size=11><b> Prefab Painter</b></size>");
            Library = IconContent("library_colored", " <size=11><b> Rock Library</b></size>");

            helpbox = new GUIStyle(EditorStyles.helpBox);
            helpbox.alignment = TextAnchor.MiddleLeft;
            helpbox.richText = true;
        }

        static GUIContent IconContent(string icon, string text)
        {
            Texture2D cached = EditorGUIUtility.Load("Assets/Ameye/RockStudio/Icons/" + icon + ".png") as Texture2D;

            return new GUIContent(text, cached);
        }
    }
}
