using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class comberScriptEnemy : MonoBehaviour
{
    public Animator anim;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public GameObject enemy;
    public GameObject[] AttackbleObjs;
    public float maxComboDelay = 1.2f;
    public bool isAttacking = false;
    public bool reseted = false;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Spell1()
    {
        anim.SetTrigger("Spell1");
    }
    public void Spell2()
    {
        anim.SetTrigger("Spell2");
    }
    public void Spell3()
    {
        anim.SetTrigger("Spell3");
    }
    public void Attack()
    {
            noOfClicks++;
        if (noOfClicks > 3)
        {
            noOfClicks = 1;
        }
        Setter();

    }
    public void TeleportEnemy()
    {
        enemy.transform.position += enemy.transform.forward * 8; // Teleport to a new location

    }
    public void ResetAttack1()
    {
            reseted = true;
            anim.SetBool("Attack1", false);
            isAttacking = false; // Reset attacking state
            foreach (GameObject obj in AttackbleObjs)
            {
                if (obj.GetComponent<AttackbleScript>() != null)
                {
                    obj.GetComponent<AttackbleScript>().isAttacking = false;
                }
            }

    }
    public void ResetAttack2()
    {
        reseted = true;
        anim.SetBool("Attack2", false);
        isAttacking = false; // Reset attacking state
        foreach (GameObject obj in AttackbleObjs)
        {
            if (obj.GetComponent<AttackbleScript>() != null)
            {
                obj.GetComponent<AttackbleScript>().isAttacking = false;
            }
        }

    }
    public void ResetAttack3()
    {
        reseted = true;
        anim.SetBool("Attack3", false);
        isAttacking = false; // Reset attacking state
        foreach (GameObject obj in AttackbleObjs)
        {
            if (obj.GetComponent<AttackbleScript>() != null)
            {
                obj.GetComponent<AttackbleScript>().isAttacking = false;
            }
        }

    }
    public void Setter()
    {

        if (noOfClicks == 1)
        {
            reseted = false;
            anim.SetBool("Attack3", false);
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack1", true);
            isAttacking = true; // Reset attacking state
            foreach (GameObject obj in AttackbleObjs)
            {
                if (obj.GetComponent<AttackbleScript>() != null)
                {
                    obj.GetComponent<AttackbleScript>().isAttacking = true;
                }
            }
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 1, 3);
        if (noOfClicks == 2)
        {
            reseted = false;
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack3", false);
            anim.SetBool("Attack2", true);
            isAttacking = true; // Reset attacking state
            foreach (GameObject obj in AttackbleObjs)
            {
                if (obj.GetComponent<AttackbleScript>() != null)
                {
                    obj.GetComponent<AttackbleScript>().isAttacking = true;
                }
            }
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 1, 3);
        if (noOfClicks == 3)
        {
            //useless reseted
            reseted = false;
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack3", true);
            isAttacking = true; // Reset attacking state
            foreach (GameObject obj in AttackbleObjs)
            {
                if (obj.GetComponent<AttackbleScript>() != null)
                {
                    obj.GetComponent<AttackbleScript>().isAttacking = true;
                }
            }
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 1, 3);

    }
    // Update is called once per frame
    void Update()
    {

    }


}
