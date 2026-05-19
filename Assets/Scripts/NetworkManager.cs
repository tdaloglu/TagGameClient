using UnityEngine;
using NativeWebSocket;
using System.Text;
using TMPro;
using System.Data;

[System.Serializable]
public class ClientMessage
{
    public string type;
    public string username;
    public string roomId;
}

[System.Serializable]
public class ServerMessage
{
    public string type;
    public string message;
    public string roomId;
    public string username;
}

public class NetworkManager : MonoBehaviour
{
    WebSocket websocket;

    [Header("UI Elements")]
    public TMP_InputField usernameInput;
    public TMP_InputField roomCodeInput;
    public TextMeshProUGUI statusText;

    async void Start() {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connected to the server!");
            UpdateStatus("The server is connected. Please enter your username and create or join a room.");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string jsonString = Encoding.UTF8.GetString(bytes);

            ServerMessage response = JsonUtility.FromJson<ServerMessage>(jsonString);
            if (response.type == "room_created" || response.type == "room_joined")
            {
                UpdateStatus($"Successful! The Room Code: {response.roomId}\n{response.message}");
            } else if (response.type == "player_joined")
            {
                UpdateStatus($"{response.username} has joined the room!");
            } else if (response.type == "error")
            {
                UpdateStatus($"Error: {response.message}");
            }
        };

        await websocket.Connect();
    }

    void Update()
    {
        #if !UNITY_WEBL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
        #endif
    }

    public async void CreateRoom()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        {
            UpdateStatus("Please enter a username!");
            return;
        }

        ClientMessage msg = new ClientMessage
        {
            type = "create_room",
            username = usernameInput.text
        };

        await websocket.SendText(JsonUtility.ToJson(msg));
    }

    public async void JoinRoom()
    {
        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(roomCodeInput.text))
        {
            UpdateStatus("Please enter a username and a room code!");
            return;
        }

        ClientMessage msg = new ClientMessage
        {
            type = "join_room",
            username = usernameInput.text,
            roomId = roomCodeInput.text
        };
        await websocket.SendText(JsonUtility.ToJson(msg));
    }

    private void UpdateStatus(string message)
    {
        if (statusText != null) statusText.text = message;
        Debug.Log(message);
    }

    private async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }
}
