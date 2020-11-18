using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class HexSpawner : SerializedMonoBehaviour
{
    [SerializeField] private Hex hexPrefab;
    [SerializeField] private HexGrid hexGrid;
    [OdinSerialize] public List<List<Hex>> hexes = new List<List<Hex>>();

    [Button("Spawn Hexes")]
    private void SpawnHexes()
    {
        if(hexes.Count > 0)
            ClearHexes();
        
        for(int row = 0; row < hexGrid.rows; row++) 
        {
            hexes.Add(new List<Hex>());
            for(int col = 0; col < hexGrid.cols; col++)
            {
                Hex newHex = Instantiate(
                    original: hexPrefab,
                    position: new Vector3(
                        x: hexGrid.radius * 3 * col + Get_X_Offset(row),
                        y: UnityEngine.Random.Range(hexGrid.minHeight, hexGrid.maxHeight),
                        z: row * hexGrid.Apothem
                    ),
                    rotation: Quaternion.identity,
                    parent: transform
                );

                newHex.transform.localScale = new Vector3(
                    x: newHex.transform.localScale.x * hexGrid.radius,
                    y: newHex.transform.localScale.y * hexGrid.height,
                    z: newHex.transform.localScale.z * hexGrid.radius
                );

                newHex.AssignIndex(new Index(row, col));
                hexes[row].Add(newHex);

                RandomizeHexColor(newHex.GetComponent<MeshRenderer>());
            }
        }
    }

    private void RandomizeHexColor(MeshRenderer meshRenderer)
    {
        float randShade = UnityEngine.Random.Range(0, 1f);
        Material mat = new Material(meshRenderer.sharedMaterial);
        mat.SetColor("_BaseColor", new Color(randShade, randShade, randShade));
        meshRenderer.material = mat;
    }

    private float Get_X_Offset(int row) => row % 2 == 0 ? hexGrid.radius * 1.5f : 0f;

    [Button("Clear Hexes")]
    private void ClearHexes()
    {
        for(int row = 0; row < hexes.Count; row++)
        {
            for(int col = 0; col < hexes[row].Count; col++)
            {
#if UNITY_EDITOR
                DestroyImmediate(hexes[row][col].gameObject);
#elif !UNITY_EDITOR
                Destroy(hexes[row][col].gameObject);
#endif                
            }
        }
        hexes = new List<List<Hex>>();
    }

    public Hex GetNeighborAt(Index hexIndex, HexNeighborDirection direction)
    {
        (int row, int col) offsets = GetOffsetInDirection(hexIndex.row % 2 == 0, direction);
        return GetHexIfInBounds(hexIndex.row + offsets.row, hexIndex.col + offsets.col);
    }

    private Hex GetHexIfInBounds(int row, int col) => 
        hexGrid.IsInBounds(row, col) ? hexes[row][col] : null;

    private (int row, int col) GetOffsetInDirection(bool isEven, HexNeighborDirection direction)
    {
        switch(direction)
        {
            case HexNeighborDirection.Up:
                return (2, 0);
            case HexNeighborDirection.UpRight:
                return isEven ? (1, 1) : (1, 0);
            case HexNeighborDirection.DownRight:
                return isEven ? (-1, 1) : (-1, 0);
            case HexNeighborDirection.Down:
                return (-2, 0);
            case HexNeighborDirection.DownLeft:
                return isEven ? (-1, 0) : (-1, -1);
            case HexNeighborDirection.UpLeft:
                return isEven ? (1, 0) : (1, -1);
        }
        return (0, 0);
    }
}

public enum HexNeighborDirection{Up, UpRight, DownRight, Down, DownLeft, UpLeft}