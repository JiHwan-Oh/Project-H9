using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : Generic.Singleton<UIManager>
{
    [HideInInspector]
    public CurrentStatusUI currentStatusUI { get; private set; }
    public TimingUI timingUI { get; private set; }
    public QuestUI questUI { get; private set; }
    public CombatWindowUI combatUI { get; private set; }
    public CharacterUI characterUI { get; private set; }
    public SkillUI skillUI { get; private set; }
    public PauseMenuUI pauseMenuUI { get; private set; }

    [Header("Canvases")]
    [SerializeField] private Canvas _worldCanvas;
    [SerializeField] private Canvas _combatCanvas;
    [SerializeField] private Canvas _characterCanvas;
    [SerializeField] private Canvas _skillCanvas;
    [SerializeField] private Canvas _pauseMenuCanvas;
    
    //[HideInInspector]
    public bool isMouseOverUI;
    public int previousLayer = 1;

    private string prevSceneName;
    private bool _isCombatScene = false;


    private new void Awake()
    {
        base.Awake();
        _combatCanvas.enabled = false;
        _characterCanvas.enabled = false;
        _skillCanvas.enabled = false;
        _pauseMenuCanvas.enabled = false;

        currentStatusUI = _worldCanvas.GetComponent<CurrentStatusUI>();
        timingUI = _worldCanvas.GetComponent<TimingUI>();
        questUI = _worldCanvas.GetComponent<QuestUI>();

        combatUI = _combatCanvas.GetComponent<CombatWindowUI>();

        characterUI = _characterCanvas.GetComponent<CharacterUI>();
        skillUI = _skillCanvas.GetComponent<SkillUI>();
        pauseMenuUI = _pauseMenuCanvas.GetComponent<PauseMenuUI>();

        SetCanvasState(_characterCanvas, characterUI, false);
        SetCanvasState(_skillCanvas, skillUI, false);
        SetCanvasState(_pauseMenuCanvas, pauseMenuUI, false);

        prevSceneName = SceneManager.GetActiveScene().name;
    }
    void Update()
    {
        isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
        if (Input.GetMouseButtonDown(0))
        {
            int currentLayer = GetPointerOverUILayer();
            if (previousLayer > currentLayer)
            {
                if (currentLayer <= 1)
                {
                    SetCharacterCanvasState(false);
                    SetSkillCanvasState(false);
                    SetPauseMenuCanvasState(false);
                }
                else if (currentLayer == 2) 
                {
                    characterUI.ClosePopupWindow();
                    skillUI.ClosePopupWindow();
                }
            }

            previousLayer = currentLayer;
        }
    }

    public void SetCanvasState(Canvas canvas, UISystem uiSys, bool isOn)
    {
        if (canvas.enabled && isOn)
        {
            isOn = false;
        }

        if (canvas.enabled != isOn) 
        {
            if (isOn)
            {
                canvas.enabled = isOn;
                uiSys.OpenUI();
            }
            else
            {
                uiSys.CloseUI();
                canvas.enabled = isOn;
            }
        }
    }
    public void SetCombatCanvasState(bool isOn)
    {
        SetCanvasState(_combatCanvas, combatUI, isOn);
    }
    public void SetCharacterCanvasState(bool isOn)
    {
        SetCanvasState(_characterCanvas, characterUI, isOn);
    }
    public void SetSkillCanvasState(bool isOn)
    {
        SetCanvasState(_skillCanvas, skillUI, isOn);
    }
    public void SetPauseMenuCanvasState(bool isOn)
    {
        SetCanvasState(_pauseMenuCanvas, pauseMenuUI, isOn);
    }

    private int GetPointerOverUILayer() 
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        foreach (RaycastResult result in results) 
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI")) 
            {
                return 0;
            }
            else if (result.gameObject.layer == LayerMask.NameToLayer("UI1"))
            {
                return 1;
            }
            else if (result.gameObject.layer == LayerMask.NameToLayer("UI2"))
            {
                return 2;
            }
            else if (result.gameObject.layer == LayerMask.NameToLayer("UI3"))
            {
                return 3;
            }
        }

        return -1;
    }

    /// <summary>
    /// for development test
    /// </summary>
    /// <param name="gameState"></param>
    public void ChangeScenePrepare(GameState gameState)
    {
        GameState _realGameState = GameState.World;
        if (SceneManager.GetActiveScene().name == "WorldScene" || SceneManager.GetActiveScene().name == "UITestScene")
        {
            _realGameState = GameState.World;
        }
        else if(SceneManager.GetActiveScene().name == "CombatScene")
        {
            _realGameState = GameState.Combat;
        }


        if (gameState != _realGameState)
        {
            StartCoroutine(csp(gameState));
        }
        else 
        {
            ChangeScene(gameState);
        }
    }
    IEnumerator csp(GameState gameState) 
    {
        while (true) 
        {
            yield return new WaitForSeconds(0.1f);
            ChangeScenePrepare(gameState);
            yield break;
        }
    }
    public void ChangeScene(GameState gameState)
    {
        //if (prevSceneName == currentSceneName) return;
        Debug.Log("Current State is " + gameState);
        switch (gameState)
        {
            case GameState.World:
                {
                    _isCombatScene = false;
                    ChangeUIToWorldScene();
                    break;
                }
            case GameState.Combat:
                {
                    _isCombatScene = true;
                    ChangeUIToCombatScene();
                    break;
                }
        }

    }
    private void ChangeUIToWorldScene()
    {
        SetCombatCanvasState(false);
        timingUI.SetTurnOrderUIState(false);
    }
    private void ChangeUIToCombatScene()
    {
        SetCombatCanvasState(true);
        timingUI.SetTurnOrderUIState(true);
    }
}
