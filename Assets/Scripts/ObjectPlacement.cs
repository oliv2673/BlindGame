
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [Header("Train Prefabs")]
    public GameObject goodTrainPrefab;
    public GameObject[] badTrainPrefabs;
    
    // [Header("Middle Prefab")]
    // public GameObject trainMiddlePrefab;

    [Header("Spawn Settings")]
    public int spawnAmount = 8;

    [Range(0, 100)]
    public int badTrainChance = 80;


    void Start()
    {

        // Calculate prefab lengths automatically
        float planeLength = GetLength(goodTrainPrefab);
        // float middleLength = GetLength(trainMiddlePrefab);

        // float totalLength = planeLength + middleLength;
        float totalLength = planeLength;

        for (int i = 0; i < spawnAmount; i++)
        {
            // Spawn middle section
            //Vector3 middlePosition = transform.position +
            //    transform.right * (planeLength / 2f +
            //    middleLength / 2f + totalLength * i);

            //Instantiate(trainMiddlePrefab, middlePosition, transform.rotation);

            // Randomly choose good or bad train
            GameObject selectedTrain;

            if (Random.Range(0, 100) < badTrainChance)
            {
                int randomIndex = Random.Range(0, badTrainPrefabs.Length);
                selectedTrain = badTrainPrefabs[randomIndex];
            }
            else
            {
                selectedTrain = goodTrainPrefab;
            }

            // Spawn selected train
            Vector3 trainPosition = transform.position +
                transform.right * (totalLength * (i + 1));

            Instantiate(selectedTrain, trainPosition, transform.rotation);
        }
    }

    float GetLength(GameObject prefab)
    {
        Renderer[] renderers =
            prefab.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogError(prefab.name + " has no Renderer!");
            return 0f;
        }

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds.size.x;
    }
}
