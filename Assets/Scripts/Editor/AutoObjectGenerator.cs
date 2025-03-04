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
    public Texture2D gameTexture = null;
    public Color color = Color.red;
    public GameObject gameObject = null;
    private List<Pos> _positionList = new();

    private static Terrain saveTerrain;
    private static Texture2D saveTexture;
    private static Color saveColor;
    private static GameObject saveObject;
    [MenuItem(("Tools/AutoObjectGenerator"))]

    public new static void Show()
    {
        DisplayWizard<AutoObjectGenerator>("오브젝트 생성");
    }

    private void OnEnable()
    {
        if(saveTerrain !=null)
            gameTerrain = saveTerrain;

        if(saveTerrain != null)
            gameTexture = saveTexture;

        if(saveObject != null)
            gameObject = saveObject;

        if(saveColor != new Color(0,0,0,0))
            color = saveColor;
    }

    private void OnWizardUpdate()
    {
        if (gameTerrain == null || gameTerrain == null || gameTexture == null)
        {
            isValid = false;
        }
        else
        {
            isValid = true;
        }

        if(saveTerrain != gameTerrain)
            saveTerrain = gameTerrain;

        if(saveTexture != gameTexture)
            saveTexture = gameTexture;

        if(saveObject != gameObject)
            saveObject = gameObject;

        if(saveColor != color)
            saveColor = color;
    }

    private void OnWizardCreate()
    {
        _positionList = new();
        float widthRatio = gameTerrain.terrainData.size.x / gameTexture.width;
        float heightRatio = gameTerrain.terrainData.size.z / gameTexture.height;

        for (int y = 0; y < gameTexture.height; ++y)
        {
            for (int x = 0; x < gameTexture.width; ++x)
            {
                Color pixel = gameTexture.GetPixel(x, y);
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
            Ray ray = new(terrainLocalPos, Vector3.down);

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
