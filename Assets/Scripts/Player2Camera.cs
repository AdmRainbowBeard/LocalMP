using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Camera : MonoBehaviour
{
    GameObject me;

    void Start()
    {
        int i = me.GetComponent<Camera>().targetDisplay;
    }

    void Update()
    {
        
    }
}
