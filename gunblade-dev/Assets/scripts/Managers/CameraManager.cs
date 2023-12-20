using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //TelaMechendo
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float duration = 1f;

    public bool start = false;

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            StartCoroutine(Shaking());
        }
        
    }
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elepsedTime = 0f;

        while (elepsedTime < duration) 
        {
            elepsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elepsedTime / duration);
            
            transform.position = startPosition + Random.insideUnitSphere*strength;
            yield return null;
        }
        
        transform.position = startPosition;    
    }
}
