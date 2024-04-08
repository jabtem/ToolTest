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
        // ���� �ؽ�ó�� �������մϴ�.
        //meshCamera.targetTexture = renderTexture;
        //meshCamera.Render();

        // �������� �̹����� ��ũ�� ĸó�Ͽ� �����ɴϴ�.
        RenderTexture.active = meshCamera.activeTexture;
        Texture2D capturedTexture = new Texture2D(meshCamera.activeTexture.width, meshCamera.activeTexture.height);
        capturedTexture.ReadPixels(new Rect(0, 0, meshCamera.activeTexture.width, meshCamera.activeTexture.height), 0, 0);
        capturedTexture.Apply();

        // ����� ������ ��������Ʈ�� ��ȯ�մϴ�.
        Sprite transparentSprite = CreateTransparentSprite(capturedTexture);
        test.sprite = transparentSprite;
        // ������ ��������Ʈ�� �����ϰų� �ٸ� �뵵�� ����� �� �ֽ��ϴ�.
        // ���� ���, ��������Ʈ�� ���Ϸ� �����Ϸ���:
        //byte[] bytes = transparentSprite.texture.EncodeToPNG();
        //System.IO.File.WriteAllBytes(savePath, bytes);

        //AssetDatabase.Refresh();
    }

    Sprite CreateTransparentSprite(Texture2D texture)
    {
        // ����� ������ ��������Ʈ�� �����մϴ�.
        Color32[] pixels = texture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r <= 1 && pixels[i].g <= 1 && pixels[i].b <= 1) // ������ �ȼ��� �������� �ٲߴϴ�.
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
