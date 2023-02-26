using UnityEngine;

public class ComputerPointer : MonoBehaviour
{
    public void com1Clicked()
    {
        Vector3 temp = transform.position;
        temp.x = -1.3f;
        transform.position = temp;
    }

    public void com2Clicked()
    {
        Vector3 temp = transform.position;
        temp.x = 1.3f;
        transform.position = temp;
    }

    public void com3Clicked()
    {
        Vector3 temp = transform.position;
        temp.x = 3.8f;
        transform.position = temp;
    }
}
