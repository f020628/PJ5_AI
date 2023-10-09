using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rulaishenzhang : MonoBehaviour
{
    private Collider2D Collider; 
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("DisableCollider", 1f);
    }

    // Update is called once per frame
   
    void DisableCollider()
    {
        Collider.enabled = false;
        ChangeSpriteSortingLayer();
    }
    void ChangeSpriteSortingLayer()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "onBackground";
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on the object!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            // ÏûÃðµÐÈË
            Destroy(enemy.gameObject);
        }

    }
}
