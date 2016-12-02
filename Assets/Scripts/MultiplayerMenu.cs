﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;

public class MultiplayerMenu : MonoBehaviour {
    string defaultLabel, defaultStart;
    NetworkLayer net;
    Button hostButton, connectButton, startButton, backButton;
    InputField addressInput;
    Text label, connectionList;
    void Start()
    {
        net = Object.FindObjectOfType<NetworkLayer>();
        hostButton = gameObject.transform.FindChild("Host Button").GetComponent<Button>();
        connectButton = gameObject.transform.FindChild("Connect Button").GetComponent<Button>();
        startButton = gameObject.transform.FindChild("Start Button").GetComponent<Button>();
        backButton = gameObject.transform.FindChild("Back Button").GetComponent<Button>();
        addressInput = gameObject.transform.FindChild("Address Input").GetComponent<InputField>();
        label = gameObject.transform.FindChild("Info Label").GetComponent<Text>();
        connectionList = gameObject.transform.FindChild("Connection List").GetComponent<Text>();
        defaultLabel = label.text;
        defaultStart = startButton.gameObject.transform.GetComponentInChildren<Text>().text;
        connectionList.text = "Connected hosts:";
    }
    public void Back()
    {
        this.gameObject.transform.parent.FindChild("Start Up Dialog").gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void Host()
    {
        hostButton.interactable = false;
        connectButton.interactable = false;
        addressInput.interactable = false;
        startButton.interactable = true;
        backButton.interactable = false;
        label.text = "My address: " + net.GetIPString();
        net.Host();
        StartCoroutine(RefreshList());
    }
    public void Connect()
    {
        hostButton.interactable = false;
        connectButton.interactable = false;
        addressInput.interactable = false;
        backButton.interactable = false;
        startButton.gameObject.transform.GetComponentInChildren<Text>().text = "Wait for server to start...";
        var ret = net.Connect(addressInput.text);
        if (ret != null)
        {
            label.text = ret.ToString();
            hostButton.interactable = true;
            connectButton.interactable = true;
            addressInput.interactable = true;
            backButton.interactable = true;
            startButton.gameObject.transform.GetComponentInChildren<Text>().text = defaultStart;
        }
        StartCoroutine(RefreshList());
    }
    public void StartGame()
    {

    }
    IEnumerator RefreshList()
    {
        while (true)
        {
            connectionList.text = "Connected hosts:\r\n" + net.GetClientList();
            yield return new WaitForSecondsRealtime(0.4f);
        }
    }
}
