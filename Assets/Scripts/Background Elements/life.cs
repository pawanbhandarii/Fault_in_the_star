using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class life : MonoBehaviour
{
    [SerializeField]
    int health_value;
    public AudioClip healthSFX;
    public GameObject effects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player.playerHealth < 100)
            {
                player.IncreaseHealth(health_value);
            }
            player.PlaySound(healthSFX);
            AfterEffects();
            Destroy(this.gameObject);

        }
    }

    void AfterEffects()
    {
        Instantiate(effects, transform.position, Quaternion.identity);
    }
}
