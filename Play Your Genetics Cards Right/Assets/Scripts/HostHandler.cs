using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HostHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtAddress;
    [SerializeField] private TextMeshProUGUI txtConnections;
    [SerializeField] private Button btnExit;
    
    // Start is called before the first frame update
    private void Start()
    {
        btnExit.onClick.AddListener(BtnExitPressed);
        
        txtAddress.text = GetLocalIPAddress();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void BtnExitPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    public void UpdateMessageList(string newMessage)
    {
        txtConnections.text += "\n" + newMessage;
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}
