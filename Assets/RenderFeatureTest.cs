using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RenderFeatureTest : MonoBehaviour
{
    public ScriptableRendererFeature feature;

    // Start is called before the first frame update
    void Start()
    {
        feature.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
