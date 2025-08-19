using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class comberScript : MonoBehaviour
{
    public Animator anim;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public GameObject[] AttackbleObjs;
    public float maxComboDelay = 1.2f;
    public bool isAttacking = false;
    public bool reseted = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay && !reseted)
        {
            reseted = true;
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack3", false);
            isAttacking = false; // Reset attacking state
            noOfClicks = 0;
            noOfClicks = 0;
            isAttacking = false; // Reset attacking state
            foreach (GameObject obj in AttackbleObjs)
            {
                if (obj.GetComponent<AttackbleScript>() != null)
                {
                    obj.GetComponent<AttackbleScript>().isAttacking = false;
                }
            }
        }

        if (InputSystem.actions.FindAction("Attack").WasPerformedThisFrame())
        {
            lastClickedTime = Time.time;
            noOfClicks++;
            if (noOfClicks == 1)
            {
                reseted = false;
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
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        }
    }

    public void return1()
    {
        if (noOfClicks >= 2)
        {
            reseted = false;
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
        else
        {
            reseted = false;
            anim.SetBool("Attack1", false);
            isAttacking = false; // Reset attacking state
            foreach (GameObject obj in AttackbleObjs)
            {
                if (obj.GetComponent<AttackbleScript>() != null)
                {
                    obj.GetComponent<AttackbleScript>().isAttacking = false;
                }
            }
            noOfClicks = 0;
        }
    }

    public void return2()
    {
        if (noOfClicks >= 3)
        {
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
        else
        {
            anim.SetBool("Attack2", false);
            isAttacking = false; // Reset attacking state
            foreach (GameObject obj in AttackbleObjs)
            {
                if (obj.GetComponent<AttackbleScript>() != null)
                {
                    obj.GetComponent<AttackbleScript>().isAttacking = false;
                }
            }
            noOfClicks = 0;
        }
    }

    public void return3()
    {
        reseted = false;
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        isAttacking = false; // Reset attacking state
        noOfClicks = 0;
    }
        

}
