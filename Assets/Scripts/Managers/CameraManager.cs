using System.Collections.Generic;
using Cinemachine;
using Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private Transform _cameraParentTransform => transform;
    private readonly Dictionary<Unit, UnitCamera> _unitCameras = new ();
    private UnitCamera _currentUnitCamera;
    [SerializeField] private CinemachineBrain _brain;
    
    public WorldCamera worldCamera;
    public GameObject unitCameraPrefab;
    
    public void CreateUnitCamera(Unit target)
    {
        if (target is null)
        {
            Debug.LogWarning("Target is null");
            return;
        }
        
        var uCam = Instantiate(unitCameraPrefab, _cameraParentTransform).GetComponent<UnitCamera>();
        _unitCameras.Add(target, uCam);
        uCam.SetTarget(target);
    }

    private void LookAt(Unit target)
    {
        if (_unitCameras.TryGetValue(target, out var unitCamera))
        {
            if (_currentUnitCamera != null)
            {
                _currentUnitCamera.SetPriority(0);
            }
            unitCamera.SetPriority(10);
            _currentUnitCamera = unitCamera;
            worldCamera.SetPosition( _currentUnitCamera.GetUnit().transform.position);
        }
    }
    
    public UnitCamera GetCamera(Unit unit) => _unitCameras[unit];
    
    private void Start()
    {
        FieldSystem.onStageStart.AddListener(OnCombatStarted);
    }
    
    public void LookWorldCamera()
    {
        if (_currentUnitCamera != null)
        {
            worldCamera.SetPosition(_currentUnitCamera.GetUnit().transform.position);
            _currentUnitCamera.SetPriority(0);
        }
        _currentUnitCamera = null;
    }
    
    private void Init()
    {
        //set all camera priority to 0
        foreach (var unitCamera in _unitCameras.Values)
        {
            unitCamera.SetPriority(0);
        }
        worldCamera.SetPriority(5);
        _currentUnitCamera = null;

        if (GameManager.instance.CompareState(GameState.Combat))
        {
            SetCombatCamOption();
        }
        else
        {
            SetWorldCamOption();
        }
    }

    private void SetWorldCamOption()
    {
        //set orthographic camera
        if (Camera.main != null) Camera.main.orthographic = true;
        worldCamera.SetPosition(GetCamera(FieldSystem.unitSystem.GetPlayer()).transform.position);
    }

    private void SetCombatCamOption()
    {
        if (Camera.main != null) Camera.main.orthographic = false;
        worldCamera.SetPosition(GetCamera(FieldSystem.unitSystem.GetPlayer()).transform.position);
    }
    
    #region EVENTS
    private void OnCombatStarted()
    {
        Init();
        
        FieldSystem.turnSystem.onTurnChanged.AddListener(OnTurnStarted);
        FieldSystem.onCombatFinish.AddListener(OnCombatFinished);
        
        Unit player = FieldSystem.unitSystem.GetPlayer();
        player.onBusyChanged.AddListener(OnPlayerBusyChanged);
        
        LookAt(player);
    }

    private void OnTurnStarted()
    {
        if (FieldSystem.turnSystem.turnOwner is Player)
        {
            LookWorldCamera();
            return;
        }
        
        var turnOwner = FieldSystem.turnSystem.turnOwner;
        LookAt(turnOwner.isVisible ? turnOwner : FieldSystem.unitSystem.GetPlayer());
    }
    private void OnCombatFinished(bool win)
    {
        FieldSystem.turnSystem.onTurnChanged.RemoveListener(OnTurnStarted);
        FieldSystem.onCombatFinish.RemoveListener(OnCombatFinished);
    }
    
    private void OnPlayerBusyChanged()
    {
        var player = FieldSystem.unitSystem.GetPlayer();
        if (player.IsBusy())
        {
            LookAt(player);
        }
        else
        {
            LookWorldCamera();
        }
    }
    
    #endregion
}