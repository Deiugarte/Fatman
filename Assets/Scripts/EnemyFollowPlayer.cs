using UnityEngine;
public class EnemyFollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public float movementSpeed = 4;
    void Update()
    {
        transform.LookAt(Player.transform);
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }
}