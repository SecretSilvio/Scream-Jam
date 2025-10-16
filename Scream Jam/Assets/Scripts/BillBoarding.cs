using UnityEngine;

public class BillBoarding : MonoBehaviour
{
    void Update()
    {
        Vector3 dirToCamera = Camera.main.transform.position - transform.position;
        dirToCamera.y = 0; // Optional: remove pitch rotation if you want upright billboard
        transform.rotation = Quaternion.LookRotation(dirToCamera);
    }
}