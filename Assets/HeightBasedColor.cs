using UnityEngine;

public class HeightBasedColor : MonoBehaviour
{
    public Shader heightBasedColorShader; // 사용할 셰이더

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
