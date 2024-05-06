using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    private int nbCherries = 0;
    [SerializeField] 
    private TextMeshProUGUI cherriesText;
    [SerializeField]
    private AudioSource cherrySound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            nbCherries++;
            cherriesText.text = nbCherries.ToString();
            cherrySound.Play();
        }
    }
}
