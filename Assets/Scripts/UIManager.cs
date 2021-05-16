using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject fixFence;
    public GameObject clearTree;

    // Start is called before the first frame update
    void Start()
    {
        fixFence.SetActive(false);
        clearTree.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
