using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private HashSet<Camera> cameras;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            cameras = new HashSet<Camera>();
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void AddCamera(Camera camera)
    { 
        cameras.Add(camera);
    }

    public void RemoveCamera(Camera camera)
    { 
        cameras.Remove(camera);
    }

    public void SetActiveCamera(Camera camera)
    {
        foreach (Camera c in cameras)
        {
            c.enabled = false;

            AudioListener listener = c.GetComponent<AudioListener>();
            listener.enabled = false;
        }
        if (cameras.Contains(camera))
        {
            camera.enabled = true;
            AudioListener listener = camera.GetComponent<AudioListener>();
            listener.enabled = true;
        }
    }
}
