using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Control_Id_Api : MonoBehaviour
{
    public Text requestResult;
    public Control_Id_class control;

    float cursorX;
    float cursorY;

    // Variáveis para armazenar as listas de pontos
    float[] pointListX;
    float[] pointListY;

    private string estadoAtual;

    MouseController _mouseController => MouseController.I;
    Draw _drawManager => Draw.I;
    Helpers _helpers => Helpers.I;

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

            cursorX = control.cursor_x;
            cursorY = control.cursor_y;

            pointListX = control.point_list_x;
            pointListY = control.point_list_y;


            if (cursorX == -1 || cursorY == -1)
            {
                _drawManager.SetCanDraw(false);
                yield return null;
            }

            if (estado != "G-pen-down" && estadoAtual == "G-pen-down")
            {
                Debug.Log("Parou de desenhar");
                _drawManager.SetPointsLists(control.point_list_x, control.point_list_y);
                _drawManager.SetPointsLastLineRenderer();
                //Debug.Log("Length" + control.point_list_x.Length);
                //for (int i = 0; i < control.point_list_x.Length; i++)
                //{
                //    Debug.Log("x: " + control.point_list_x[i]);
                //    Debug.Log("y: " + control.point_list_y[i]);
                //}
            }

            _mouseController.SetCursorPosControlId(cursorX, cursorY);

            if (estado != null)
            {
                requestResult.text = estado;
                ChecarFuncionalidade(estado);
            }


            /*
            Debug.Log("estado" + estado);
            Debug.Log("estadoAtual" + estadoAtual);
            if (estado != "G-pen-down" && estadoAtual == "G-pen-down")
            {
                _drawManager.SetPointsLastLineRenderer(control.point_list_x, control.point_list_y);
                Debug.Log("Parou de desenhar");
                for (int i = 0; i < control.point_list_x.Length; i++)
                {
                    Debug.Log(control.point_list_x[i]);
                    Debug.Log(control.point_list_y[i]);
                }
            }


            if (estado != null)
            {
                requestResult.text = estado;
                ChecarFuncionalidade(estado);
            }
            */

        }

    }

    void ChecarFuncionalidade(string estado)
    {
        switch (estado)
        {
            case "G-pen-down":
                // Checar que mouse não está movendo
                if (_mouseController.IsMouseMoving())
                {
                    _drawManager.SetCanDraw(false);
                    break;
                }

                if (!_mouseController.ferramentaAtivada && estadoAtual != estado)
                {
                    Mouse.current.WarpCursorPosition(new Vector2(_helpers.screenWidth * cursorX, _helpers.screenHeight * cursorY));
                    _mouseController.ferramentaAtivada = true;
                }

                _drawManager.SetCanDraw(_mouseController.ferramentaAtivada);

                break;
            default:
                _drawManager.SetCanDraw(false);
                break;
        }
        estadoAtual = estado;
    }

}
