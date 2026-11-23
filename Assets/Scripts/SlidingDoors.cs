
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3, 0);
    public float slideSpeed = 3f;

    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool initialized = false;

    void Start()
    {
        closedPosition = transform.localPosition;
        targetPosition = closedPosition;
        initialized = true;
    }

    void Update()
    {
        if (!initialized)
            return;

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetPosition,
            slideSpeed * Time.deltaTime
        );
    }

    public void OpenDoor()
    {
        targetPosition = closedPosition + openOffset;
    }

    public void CloseDoor()
    {
        targetPosition = closedPosition;
    }
}
