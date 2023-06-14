using UnityEngine;

public class SetTargetFramerate : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}