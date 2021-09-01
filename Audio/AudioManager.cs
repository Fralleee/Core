using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fralle.Core.Audio
{
  public class AudioManager : MonoBehaviour
  {
    [SerializeField] AudioSource source = null;
    [SerializeField] AudioSource musicSource;
    AudioSourcePool audioSourcePool;

    public float Volume
    {
      get => source.volume;
      set => source.volume = value;
    }
    public float MusicVolume
    {
      get => musicSource.volume;
      set => musicSource.volume = value;
    }

    [FormerlySerializedAs("Playlist")] public MusicPlaylist playlist;
    float timePlaying = 0;
    int currentTrackIndex;
    bool musicIsPlaying;

    void Awake()
    {
      audioSourcePool = new AudioSourcePool(source);
    }

    void Start()
    {
      if (playlist != null && playlist.songs.Count > 0)
        musicSource.clip = playlist.songs[currentTrackIndex];
    }

    void Update()
    {
      if (playlist.songs.Count > 0 && musicIsPlaying && timePlaying > musicSource.clip.length)
      {
        Next();
      }

      if (musicSource.isPlaying)
      {
        timePlaying += Time.deltaTime;
      }
    }

    public void Play(AudioEvent audioEvent)
    {
      if (audioEvent.playCount > 1)
      {
        StartCoroutine(PlayIEnumerator(audioEvent));
      }
      else
      {
        audioEvent.Play(audioSourcePool.GetSource());
      }
    }

    IEnumerator PlayIEnumerator(AudioEvent audioEvent)
    {
      for (int i = 0; i < audioEvent.playCount; i++)
      {
        audioEvent.Play(audioSourcePool.GetSource());
        yield return new WaitForSeconds(audioEvent.playDelay);
      }
    }


    #region Music
    public string CurrentTrack => musicSource.clip.name;

    public void PlayMusic()
    {
      musicSource.Play();
      musicIsPlaying = true;
    }

    public void StopMusic(bool fade)
    {
      musicSource.Stop();
      musicIsPlaying = false;
    }

    public void PauseMusic()
    {
      musicSource.Pause();
      musicIsPlaying = false;
    }

    public void Next()
    {
      currentTrackIndex++;
      if (currentTrackIndex > playlist.songs.Count - 1)
      {
        currentTrackIndex = 0;
      }
      musicSource.clip = playlist.songs[currentTrackIndex];
      musicSource.Play();
      timePlaying = 0;
    }

    public void Prev()
    {
      currentTrackIndex--;
      if (currentTrackIndex <= 0)
      {
        currentTrackIndex = playlist.songs.Count - 1;
      }
      musicSource.clip = playlist.songs[currentTrackIndex];
      musicSource.Play();
      timePlaying = 0;
    }

    public void ChangePlaylist(MusicPlaylist list)
    {
      playlist = list;
    }
    #endregion

  }
}
