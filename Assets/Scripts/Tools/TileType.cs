using UnityEngine;

public enum TileType
{
    // No connections
    Island = 0,          // Single tile with no neighbors
    
    // One connection
    EndNorth = 1,        // Only connected to north
    EndEast = 2,         // Only connected to east
    EndSouth = 3,        // Only connected to south
    EndWest = 4,         // Only connected to west
    
    // Two connections
    StraightHorizontal = 5,  // East-West connection
    StraightVertical = 6,    // North-South connection
    CornerNE = 7,         // North-East corner
    CornerSE = 8,         // South-East corner
    CornerSW = 9,         // South-West corner
    CornerNW = 10,        // North-West corner
    
    // Three connections
    TSplitNorth = 11,     // Connected to North, East, West
    TSplitEast = 12,      // Connected to East, North, South
    TSplitSouth = 13,     // Connected to South, East, West
    TSplitWest = 14,      // Connected to West, North, South
    
    // Four connections
    Cross = 15,           // Connected to all four directions
    
    // Special cases
    DiagonalNE = 16,      // Connected diagonally to North-East
    DiagonalSE = 17,      // Connected diagonally to South-East
    DiagonalSW = 18,      // Connected diagonally to South-West
    DiagonalNW = 19,      // Connected diagonally to North-West
}
