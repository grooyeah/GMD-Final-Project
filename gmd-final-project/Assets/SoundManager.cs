using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Sound Effects")]
    [Header("Enemy")]
    public AudioClip enemyHitClip;
    public AudioClip enemyDeathClip;
    public AudioClip enemyChargeClip;

    [Header("Player")]
    public AudioClip walkClip;
    public AudioClip playerHitClip;
    public AudioClip playerDeathClip;

    [Header("Items")]
    public AudioClip healthPickupClip;

    [Header("Weapon")]
    public AudioClip swordWhooshClip;

    [Header("Portal")]
    public AudioClip portalOpenClip;
    public AudioClip levelTeleportClip;

    [Header("User Interface")]
    public AudioClip gambleDrumrollClip;
    public AudioClip gambleWinClip;
    public AudioClip gambleLoseClip;
    public AudioClip uiClickClip;

    [Header("Sound Effect Volumes")]
    [Header("Enemy")]
    [Range(0f, 1f)] public float enemyHitVolume = 1f;
    [Range(0f, 1f)] public float enemyDeathVolume = 1f;
    [Range(0f, 1f)] public float enemyChargeVolume = 1f;

    [Header("Player")]
    [Range(0f, 1f)] public float walkVolume = 1f;
    [Range(0f, 1f)] public float playerHitVolume = 1f;
    [Range(0f, 1f)] public float playerDeathVolume = 1f;

    [Header("Items")]
    [Range(0f, 1f)] public float healthPickupVolume = 1f;

    [Header("Weapon")]
    [Range(0f, 1f)] public float swordWhooshVolume = 1f;

    [Header("Portal")]
    [Range(0f, 1f)] public float portalOpenVolume = 1f;
    [Range(0f, 1f)] public float levelTeleportVolume = 1f;

    [Header("User Interface")]

    [Range(0f, 1f)] public float gambleDrumrollVolume = 1f;
    [Range(0f, 1f)] public float gambleWinVolume = 1f;
    [Range(0f, 1f)] public float gambleLoseVolume = 1f;
    [Range(0f, 1f)] public float uiClickVolume = 1f;

    [Header("Background Music")]
    public AudioClip backgroundMusicClip;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    private AudioSource sfxSource;
    private AudioSource musicSource;
    private AudioSource walkSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Add AudioSource components
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
            walkSource = gameObject.AddComponent<AudioSource>();

            // Configure music source
            musicSource.loop = true;
            musicSource.clip = backgroundMusicClip;
            musicSource.volume = musicVolume;
            musicSource.Play();

            walkSource.loop = true;
            walkSource.volume = walkVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    // Enemy

    public void PlayEnemyHitSound()
    {
        PlaySound(enemyHitClip, enemyHitVolume);
    }
    public void PlayEnemyDeathSound()
    {
        PlaySound(enemyDeathClip, enemyDeathVolume);
    }

    public void PlayEnemyChargeSound()
    {
        PlaySound(enemyChargeClip, enemyChargeVolume);
    }

    // Player

    public void PlayWalkSound()
    {
        if (!walkSource.isPlaying)
        {
            walkSource.clip = walkClip;
            walkSource.Play();
        }
    }

    public void StopWalkSound()
    {
        if (walkSource.isPlaying)
        {
            walkSource.Stop();
        }
    }

    public void PlayPlayerHitSound()
    {
        PlaySound(playerHitClip, playerHitVolume);
    }

    public void PlayPlayerDeathSound()
    {
        PlaySound(playerDeathClip, playerDeathVolume);
    }

    // Items

    public void PlayHealthPickupSound()
    {
        PlaySound(healthPickupClip, healthPickupVolume);
    }


    // Weapon

    public void PlaySwordWhooshSound()
    {
        PlaySound(swordWhooshClip, swordWhooshVolume);
    }

    // Portal

    public void PlayPortalOpenSound()
    {
        PlaySound(portalOpenClip, portalOpenVolume);
    }

    public void PlayLevelTeleportSound()
    {
        PlaySound(levelTeleportClip, levelTeleportVolume);
    }

    // User Interface

    public void PlayGambleWinSound()
    {
        PlaySound(gambleWinClip, gambleWinVolume);
    }

    public void PlayGambleLoseSound()
    {
        PlaySound(gambleLoseClip, gambleLoseVolume);
    }

    public void PlayGambleDrumrollSound()
    {
        PlaySound(gambleDrumrollClip, gambleDrumrollVolume);
    }

    public void PlayUIClickSound()
    {
        PlaySound(uiClickClip, uiClickVolume);
    }


}
