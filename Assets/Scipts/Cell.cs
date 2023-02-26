using UnityEngine;

//T?o ki?u c?u trúc cho t?ng ô
public struct Cell
{
    public enum Type
    {
        Blank, Number
    }

    public Type type;
    public Vector3Int position;
    public int number; //S? g?n li?n v?i ô
    public bool had; //?ã có s? này hay ch?a
}
