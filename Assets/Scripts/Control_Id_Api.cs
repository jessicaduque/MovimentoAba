using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UnityEngine.UI;


public class Control_Id_Api: MonoBehaviour
{
    public Text requestResult;
    public Control_Id_class control;

    private string estadoAtual;

    MouseController _mouseController => MouseController.I;
    Draw _drawManager => Draw.I;

    void Update()
    {
        StartCoroutine(GetDateTimeOnline());
    }
    public IEnumerator GetDateTimeOnline()
    {
        string url = "http://127.0.0.1:5000/returnjson";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            requestResult.text = "Erro de conexão: " + www.error;
        }
        else
        {
            string jsonDownloaded = www.downloadHandler.text;
            control = JsonUtility.FromJson<Control_Id_class>(jsonDownloaded);
            string estado = control.control_id;

            float cursorX = control.cursor_x;
            float cursorY = control.cursor_y;

            if(cursorX == -1 || cursorY == -1)
            {
                _drawManager.SetCanDraw(false);
                _mouseController.SetMouseParado(true);
                yield return null;
            }

            _mouseController.SetCursorPosControlId(cursorX, cursorY);

            if (estado != null)
            {
                requestResult.text = estado;
                ChecarFuncionalidade(estado);
            }
            
        }

    }

    void ChecarFuncionalidade(string estado)
    {
        switch (estado)
        {
            case "G-pen-down":
                if(estadoAtual != estado && !_mouseController.IsMouseMoving())
                {
                    _drawManager.SetCanDraw(true);
                }
                estadoAtual = estado;
                break;
            default:
                estadoAtual = "";
                _drawManager.SetCanDraw(false);
                break;
        }
    }
}
