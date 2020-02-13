using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class SubmitScore : MonoBehaviour
{
    [SerializeField] private Button _btnSubmitScore;
    [SerializeField] private Button _btnCancel;
    [SerializeField] private TMP_InputField _inputAddress;
    [SerializeField] private TextMeshProUGUI _txtOutputInfo;

    [SerializeField] private Color _errorColour;
    [SerializeField] private Color _neutralColour;

    [SerializeField] private int _port = 4444;
    //[SerializeField] private string _ip = "localhost";

    //The id we use to identify our messages and register the handler
    private short _messageID = 1000;

    private NetworkClient _client;

    private ResultsHandler resultsHandler;
    
    // Start is called before the first frame update
    private void Start()
    {
        resultsHandler = GameObject.FindGameObjectWithTag("ResultsPopup").GetComponent<ResultsHandler>();
        
        var config = new ConnectionConfig();

        // Config the Channels we will use
        config.AddChannel (QosType.ReliableFragmented);
        config.AddChannel (QosType.UnreliableFragmented);

        // Create the client and attach the configuration
        _client = new NetworkClient ();
        _client.Configure (config,1);

        // Register the handlers for the different network messages
        RegisterHandlers();
        
        //start with the object offscreen and lerp onto screen
        transform.localPosition = new Vector3(0.0f, -1000.0f, 0.0f);
        StartCoroutine(LerpGameObjectY(gameObject, 0.0f, 1.0f, false, 0.0f));

        _btnSubmitScore.onClick.AddListener(BtnSubmitScorePressed);
        _btnCancel.onClick.AddListener(BtnCancelPressed);
    }

    // Register the handlers for the different message types
    void RegisterHandlers () {
    
        // Unity have different Messages types defined in MsgType
        _client.RegisterHandler (_messageID, OnMessageReceived);
        _client.RegisterHandler(MsgType.Connect, OnConnected);
        _client.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        _client.RegisterHandler(MsgType.Error, OnError);
    }

    void OnConnected(NetworkMessage message) {        
        // Do stuff when connected to the server

        MyNetworkMessage messageContainer = new MyNetworkMessage();
        messageContainer.message = "Score: " + GameData.Points.Combined.Get();

        // Say hi to the server when connected
        _client.Send(_messageID, messageContainer);
    }

    void OnError(NetworkMessage message)
    {
        var errorMsg = message.ReadMessage<ErrorMessage>();
        Debug.Log("Error:" + errorMsg.errorCode);

        //if failed to connect
        if (errorMsg.errorCode == 11)
        {
            StopAllCoroutines();
            _txtOutputInfo.color = _errorColour;
            _txtOutputInfo.text = "Could not connect to address...";
        }
    }

    void OnDisconnected(NetworkMessage message) {
        // Do stuff when disconnected to the server
        Debug.Log("Disconnected from server");
    }

    void OnFailedToConnect(NetworkIdentity.ClientAuthorityCallback error) {
        Debug.Log("Could not connect to server: " + error);
    }

    // Message received from the server
    void OnMessageReceived(NetworkMessage netMessage)
    {
        // You can send any object that inherence from MessageBase
        // The client and server can be on different projects, as long as the MyNetworkMessage or the class you are using have the same implementation on both projects
        // The first thing we do is deserialize the message to our custom type
        var objectMessage = netMessage.ReadMessage<MyNetworkMessage>();

        Debug.Log("Message received: " + objectMessage.message);

        //we have received a response, therefore the score was submitted
        StopAllCoroutines();
        _txtOutputInfo.color = _neutralColour;
        _txtOutputInfo.text = "Score successfully submitted";
        //resultsHandler.SetScoreSubmitted(true);

        //once the server has received a response, shut down the client
        _client.Shutdown();

        StartCoroutine(LerpGameObjectY(gameObject, 1000.0f, 1.0f, true, 1.0f));
    }

    private IEnumerator LerpGameObjectY(GameObject c, float finalY, float speed, bool destroyObject, float waitTime)
    {
        float lerpTimer = 0.0f;
        Vector3 pos = c.transform.localPosition;

        //wait for a certain time before proceeding with the lerp
        float waitTimer = 0.0f;
        while (waitTimer < waitTime)
        {
            waitTimer += Time.deltaTime;
        }

        while (lerpTimer < speed)
        {
            lerpTimer += Time.deltaTime;

            pos = Vector3.Lerp(pos, new Vector3(pos.x, finalY, pos.z), lerpTimer / speed);
            c.transform.localPosition = pos;

            yield return null;
        }

        if (destroyObject)
        {
            Destroy(transform.parent.gameObject);
        }
            
    }

    private IEnumerator UpdateConnectingText(float speed)
    {
        _txtOutputInfo.color = _neutralColour;

        float timer = 0.0f;
        string suffix = "";

        while (true)
        {
            timer += Time.deltaTime;
            
            _txtOutputInfo.text = "Connecting" + suffix;

            if (timer > speed)
            {
                suffix += ".";
                if (suffix.Length >= 4)
                    suffix = "";

                timer = 0.0f;
            }


            yield return null;
        }
    }

    private void BtnSubmitScorePressed()
    {
        //prevent the user from submitting a score when the ip address box is blank
        if (_inputAddress.text == "")
        {
            StopAllCoroutines();
            _txtOutputInfo.color = _errorColour;
            _txtOutputInfo.text = "Field cannot be left blank.";
            return;
        }
        
        // Connect to the server
        _client.Connect (_inputAddress.text, _port);
        _txtOutputInfo.text = "";
        StartCoroutine(UpdateConnectingText(0.5f));
    }

    private void BtnCancelPressed()
    {
        StartCoroutine(LerpGameObjectY(gameObject, -1000.0f, 1.0f, true, 0.0f));

        _client.Shutdown();
    }

    private void Update()
    {
    }


}
