using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleEdge : MonoBehaviour
{
   public float scaleAmount = 2.0f; // Adjust this as needed

    private Vector3 originalChildLocalPosition;

    private void Start()
    {
        // Store the original local position of the child
        originalChildLocalPosition = transform.GetChild(0).localPosition;
    }

    private void Update()
    {
        // Apply scaling to the parent GameObject
        transform.localScale = new Vector3(scaleAmount, scaleAmount, 1);

        // Calculate the new local position for the child based on scaling
        Vector3 newChildLocalPosition = originalChildLocalPosition * (1 / scaleAmount);

        // Apply the new local position to the child
        transform.GetChild(0).localPosition = newChildLocalPosition;
    }
}
