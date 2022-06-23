using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 7.5f;
    public Rigidbody2D RB;

    public GameObject impactEffect;


    //Mermi kac hasar vercek
    public int damageToGive = 50;


    void Start()
    {
        
    }

    void Update()
    {
        RB.velocity = transform.right * speed; //Merminin z rotasyonu hangi degerde olursa olsun dümdüz gidecek
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);

        //Impact music
        AudioManager.instance.PlaySFX(4);

        if (other.tag=="Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive); //Dusman kodundaki fonk. cagirdik ve kac hasar alcak tanýmladýk
        }

        if(other.tag == "Boss")
        {
            BossController.instance.TakeDamage(damageToGive);

            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);

        }
    }

    private void OnBecameInvisible() //Bi süre sonra o nesneyi yok edelim
    {
        Destroy(gameObject);
    }
}
