using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushes : MonoBehaviour
{
    [SerializeField] private int encounterRatePercentage;

    public event Action OnEncountered;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 velocity = collision.GetComponent<Rigidbody2D>().velocity;

            if (velocity != Vector2.zero)
            {
                CheckEncounter();
            }
        }
    }

    private void CheckEncounter()
    {
        if (PlayerManager.instance.Player.engagedInCombat)
            return;

        if (UnityEngine.Random.Range(1, 101) <= encounterRatePercentage)
        {
            PlayerManager.instance.Player.engagedInCombat = true;
            OnEncountered();
        }   
    }
}
