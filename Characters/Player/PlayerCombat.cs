using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    private PlayerCharacter PC;

    private void Start()
    {
        anim = GetComponent<Animator>();
        PC = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if (!PC.TakingDamage || PC.Dead)
        {
            if (Input.GetKeyDown(KeyCode.X)) anim.SetTrigger("attack");
            if (Input.GetKeyDown(KeyCode.V)) anim.SetTrigger("shoot");
        }
    }
}