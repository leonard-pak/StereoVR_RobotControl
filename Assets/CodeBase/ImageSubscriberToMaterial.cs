using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using CompressedImage = RosMessageTypes.Sensor.CompressedImageMsg;

public class ImageSubscriberToMaterial : MonoBehaviour
{
    public string Topic;
    public Material Material;
    public string ShaderProperty = "_MainTex";

    private Texture2D texture2D;
    private byte[] imageData;
    private bool isMessageReceived;

    private void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<CompressedImage>(Topic, ReceiveMessage);

        texture2D = new Texture2D(1, 1);
    }
    private void Update()
    {
        if (isMessageReceived)
            ProcessMessage();
    }

    private void ReceiveMessage(CompressedImage message)
    {
        imageData = message.data;
        isMessageReceived = true;
    }

    private void ProcessMessage()
    {
        texture2D.LoadImage(imageData);
        texture2D.Apply();
        Material.SetTexture(ShaderProperty, texture2D);
        isMessageReceived = false;
    }

}


