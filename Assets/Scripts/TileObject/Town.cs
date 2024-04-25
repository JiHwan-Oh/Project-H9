using UnityEngine;
using System.Collections.Generic;

public class Town : TileObject
{
    public enum BuildingType 
    {
        NULL,
        Ammunition,
        Saloon,
        Sheriff
    }
    private int _townIndex;
    [SerializeField] private BuildingType buildingType;
    
    protected override void SetTile(Tile t)
    {
        Debug.Log("Se tTile call");
        base.SetTile(t);

        t.walkable = true;
        t.visible = true;
        t.rayThroughable = false;
    }

    public override void OnCollision(Unit other)
    {
        Debug.Log($"�÷��̾� ���� : {_townIndex}�� ������ {buildingType} �ǹ�");

        //other.GetSelectedAction().ForceFinish();

        Debug.Log("On Collision Calls");

    }


    public override string[] GetArgs()
    {
        return new[] { _townIndex.ToString() };
    }

    public override void SetArgs(string[] args)
    {
        if(args.Length != 1) throw new System.Exception();
        
        _townIndex = int.Parse(args[0]);
    }

    public TileEffectType GetTileEffectType() 
    {
        Dictionary<BuildingType, TileEffectType> effect = new Dictionary<BuildingType, TileEffectType>
        {
            {BuildingType.NULL,         TileEffectType.Normal },
            {BuildingType.Ammunition,   TileEffectType.Ammunition },
            {BuildingType.Saloon,        TileEffectType.Saloon },
            {BuildingType.Sheriff,      TileEffectType.Sheriff },
        };
        effect.TryGetValue(buildingType, out TileEffectType effType);
        return effType;
    }
}
