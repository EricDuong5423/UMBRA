using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActivateTrap : MonoBehaviour
{
    [SerializeField] private GridLayout _gridLayout;
    [SerializeField] private TileBase _trapTile;
    [SerializeField] private TileBase _normalTile;
    [SerializeField] private Tilemap _tilemap;
    
    bool hasActiveTrap = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasActiveTrap) return;
        Vector3Int cellPos = _gridLayout.WorldToCell(transform.position);
        _tilemap.SetTile(cellPos, _trapTile);
        _tilemap.SetTileAnimationFlags(cellPos, TileAnimationFlags.LoopOnce);
        hasActiveTrap = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Vector3Int cellPos = _gridLayout.WorldToCell(transform.position);
        _tilemap.SetTile(cellPos, _normalTile);
    }
}
