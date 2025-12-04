using UnityEngine;

public class UI_Win : MonoBehaviour
{

    private void Update()
    {
        if(this.enabled)
            Player.instance.input.Disable();
    }
    public void GoToCampButton()
    {
        GameManager.instance.ChangeScene("Level_0", RespawnType.NonSpecific);
    }
    public void GoToMainMenu()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);
    }
}
