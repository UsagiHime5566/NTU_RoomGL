using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUI : MonoBehaviour
{
    public CanvasGroup cv;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (cv.alpha == 0) cv.alpha = 1;
            else cv.alpha = 0;
        }
    }
}
