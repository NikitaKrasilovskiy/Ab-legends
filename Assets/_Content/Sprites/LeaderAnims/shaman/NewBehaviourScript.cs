using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class NewBehaviourScript : MonoBehaviour
{
    public UnityArmatureComponent unityArmatureComponent;

    public void PlayAnim()
    {
        if(unityArmatureComponent)
            unityArmatureComponent.animation.Play();
    }
}