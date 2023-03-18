using UnityEngine;
using XIV.Utils;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] GifSO gifSO;
    
    int currentSpriteIndex;
    Color[] pixelBuffer;
    Timer timer;
    
    void Start()
    {
        timer = new Timer(1f / gifSO.framesPerSecond);
        pixelBuffer = new Color[renderTexture.width * renderTexture.height];
    }

    void Update()
    {
        // Check if it's time to write the next frame
        if (timer.Update(Time.deltaTime) == false) return;
        
        timer.Restart();

        // renderTexture.DiscardContents(true, false);
        var currentSprite = gifSO.frames[currentSpriteIndex];
        var texture = TextureUtils.CreateTextureNonAlloc(currentSprite, renderTexture.width, renderTexture.height, pixelBuffer);
        Graphics.Blit(texture, renderTexture);
        currentSpriteIndex = (currentSpriteIndex + 1) % gifSO.frames.Length;
    }

    void OnDestroy()
    {
        renderTexture.Release();
    }
}