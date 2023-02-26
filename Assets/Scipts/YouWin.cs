using UnityEngine;

public class YouWin : MonoBehaviour
{
    public float zoomSpeed;
    private float zoomInOutSpeed = 3.0f;
    private float presentTime;
    private Vector3 scale = new Vector3(1, 1, 0);

    private void Awake()
    {
        presentTime = Time.time;
    }

    private void FixedUpdate()
    {
        blink();
    }
    private void blink()
    {
        if(Time.time < presentTime + 2)
        {
            transform.localScale += zoomSpeed * Time.deltaTime * scale;
        }
        if(Time.time>=presentTime+2)
        {
            if(Time.time%2==0)
            {
                transform.localScale -= zoomInOutSpeed * Time.deltaTime * scale;
            }
            else if(Time.time%2==1)
            {
                transform.localScale += zoomInOutSpeed * Time.deltaTime * scale;
            }
        }
    }
}
