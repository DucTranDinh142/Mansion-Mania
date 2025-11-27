using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioDatabaseSO audioDB;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [Space]

    private AudioClip lastMusicPlayed;
    private string currentBGMGroupName;
    private Coroutine currentBGMCoroutine;
    [SerializeField] private bool bgmShouldPlay;
    private Transform player;
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
    private void Update()
    {
        if (bgmSource.isPlaying == false && bgmShouldPlay)
        {
            if (string.IsNullOrEmpty(currentBGMGroupName) == false)
                NextBGM(currentBGMGroupName);
        }
        if (bgmSource.isPlaying && bgmShouldPlay == false)
            StopBGM();
    }
    public void StartBGM(string musicGroup)
    {
        bgmShouldPlay = true;

        if (musicGroup == currentBGMGroupName)
            return;

        NextBGM(musicGroup);
    }
    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        currentBGMGroupName = musicGroup;

        if (currentBGMCoroutine != null)
            StopCoroutine(currentBGMCoroutine);

        currentBGMCoroutine = StartCoroutine(SwitchMusicCoroutine(musicGroup));
    }
    public void StopBGM()
    {
        bgmShouldPlay = false;

        StartCoroutine(FadeVolumeCoroutine(bgmSource, 0, 1));

        if (currentBGMCoroutine != null)
            StopCoroutine(currentBGMCoroutine);
    }
    private IEnumerator SwitchMusicCoroutine(string musicGroup)
    {
        AudioClipData data = audioDB.Get(musicGroup);
        AudioClip nextMusic = data.GetRandomClip();

        if (data == null || data.clips.Count == 0) yield break;

        if (data.clips.Count > 1)
        {
            while (nextMusic == lastMusicPlayed)
                nextMusic = data.GetRandomClip();
        }
        if (bgmSource.isPlaying)
            yield return FadeVolumeCoroutine(bgmSource, 0, 1f);

        lastMusicPlayed = nextMusic;
        bgmSource.clip = nextMusic;
        bgmSource.volume = 0;
        bgmSource.Play();
        StartCoroutine(FadeVolumeCoroutine(bgmSource, data.maxVolume, 1f));

    }
    private IEnumerator FadeVolumeCoroutine(AudioSource source, float targetVolume, float duration)
    {
        float time = 0;
        float startVolume = source.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistanceToHearSound = 5, bool isLooped = false)
    {
        if (player == null)
            player = Player.instance.transform;

        var data = audioDB.Get(soundName);
        if (data == null) return;

        var clip = data.GetRandomClip();
        if (clip == null) return;

        float maxVolume = data.maxVolume;
        float distance = Vector2.Distance(sfxSource.transform.position, player.position);
        float t = Mathf.Clamp01(1 - (distance / minDistanceToHearSound));

        sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(.95f, 1.2f);
        sfxSource.volume = Mathf.Lerp(0, maxVolume, t * t);
        if (isLooped == false)
            sfxSource.PlayOneShot(clip);
        else sfxSource.Play();
    }

    public void PlayGlobalSFX(string soundName, bool isLooped = false)
    {
        var data = audioDB.Get(soundName);
        if (data == null) return;

        var clip = data.GetRandomClip();
        if (clip == null) return;

        sfxSource.pitch = Random.Range(.95f, 1.2f);
        sfxSource.volume = data.maxVolume;
        if (isLooped == false)
            sfxSource.PlayOneShot(clip);
        else sfxSource.Play();
    }
}
