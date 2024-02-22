using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class Draw : Singleton<Draw>
{
    // Referência para a câmera usada para converter as coordenadas do mouse para o mundo virtual
    [SerializeField] Camera m_camera;

    // Prefab do pincel que será desenhado.
    [SerializeField] GameObject brush;

    // Referência para o LineRenderer da linha atual sendo desenhada.
    LineRenderer currentLineRenderer;

    // Última posição registrada do mouse.
    Vector2 lastPos;

    // Lista que armazenará os objetos dos pinceis desenhados.
    [SerializeField] List<GameObject> brushStrokes;
    // Lista que armazenará os objetos dos pinceis desfeitos (undo).
    [SerializeField] List<GameObject> reUndoBrush;

    // Booleano que indica se pode ou não desenhar
    bool canDraw = true;

    private void Awake()
    {
        // Inicialização das listas no início do script.
        brushStrokes = new List<GameObject>();
        reUndoBrush = new List<GameObject>();
    }

    // Método chamado a cada frame.
    private void Update()
    {
        // Se for permitido desenhar, chama o método responsável pelo desenho.
        if (canDraw)
        {
            Drawing();
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    // Função que controla o desenho.
    void Drawing()
    {
        // Verifica se o botão esquerdo do mouse foi pressionado.
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Se pressionado, cria um novo pincel.
            CreateBrush();
            ClearReUndo();

        }
        // Verifica se o botão esquerdo do mouse está sendo mantido pressionado.
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            // Se mantido pressionado, atualiza a posição do desenho.
            PointToMousePos();
        }
        // Se o botão não está sendo pressionado, limpa a referência para a linha atual.
        else
        {
            currentLineRenderer = null;
        }

        // Verifica se "CTRL + Z" está sendo mantido pressionado.
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Desfazer rabisco");
            UndoBrush();
        }
        // Verifica se "ESC" está sendo mantido pressionado.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Limpar a tela");
            ClearBoard();
        }
        // Verifica se "CTRL + Y" está sendo mantido pressionado.
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
            // Adiciona o último pincel à lista de desfeitos (undo).
            reUndoBrush.Add(brushStrokes[brushStrokes.Count - 1]);
            Debug.Log("Adicionado a lista reUndo");
            brushStrokes[brushStrokes.Count-1].SetActive(false);
            brushStrokes.RemoveAt(brushStrokes.Count - 1);
        }
    }
    // Refaz o último pincel da lista de desfeitos.
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

    // Função que cria um novo pincel.
    void CreateBrush()
    {
        // Instancia um novo objeto pincel.
        GameObject brushInstance = Instantiate(brush);

        // Adiciona o pincel à lista de desenhos.
        brushStrokes.Add(brushInstance);

        // Obtém a referência para o LineRenderer do pincel.
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        // Define as posições inicial e final da linha do pincel na posição do mouse.
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    // Função que adiciona um ponto à linha do pincel.
    void AddAPoint(Vector2 pointPos)
    {
        // Adiciona um novo ponto à linha do pincel.
        currentLineRenderer.positionCount++;
        Debug.Log(currentLineRenderer);
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    // Função que move a linha do pincel para a posição do mouse.
    void PointToMousePos()
    {
        // Converte a posição do mouse para as coordenadas do mundo do jogo.
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        // Verifica se a posição do mouse é diferente da última posição registrada.
        if (lastPos != mousePos)
        {
            // Adiciona um ponto à linha do pincel e atualiza a última posição registrada.
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