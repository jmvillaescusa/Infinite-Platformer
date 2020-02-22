    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public GameObject m_pauseMenu;
    private GameObject m_goose;
    public BlockPool m_blockPool;

    public UIController m_gameUI;

    public bool m_isPaused = false;

    

    private void Awake()
    {
        m_goose = GameObject.Find("Goose");
        SetInstance();
    }

    void SetInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PauseGame()
    {
        if (!m_isPaused)
        {
            Time.timeScale = 0;
            m_isPaused = true;
            m_pauseMenu.SetActive(true);
        }
    }

    public void UnpauseGame()
    {
        if (m_isPaused)
        {
            Time.timeScale = 1;
            m_isPaused = false;
            m_pauseMenu.SetActive(false);
        }
    } 

    public void GameOver()
    {
        m_gameUI.gameUI.SetActive(false);
        m_gameUI.gameOverUI.SetActive(true);

        if (m_gameUI.distance >= m_gameUI.highesetDistance)
        {
            m_gameUI.UpdateHighDistance();

            m_gameUI.newRecordTimer += 0.1f;
            if (m_gameUI.newRecordTimer >= 5.0f)
            {
                m_gameUI.newRecordText.SetActive(true);
            }
        }
    }

    public void ResetGame()
    {
        m_gameUI.ResetUI();
        m_blockPool.ResetPool();
        m_goose.GetComponent<GooseController>().ResetGoose();
    }
}