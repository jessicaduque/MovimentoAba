using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarSlotsController : MonoBehaviour
{
    [SerializeField] private Button b_1, b_2, b_3, b_4, b_5;
    private Toolbar _toolbar;

    private void Awake()
    {
        b_1.onClick.AddListener(Function_b1);
        b_2.onClick.AddListener(Function_b2);
        b_3.onClick.AddListener(Function_b3);
        b_4.onClick.AddListener(Function_b4);
        b_5.onClick.AddListener(Function_b5);

        _toolbar = FindObjectOfType<Toolbar>();

    }


    private void Function_b1()
    {
        Debug.Log("SOU O BOTAO 1 CHU´PA");
    }
    private void Function_b2()
    {
        Debug.Log("SOU O BOTAO 2 CHU´PA");
    }
    private void Function_b3()
    {
        Debug.Log("SOU O BOTAO 3 CHU´PA");
    }
    private void Function_b4()
    {
        Debug.Log("SOU O BOTAO 4 CHU´PA");
    }
    private void Function_b5()
    {
        Debug.Log("SOU O BOTAO 5 CHU´PA");
    }
}
