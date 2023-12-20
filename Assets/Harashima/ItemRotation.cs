using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    private void Update()
    {
        this.transform.Rotate(new Vector3(0,0.3f,0),Space.World);
    }
}
