using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public PlayerMovement playerMovement;
    private bool recovering = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isFallen || recovering)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, playerMovement.transform.rotation, Time.deltaTime * 3f);
            //orientation.rotation = Quaternion.Euler(playerMovement.transform.eulerAngles.x, playerMovement.transform.eulerAngles.y, playerMovement.transform.eulerAngles.z);
            return;
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public IEnumerator RecoveryCoroutine()
    {
        recovering = true;

        // Cache the current player rotation
        Quaternion currentRotation = transform.rotation;

        // Calculate what rotation the camera "should" be at based on mouse input
        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0);

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, elapsed / duration);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to final rotation just in case
        transform.rotation = targetRotation;
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        recovering = false;
    }
}
