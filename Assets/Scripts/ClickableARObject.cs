using UnityEngine;

public class ClickableARObject : MonoBehaviour
{
    [SerializeField] GameObject prefabToSpawn ;

    public void OnTapped()
    {
        Debug.Log("This AR object was tapped!");

        gameObject.SetActive(false);
        prefabToSpawn.SetActive(true);

    }
}
