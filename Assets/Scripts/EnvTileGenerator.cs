using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvTileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _envParent;
    [SerializeField] private GameObject _tileParent;

    public GameObject envTile;

    [ContextMenu("Generate Env")]
    private void Create()
    {
        Remove();
        
        var tiles = _tileParent.GetComponentsInChildren<Tile>();

        foreach (var tile in tiles)
        {
            HexTransform objTsf = Instantiate(envTile, _envParent.transform).GetComponent<HexTransform>();

            var worldPos = objTsf.transform.position;
            worldPos.y = -0.7f;
            objTsf.transform.position = worldPos;
            
            objTsf.position = tile.hexPosition;
        }   
    }

    [ContextMenu(("Remove Env"))]
    private void Remove()
    {
        var envs = _envParent.GetComponentsInChildren<HexTransform>();
        foreach(var env in envs)
        {
            DestroyImmediate(env.gameObject);
        }
    }
}
