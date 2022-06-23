using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    private SpriteRenderer SR;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();

        SR.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f); // y degeri float ama order in layer kismi int olmak zorunda
    }

    void Update()
    {
        
    }
}
