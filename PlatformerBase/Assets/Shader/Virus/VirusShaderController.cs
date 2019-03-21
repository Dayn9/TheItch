using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusShaderController : MonoBehaviour
{
    public Material VirusMat;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, VirusMat);
    }
}
