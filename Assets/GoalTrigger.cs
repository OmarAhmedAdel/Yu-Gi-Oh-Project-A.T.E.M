using TMPro;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private TextMeshPro textObject;

    void Start()
    {
        textObject = GameObject.Find("WinningText").GetComponent<TextMeshPro>();  
    }

    void OnTriggerEnter(Collider other)
    {
        textObject.gameObject.SetActive(true);
    }
}
