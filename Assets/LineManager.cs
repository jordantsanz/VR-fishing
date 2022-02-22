using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public Transform[] linePos;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = linePos.Length;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i < linePos.Length; i++)
        {
            lineRenderer.SetPosition(i, linePos[i].position);
        } 
    }
}
