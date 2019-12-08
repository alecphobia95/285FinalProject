using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    /// Buttons
    public Button startButton;
    public Button instructButton;
    public Button quitButton;
    public Button leaveButton;
    public Button stayButton;

    ///Game Objects
    public GameObject buttonPanel;
    public GameObject instructPanel;
    public GameObject quitPanel;


    ///Audio
    public AudioClip bg;

    public void Start()
    {
        instructPanel.SetActive(false);
        quitPanel.SetActive(false);
        buttonPanel.SetActive(true);
    }

    public void closeAction()
    {
        
    }

    public void StartButton()
    {
        instructPanel.SetActive(false);
        quitPanel.SetActive(false);
        buttonPanel.SetActive(false);
    }

    public void InstructButton()
    {
        instructPanel.SetActive(true);
        buttonPanel.SetActive(true);
        quitPanel.SetActive(false);
    }

    public void QuitButton()
    {
        instructPanel.SetActive(false);
        buttonPanel.SetActive(false);
        quitPanel.SetActive(true);
    }

    public void LeaveButton()
    {
        Debug.Log("Application Closed");
        Application.Quit();
    }

    public void StayButton()
    {
        instructPanel.SetActive(false);
        buttonPanel.SetActive(true);
        quitPanel.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check whether it's active / inactive
            bool isActive = instructPanel.activeSelf;

            instructPanel.SetActive(!isActive);
        }

    }
}
