using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{

    private Animator windowAnimLeft;
    private Animator windowAnimRight;

    private bool openWindowLeft = true;
    private bool openWindowRight = true;


    private void Awake()
    {
        windowAnimLeft = transform.GetChild(0).GetComponent<Animator>();
        windowAnimRight = transform.GetChild(1).GetComponent<Animator>();       
    }

    public void PlayWindowAnimation()
    {
        if (!openWindowRight)
        {
            windowAnimRight.Play("RightWindowOpen", 0, 0.0f);
            openWindowRight = true;
        }
        else
        {
            windowAnimRight.Play("RightWindowClose", 0, 0.0f);
            openWindowRight = false;
        }


        if (!openWindowLeft)
        {
            windowAnimLeft.Play("LeftWindowOpen", 0, 0.0f);
            openWindowLeft = true;
            
        }
        else
        {
            windowAnimLeft.Play("LeftWindowClose", 0, 0.0f);          
            openWindowLeft = false;
        }
    }

}
