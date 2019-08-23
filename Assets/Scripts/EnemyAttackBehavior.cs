using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackBehavior : MonoBehaviour {
  public int startingHealth = 100; // The amount of health the player starts the game with.
  public int currentHealth; // The current health the player has.
  public Slider healthSlider; // Reference to the UI's health bar.
  public Image damageImage; // Reference to an image to flash on the screen on being hurt.
  public AudioClip deathClip; // The audio clip to play when the player dies.
  public float flashSpeed = 5f; // The speed the damageImage will fade at.
  public Color flashColour = new Color (1f, 0f, 0f, 0.15f); // The colour the damageImage is set to, to flash.

  public AudioClip auchSound;


     void Start ()   
     {
         GetComponent<AudioSource> ().playOnAwake = false;
         GetComponent<AudioSource> ().clip = auchSound;
     } 

  void OnTriggerEnter (Collider other) {
    if (other.gameObject == GameObject.FindGameObjectWithTag ("Enemy")) {
      healthSlider.value -= 10;
      currentHealth -= 10;
      damageImage.color = flashColour;
      GetComponent<AudioSource> ().Play ();
      // damageImage.color = flashColour;

      Debug.Log ("*** Player is in contact with Enemy. ***");
    }
    if (other.gameObject == GameObject.FindGameObjectWithTag ("Health")) {
      healthSlider.value += 10;
      currentHealth += 10;
      Debug.Log ("*** Player is in contact with Enemy. ***");
    }
  }
  void OnTriggerExit (Collider other) {
    if (other.gameObject == GameObject.FindGameObjectWithTag ("Enemy")) {
      damageImage.color = Color.Lerp (new Color (1f, 0f, 0f, 0), Color.clear, flashSpeed * Time.deltaTime);
    }
  }

}