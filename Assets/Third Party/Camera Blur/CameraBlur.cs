using UnityEngine;

//behaviour which should lie on the same gameobject as the main camera
[ExecuteInEditMode]
public class CameraBlur : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    protected float scale = 0.5f;
    public float Scale
    {
        get => scale;
        set => scale = Mathf.Clamp01(value);
    }

    //material that's applied when doing postprocessing
    [SerializeField]
    protected Material material;

    //method which is automatically called by unity after the camera is done rendering
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //draws the pixels from the source texture to the destination texture
        var renderTexture = RenderTexture.GetTemporary(source.width, source.height);

        material.SetFloat("_Scale", scale);

        Graphics.Blit(source, renderTexture, material, 0);
        Graphics.Blit(renderTexture, destination, material, 1);

        RenderTexture.ReleaseTemporary(renderTexture);
    }
}