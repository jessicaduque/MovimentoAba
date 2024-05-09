using System.Collections;
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
    bool isDrawing;

    // Variáveis para armazenar as listas de pontos
    float[] pointListX;
    float[] pointListY;

    LineRenderer lr;
    Helpers _helpers => Helpers.I;
    protected override void Awake()
    {
        base.Awake();
        // Inicialização das listas no início do script.
        brushStrokes = new List<GameObject>();
        reUndoBrush = new List<GameObject>();
    }

    // Método chamado a cada frame.
    private void Update()
    {
        // Se for permitido desenhar, chama o método responsável pelo desenho
        Drawing();
        if (!canDraw)
        {
            isDrawing = false;
            currentLineRenderer = null;
        }
    }

    // Função que controla o desenho.
    void Drawing()
    {
        // Verifica se o botão esquerdo do mouse foi pressionado.
        if (canDraw && !isDrawing)
        {         
            StartCoroutine(PointToMousePos());
            isDrawing = true;
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
        //if (Input.GetKey(KeyCode.S)) 
        //{
        //    Debug.Log("Suaviazar");
        //    SetPointsLastLineRenderer();
        //}

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
        if (currentLineRenderer != null) 
        {
            currentLineRenderer.positionCount++;
            int positionIndex = currentLineRenderer.positionCount - 1;
            currentLineRenderer.SetPosition(positionIndex, pointPos);
        }
        
    }

    // Função que move a linha do pincel para a posição do mouse.
    IEnumerator PointToMousePos()
    {
        CreateBrush();
        ClearReUndo();

        while (canDraw)
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

            yield return null;
        }       
    }

    #region Set
    // Israel sabe como funciona. QUualquer dúvida extra, NÃO procurar a Jessica.

    public void SetPointsLists(float[] point_list_x, float[] point_list_y)
    {
        Debug.Log("Defini as listas!");
        pointListX = point_list_x;
        pointListY = point_list_y;  
    }

    public void SetPointsLastLineRenderer()
    {
        if(brushStrokes.Count == 0)
        {
            Debug.Log("Nada foi desenhado ainda!");
            return;
        }
        float[] testArray1 = pointListX;
        float[] testArray2 = pointListY;
        lr = brushStrokes[brushStrokes.Count - 1].GetComponent<LineRenderer>();
        lr.positionCount = testArray1.Length;

        for(int i = 0; i < lr.positionCount; i++)
        {
            Vector3 point = _helpers.PythonToScreenPoints(testArray1[i], testArray2[i]);
            lr.SetPosition(i, new Vector3(m_camera.ScreenToWorldPoint(point).x, m_camera.ScreenToWorldPoint(point).y, 0));
        }

        if(testArray1.Length - lr.positionCount > 0)
        {
            for(int i= lr.positionCount; i< testArray1.Length - lr.positionCount; i++)
            {
                Vector3 point = _helpers.PythonToScreenPoints(testArray1[i], testArray2[i]);
                lr.SetPosition(i, new Vector3(m_camera.ScreenToWorldPoint(point).x, m_camera.ScreenToWorldPoint(point).y, 0));
            }
        }
    }


    public void SetCanDraw(bool state)
    {
        canDraw = state;
    }

    #endregion
}