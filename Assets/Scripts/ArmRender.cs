using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRender : MonoBehaviour
{
    [SerializeField]
    private GameObject centerAnchor;
    
    [SerializeField]
    private GameObject debugSphereRight;

    [SerializeField]
    private GameObject debugSphereLeft;

    private LineRenderer lineRendererRight;
    private LineRenderer lineRendererLeft;


    // Start is called before the first frame update
    void Start()
    {
        lineRendererRight = debugSphereRight.GetComponent<LineRenderer>();
        lineRendererLeft = debugSphereLeft.GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posStartRight = centerAnchor.transform.position + 2 * Vector3.up + 2 * centerAnchor.transform.right;
        Vector3 posEndRight = debugSphereRight.transform.position;
        lineRendererRight.SetPosition(0, posStartRight);
        lineRendererRight.SetPosition(1, (posStartRight+posEndRight)/2 + 2*Vector3.up);
        lineRendererRight.SetPosition(2, (posStartRight + 5 * posEndRight) / 6 + Vector3.up);
        lineRendererRight.SetPosition(3, posEndRight);


        Vector3 posStartLeft = centerAnchor.transform.position + 2 * Vector3.up - 2 * centerAnchor.transform.right;
        Vector3 posEndLeft = debugSphereLeft.transform.position;
        lineRendererLeft.SetPosition(0, posStartLeft);
        lineRendererLeft.SetPosition(1, (posStartLeft + posEndLeft) / 2 + 2 * Vector3.up);
        lineRendererLeft.SetPosition(2, (posStartLeft + 5 * posEndLeft) / 6 + Vector3.up);
        lineRendererLeft.SetPosition(3, posEndLeft);

    }
}
