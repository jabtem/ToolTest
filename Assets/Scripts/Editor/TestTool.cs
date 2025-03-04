using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestTool : EditorWindow
{
    [MenuItem(("Tools/TerrainHoleTest"))]
    public new static void Show()
    {
        TestTool wnd = GetWindow<TestTool>();
        wnd.titleContent = new GUIContent("Test Tool UI");
    }

    bool b_Start;
    bool b_Click;
    GameObject holeGroupObject = null;
    GameObject holecupObject = null;
    GameObject greenObject = null;
    int holeIndex = 0;
    string[] op = { "A", "B", "C", "D", "E", "F" };

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUI.backgroundColor = Color.gray;
        holeIndex = EditorGUILayout.Popup("코스", holeIndex, op);

        GUILayout.Label("코스 오브젝트");
        holeGroupObject = (GameObject)EditorGUILayout.ObjectField(holeGroupObject, typeof(GameObject), true);
        GUILayout.Label("홀컵 모델");
        holecupObject = (GameObject)EditorGUILayout.ObjectField(holecupObject, typeof(GameObject), true);

        if (b_Start)
            GUI.backgroundColor = Color.green;
        else if (!b_Start)
            GUI.backgroundColor = Color.red;
        if (GUILayout.Button("b_Start"))
            b_Start = !b_Start;


    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!b_Start)
            return;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        int layerMask = (1 << LayerMask.NameToLayer("Ground"));

        Event @event = Event.current;

        if (@event.type == EventType.MouseDown && @event.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layerMask))
            {
                if(!b_Click)
                {
                    b_Click = true;
                    if (hit.collider.gameObject.TryGetComponent<Terrain>(out Terrain terrain))
                    {
                        //Undo는 명령을 실행하기 전 등록되어있어야함
                        Undo.RegisterCompleteObjectUndo(terrain.terrainData, "Modify Terrain");
                        PaintHoleAtLocation(terrain, hit.point);
                    }
                }
            }
        }
        else if(@event.type == EventType.MouseUp && @event.button == 0)
            b_Click = false;
    }


    //터레인에 구멍뚫는 함수
    void PaintHoleAtLocation(Terrain terrain, Vector3 worldPosition)
    {
        Vector3 terrainLocalPos = terrain.transform.InverseTransformPoint(worldPosition);
        int mapX = (int)((terrainLocalPos.x / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth);
        int mapZ = (int)((terrainLocalPos.z / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight);

        bool[,] hole = new bool[1, 1];
        hole[0, 0] = false;

        terrain.terrainData.SetHoles(mapX, mapZ, hole);
    }

    
}
