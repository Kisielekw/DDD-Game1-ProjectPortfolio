using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class DrawBox2D : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    LineRenderer lineRenderer;
    BoxCollider2D boxCollider2D;

    void Start()
    {
        lineRenderer = Instantiate(linePrefab).GetComponent<LineRenderer>();
        lineRenderer.transform.SetParent(transform);
        lineRenderer.transform.localPosition = Vector3.zero;
        boxCollider2D = GetComponent<BoxCollider2D>();
       
    }

    void Update()
    {
        HiliteBox();
    }

    void HiliteBox()
    {
        Vector3 offset = new Vector3(boxCollider2D.offset.x, boxCollider2D.offset.y, 0);

        Vector3[] positions = new Vector3[4];
        positions[0] = transform.TransformPoint( new Vector3(boxCollider2D.size.x / 2.0f, boxCollider2D.size.y / 2.0f,0) + offset);
        positions[1] = transform.TransformPoint(new Vector3(-boxCollider2D.size.x / 2.0f, boxCollider2D.size.y / 2.0f,0) + offset);
        positions[2] = transform.TransformPoint(new Vector3(-boxCollider2D.size.x / 2.0f, -boxCollider2D.size.y / 2.0f,0) + offset);
        positions[3] = transform.TransformPoint(new Vector3(boxCollider2D.size.x / 2.0f, -boxCollider2D.size.y / 2.0f,0) + offset);
        lineRenderer.SetPositions(positions);
    }
}