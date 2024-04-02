using UnityEngine;

public class PaintHolesInCenter : MonoBehaviour
{
    public Terrain terrain;
    public float radius = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f); // Ray를 그립니다.

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PaintHoleAtLocation(hit.point);
            }
        }
    }

    void PaintHoleAtLocation(Vector3 worldPosition)
    {
        Vector3 terrainLocalPos = terrain.transform.InverseTransformPoint(worldPosition);
        int mapX = (int)((terrainLocalPos.x / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth);
        int mapZ = (int)((terrainLocalPos.z / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight);

        bool[,] hole = new bool[1, 1];
        hole[0, 0] = false;

        int size = Mathf.RoundToInt(radius);
        int offset = size / 2;

        terrain.terrainData.SetHoles(mapX - offset, mapZ - offset, hole);
    }
}
