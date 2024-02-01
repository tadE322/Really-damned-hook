using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    [SerializeField] private Collider2D _PlayerCollider;

    private Collider2D _PlatformCollider;
    private bool _playerOnPlatform;

    private void Start()
    {
        _PlatformCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            Physics2D.IgnoreCollision(_PlatformCollider, _PlayerCollider, true);
            StartCoroutine(EnabledCollider());
        }
    }

    private IEnumerator EnabledCollider()
    {
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreCollision(_PlatformCollider, _PlayerCollider, false);
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<Player>();
        if(player != null )
        {
            _playerOnPlatform = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, false);
    }
}
