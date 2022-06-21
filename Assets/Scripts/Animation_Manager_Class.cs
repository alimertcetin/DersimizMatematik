using UnityEngine;

public abstract class Animation_Manager_Class : MonoBehaviour
{

    protected abstract void AnimationToPlay(Animator animController, string animationName);

    protected abstract void AnimationToStop(Animator animController, string animationName);

}
