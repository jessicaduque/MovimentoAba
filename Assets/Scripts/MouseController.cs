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

    bool podeUsarMouse = true;

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
        podeUsarMouse = !IsMouseMoving();

        if (!podeUsarMouse)
        {
            MoveCursor();
        }
    }

    void MoveCursor()
    {
        Mouse.current.WarpCursorPosition(new Vector2(screenWidth * cursorX_controlId, screenHeight * cursory_controlId));
    }

    public bool IsMouseMoving()
    {
        return Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;
    }

    public void SetCursorPosControlId(float x, float y)
    {
        cursorX_controlId = x; 
        cursory_controlId = 1 - y;
    }

}