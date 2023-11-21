using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform platePrefab;

    private List<GameObject> plateVisualObjectList;

    private void Awake()
    {
        plateVisualObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateAdded += PlatesCounter_OnPlateAdded;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateVisualGameObject = plateVisualObjectList[plateVisualObjectList.Count - 1];
        plateVisualObjectList.Remove(plateVisualGameObject);
        Destroy(plateVisualGameObject);
    }

    private void PlatesCounter_OnPlateAdded(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(platePrefab, counterTopPoint);

        float plateOffsetY = .1f * plateVisualObjectList.Count; 
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY, 0);

        plateVisualObjectList.Add(plateVisualTransform.gameObject);
    }
}
