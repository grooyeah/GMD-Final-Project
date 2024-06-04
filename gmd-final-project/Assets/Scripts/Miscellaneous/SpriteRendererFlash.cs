using System.Collections;
using UnityEngine;

public class SpriteRendererFlash : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Coroutine _flashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartFlash()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }
        _flashCoroutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        Color originalColor = _spriteRenderer.color;

        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        _spriteRenderer.color = originalColor;
    }
}
