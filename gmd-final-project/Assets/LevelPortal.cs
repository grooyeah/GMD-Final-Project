using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            LevelManager.Instance.LoadNextLevel();
            SoundManager.Instance.PlayLevelTeleportSound();
        }
    }
}
