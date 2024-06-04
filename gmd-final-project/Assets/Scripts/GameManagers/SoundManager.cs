using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundManager
{
    public static ISoundManager Instance { get; private set; }

    [Header("Sound Effects")]
    [Header("Enemy")]
    [SerializeField] private AudioClip _enemyHitClip;
    [SerializeField] private AudioClip _enemyDeathClip;
    [SerializeField] private AudioClip _enemyChargeClip;

    [Header("Player")]
    [SerializeField] private AudioClip _walkClip;
    [SerializeField] private AudioClip _playerHitClip;
    [SerializeField] private AudioClip _playerDeathClip;

    [Header("Items")]
    [SerializeField] private AudioClip _healthPickupClip;

    [Header("Weapon")]
    [SerializeField] private AudioClip _swordWhooshClip;

    [Header("Portal")]
    [SerializeField] private AudioClip _portalOpenClip;
    [SerializeField] private AudioClip _levelTeleportClip;

    [Header("User Interface")]
    [SerializeField] private AudioClip _gambleDrumrollClip;
    [SerializeField] private AudioClip _gambleWinClip;
    [SerializeField] private AudioClip _gambleLoseClip;
    [SerializeField] private AudioClip _uiClickClip;

    [Header("Objects")]
    public AudioClip objectBreakingClip;

    [Header("Sound Effect Volumes")]
    [Range(0f, 1f)] [SerializeField] private float _enemyHitVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _enemyDeathVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _enemyChargeVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _walkVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _playerHitVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _playerDeathVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _healthPickupVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _swordWhooshVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _portalOpenVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _levelTeleportVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _gambleDrumrollVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _gambleWinVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _gambleLoseVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _uiClickVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float _objectBreakingVolume = 1f;

    [Header("Background Music")]
    [SerializeField] private AudioClip _backgroundMusicClip;
    [Range(0f, 1f)] [SerializeField] private float _musicVolume = 0.5f;

    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    private AudioSource _walkSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _sfxSource = gameObject.AddComponent<AudioSource>();
            _musicSource = gameObject.AddComponent<AudioSource>();
            _walkSource = gameObject.AddComponent<AudioSource>();

            _musicSource.loop = true;
            _musicSource.clip = _backgroundMusicClip;
            _musicSource.volume = _musicVolume;
            _musicSource.Play();

            _walkSource.loop = true;
            _walkSource.volume = _walkVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        _sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayEnemyHitSound() => PlaySound(_enemyHitClip, _enemyHitVolume);
    public void PlayEnemyDeathSound() => PlaySound(_enemyDeathClip, _enemyDeathVolume);
    public void PlayEnemyChargeSound() => PlaySound(_enemyChargeClip, _enemyChargeVolume);
    public void PlayWalkSound() => _walkSource.Play();
    public void StopWalkSound() => _walkSource.Stop();
    public void PlayPlayerHitSound() => PlaySound(_playerHitClip, _playerHitVolume);
    public void PlayPlayerDeathSound() => PlaySound(_playerDeathClip, _playerDeathVolume);
    public void PlayHealthPickupSound() => PlaySound(_healthPickupClip, _healthPickupVolume);
    public void PlaySwordWhooshSound() => PlaySound(_swordWhooshClip, _swordWhooshVolume);
    public void PlayPortalOpenSound() => PlaySound(_portalOpenClip, _portalOpenVolume);
    public void PlayLevelTeleportSound() => PlaySound(_levelTeleportClip, _levelTeleportVolume);
    public void PlayGambleWinSound() => PlaySound(_gambleWinClip, _gambleWinVolume);
    public void PlayGambleLoseSound() => PlaySound(_gambleLoseClip, _gambleLoseVolume);
    public void PlayGambleDrumrollSound() => PlaySound(_gambleDrumrollClip, _gambleDrumrollVolume);
    public void PlayUIClickSound() => PlaySound(_uiClickClip, _uiClickVolume);
    public void PlayObjectBreakingSound() => PlaySound(objectBreakingClip, _objectBreakingVolume);
}
