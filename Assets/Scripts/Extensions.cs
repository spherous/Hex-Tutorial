using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class Extensions 
    {
        public static int Floor(this float val) => Mathf.FloorToInt(val);
        public static HexNeighborDirection Opposite(this HexNeighborDirection direction) =>
            (HexNeighborDirection)(((int)direction + 3) % 6);
    }
}