using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ÿ�� ������ ���� �ϱ� ���� ������ ���� ���ó��. ������� �� ��
/// </summary>
public class Wall : TileObject
{
    [Header("Wall type")]
    public bool visible;
    public bool walkable;
    public bool rayThroughable;

    protected override void SetTile(Tile t)
    {
        Debug.Log("Se tTile call");
        base.SetTile(t);

        t.walkable = walkable;
        t.visible = visible;
        t.rayThroughable = rayThroughable;
    }

    public override string[] GetArgs()
    {
        throw new System.NotImplementedException();
    }

    public override void SetArgs(string[] args)
    {
        throw new System.NotImplementedException();
    }
}
