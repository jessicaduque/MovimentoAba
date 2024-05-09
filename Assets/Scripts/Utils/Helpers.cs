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

    public Vector3 PythonToScreenPoints(float pointX, float pointY)
    {
        return new Vector3(pointX * screenWidth, (1 - pointY) * screenHeight, 0);
    }
}
