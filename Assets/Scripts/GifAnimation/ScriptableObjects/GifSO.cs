using UnityEngine;

[CreateAssetMenu(menuName = "DataContainers/GifSO")]
public class GifSO : ScriptableObject
{
    public Sprite[] frames;
    public int framesPerSecond;
}