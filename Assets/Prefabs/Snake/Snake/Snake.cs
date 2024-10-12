using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    Animator animator;

    bool isAlive = true;

    public float Health
    {
        set
        {
            if (value < _health)
            {
                animator.SetTrigger("Snake_Hit");
            }
            _health = value;

            

            if (_health <= 0)
            {
                animator.SetBool("SnakeAlive", false);
            }
        }
        get
        {
            return _health;
        }
    }

    public void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("SnakeAlive", isAlive);
    }

    public float _health = 3;
    void OnHit(float damage)
    {
        Health -= damage;
    }
}
