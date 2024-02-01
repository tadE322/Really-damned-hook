using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] private GameObject blood;

    private Animator _anim;
    public bool CanGetHit { get; private set; }

    private void Start()
    {
        health = maxHealth;
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        CanGetHit = true;
        health -= damage;
        SpawnBlood();
        _anim.SetTrigger("Hit");
        CanGetHit = false;

        if (health <= 0)
        {
            Kill();
        }
    }

    public float MaxHealthValue()
    {
        return maxHealth;
    }
    public float HealthValue()
    {
        return health;
    }

    private void SpawnBlood()
    {
        if (!gameObject.CompareTag("Chest") && !gameObject.CompareTag("Box"))
        {
            Instantiate(blood, gameObject.transform.position, Quaternion.identity);
        }
    }

    private void Kill() // вызывается в анимации
    {
        TreasuresDrop td = gameObject.GetComponent<TreasuresDrop>();
        SpawnBlood();
        if (gameObject.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        var tags = gameObject.tag;
        switch (tags)
        {
            case "Enemy":
                td.DropTreasures(1, transform.position, 0, -2);
                td.DropTreasures(2, transform.position, 1, -3);
                td.DropTreasures(3, transform.position, 1, -4);
                td.DropTreasures(4, transform.position, 1, -4);
                td.DropTreasures(5, transform.position, 1, -4);
                break;
            case "Box":
                td.DropTreasures(1, transform.position, 1, -2);
                td.DropTreasures(2, transform.position, 0, -4);
                break;
            case "Chest":
                td.DropTreasures(2, transform.position, 1, -2);
                td.DropTreasures(3, transform.position, 1, -3);
                td.DropTreasures(4, transform.position, 1, -4);
                td.DropTreasures(5, transform.position, 1, -4);
                td.DropTreasures(6, transform.position, 1, -4);
                break;
        }

        Destroy(gameObject);
    }
}
