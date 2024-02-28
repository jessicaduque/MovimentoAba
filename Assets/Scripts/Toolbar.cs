using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Toolbar : MonoBehaviour, IPointerClickHandler
{
    private bool cursorSeguindo = false;
    private bool dentroArea = false;
    private Button botao;
    private int ultimaArea;
    private int areaAtual;
    
    [Header("Áreas")]
    [SerializeField] AreaParaToolbar[] areas;
    [SerializeField] CanvasGroup alphaAreas;

    [Header("Toolbar Aberto")]
    [SerializeField] RectTransform rectToolbarAberto;
    [SerializeField] RectTransform[] opcoesToolbar;
    private bool tbAberto = false;

    [Header("Colors")]
    [SerializeField] Color green;
    [SerializeField] Color gray;

    Draw _draw => Draw.I;

    private void Awake()
    {
        botao = GetComponent<Button>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.001f);
        areas[0].EncaixarEmArea();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && !cursorSeguindo)
        {
            AbrirToolbar();
        }
    }

    private void AbrirToolbar()
    {
        foreach(RectTransform op in opcoesToolbar)
        {
            op.transform.eulerAngles = Vector3.zero;
        }
        if (tbAberto)
        {
            rectToolbarAberto.DOScaleY(0, 0.4f).OnComplete(() => tbAberto = false);
        }
        else
        {
            rectToolbarAberto.DOScaleY(1, 0.4f).SetEase(Ease.OutBounce).OnComplete(() => tbAberto = true);
        }
    }

    private void OnMouseDown()
    {
        _draw.SetCanDraw(false);
        rectToolbarAberto.DOScaleY(0, 0.4f);
        GetComponent<RectTransform>().pivot = new Vector3(0.5f, 0.5f);
        botao.enabled = false;
        alphaAreas.DOFade(1, 0.2f);
        cursorSeguindo = true;
        StartCoroutine(SeguirCursor());
        
    }

    IEnumerator SeguirCursor()
    {
        while (cursorSeguindo)
        {
            PosCursor();
            yield return null;
        }
    }

    private void OnMouseUp()
    {
        if (dentroArea)
        {
            areas[areaAtual].EncaixarEmArea();
        }
        else
        {
            areas[ultimaArea].EncaixarEmArea();
        }
        _draw.SetCanDraw(true);

    }

    void PosCursor()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(cursorPos.x, cursorPos.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        areaAtual = collision.GetComponent<AreaParaToolbar>().GetAreaNum();
        collision.gameObject.GetComponent<Image>().DOColor(green, 0.4f);
        dentroArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dentroArea = false;
        collision.gameObject.GetComponent<Image>().DOColor(gray, 0.4f);
    }

    public void SetarArea(int area)
    {
        ultimaArea = area;
        cursorSeguindo = false;
        alphaAreas.DOFade(0, 0.2f).OnComplete(() => botao.enabled = true);
    }


}