using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject gameUI;
    public Text distanceValue;
    public float distance;
    public float highesetDistance;
    public float count = 0;

    public Text highValue;

    public GameObject gameOverUI;
    public GameObject newRecordText;
    public Text gameOverText;
    public float newRecordTimer = 0.0f;

    private void Start()
    {
        distance = 0;
        highesetDistance = 50;

        gameOverUI.SetActive(false);
        newRecordText.SetActive(false);

        highValue.text = highesetDistance.ToString();
    }

    public float GetDistance() { return distance; }
    public void AddDistance() { distance++; }

    private void Update()
    {
        distanceValue.text = GetDistance().ToString();
    }

    public void UpdateHighDistance()
    {
        highesetDistance = distance;
        highValue.text = highesetDistance.ToString();
    }

    public void ResetUI()
    {
        distance = 0;
        newRecordTimer = 0;

        gameOverUI.SetActive(false);
        newRecordText.SetActive(false);

        gameUI.SetActive(true);
    }
}
