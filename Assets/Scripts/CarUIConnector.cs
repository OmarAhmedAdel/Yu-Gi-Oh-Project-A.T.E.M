using UnityEngine;
using UnityEngine.UI;

public class CarUIConnector : MonoBehaviour
{
    public Button interactButton; // Assign via Inspector
    private Car currentCar;

    public void SetCurrentCar(Car car)
    {
        currentCar = car;

        // Clear old listeners and add the new one
        interactButton.onClick.RemoveAllListeners();
        // interactButton.onClick.AddListener(car.TriggerInteraction);
    }
}
