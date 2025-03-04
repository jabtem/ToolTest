using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScanTest : MonoBehaviour
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

    public Texture2D texture;
    public Terrain terrain;
    public GameObject testObj;

    private List<Pos> _positionList = new();

    [ContextMenu("Scan")]
    public void SacnTerrain()
    {
        _positionList = new();
        float widthRatio = terrain.terrainData.size.x / texture.width;
        float heightRatio = terrain.terrainData.size.z / texture.height;

        for (int y = 0; y < texture.height; ++y)
        {
            for (int x = 0; x < texture.width; ++x)
            {
                Color32 pixel = texture.GetPixel(x, y);

                if (pixel.r == 255 && pixel.g == 255)
                {
                    _positionList.Add(new Pos(x, y));
                }
            }
        }

        int layerMask = 1 << LayerMask.NameToLayer("TEST");

        foreach(var pos in _positionList)
        {

            Vector3 terrainLocalPos = terrain.transform.position + new Vector3(pos.X * widthRatio, 600f, pos.Y * heightRatio);
            Ray ray = new Ray(terrainLocalPos, Vector3.down);

            if(Physics.Raycast(ray, out RaycastHit rayhit, 1000f, layerMask))
            {
                GameObject newHolecup = Instantiate(testObj);
                newHolecup.transform.position = rayhit.point;
                newHolecup.name = testObj.name;
            }
        }
    }
}
