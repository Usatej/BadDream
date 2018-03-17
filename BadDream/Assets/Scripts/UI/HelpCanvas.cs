using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpCanvas : MonoBehaviour
{

    public InputField emailField;
    public InputField descField;

    public Image mainPanel;
    public Image errorPanel;
    public Image completePanel;



    private GameMaster gm;

    [SerializeField]
    private string base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSceaFE__ZcpZA4y6ZUTQt0unG5oDoGmt7bpJtocz9ZN7Cc6ng/formResponse";

    private void Start()
    {
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        errorPanel.gameObject.SetActive(false);
        completePanel.gameObject.SetActive(false);
        mainPanel.gameObject.SetActive(true);
    }

    private void SendReport()
    {
        string system;
        string device;
        string email;
        string bugDescription;
        string appVersion;
        system = SystemInfo.operatingSystem;
        device = SystemInfo.deviceModel;
        email = emailField.text;
        bugDescription = descField.text;
        appVersion = Application.version;

        StartCoroutine(Post(system, device, appVersion, email, bugDescription));
    }

    IEnumerator Post(string _system, string _device, string _appVersion, string _email, string _bug)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.4209489", _system);
        form.AddField("entry.1095147840", _device);
        form.AddField("entry.2127896114", _appVersion);
        form.AddField("entry.974611932", _email);
        form.AddField("entry.1903331898", _bug);
        byte[] rawData = form.data;
        WWW www = new WWW(base_URL, rawData);
        yield return www;
    }

    public void ResetInputFields()
    {
        emailField.text = descField.text = "";
        SetWhiteTextDesc();
        SetWhiteTextEmail();
        ((Text)emailField.placeholder).text = "Enter your email adress..";
        ((Text)descField.placeholder).text = "Enter description of bug..";
    }

    public void SendButton()
    {
        bool error = false;
        if (EmailValidation(emailField.text))
        {
            emailField.text = "";
            ((Text)emailField.placeholder).text = "Please enter correct email adress!";
            ((Text)emailField.placeholder).color = Color.red;
            error = true;
        }
        if (descField.text.Length < 10)
        {
            descField.text = "";
            ((Text)descField.placeholder).text = "Please enter description of the bug!";
            ((Text)descField.placeholder).color = Color.red;
            error = true;
        }
        if (error) return;

        StartCoroutine(checkInternetConnection((isConnected) =>
        {
            if (isConnected)
            {
                SendReport();
                ShowCompleteAnnounce();
            }
            else ShowNetworkError();
        }));
    }

    private void SendToSheets()
    {

    }

    private void ShowNetworkError()
    {
        mainPanel.gameObject.SetActive(false);
        errorPanel.gameObject.SetActive(true);
    }

    public void HideNetworkError()
    {
        mainPanel.gameObject.SetActive(true);
        errorPanel.gameObject.SetActive(false);
    }

    public IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public void SetWhiteTextEmail()
    {
        ((Text)emailField.placeholder).color = Color.white;
    }

    public void SetWhiteTextDesc()
    {
        ((Text)descField.placeholder).color = Color.white;
    }

    public void ShowCompleteAnnounce()
    {
        mainPanel.gameObject.SetActive(false);
        completePanel.gameObject.SetActive(true);
    }

    public void CloseCompleteAnnounce()
    {
        completePanel.gameObject.SetActive(false);
        gm.ReportClose();
    }

    private bool EmailValidation(string email)
    {
        return (email.Length < 3) || (email.IndexOf("@") < 1);
    }
}
