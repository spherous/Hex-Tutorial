using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class Hex : SerializedMonoBehaviour
{
    public bool selected {get; private set;} = false;
    private HexSpawner hexSpawner;
    [OdinSerialize, ReadOnly] private Index hexIndex;
    [SerializeField] private MeshRenderer meshRenderer;

    IEnumerable<(Hex neighbor, HexNeighborDirection direction)> NeighborsWithDirection()
    {
        foreach(HexNeighborDirection direction in EnumArray<HexNeighborDirection>.Values)
        {
            Hex neighbor = hexSpawner.GetNeighborAt(hexIndex, direction);
            yield return (neighbor, direction);
        }
    }

    private void Awake() => hexSpawner = GameObject.FindObjectOfType<HexSpawner>();

    public void AssignIndex(Index index) => hexIndex = index;

    public void ToggleSelect() => (selected ? (Action)Deselect : (Action)Select)();

    private void Select()
    {
        selected = true;

        // Update edges of this hex based on the direction to each neighbor
        foreach(var (neighbor, direction) in NeighborsWithDirection())
            if(neighbor == null || !neighbor.selected)
                UpdateEdge(direction);

        UpdateNeighbors();
    }


    private void Deselect()
    {
        selected = false;

        // Clear self
        for(int i = 0; i < 6; i++)
            meshRenderer.material.SetFloat($"_Edge{i}", 0f);

        UpdateNeighbors();
    }

    public void UpdateNeighbors()
    {
        foreach(var (neighbor, direction) in NeighborsWithDirection())
            if(neighbor != null && neighbor.selected)
                neighbor.UpdateEdge(direction.Opposite());
    }

    public void UpdateEdge(HexNeighborDirection direction) => 
        meshRenderer.material.SetFloat(
            name: $"_Edge{(int)direction}",
            value: Mathf.Abs(meshRenderer.material.GetFloat($"_Edge{(int)direction}") - 1).Floor()
        );
}

[System.Serializable]
public struct Index
{
    public int row;
    public int col;

    public Index(int _row, int _col)
    {
        row = _row;
        col = _col;
    }
}