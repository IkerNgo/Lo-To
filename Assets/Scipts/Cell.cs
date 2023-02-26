using UnityEngine;

//T?o ki?u c?u tr�c cho t?ng �
public struct Cell
{
    public enum Type
    {
        Blank, Number
    }

    public Type type;
    public Vector3Int position;
    public int number; //S? g?n li?n v?i �
    public bool had; //?� c� s? n�y hay ch?a
}
