using UnityEngine;

public class MessageListener : MonoBehaviour
{
    public void OnMessageArrived(string message)
    {
        Debug.Log("Message from Arduino: " + message);
    }

    public void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Connected to Arduino!" : "Connection failed.");
    }
}
