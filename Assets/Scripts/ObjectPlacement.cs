
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    [Header("Train Prefabs")]
    public GameObject goodTrainPrefab;
    public GameObject[] badTrainPrefabs;

    [Header("Spawn Settings")]
    public int maxSections = 10;

    [Range(0, 100)]
    public int badTrainChance = 80;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    public int TotalSpawned => totalSpawned;
    public int MaxSections => maxSections;

    private GameObject lastSpawnedTrain;
    private bool currentTrainIsBad;

    private int totalSpawned = 0;
    private int correctStreak = 0;

    private int spawnIndex = 0;
    private float trainLength;

    private bool hasAnsweredCurrentTrain = false;

    private List<GameObject> spawnedTrains = new List<GameObject>();

    void Start()
    {
        trainLength = GetLength(goodTrainPrefab);

        // First train stays permanently.
        totalSpawned = 1;
        spawnIndex = 0;
        currentTrainIsBad = false;
        hasAnsweredCurrentTrain = false;
    }

    public void SubmitAnswer(bool choseAnomaly)
    {
        if (hasAnsweredCurrentTrain)
            return;

        hasAnsweredCurrentTrain = true;
        bool correct = choseAnomaly == currentTrainIsBad;

        if (correct)
        {
            Debug.Log("Correct answer!");
            correctStreak++;

            if (audioSource != null && correctSound != null)
                audioSource.PlayOneShot(correctSound);

            SpawnTrain(false);
        }
        else
        {
            Debug.Log("Wrong answer!");
            correctStreak = 0;

            if (audioSource != null && wrongSound != null)
                audioSource.PlayOneShot(wrongSound);

            totalSpawned = 0;
            SpawnTrain(true);
        }
    }

    void SpawnTrain(bool forceGood)
    {
        if (totalSpawned >= maxSections)
        {
            Debug.Log("Maximum number of trains reached!");
            return;
        }

        GameObject selectedTrain;

        if (forceGood)
            selectedTrain = goodTrainPrefab;
        else
            selectedTrain = ChooseRandomTrain();

        if (selectedTrain == null)
            return;

        Vector3 trainPosition = transform.position +
            transform.right * (trainLength * (spawnIndex + 1));

        GameObject spawnedTrain = Instantiate(
            selectedTrain,
            trainPosition,
            transform.rotation
        );

        Debug.Log("Train spawned: " + selectedTrain.name);

        spawnedTrains.Add(spawnedTrain);

        lastSpawnedTrain = selectedTrain;
        currentTrainIsBad = selectedTrain != goodTrainPrefab;

        totalSpawned++;
        spawnIndex++;
        hasAnsweredCurrentTrain = false;

        // Keep only the two newest spawned trains.
        while (spawnedTrains.Count > 2)
        {
            GameObject oldestTrain = spawnedTrains[0];

            if (oldestTrain != null)
                Destroy(oldestTrain);

            spawnedTrains.RemoveAt(0);
        }

        Debug.Log("Spawned train " + totalSpawned + " / " + maxSections);
    }

    GameObject ChooseRandomTrain()
    {
        List<GameObject> availableBadTrains = new List<GameObject>();

        foreach (GameObject train in badTrainPrefabs)
        {
            if (train != null && train != lastSpawnedTrain)
                availableBadTrains.Add(train);
        }

        bool goodAvailable =
            goodTrainPrefab != null &&
            goodTrainPrefab != lastSpawnedTrain;

        bool wantBad = Random.Range(0, 100) < badTrainChance;

        if (wantBad && availableBadTrains.Count > 0)
            return availableBadTrains[Random.Range(0, availableBadTrains.Count)];

        if (!wantBad && goodAvailable)
            return goodTrainPrefab;

        if (availableBadTrains.Count > 0)
            return availableBadTrains[Random.Range(0, availableBadTrains.Count)];

        if (goodTrainPrefab != null)
            return goodTrainPrefab;

        return null;
    }

    float GetLength(GameObject prefab)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogError(prefab.name + " has no Renderer!");
            return 0f;
        }

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
            bounds.Encapsulate(renderer.bounds);

        return bounds.size.x;
    }
}
