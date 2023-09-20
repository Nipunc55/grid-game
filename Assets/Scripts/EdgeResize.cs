using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeResize : MonoBehaviour
{
     private Vector3 initialScaleChild;
     private Vector3 initialScaleParent;
     public GameObject parent;
        float scaleFactorY;
        float scaleFactorX;
        public bool xAxisScale=false;
        public bool yAxisScale=false;
     private void Start()
    {
        // Store the initial local scale of the child image.
        initialScaleChild = transform.localScale;
        initialScaleParent=parent.transform.localScale;
        //  scaleFactor = initialScaleChild.y/initialScaleParent.y ;
    }

    private void Update()
    {
        

        // Keep the child image's Y scale constant.
        scaleFactorY =parent.transform.localScale.y/ initialScaleParent.y;
        scaleFactorX =parent.transform.localScale.x/ initialScaleParent.x;
         
       /* if(xAxisScale) {
            transform.localScale = new Vector3(initialScaleChild.x/scaleFactorX, initialScaleChild.y/scaleFactorY, transform.localScale.z);
        }else{
              transform.localScale = new Vector3(transform.localScale.x, initialScaleChild.y/scaleFactorY, transform.localScale.z);

        }*/

        float xscale = transform.localScale.x;
        float yscale = transform.localScale.y;

        if (xAxisScale)
        {
            xscale = initialScaleChild.x / scaleFactorX;
        }

        if (yAxisScale)
        {
            yscale = initialScaleChild.y / scaleFactorY;
        }


        transform.localScale = new Vector3(xscale, yscale, transform.localScale.z);

    }

}
