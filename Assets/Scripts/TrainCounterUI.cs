using UnityEngine;
using TMPro;

public class TrainCounterUI : MonoBehaviour
{
    public ObjectPlacement trainManager;
    public TMP_Text counterText;

    void Update()
    {
        counterText.text = "Rooms cleared: " + trainManager.TotalSpawned
            + " / " + trainManager.MaxSections;
    }
}