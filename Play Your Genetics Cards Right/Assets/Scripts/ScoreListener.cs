using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkMessage : MessageBase
{
    public string message;
}

public class ScoreListener : MonoBehaviour
{
    [SerializeField] private int port = 4444;
    [SerializeField] private int maxConnections = 50;

    [SerializeField] private TextMeshProUGUI txtAddress;

    [SerializeField] private HostHandler hostHandler;

    // The id we use to identify our messages and register the handler
    short messageID = 1000;

    // Start is called before the first frame update
    private void Start()
    {
        SetupServer();
    }

    public void SetupServer()
    {
        // Register handlers for the types of messages we can receive
        RegisterHandlers();

        var config = new ConnectionConfig ();
        // There are different types of channels you can use, check the official documentation
        config.AddChannel (QosType.ReliableFragmented);
        config.AddChannel (QosType.UnreliableFragmented);

        var ht = new HostTopology (config, maxConnections);

        if (!NetworkServer.Configure (ht)) {
            Debug.Log ("No server created, error on the configuration definition");
            return;
        } else {
            // Start listening on the defined port
            if(NetworkServer.Listen (port))
                Debug.Log ("Server created, listening on port: " + port);
            else
                Debug.Log ("No server created, could not listen to the port: " + port);
        }
    }

    void OnApplicationQuit() {
        NetworkServer.Shutdown ();
    }

    private void RegisterHandlers () {
        // Unity have different Messages types defined in MsgType
        NetworkServer.RegisterHandler (MsgType.Connect, OnClientConnected);
        NetworkServer.RegisterHandler (MsgType.Disconnect, OnClientDisconnected);

        // Our message use his own message type.
        NetworkServer.RegisterHandler (messageID, OnMessageReceived);
    }

    private void RegisterHandler(short t, NetworkMessageDelegate handler) {
        NetworkServer.RegisterHandler (t, handler);
    }

    void OnClientConnected(NetworkMessage netMessage)
    {
        // Do stuff when a client connects to this server

        // Send a thank you message to the client that just connected
        MyNetworkMessage messageContainer = new MyNetworkMessage();
        messageContainer.message = "Score has been submitted.";

        // This sends a message to a specific client, using the connectionId
        NetworkServer.SendToClient(netMessage.conn.connectionId,messageID,messageContainer);

        //// Send a message to all the clients connected
        //messageContainer = new MyNetworkMessage();
        //messageContainer.message = "A new player has connected to the server";

        //// Broadcast a message a to everyone connected
        //NetworkServer.SendToAll(messageID,messageContainer);
    }

    void OnClientDisconnected(NetworkMessage netMessage)
    {
        // Do stuff when a client dissconnects
    }

    void OnMessageReceived(NetworkMessage netMessage)
    {
        // You can send any object that inherence from MessageBase
        // The client and server can be on different projects, as long as the MyNetworkMessage or the class you are using have the same implementation on both projects
        // The first thing we do is deserialize the message to our custom type
        var objectMessage = netMessage.ReadMessage<MyNetworkMessage>();
        Debug.Log("Message received: " + objectMessage.message);

        if (hostHandler)
            hostHandler.UpdateMessageList(objectMessage.message);
    }
}
