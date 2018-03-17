using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public Camera newCam;

    public Canvas levelSelect;
    public Canvas reportCanvas;

    private Camera oldCam;

    public ButtonEvents play;
    public ButtonEvents options;
    public ButtonEvents credits;
    public ButtonEvents report;
    public ButtonEvents exit;

    private void Start()
    {
        newCam.gameObject.SetActive(false);
        levelSelect.gameObject.SetActive(false);
        reportCanvas.gameObject.SetActive(false);

    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void PlayEvent()
    {
        oldCam = Camera.main;
        ActiveMainMenuButtons(false);
        oldCam.gameObject.SetActive(false);
        newCam.gameObject.SetActive(true);
        newCam.GetComponent<Animator>().Play("MoveToBed");
    }

    private void ActiveMainMenuButtons(bool isActive)
    {
        play.SetActive(isActive);
        options.SetActive(isActive);
        credits.SetActive(isActive);
        report.SetActive(isActive);
        exit.SetActive(isActive);
    }

    public void ActivateLevelUI(bool isActiv)
    {
        levelSelect.gameObject.SetActive(isActiv);
    }

    public void ActivateMainMenu()
    {
        ActivateLevelUI(false);
        ActiveMainMenuButtons(true);
        newCam.gameObject.SetActive(false);
        oldCam.gameObject.SetActive(true);
    }

    public void ReportOpen()
    {
        ActiveMainMenuButtons(false);
        reportCanvas.gameObject.SetActive(true);
        reportCanvas.GetComponent<HelpCanvas>().ResetInputFields();
        reportCanvas.GetComponent<HelpCanvas>().mainPanel.gameObject.SetActive(true);
    }

    public void ReportClose ()
    {
        ActiveMainMenuButtons(true);
        reportCanvas.gameObject.SetActive(false);
    }
}
