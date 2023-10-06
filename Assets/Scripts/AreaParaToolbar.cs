using UnityEngine;
using DG.Tweening;

public class AreaParaToolbar : MonoBehaviour
{
    [SerializeField] private int numArea;
    [SerializeField] private TipoArea tipoArea;
    [SerializeField] private RectTransform tbRect;
    [SerializeField] private RectTransform thistbRect;

    private RectTransform interf;
    private Toolbar tb;


    private void Awake()
    {
        tb = tbRect.GetComponent<Toolbar>();
        thistbRect = GetComponentsInChildren<RectTransform>()[1];
        interf = GameObject.FindGameObjectWithTag("Interface").GetComponent<RectTransform>();
    }


    public void EncaixarEmArea()
    {
        tbRect.transform.SetParent(thistbRect);
        switch (tipoArea)
        {
            case TipoArea.DIREITA:
                tbRect.transform.eulerAngles = new Vector3(0, 0, -90);
                break;
            case TipoArea.ESQUERDA:
                tbRect.transform.eulerAngles = new Vector3(0, 0, -270);
                break;
            case TipoArea.CIMA:
                tbRect.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case TipoArea.BAIXO:
                tbRect.transform.eulerAngles = new Vector3(0, 0, -180);
                break;
        }
        tbRect.transform.localPosition = Vector3.zero;
        tbRect.transform.SetParent(interf.transform);
        
        tb.SetarArea(numArea);
    }


    public int GetAreaNum()
    {
        return numArea;
    }
    
}
