using UnityEngine;

public class UI_Death : MonoBehaviour
{
    public void GoToCampButton()
    {
        GameManager.instance.ChangeScene("Level_0", RespawnType.NonSpecific);
    }

    public void GoToCheckPoint()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenu()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);
    }
}
