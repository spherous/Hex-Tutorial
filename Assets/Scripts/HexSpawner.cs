using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class HexSpawner : SerializedMonoBehaviour
{
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private HexGrid hexGrid;
    [OdinSerialize, ReadOnly] public List<List<GameObject>> hexes = new List<List<GameObject>>();

    [Button("Spawn Hexes")]
    private void SpawnHexes()
    {
        if(hexes.Count > 0)
            ClearHexes();
        
        for(int row = 0; row < hexGrid.rows; row++) 
        {
            hexes.Add(new List<GameObject>());
            for(int col = 0; col < hexGrid.cols; col++)
            {
                GameObject newHex = Instantiate(
                    original: hexPrefab,
                    position: new Vector3(
                        x: hexGrid.radius * 3 * col + Get_X_Offset(row),
                        y: Random.Range(hexGrid.minHeight, hexGrid.maxHeight),
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

                hexes[row].Add(newHex);
            }
        }
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
                DestroyImmediate(hexes[row][col]);
#elif !UNITY_EDITOR
                Destroy(hexes[row][col]);
#endif                
            }
        }
        hexes = new List<List<GameObject>>();
    }
}