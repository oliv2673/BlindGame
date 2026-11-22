
using UnityEngine;

public class AmbienceSound : MonoBehaviour
{
    public Collider Area;
    public GameObject Player;

    void Start()
    {
        // Find the player if one hasn't been assigned
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

    }

    void Update()
    {
        // Make sure references exist
        if (Player == null || Area == null)
            return;

        // Find closest point on collider to player
        Vector3 closestPoint = Area.ClosestPoint(Player.transform.position);

        // Move ambience sound to that position
        transform.position = closestPoint;
    }
}
