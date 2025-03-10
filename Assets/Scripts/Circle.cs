using UnityEngine;
using System.Collections;
public class Circle : MonoBehaviour
{
	public int segments;
	public float xradius;
	public float yradius;
	LineRenderer line;

	void Start()
	{
		line = gameObject.GetComponent<LineRenderer>();

		line.positionCount = segments + 1;
		line.useWorldSpace = false;
		CreatePoints();
	}


	void CreatePoints()
	{
		float x;
		float y;
		float z = 0f;

		//원그리는 시작위치의 각도
		float angle = 0f;

		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Cos(Mathf.Deg2Rad * angle) * xradius;
			y = Mathf.Sin(Mathf.Deg2Rad * angle) * yradius;

			line.SetPosition(i, new Vector3(x, y, z));

			angle += (360f / segments);
		}
	}
}