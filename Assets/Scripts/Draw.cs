using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class Draw : Singleton<Draw>
{
    // Refer�ncia para a c�mera usada para converter as coordenadas do mouse para o mundo virtual
    [SerializeField] Camera m_camera;

    // Prefab do pincel que ser� desenhado.
    [SerializeField] GameObject brush;

    // Refer�ncia para o LineRenderer da linha atual sendo desenhada.
    LineRenderer currentLineRenderer;

    // �ltima posi��o registrada do mouse.
    Vector2 lastPos;

    // Lista que armazenar� os objetos dos pinceis desenhados.
    [SerializeField] List<GameObject> brushStrokes;
    // Lista que armazenar� os objetos dos pinceis desfeitos (undo).
    [SerializeField] List<GameObject> reUndoBrush;

    // Booleano que indica se pode ou n�o desenhar
    bool canDraw = true;

    private void Awake()
    {
        // Inicializa��o das listas no in�cio do script.
        brushStrokes = new List<GameObject>();
        reUndoBrush = new List<GameObject>();
    }

    // M�todo chamado a cada frame.
    private void Update()
    {
        // Se for permitido desenhar, chama o m�todo respons�vel pelo desenho.
        if (canDraw)
        {
            Drawing();
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    // Fun��o que controla o desenho.
    void Drawing()
    {
        // Verifica se o bot�o esquerdo do mouse foi pressionado.
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Se pressionado, cria um novo pincel.
            CreateBrush();
            ClearReUndo();

        }
        // Verifica se o bot�o esquerdo do mouse est� sendo mantido pressionado.
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            // Se mantido pressionado, atualiza a posi��o do desenho.
            PointToMousePos();
        }
        // Se o bot�o n�o est� sendo pressionado, limpa a refer�ncia para a linha atual.
        else
        {
            currentLineRenderer = null;
        }

        // Verifica se "CTRL + Z" est� sendo mantido pressionado.
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Desfazer rabisco");
            UndoBrush();
        }
        // Verifica se "ESC" est� sendo mantido pressionado.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Limpar a tela");
            ClearBoard();
        }
        // Verifica se "CTRL + Y" est� sendo mantido pressionado.
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Refazer rabisco");
            ReDoBrush();
        }
    }

    #region Undo & Redo
    // Desfaz o ultimo pincel da lista 
    void UndoBrush()
    {
        if (brushStrokes.Count > 0)
        {
            // Adiciona o �ltimo pincel � lista de desfeitos (undo).
            reUndoBrush.Add(brushStrokes[brushStrokes.Count - 1]);
            Debug.Log("Adicionado a lista reUndo");
            brushStrokes[brushStrokes.Count-1].SetActive(false);
            brushStrokes.RemoveAt(brushStrokes.Count - 1);
        }
    }
    // Refaz o �ltimo pincel da lista de desfeitos.
    void ReDoBrush() 
    {
        if (reUndoBrush.Count > 0)
        {
            brushStrokes.Add(reUndoBrush[reUndoBrush.Count - 1]);
            reUndoBrush[reUndoBrush.Count - 1].SetActive(true);
            reUndoBrush.RemoveAt(reUndoBrush.Count - 1);
        }
    }
    #endregion

    #region Clear

    // Limpa todos os pinceis da lista
    void ClearBoard()
    {
        if (brushStrokes.Count > 0)
        {
            // Destroi todos os pinceis desenhados e limpa a lista.
            foreach (GameObject bs in brushStrokes)
            {
;
                Debug.Log("Tela sendo limpa");
                Destroy(bs);

            }
            Debug.Log("Tela completamente limpa");
            brushStrokes.Clear();
        }
    }

    void ClearReUndo()
    {
        if (reUndoBrush.Count > 0)
        {
            // Destroi todos os pinceis desenhados e limpa a lista.
            foreach (GameObject bs in reUndoBrush)
            {
                
                Destroy(bs);
            }
            Debug.Log("Lista de refazer limpa");
            reUndoBrush.Clear();
        }

    }

    #endregion

    // Fun��o que cria um novo pincel.
    void CreateBrush()
    {
        // Instancia um novo objeto pincel.
        GameObject brushInstance = Instantiate(brush);

        // Adiciona o pincel � lista de desenhos.
        brushStrokes.Add(brushInstance);

        // Obt�m a refer�ncia para o LineRenderer do pincel.
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        // Define as posi��es inicial e final da linha do pincel na posi��o do mouse.
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    // Fun��o que adiciona um ponto � linha do pincel.
    void AddAPoint(Vector2 pointPos)
    {
        // Adiciona um novo ponto � linha do pincel.
        currentLineRenderer.positionCount++;
        Debug.Log(currentLineRenderer);
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    // Fun��o que move a linha do pincel para a posi��o do mouse.
    void PointToMousePos()
    {
        // Converte a posi��o do mouse para as coordenadas do mundo do jogo.
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        // Verifica se a posi��o do mouse � diferente da �ltima posi��o registrada.
        if (lastPos != mousePos)
        {
            // Adiciona um ponto � linha do pincel e atualiza a �ltima posi��o registrada.
            AddAPoint(mousePos);
            lastPos = mousePos;
        }
    }

    #region Set

    public void SetCanDraw(bool state)
    {
        canDraw = state;
    }

    #endregion
}