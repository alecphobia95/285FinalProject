using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenuScript : MonoBehaviour
{


    ///Game Objects
    public GameObject instructPanel;



    ///Audio
    public AudioClip bg;

    public void Start()
    {
        instructPanel.SetActive(false);


    }

    public void InstructButton()
    {
        instructPanel.SetActive(true);

    }

    public void QuitButton()
    {
        instructPanel.SetActive(false);

    }

    public void LeaveButton()
    {
        Debug.Log("Application Closed");
        Application.Quit();
    }

    public void StayButton()
    {
        instructPanel.SetActive(false);
   
    }

    private void Update()
    {
        ///Handles in game access to control menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check whether it's active / inactive
            bool isActive = instructPanel.activeSelf;

            instructPanel.SetActive(!isActive);
        }

    }
}
