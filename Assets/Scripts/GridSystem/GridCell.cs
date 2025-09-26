using UnityEngine;

[System.Serializable]
public class GridCell
{
    public int x, y;
    public Vector3 worldPosition;
    public bool isOccupied;
    
    public GridCell(int x, int y, Vector3 worldPosition)
    {
        this.x = x;
        this.y = y;
        this.worldPosition = worldPosition;
        this.isOccupied = false;
    }
}
