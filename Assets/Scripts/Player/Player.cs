using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform _attackPos;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _enemy;
    [SerializeField] private int _damage;

    public PlayerController controller;
    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;


    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;   
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        
        if(horizontalMove != 0)
        {
            _anim.SetBool("isRunning", true);
        }
        else
        {
            _anim.SetBool("isRunning", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            _anim.SetBool("isJumping", true);
            jump = true;
        }

        if(Input.GetMouseButtonDown(0))
        {
            _anim.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        _anim.SetBool("isJumping", false);
        jump = false;
    }

    public void Attack() // вызывается в анимации
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_attackPos.position, _attackRange, _enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Health>().TakeDamage(_damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_attackPos.position, _attackRange);
    }
}
