using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoinPickup = 100;

    //This bool is needed cos sometimes the coins get collected twice on pickup, this makes sure that doesn't happen
    bool wasCollected = false;

    //If player touches coin, will destroy coin
   void OnTriggerEnter2D(Collider2D other) {
       if(other.tag == "Player" && !wasCollected){
           wasCollected = true;   
           //This FindObjectOfType finds the public function AddToScore in GameSession script 
           FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);

           AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
           
           //This set active is technically not needed due to the bool, but it is to make doubly sure the coins don't get collected twice on pickup
           gameObject.SetActive(false);
           Destroy(gameObject);
       }
   }
}
