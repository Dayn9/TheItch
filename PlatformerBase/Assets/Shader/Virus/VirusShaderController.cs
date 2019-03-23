using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusShaderController : MonoBehaviour
{
   public Material virusMat;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, virusMat);
    }
}
