using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameLogic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CinemachineConfiner : MonoBehaviour
{
    [Header("References")] 
    [SerializeField]
    private CinemachineConfiner2D confiner;

    [Header("Settings")]
    [SerializeField]
    private float margin = 2f;

    private PolygonCollider2D _confiningBox;

    private void Start()
    {
        _confiningBox = GetComponent<PolygonCollider2D>();
        SetConfiningBox(GameManager.Instance.World.Size);
    }

    private void SetConfiningBox(Vector2Int mapSize)
    {
        Vector2[] newVertices = new Vector2[4];
        newVertices[0] = new Vector2(-margin, -margin);
        newVertices[1] = new Vector2(-margin, mapSize.y + margin);
        newVertices[2] = new Vector2(mapSize.x + margin, mapSize.y + margin);
        newVertices[3] = new Vector2(mapSize.x + margin, -margin);
        
        _confiningBox.points = newVertices;

        if (confiner != null)
        {
            confiner.m_BoundingShape2D = _confiningBox;
            confiner.InvalidateCache();
        }
        else
        {
            Debug.LogWarning("CinemachineConfiner component not set!");
        }
    }
}
