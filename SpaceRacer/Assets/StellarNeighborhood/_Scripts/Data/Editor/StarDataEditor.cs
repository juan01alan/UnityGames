using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(StarData))]
public class StarDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(targets.Length > 1) {
            if (GUILayout.Button("Create Barycenter and Parent")) {
                Vector3 averagePosition = Vector3.zero;
                int numberOfStars = 0;

                for (int i = 0; i < targets.Length; i++) {
                    StarData sd = (StarData)targets[i];

                    sd.transform.parent = null;
                    averagePosition += sd.transform.position;
                    numberOfStars++;
                }

                averagePosition /= numberOfStars;

                GameObject barycenter = new GameObject("New Barycenter");
                barycenter.transform.position = averagePosition;

                for (int i = 0; i < targets.Length; i++) {
                    StarData sd = (StarData)targets[i];
                    sd.transform.parent = barycenter.transform;
                }
            }
        }
        else if (targets.Length == 1) {   
            if (GUILayout.Button("Get Nearby Stars and Parent")) {
                StarData[] stars = FindObjectsOfType<StarData>();
                List<StarData> nearbyStars = new List<StarData>();
                StarData sd = (StarData)target;
                Vector3 pos = sd.transform.position;

                for (int i = 0; i < stars.Length; i++) {
                    float distance = Vector3.Distance(pos, stars[i].transform.position);
                    if(distance <= sd.SearchNearbyRange) {
                        if(!nearbyStars.Contains(stars[i]))
                            nearbyStars.Add(stars[i]);
                    }
                }

                Vector3 averagePosition = Vector3.zero;
                int numberOfStars = 0;

                for (int i = 0; i < nearbyStars.Count; i++) {
                    nearbyStars[i].transform.parent = null;
                    averagePosition += nearbyStars[i].transform.position;
                    numberOfStars++;
                }

                if(numberOfStars == 0) {
                    Debug.Log("Nearby Stars is 0. This probably means distance to check is negative");
                    return;
                }

                averagePosition /= numberOfStars;

                GameObject barycenter = new GameObject("New Barycenter");
                barycenter.transform.position = averagePosition;

                for (int i = 0; i < nearbyStars.Count; i++) {
                    nearbyStars[i].transform.parent = barycenter.transform;
                }
            }
        }        

        base.OnInspectorGUI();                
    }
}