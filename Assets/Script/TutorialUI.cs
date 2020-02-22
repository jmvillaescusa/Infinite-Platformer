using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private GameObject goose;
    public GameObject tutorialUI;
    public bool isOffscreen;

    public float count = 0;

    Vector3 pos;

    private void Start()
    {
        isOffscreen = false;
        pos = transform.position;

        goose = GameObject.Find("Goose");
    }

    private void Update()
    {
        if (isOffscreen && count < 5)
        {
            count += 0.5f;
        }

        if (count >= 5)
        {
            tutorialUI.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        isOffscreen = true;
    }
    private void OnBecameVisible()
    {
        isOffscreen = false;
    }

    private void LateUpdate()
    {
        if (goose.GetComponent<GooseController>().m_frozen && goose.GetComponent<GooseController>().m_isAlive)
        {
            transform.position = pos;
            pos.x -= goose.GetComponent<GooseController>().m_moveSpeed * Time.deltaTime;
        }
    }
}
