using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoorController : MonoBehaviour
{
    private Animator doorAnim;

    private bool openDoor = false;

    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (!openDoor)
        {
            doorAnim.Play("DoorOpen", 0, 0.0f);
            openDoor = true;
        }
        else
        {
            doorAnim.Play("DoorClose", 0, 0.0f);
            openDoor = false;
        }
    }

}
