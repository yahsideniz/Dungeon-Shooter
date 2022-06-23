using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    //Kirik parcalarin hareket etmesi
    public float moveSpeed = 3f;
    private Vector3 moveDirection;

    public float deceleration = 5f;

    //Parcalar yok olsun diye
    public float lifetime = 3f;

    // Parcalarin rengi solarak yok olsun
    public SpriteRenderer SR;
    public float fadeSpeed = 2.5f;

    void Start()
    {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);

    }

    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);


        //Parcalar yok olsun diye
        lifetime -= Time.deltaTime;

        if(lifetime < 0)
        {
            // Rengi solarak yok olsun
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, Mathf.MoveTowards(SR.color.a, 0f, fadeSpeed * Time.deltaTime));

            if(SR.color.a == 0f) // rengi solunca yok et 
            {
                Destroy(gameObject);
            }
        }

    }
}
