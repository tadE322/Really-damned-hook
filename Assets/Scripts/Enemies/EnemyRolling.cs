using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRolling : MonoBehaviour
{
    [SerializeField] private Collider2D defaultCollider;
    [SerializeField] private Collider2D circleCollider;

    [SerializeField] private float speedInAttack;
    [SerializeField] private int damage;
}
