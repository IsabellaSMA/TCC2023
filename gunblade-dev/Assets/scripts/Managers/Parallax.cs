using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject CameraPlayer;

    private float sizeX, sizeY, startPosX, startPosY;
    public float parallaxXSpeed, parallaxYSpeed;

    private float previousX;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        sizeX = GetComponent<SpriteRenderer>().bounds.size.x;
        sizeY = GetComponent<SpriteRenderer>().bounds.size.y * 4;
    }

    void Update()
    {
        // x
        float tempX = (CameraPlayer.transform.position.x * (1 - parallaxXSpeed));
        float distX = (CameraPlayer.transform.position.x * parallaxXSpeed);

        transform.position = new Vector2(startPosX + distX, transform.position.y);

        if (tempX > startPosX + sizeX)
        {
            startPosX += sizeX / 2;
            //Debug.Log("Pulo");
        }
        else if (tempX < startPosX - sizeX)
        {
            startPosX -= sizeX / 2;
            //Debug.Log("Pulo Macaco");
        }

        // y

        float tempY = (CameraPlayer.transform.position.y * (1 - parallaxYSpeed));
        float distY = (CameraPlayer.transform.position.y * parallaxYSpeed);

        transform.position = new Vector2(transform.position.x, startPosY + distY / 2);

        if (tempY > startPosY + sizeY / 2)
        {
            startPosY += sizeY;
        }
        else if (tempY < startPosY - sizeY / 2)
        {
            startPosY -= sizeY; 
            
        }

        float currentX = transform.position.x;              
        float difference = Mathf.Abs(currentX - previousX);

        if (difference > 1)
        {
            //Debug.Log("salto grande distancia" + difference + " de" + currentX + " a " + previousX  );
        }

        previousX = currentX;
    }
}