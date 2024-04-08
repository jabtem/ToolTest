using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class CreateSprite : MonoBehaviour
{
    public Camera meshCamera;
    string savePath;
    public Image test;

    [ContextMenu("Test")]
    public void CaptureMeshToTransparentSprite()
    {
        savePath = Application.dataPath + "/abc.png";
        // 렌더 텍스처에 렌더링합니다.
        //meshCamera.targetTexture = renderTexture;
        //meshCamera.Render();

        // 렌더링된 이미지를 스크린 캡처하여 가져옵니다.
        RenderTexture.active = meshCamera.activeTexture;
        Texture2D capturedTexture = new Texture2D(meshCamera.activeTexture.width, meshCamera.activeTexture.height);
        capturedTexture.ReadPixels(new Rect(0, 0, meshCamera.activeTexture.width, meshCamera.activeTexture.height), 0, 0);
        capturedTexture.Apply();

        // 배경이 투명한 스프라이트로 변환합니다.
        Sprite transparentSprite = CreateTransparentSprite(capturedTexture);
        test.sprite = transparentSprite;
        // 생성된 스프라이트를 저장하거나 다른 용도로 사용할 수 있습니다.
        // 예를 들어, 스프라이트를 파일로 저장하려면:
        //byte[] bytes = transparentSprite.texture.EncodeToPNG();
        //System.IO.File.WriteAllBytes(savePath, bytes);

        //AssetDatabase.Refresh();
    }

    Sprite CreateTransparentSprite(Texture2D texture)
    {
        // 배경이 투명한 스프라이트를 생성합니다.
        Color32[] pixels = texture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r <= 1 && pixels[i].g <= 1 && pixels[i].b <= 1) // 검은색 픽셀을 투명으로 바꿉니다.
            {
                pixels[i].a = 0;
            }
        }
        texture.SetPixels32(pixels);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100f, 0, SpriteMeshType.FullRect);
        return sprite;
    }
}
