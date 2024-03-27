using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : Utils.Singleton.Singleton<MouseController>
{
    Mouse mouse = Mouse.current;    

    float cursorX_controlId;
    float cursory_controlId;

    public bool ferramentaAtivada = false;

    Helpers _helpers => Helpers.I;

    protected override void Awake()
    {
        mouse = Mouse.current;
    }

    private void Start()
    {
        mouse.WarpCursorPosition(new Vector2(_helpers.screenWidth / 2, _helpers.screenHeight / 2));
    }


    private void Update()
    {
        if (IsMouseMoving())
            ferramentaAtivada = false;
        if (ferramentaAtivada)
            MoveCursor();
    }

    void MoveCursor()
    {
        Mouse.current.WarpCursorPosition(new Vector2(_helpers.screenWidth * cursorX_controlId, _helpers.screenHeight * cursory_controlId));
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