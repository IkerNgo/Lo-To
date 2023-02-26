using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour
{
    public int numberComputer;
    public int color;

    public void setNumberComputer1()
    {
        numberComputer = 1;
    }
    public void setNumberComputer2()
    {
        numberComputer = 2;
    }

    public void setNumberComputer3()
    {
        numberComputer = 3;
    }

    public void setBlue()
    {
        color = 1;
    }

    public void setGreen()
    {
        color = 2;
    }

    public void setRed()
    {
        color = 3;
    }

    public void setYellow()
    {
        color = 4;
    }

    public void play()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(1);
    }

    public GameObject getRootOfDontDestroyOnLoad()
    {
        return this.gameObject.scene.GetRootGameObjects()[0];
    }

    public void doExitGame()
    {
        Application.Quit();
    }
}
