
using UnityEngine;

public class TrainButtonInput : MonoBehaviour
{
    public ObjectPlacement trainManager;
    public float interactionDistance = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckButton();
        }
    }

    void CheckButton()
    {
        Camera cam = GetComponent<Camera>();

        if (cam == null)
            return;

        Ray ray = cam.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            Transform hitObject = hit.collider.transform;

            while (hitObject != null)
            {
                if (hitObject.CompareTag("Normal"))
                {
                    PressButton(false, hit.collider.transform);
                    return;
                }

                if (hitObject.CompareTag("Anomaly"))
                {
                    PressButton(true, hit.collider.transform);
                    return;
                }

                hitObject = hitObject.parent;
            }
        }
    }

    DoorwayTrigger FindExitDoorTrigger(Transform buttonTransform)
    {
        Transform trainRoot = buttonTransform.root;

        DoorwayTrigger[] triggers =
            trainRoot.GetComponentsInChildren<DoorwayTrigger>(true);

        foreach (DoorwayTrigger trigger in triggers)
        {
            if (trigger.CompareTag("ExitTrigger"))
            {
                return trigger;
            }
        }

        Debug.LogError("No ExitTrigger found in " + trainRoot.name);
        return null;
    }

    void PressButton(bool choseAnomaly, Transform buttonTransform)
    {
        if (trainManager == null)
        {
            Debug.LogError("Train Manager is missing!");
            return;
        }

        DoorwayTrigger currentExit =
            FindExitDoorTrigger(buttonTransform);

        // Disable both answer buttons on this train
        Transform trainRoot = buttonTransform.root;

        Transform[] allChildren =
            trainRoot.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            if (child.CompareTag("Normal") || child.CompareTag("Anomaly"))
            {
                child.gameObject.SetActive(false);
            }
        }

        trainManager.SubmitAnswer(choseAnomaly);

        if (currentExit != null)
        {
            currentExit.OpenAfterChoice();
        }
    }
}
