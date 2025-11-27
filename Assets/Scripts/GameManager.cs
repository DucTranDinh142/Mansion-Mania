using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;
    private Vector3 lastPlayerPosition;

    public string lastScenePlayed ="Level_0";
    private bool dataLoaded;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //public void SetLastPlayerPosition(Vector3 position) => lastPlayerPosition = position;

    public void ContinuePlay()     
    {
        ChangeScene(lastScenePlayed, RespawnType.NonSpecific);
    }

    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.NonSpecific);
    }
    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCoroutine(sceneName, respawnType));
    }
    private IEnumerator ChangeSceneCoroutine(string sceneName, RespawnType respawnType)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();

        fadeScreen.DoFadeOut();
        yield return fadeScreen.fadeEffectCoroutine;

        if(sceneName == null)
            yield break;
        SceneManager.LoadScene(sceneName);

        dataLoaded = false;
        yield return null;

        while(dataLoaded == false)
        {
            yield return null;
        }

        fadeScreen = FindFadeScreenUI();
        fadeScreen.DoFadeIn();

        Player player = Player.instance;

        if (player == null) yield break;

        Vector3 position = GetNewPlayerPosition(respawnType);

        if(position !=  Vector3.zero)
            player.TeleportPlayer(position);
    }
    private UI_FadeScreen FindFadeScreenUI()
    {
        if (UI.instance !=null)
            return UI.instance.fadeScreenUI;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }
    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if(type == RespawnType.NonSpecific)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_SavePoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints
                .Where(checkpoint => data.unlockedCheckpoints.TryGetValue(checkpoint.GetCheckpointID(), out bool unlocked) && unlocked)
                .Select(checkpoint => checkpoint.GetPosition()).ToList();

            var enterWaypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                .Where(waypoint => waypoint.GetWaypointType() == RespawnType.Enter)
                .Select(waypoint => waypoint.GetPositionAndSetTriggerFalse()).ToList();

            var selectedPositions = unlockedCheckpoints.Concat(enterWaypoints).ToList();

            if(selectedPositions.Count == 0)
                return Vector3.zero;

            return selectedPositions.OrderBy(position => Vector3.Distance(position, lastPlayerPosition)).First();
        }
        return GetWaypointPosition(type);
    }
    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach(var point in waypoints)
        {
            if(point.GetWaypointType() == type)
                return point.GetPositionAndSetTriggerFalse();
        }
        return Vector3.zero;
    }

    public void LoadData(GameData data)
    {
        lastScenePlayed = data.lastScenePlayed;
        lastPlayerPosition = data.lastPlayerPosition;

        if (string.IsNullOrEmpty(lastScenePlayed))
            lastScenePlayed = "Level_0";
        dataLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
            return;

        data.lastPlayerPosition = Player.instance.transform.position;
        data.lastScenePlayed = currentScene;
        dataLoaded = false;
    }
}
