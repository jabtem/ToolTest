using UnityEngine;
using UnityEditor;

public class ObjectPlacementWindow : EditorWindow
{
    private bool isPlacingObject = false;

    [MenuItem("Window/Object Placement")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectPlacementWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Object Placement", EditorStyles.boldLabel);
        isPlacingObject = GUILayout.Toggle(isPlacingObject, "Start Placement");

        if (isPlacingObject)
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        else
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    newObject.transform.position = hit.point;
                    newObject.tag = "PlacedObject";
                    Undo.RegisterCreatedObjectUndo(newObject, "Create Object");
                }
            }
        }
    }
}
