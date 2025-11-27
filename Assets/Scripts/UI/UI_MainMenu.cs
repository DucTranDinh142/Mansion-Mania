using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_Options>(true).LoadUpVolume();
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();
        AudioManager.instance.StartBGM("Playlist_mainMenu");
    }

    public void PlayButton()
    {
        GameManager.instance.ContinuePlay();
        AudioManager.instance.PlayGlobalSFX("UI_buttonClick");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
