using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


[CustomEditor(typeof(ParentObject))]
public class EditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Cast the target object to ParentObject
        ParentObject parentObject = (ParentObject)target;
         int intValue = parentObject.startNumber;

        // Check if any child objects should be edited
        if (GUILayout.Button("Edit Child Text Fields"))
        {
            int i=intValue;
           
             // Loop through child objects
            foreach (Transform child in parentObject.transform)
            {
                // RemoveStars(child);
                ButtonInteractable(child);
            

                //  child.name="Level "+i;
                //  // Check if the first child object is a button
                //  Transform firstChild = child.transform.GetChild(0);
                //  Debug.Log(firstChild.name);
                //  if (firstChild != null) {
                //               Debug.Log("renaming");
                //       // Get the Text component within the button
                //          Text textComponent = firstChild.GetComponentInChildren<Text>();

                //       if (textComponent != null)
                //      {
                //              // Open a text field for editing
                //              textComponent.text = i.ToString();
                //      }
                //     }
                // i++;

            }
        }
    }

    void RemoveStars(Transform child){
        for(int i = 1; i < 4; i++) {
            child.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
             child.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
            
        }
      var children=  child.transform.GetChild(0);

    }
    void ButtonInteractable(Transform child){
        Transform button = child.transform.GetChild(0);
        button.GetComponent<Ricimi.AnimatedButton>().interactable=false;
    }
}
