using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public int damageDone=10;
    public GameObject hitImapctprefab;

    private void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            player.AppplyDamage(damageDone);
        }

        Instantiate(hitImapctprefab, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
