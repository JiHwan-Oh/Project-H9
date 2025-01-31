﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WorldMapEditor : MonoBehaviour
{
    [SerializeField]private WorldData worldData;
    [SerializeField]private Transform tileObjectsTransform;
    [SerializeField]private GameObject linkPrefab;
    
    [HideInInspector] public UnityEvent onLinkChanged;
    
    private void Awake()
    {
        //Find TileObjects;
        var tileObjects = GameObject.Find("TileObjects");
        if (tileObjects == null)
        {
            Debug.LogError("TileObjects not found");
            return;
        }
        tileObjectsTransform = tileObjects.transform;
        
        onLinkChanged = new UnityEvent();
        
        UIManager.instance.gameObject.SetActive(false);
        GameManager.instance.SetEditor();
        TileEffectManager.instance.gameObject.SetActive(false);
    }

    [ContextMenu("Load Link")]
    private void LoadLink()
    {
        //Clear link objects
        var links = FindObjectsOfType<Link>();
        foreach (var link in links)
        {
            DestroyImmediate(link.gameObject);
        }
        
        //Instantiate link object by link data
        foreach (var linkData in worldData.links)
        {
            //instantiate
            var link = Instantiate(linkPrefab, Hex.Hex2World(linkData.pos), Quaternion.identity);
            link.transform.SetParent(tileObjectsTransform);
            link.transform.rotation = Quaternion.Euler(0, linkData.rotation, 0);
            
            var linkComponent = link.GetComponent<Link>();
            linkComponent.linkIndex = linkData.linkIndex;
            linkComponent.combatMapIndex = linkData.combatMapIndex;
            linkComponent.isRepeatable = linkData.isRepeatable;

            if (link.TryGetComponent(out linkComponent.hexTransform))
            {
                linkComponent.hexPosition = linkData.pos;
            }
            
            //remove "(Clone)" from name
            // link.name = linkData.model.name.Replace("(Clone)", "");
        }
    }
    
    [ContextMenu("Save Link")]
    public void SaveLink()
    {
        //Get link data by link object
        worldData.links.Clear();
        foreach (Transform child in tileObjectsTransform)
        {
            var link = child.GetComponent<Link>();
            if (link == null) continue;
            
            var linkData = new LinkObjectData
            {
                pos = link.hexPosition,
                rotation = child.transform.rotation.eulerAngles.y,
                linkIndex = link.linkIndex,
                combatMapIndex = link.combatMapIndex,
                isRepeatable = link.isRepeatable,
                model = null
            };
            worldData.links.Add(linkData);
        }
        
        worldData.SaveChangesToScriptableObject(worldData);
        
        onLinkChanged.Invoke();
    }
}