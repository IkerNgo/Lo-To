using UnityEngine;
using TMPro;

public class NumberCoin : MonoBehaviour
{
    private Rigidbody2D myBody;
    private GameObject canvas;
    [SerializeField] private TextMeshProUGUI onCoinText;
    private TextMeshProUGUI text;
    private GameManager gameManager;

    private float presentTime;
    public float timeMove = 2.0f;
    public float timeZoom = 4.0f;
    public float speedMove = 2.5f;
    public float scaleSpeed = 0.1f;
    private Vector3 scale = new Vector3(1, 1, 0);
    public bool isRunning;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        canvas = GameObject.Find("Canvas");
        creatNumberCoin();
        presentTime = Time.time;
        isRunning = true;
    }
    private void FixedUpdate()
    {
        if(Time.time <= (timeMove + presentTime))
        {
            myBody.MovePosition(myBody.position + Vector2.up * speedMove * Time.deltaTime);

            text.transform.position = myBody.position;
        }
        else if (Time.time <= (timeZoom + presentTime) && Time.time > (timeMove + presentTime))
        {
            transform.localScale += scale * Time.deltaTime * scaleSpeed;
            text.transform.localScale += scale * Time.deltaTime * scaleSpeed;
            isRunning = false;
        }
    }

    private void creatNumberCoin()
    {
        gameManager = FindObjectOfType<GameManager>();
        text = Instantiate(onCoinText, canvas.transform);
        text.text = gameManager.numberCalled.ToString();
    }
}
