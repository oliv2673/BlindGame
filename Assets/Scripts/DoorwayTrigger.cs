
using UnityEngine;

public class DoorwayTrigger : MonoBehaviour
{
    [Header("Doors")]
    public SlidingDoor leftDoor;
    public SlidingDoor rightDoor;

    [Header("Door Settings")]
    public bool openWhenPlayerEnters = true;
    public bool closeWhenPlayerLeaves = true;
    public bool requireButtonPress = false;

    private bool buttonPressed = false;
    private int playersInside = 0;

    public void OpenAfterChoice()
    {


        buttonPressed = true;
        OpenDoors();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        playersInside++;

        if (!requireButtonPress && openWhenPlayerEnters)
        {
            OpenDoors();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.transform.root.CompareTag("Player"))
            return;

        playersInside = Mathf.Max(0, playersInside - 1);

        if (playersInside == 0 && closeWhenPlayerLeaves)
        {
            if (!requireButtonPress || buttonPressed)
            {


                CloseDoors();
                buttonPressed = false;
            }
        }
    }

    void OpenDoors()
    {
        if (leftDoor != null)
            leftDoor.OpenDoor();

        if (rightDoor != null)
            rightDoor.OpenDoor();
    }

    void CloseDoors()
    {
        if (leftDoor != null)
            leftDoor.CloseDoor();

        if (rightDoor != null)
            rightDoor.CloseDoor();
    }
}
