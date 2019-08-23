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

  void Start () {
    GetComponent<AudioSource> ().playOnAwake = false;
    GetComponent<AudioSource> ().clip = auchSound;
  }

  void OnTriggerEnter (Collider other) {
    if (other.gameObject == GameObject.FindGameObjectWithTag ("Enemy")) {
      healthSlider.value -= 6;
      currentHealth -= 6;
      damageImage.color = flashColour;
      GetComponent<AudioSource> ().Play ();
      // damageImage.color = flashColour;

      Debug.Log ("*** Player is in contact with Enemy. ***");
    }
    if (other.gameObject == GameObject.FindGameObjectWithTag ("Enemy_2")) {
      healthSlider.value -= 6;
      currentHealth -= 6;
      damageImage.color = flashColour;
      GetComponent<AudioSource> ().Play ();
      // damageImage.color = flashColour;

      Debug.Log ("*** Player is in contact with Enemy 2 (Eggy). ***");
    }

    if (other.gameObject == GameObject.FindGameObjectWithTag ("Enemy_3")) {
      healthSlider.value -= 5;
      currentHealth -= 5;
      damageImage.color = flashColour;
      GetComponent<AudioSource> ().Play ();
      // damageImage.color = flashColour;

      Debug.Log ("*** Player is in contact with Enemy 3 (Chilli). ***");
    }
    if (other.gameObject == GameObject.FindGameObjectWithTag ("Health")) {
      healthSlider.value += 15;
      currentHealth += 15;
      Debug.Log ("*** Player has been healed! (+15 HP) ***");
    }
  }
  void OnTriggerExit (Collider other) {

    damageImage.color = Color.Lerp (new Color (1f, 0f, 0f, 0), Color.clear, flashSpeed * Time.deltaTime);

  }
}

