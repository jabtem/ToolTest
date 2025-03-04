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
        holeIndex = EditorGUILayout.Popup("�ڽ�", holeIndex, op);

        GUILayout.Label("�ڽ� ������Ʈ");
        holeGroupObject = (GameObject)EditorGUILayout.ObjectField(holeGroupObject, typeof(GameObject), true);
        GUILayout.Label("Ȧ�� ��");
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
                        //Undo�� ����� �����ϱ� �� ��ϵǾ��־����
                        Undo.RegisterCompleteObjectUndo(terrain.terrainData, "Modify Terrain");
                        PaintHoleAtLocation(terrain, hit.point);
                    }
                }
            }
        }
        else if(@event.type == EventType.MouseUp && @event.button == 0)
            b_Click = false;
    }


    //�ͷ��ο� ���۶մ� �Լ�
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
