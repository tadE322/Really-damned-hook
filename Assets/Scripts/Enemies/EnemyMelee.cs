using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int attackCooldown;
    [SerializeField] private new CapsuleCollider2D collider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private float closeDistance = 1.65f;
    [SerializeField] private EnemyPatrol ep;


    private float closeCollDistance = 0.28f;
    private float speed;
    private float cooldownTimer = Mathf.Infinity;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerIsClose())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack");
            }
        }
    }

    private bool PlayerIsClose()
    {
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center + transform.right * closeDistance * transform.localScale.x * closeCollDistance, new Vector3(collider.bounds.size.x * range, collider.bounds.size.y, collider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void EnemyAttack() // вызывается в анимации
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerLayer);
        for (int i = 0; i < player.Length; i++)
        {
            player[i].GetComponent<Health>().TakeDamage(damage);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(collider.bounds.center + transform.right * closeDistance * transform.localScale.x * closeCollDistance, new Vector3(collider.bounds.size.x * closeDistance, collider.bounds.size.y, collider.bounds.size.z));
        Gizmos.DrawRay(collider.bounds.center + transform.right * closeDistance * transform.localScale.x * closeCollDistance, new Vector3(collider.bounds.size.x * closeDistance, collider.bounds.size.y, collider.bounds.size.z));
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
