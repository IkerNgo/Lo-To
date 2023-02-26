using UnityEngine;

public class ColorPointer : MonoBehaviour
{
    public void blueClicked()
    {
        Vector3 temp = transform.position;
        temp.x = -2.55f;
        transform.position = temp;
    }

    public void greenClicked()
    {
        Vector3 temp = transform.position;
        temp.x = 0f;
        transform.position = temp;
    }

    public void redClicked()
    {
        Vector3 temp = transform.position;
        temp.x = 2.55f;
        transform.position = temp;
    }

    public void yellowClicked()
    {
        Vector3 temp = transform.position;
        temp.x = 5.1f;
        transform.position = temp;
    }
}
