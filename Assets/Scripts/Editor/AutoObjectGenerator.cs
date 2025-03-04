using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutoObjectGenerator : ScriptableWizard
{
    private class Pos
    {
        public int X;
        public int Y;

        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public Terrain gameTerrain = null;
    public Texture2D mapTexture = null;
    public Color color = Color.red;
    public GameObject gameObject = null;
    private List<Pos> _positionList = new();
    [MenuItem(("Tools/AutoObjectGenerator"))]

    public new static void Show()
    {
        DisplayWizard<AutoObjectGenerator>("오브젝트 생성");
    }

    private void OnWizardUpdate()
    {
        if (gameTerrain == null || gameTerrain == null || mapTexture == null)
        {
            isValid = false;
        }
        else
            isValid = true;
    }

    private void OnWizardCreate()
    {
        _positionList = new();
        float widthRatio = gameTerrain.terrainData.size.x / mapTexture.width;
        float heightRatio = gameTerrain.terrainData.size.z / mapTexture.height;

        for (int y = 0; y < mapTexture.height; ++y)
        {
            for (int x = 0; x < mapTexture.width; ++x)
            {
                Color pixel = mapTexture.GetPixel(x, y);
                pixel.a = color.a;

                if (pixel == color)
                {
                    _positionList.Add(new Pos(x, y));
                }
            }
        }

        int layerMask = 1 << LayerMask.NameToLayer("TEST");

        foreach (var pos in _positionList)
        {

            Vector3 terrainLocalPos = gameTerrain.transform.position + new Vector3(pos.X * widthRatio, 600f, pos.Y * heightRatio);
            Ray ray = new Ray(terrainLocalPos, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit rayhit, 1000f, layerMask))
            {
                GameObject newObject = Instantiate(gameObject);
                newObject.transform.position = rayhit.point;
                newObject.name = gameObject.name;
                Undo.RegisterCreatedObjectUndo(newObject, "Create Object");
            }
        }
    }
}
