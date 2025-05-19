using UnityEngine;

public class ARTapToSelect : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Tapped on: " + hit.collider.gameObject.name);
                // Optional: Call method on tapped object
                hit.collider.gameObject.SendMessage("OnTapped", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}