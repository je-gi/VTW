using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        if (Cursor.visible)
            {
                Cursor.visible = false;
            }
    }
}
