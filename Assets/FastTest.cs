using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTest : MonoBehaviour
{
    [SerializeField]
    private Transform spellPoint;
    [SerializeField]
    private GameObject bulletPrefab;

    private FastBullet bullet;

    [SerializeField]
    private int health = 2;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetInteger("SpellForm", 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetInteger("SpellForm", 2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetInteger("SpellForm", 3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            anim.SetInteger("SpellForm", 4);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Attack");
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Heal();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetDamage();
        }
    }

    public void SpawnBullet()
    {
       bullet = Instantiate(bulletPrefab, spellPoint.position, spellPoint.rotation).GetComponent<FastBullet>();
    }

    public void LaunchBullet()
    {
        bullet.Launch();
    }

    public void GetDamage()
    {
        health = Mathf.Clamp(health - 1, 0, 2);
        anim.SetInteger("Health", health);
    }

    public void Heal()
    {
        health = Mathf.Clamp(health+1, 0, 2);
        anim.SetInteger("Health", health);
    }
}
