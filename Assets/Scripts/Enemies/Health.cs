using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;

    private Animator _anim;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            _anim.SetTrigger("Hit");
            health -= damage;
        }
        else if(health <= 0)
        {
            _anim.SetTrigger("Dead");
        }
    }

    private void Kill()
    {
        gameObject.SetActive(false);
    }
}
