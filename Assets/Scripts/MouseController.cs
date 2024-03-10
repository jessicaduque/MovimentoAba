using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : Utils.Singleton.Singleton<MouseController>
{
    Mouse mouse = Mouse.current;
    float screenWidth;
    float screenHeight;

    float cursorX_controlId;
    float cursory_controlId;

    protected override void Awake()
    {
        mouse = Mouse.current;

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void Start()
    {
        mouse.WarpCursorPosition(new Vector2(screenWidth / 2, screenHeight / 2));
    }


    private void Update()
    {
        MoveCursor();
    }

    void MoveCursor()
    {
        Mouse.current.WarpCursorPosition(new Vector2(screenWidth * cursorX_controlId, screenHeight * cursory_controlId));
    }

    public void SetCursorPosControlId(float x, float y)
    {
        cursorX_controlId = x; 
        cursory_controlId = 1 - y;
    }

}