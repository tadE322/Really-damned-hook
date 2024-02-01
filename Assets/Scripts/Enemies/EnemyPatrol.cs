using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform firstPoint;
    [SerializeField] private Transform secondPoint;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform player;
    [SerializeField] private float normalSpeed;

    private Health health;
    public float speed { get; private set; }
    private Animator anim;
    private Vector3 initScale;
    private bool movingLeft;

    private void Awake()
    {
        health = GetComponent<Health>();
        initScale = enemy.transform.localScale;
        anim = GetComponent<Animator>();
        speed = normalSpeed;
    }

    private void Update()
    {
        if(health.CanGetHit)
        {
            Stop();
        }
        else if(!health.CanGetHit)
        {
            Go();
        }



        if (movingLeft)
        {
            if (enemy.position.x >= firstPoint.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= secondPoint.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        movingLeft = !movingLeft;
    }


    public void MoveInDirection(int _direction)
    {
        anim.SetBool("isRunning", true);
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }

    public void Stop() // вызываются в анимации
    {
        speed = 0;
    }
    public void Go() // вызываются в анимации
    {
        speed = normalSpeed;
    }

    private void ChasingPlayer()
    {
        enemy.position = Vector2.MoveTowards(new Vector2 (transform.position.x, 0f), new Vector2 (player.transform.position.x, 0f), speed * Time.deltaTime);
    }
}
