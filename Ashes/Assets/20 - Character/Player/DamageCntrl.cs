using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCntrl : MonoBehaviour
{
    private float maxDamage = 30.0f;

    public bool TakeDamage(float damage)
    {
        maxDamage -= damage;

        return (maxDamage <= 0.0f);
    }
}
