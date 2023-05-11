using System.Collections;
using UnityEngine;

public class PlayerCharacter : Character
{
    /* components and instances */
    [field: SerializeField] public bool Grounded { get; set; }
    private static PlayerCharacter instance;
    public static PlayerCharacter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerCharacter>();
            }
            return instance;
        }
    }

    public override void Start()
    {
        // Additional initialization specific to PlayerCharacter
        base.Start();
    }

    public override IEnumerator TakeDamage()
    {
        healthStat.CurrentVal--;

        if (!Dead)
        {
            Debug.Log("Player health at " + healthStat.CurrentVal);
            //add camera shake and knockback fx
            anim.SetTrigger("damage");
        }
        else
        {
            anim.SetLayerWeight(0, 0);
            anim.SetTrigger("die");
        }

        yield return null;
    }

    public override void Death()
    {
        Debug.Log("Player Died");
    }
}