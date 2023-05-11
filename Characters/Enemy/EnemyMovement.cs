using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private float speed;

    private EnemyCharacter enemy;

    private void Start()
    {
        enemy = GetComponent<EnemyCharacter>();
    }

    public void Move()
    {
        if (!enemy.Attacking && !enemy.Dead)
        {
            if ((enemy.GetDirection().x > 0 && transform.position.x < rightEdge.position.x) ||
                (enemy.GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                enemy.anim.SetFloat("speed", 1);
                transform.Translate(enemy.GetDirection() * (speed * Time.deltaTime));
            }
            else if (enemy.currentState is PatrolState)
            {
                enemy.ChangeDirection();
            }
        }
    }
}