using UnityEngine;

public interface ISoundManager
{
    void PlaySound(AudioClip clip, float volume = 1f);
    void PlayEnemyHitSound();
    void PlayEnemyDeathSound();
    void PlayEnemyChargeSound();
    void PlayWalkSound();
    void StopWalkSound();
    void PlayPlayerHitSound();
    void PlayPlayerDeathSound();
    void PlayHealthPickupSound();
    void PlaySwordWhooshSound();
    void PlayPortalOpenSound();
    void PlayLevelTeleportSound();
    void PlayGambleWinSound();
    void PlayGambleLoseSound();
    void PlayGambleDrumrollSound();
    void PlayUIClickSound();
    void PlayObjectBreakingSound();
}
