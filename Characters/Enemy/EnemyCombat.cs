using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private float meleeRange;
    [SerializeField] private float visibleRange;

    private EnemyCharacter enemy;

    private void Start()
    {
        enemy = GetComponent<EnemyCharacter>();
    }

    public void Attack()
    {
        // Add attack logic here
    }

    public bool InMeleeRange()
    {
        if (enemy.Target != null)
        {
            return Vector2.Distance(transform.position, enemy.Target.transform.position) <= meleeRange;
        }
        return false;
    }

    public bool VisibleRange()
    {
        if (enemy.Target != null)
        {
            return Vector2.Distance(transform.position, enemy.Target.transform.position) <= visibleRange;
        }
        return false;
    }
}