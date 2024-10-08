using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCntrl : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float speed = 0.5f;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        animator.SetFloat("speed", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > 0.1f)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(speed * direction * Time.deltaTime, Space.World);
            Quaternion rotTarget = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, 800.0f * Time.deltaTime);
        }
    }

   
}
