using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepController : MonoBehaviour
{
    [SerializeField] int maxPage;
   [SerializeField] int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;

    public void Awake()
    {
        
        targetPos = levelPagesRect.localPosition;
    }
    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Previous()
    { 
        if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }
     void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }
}
