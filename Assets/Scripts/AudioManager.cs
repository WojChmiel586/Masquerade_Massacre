using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    //Doors
    public AudioClip doorOpen;
    public AudioClip doorClose;

    //Shoot
    public AudioClip shoot;

    //Music Tracks
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    //end states
    public AudioClip loseGame;
    public AudioClip winGame;

    //dossier
    public AudioClip dossierSound;

    //Audio Sources
    public AudioSource ShootSource;
    public AudioSource MusicSource;
    public AudioSource DoorSource;
    public AudioSource CrowdSource;
    public AudioSource EndGameSource;
    public AudioSource DossierSource;



    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void OpenDoorSFX()
    {
        DoorSource.PlayOneShot(doorOpen);
    }

    public void CloseDoorSFX() 
    {
        DoorSource.PlayOneShot(doorClose);
    }
    public void OpenDossierSFX() 
    {
        DossierSource.PlayOneShot(dossierSound);
    }
    public void FailSFX()
    {
        EndGameSource.PlayOneShot(loseGame);
    }
    public void WinSFX()
    {
        EndGameSource.PlayOneShot(winGame);
    }
    public void ShootSFX()
    {
        ShootSource.PlayOneShot(shoot);
    }
    public void PlayMenuMusic()
    {
        MusicSource.clip = menuMusic;
        MusicSource.Play();
    }
    public void PlayGameMusic()
    {
        MusicSource.clip = gameMusic;
        MusicSource.Play();
    }

}
