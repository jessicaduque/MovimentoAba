using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class Helpers : Singleton<Helpers>
{
    public float screenWidth { get; private set; }
    public float screenHeight { get; private set; }

    protected override void Awake()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
}
