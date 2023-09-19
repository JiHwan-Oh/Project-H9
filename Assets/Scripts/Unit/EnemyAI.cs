using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private HexTransform _hexTransform;
    private Enemy _enemy;

    [SerializeField]
    private Vector3Int _playerPosMemory;

    public IUnitAction resultAction;
    public Vector3Int resultPosition;
    
    
    [SerializeField] private BehaviorTreeRunner _tree;

    private void Awake()
    {
        _hexTransform = GetComponent<HexTransform>();
        _enemy = GetComponent<Enemy>();
        
        //Create AI
        _tree = new BehaviorTreeRunner(
            new SelectorNode(
                new List<INode>
                {
                    new SequenceNode(
                        new List<INode>
                        {
                            new ActionNode(IsOutOfAmmo),
                            new ActionNode(() => {
                                resultAction = _enemy.GetAction<ReloadAction>();
                                resultPosition = Hex.zero;
                                return INode.ENodeState.Success;
                            })
                        }),
                    new SequenceNode(
                        new List<INode>
                        {
                            new ActionNode(IsPlayerOutOfSight),
                            new ActionNode(() => 
                                _enemy.hasAttacked is false ? 
                                INode.ENodeState.Success :
                                INode.ENodeState.Failure),
                            new ActionNode(() => {
                                resultAction = _enemy.GetAction<MoveAction>();

                                if (TryMoveOnePos(_playerPosMemory))
                                {
                                    return INode.ENodeState.Success;
                                }
                                return INode.ENodeState.Failure;
                            })
                        }),
                    new SequenceNode(
                        new List<INode>
                        {
                            new ActionNode(IsOutOfRange),
                            new ActionNode(() => 
                                _enemy.hasAttacked is false ? 
                                    INode.ENodeState.Success :
                                    INode.ENodeState.Failure),
                            new ActionNode(() => {
                                resultAction = _enemy.GetAction<MoveAction>();

                                if (TryMoveOnePos(_playerPosMemory))
                                {
                                    return INode.ENodeState.Success;
                                }
                                return INode.ENodeState.Failure;
                            })
                        }),
                    new SequenceNode(new List<INode>
                        {
                            new ActionNode(() => 
                                _enemy.hasAttacked is false ? 
                                    INode.ENodeState.Success :
                                    INode.ENodeState.Failure),
                            new ActionNode(() =>
                            {
                                resultAction = _enemy.GetAction<AttackAction>();
                                resultPosition = _playerPosMemory;
                                return INode.ENodeState.Success;
                            })
                        }
                        ),
                    new ActionNode(() =>
                    {
                        resultAction = _enemy.GetAction<IdleAction>();
                        resultPosition = Hex.zero;
                        return INode.ENodeState.Success;
                    })
                }));
    }
    
    public void Start() 
    {
        _playerPosMemory = FieldSystem.unitSystem.GetPlayer().hexPosition;
    }
    
    public void SelectAction()
    {
        _tree.Operate();
    }

    private INode.ENodeState IsPlayerOutOfSight()
    {
        var curPlayerPos = FieldSystem.unitSystem.GetPlayer().hexPosition;
        if (FieldSystem.tileSystem.VisionCheck(_enemy.hexPosition, curPlayerPos))
        {
            _playerPosMemory = curPlayerPos;
            
            Debug.Log("AI Think : Player is in sight");
            
            return INode.ENodeState.Failure;
        }
        
        
        Debug.Log("AI Think : Player is out of sight");
        return INode.ENodeState.Success;
    }

    private INode.ENodeState IsOutOfAmmo()
    {
        Debug.Log("AI Think : Ammo is " + _enemy.weapon.currentAmmo);
        return _enemy.weapon.currentAmmo is 0 ? INode.ENodeState.Success : INode.ENodeState.Failure;
    }

    private INode.ENodeState IsOutOfRange()
    {
        if (_enemy.weapon.GetRange() < Hex.Distance(_playerPosMemory, _enemy.hexPosition))
        {
            Debug.Log("AI Think player is out of range");
            return INode.ENodeState.Success;
        }
        else
        {
            
            Debug.Log("AI Think player is in range");
            return INode.ENodeState.Failure;
        }
    }

    private bool TryMoveOnePos(Vector3Int target)
    {
        var route = FieldSystem.tileSystem.FindPath(_enemy.hexPosition, target);
        if (route is null)
        {
            resultPosition = _enemy.hexPosition;
            return false;
        }

        if (route.Count <= 1)
        {
            resultPosition = route[0].hexPosition;
            return false;
        }

        resultPosition = route[1].hexPosition;
        return true;
    }

    private float _waitingTime = 0f;
    public void AiWaitingCall(float time)
    {
        _waitingTime = time;
    }

    public bool IsWaiting() => _waitingTime > 0;

    private void Update()
    {
        if (_waitingTime <= 0) return;
        _waitingTime -= Time.deltaTime;
    }
}
