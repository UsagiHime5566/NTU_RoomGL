using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITab : MonoBehaviour
{
    public Button BTN_Colapse;
    public Button BTN_Seperate;

    public GameObject Group_Wall;
    public GameObject Group_Wall1;
    public GameObject Group_Wall2;
    public GameObject Group_Wall3;

    void Start()
    {
        BTN_Colapse.onClick.AddListener(() =>
        {
            //MainManager.instance.SetPlayMode(0);
            Group_Wall.SetActive(true);
            Group_Wall1.SetActive(false);
            Group_Wall2.SetActive(false);
            Group_Wall3.SetActive(false);
        });

        BTN_Seperate.onClick.AddListener(() =>
        {
            //MainManager.instance.SetPlayMode(1);
            Group_Wall.SetActive(false);
            Group_Wall1.SetActive(true);
            Group_Wall2.SetActive(true);
            Group_Wall3.SetActive(true);
        });
    }
}
