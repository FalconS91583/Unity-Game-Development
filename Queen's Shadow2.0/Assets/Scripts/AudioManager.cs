using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioDatabaseSO audioDatabase;
    [SerializeField] private AudioSource btmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private bool bgmShokdPlay;

    private AudioClip lastMusicPlayed;
    private string currentBGMGroupName;
    private Coroutine currentBgmCo;


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
        if(btmSource.isPlaying == false && bgmShokdPlay)
        {
            if(string.IsNullOrEmpty(currentBGMGroupName) == false) 
                NextBGM(currentBGMGroupName);
        }

        if (btmSource.isPlaying && bgmShokdPlay == false)
            StopBGM();
    }

    public void StartBGM(string musicGroup)
    {
        bgmShokdPlay = true;

        if (musicGroup == currentBGMGroupName)
            return;

        NextBGM(musicGroup);
    }

    public void NextBGM(string musicGroup)
    {
        bgmShokdPlay = true;
        currentBGMGroupName = musicGroup;

        if(currentBgmCo != null) 
            StopCoroutine(currentBgmCo);

        currentBgmCo =  StartCoroutine(SwitchMusicCo(musicGroup));
    }

    public void StopBGM()
    {
        bgmShokdPlay = false;

        StartCoroutine(FadeVolumeCo(btmSource, 0, 1));

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);
    }


    private IEnumerator SwitchMusicCo(string musicGroup)
    {
        AudioClipData data = audioDatabase.Get(musicGroup);
        AudioClip nextMusic = data.GetRandomClip();

        if (data == null || data.clips.Count == 0)
            yield break;

        if (data.clips.Count > 1)
        {
            while (nextMusic == lastMusicPlayed)
                nextMusic = data.GetRandomClip();
        }

        if (btmSource.isPlaying)
            yield return FadeVolumeCo(btmSource, 0, 1f);

        lastMusicPlayed = nextMusic;
        btmSource.clip = nextMusic;
        btmSource.volume = 0;
        btmSource.Play();

        StartCoroutine(FadeVolumeCo(btmSource, data.volume, 1f));
    }

    private IEnumerator FadeVolumeCo(AudioSource source, float targetVolume, float duration)
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

    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistanceToHearSound = 5)
    {
        if (player == null)
            player = Player.instance.transform;

        var data = audioDatabase.Get(soundName);
        if (data == null)
            return;

        var clip = data.GetRandomClip();
        if (clip == null)
            return;

        float maxVolume = data.volume;
        float distance = Vector2.Distance(sfxSource.transform.position, player.position);
        float t = Mathf.Clamp01(1 - (distance / minDistanceToHearSound));

        sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(.95f, 1.1f);

        sfxSource.volume = Mathf.Lerp(0, maxVolume, t * t);
        sfxSource.PlayOneShot(clip);
    }

    public void PlayGlobalSFX(string soundName)
    {
        var data = audioDatabase.Get(soundName);
        if (data == null) return;

        var clip = data.GetRandomClip();
        if (clip == null) return;

        sfxSource.pitch = Random.Range(0.95f, 1.1f);
        sfxSource.clip = clip;
        sfxSource.PlayOneShot(clip, data.volume);
    }
}
