using System;
using Sirenix.OdinInspector;
using UnityEngine;

public struct HexGrid
{
    public int cols;
    [ShowInInspector, ReadOnly] public int maxCol => cols - 1;
    public int rows;
    [ShowInInspector, ReadOnly] public int maxRow => rows - 1;
    public int radius;
    public float height;
    [SerializeField, MinMaxSlider(-64, 64, true)] private Vector2 hexHeightVariance;
    [ShowInInspector, ReadOnly] public float minHeight => hexHeightVariance.x;
    [ShowInInspector, ReadOnly] public float maxHeight => hexHeightVariance.y;
    public float Apothem =>
        Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(radius * 0.5f, 2f));

    public bool IsInBounds(int row, int col) => 
        row * (row - maxRow) <= 0 && col * (col - maxCol) <= 0;
}