using UnityEngine;

public class HeightBasedColor : MonoBehaviour
{
    public Shader heightBasedColorShader; // ����� ���̴�

    void Start()
    {
        Camera.main.SetReplacementShader(heightBasedColorShader, "");
    }


    [ContextMenu("text")]
    void test()
    {
        Camera.main.SetReplacementShader(heightBasedColorShader, "");
    }
}
