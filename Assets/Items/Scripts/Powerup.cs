using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var player))
        {
            ActivatePowerup(player);
            Destroy(gameObject);
        }
    }

    public abstract void ActivatePowerup(Player player);
}
