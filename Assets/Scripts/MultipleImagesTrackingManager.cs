using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using TMPro;

public class MultipleImagesTrackingManager : MonoBehaviour
{

    [SerializeField] List<GameObject> prefabsToSpawn = new List<GameObject>();
    [SerializeField] List<GameObject> buttons = new List<GameObject>();

    [SerializeField] TMP_Text Score1;
    [SerializeField] TMP_Text Score2;

    private ARTrackedImageManager _trackedImageManager;

    private Dictionary<string, GameObject> _arObjects;
    private int playerturn;
    void Start()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();

        if (_trackedImageManager == null)
        {
            Debug.LogError("ARTrackedImageManager not found on this GameObject.");
            return;
        }
        _trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);
        _arObjects = new Dictionary<string, GameObject>();
        SetupSceneElements();
        Score1.text = Score2.text = "Score: 0";

        int playerturn = Random.Range(0, 2); // Random int between 1 and 2
        if (playerturn == 1)
        {
            Button button = buttons[0].GetComponent<Button>(); ;
            if (button != null)
            {
                button.interactable = false;
            }
        }
        else
        {
            Button button = buttons[1].GetComponent<Button>(); ;
            if (button != null)
            {
                button.interactable = false;
            }
        }

    }

    private void OnDestroy()
    {
        _trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
    }

    private void SetupSceneElements()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            if (prefab != null && prefab.GetComponent<ARTrackedImage>() != null)
            {
                var arObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                arObject.name = prefab.name;
                arObject.gameObject.SetActive(false);
                _arObjects.Add(arObject.name, arObject);
            }
        }
    }

    private void OnImagesTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateTrackedImages(trackedImage);
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImages(trackedImage);
        }
        foreach (var trackedImage in eventArgs.removed)
        {
            UpdateTrackedImages(trackedImage.Value);
        }
    }

    private void UpdateTrackedImages(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;
        if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            return;
        }

        _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
        _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        _arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
    }

    // 🔥 PUBLIC METHOD TO CALL FROM UI BUTTON
    public void TriggerCarInteraction(string objectName)
    {
        if (_arObjects.TryGetValue(objectName, out GameObject arObject))
        {
            var car = arObject.GetComponentInChildren<Car>();
            if (car != null)
            {
                if (playerturn == 0)
                {
                    playerturn = 1;
                }
                else
                {
                    playerturn = 0;
                }
                car.TriggerInteraction();
            }
            else
            {
                Debug.LogWarning($"No Car component found on '{objectName}' prefab.");
            }
        }
        else
        {
            Debug.LogWarning($"AR object with name '{objectName}' not found.");
        }
    }

    // 🔥 Call this from the Car script when button is clicked
    public void DamageOtherModel(string objectName)
    {
        if (_arObjects.TryGetValue(objectName, out GameObject arObject))
        {
            var health = arObject.GetComponentInChildren<PlayerHealth>();
            if (health != null)
            {
                health.SetHealth();

                // Get the damage dealt
                int damage = (int)health.GetDamage();

                // Update score based on player turn
                if (playerturn == 0)
                {
                    // Parse current score from Score1.text
                    if (int.TryParse(Score1.text.Split(": ")[1], out int currentScore))
                    {
                        int newScore = currentScore + damage;
                        SetScore1($"Score: {newScore}");
                    }
                    else
                    {
                        Debug.LogError("Failed to parse Score1 text: " + Score1.text);
                    }
                }
                else
                {
                    // Parse current score from Score2.text
                    if (int.TryParse(Score2.text.Split(": ")[1], out int currentScore))
                    {
                        int newScore = currentScore + damage;
                        SetScore2($"Score: {newScore}");
                    }
                    else
                    {
                        Debug.LogError("Failed to parse Score2 text: " + Score2.text);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"No PlayerHealth component found in '{objectName}'.");
            }
        }
        else
        {
            Debug.LogWarning($"AR object with name '{objectName}' not found.");
        }
    }
    void Update()
    {
        if (playerturn == 1)
        {
            Button button = buttons[0].GetComponent<Button>();
            Button button1 = buttons[1].GetComponent<Button>();

            if (button != null)
            {
                button.interactable = false;

                if (_arObjects["Marker-2"].gameObject.GetComponentInChildren<PlayerHealth>().GetHealth() != 0)
                {
                    button1.interactable = true;
                }
            }
        }
        else
        {
            Button button = buttons[1].GetComponent<Button>();
            Button button1 = buttons[0].GetComponent<Button>();

            button.interactable = false;

            if (_arObjects["Marker-1"].gameObject.GetComponentInChildren<PlayerHealth>().GetHealth() != 0)
            {
                button1.interactable = true;
            }
        }
    }
    // public string GetScore1()
    // {
    //     return Score1.text;
    // }
    public string SetScore1(string newScore)
    {
        return Score1.text = newScore;
    }

    public string SetScore2(string newScore)
    {
        return Score2.text = newScore;
    }
}