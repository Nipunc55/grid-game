using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSlider : MonoBehaviour
{
    public int totalPagesCount=6;
   public int currentPageNum=0;
    public GameObject[] pages;
    public void NextPage(){
        if( currentPageNum<(totalPagesCount-1)){
            currentPageNum++;
          ActivatePage(currentPageNum);

        }

    }
    public void PrePage(){
        if(currentPageNum >0 ){
            currentPageNum--;
            ActivatePage(currentPageNum);
          

        }

    }
    void ActivatePage(int number){
         foreach(GameObject page in pages) {
            page.SetActive(false);
            
           }
           pages[number].SetActive(true);

    }
}
