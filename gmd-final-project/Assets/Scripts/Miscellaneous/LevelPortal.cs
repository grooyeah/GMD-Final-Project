using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ServiceLocator.Instance.GetService<ILevelManager>().LoadNextLevel();
            ServiceLocator.Instance.GetService<ISoundManager>().PlayLevelTeleportSound();
        }
    }
}
