using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StarGroupUtilities))]
public class StarGroupUtilitiesEditor : Editor
{    
    private List<GameObject> copy;

    public override void OnInspectorGUI() {        
        base.OnInspectorGUI();

        StarGroupUtilities starGroupUtils = (StarGroupUtilities)target;

        if(starGroupUtils.Focus != null)
            if(GUILayout.Button("Get Nearby Stars"))
                GetNearbyStars(starGroupUtils);

        if(starGroupUtils.StarGroup != null) {
            if(starGroupUtils.StarGroup.Count > 0) {
                if(GUILayout.Button("Create Star Group"))
                    CreateStarGroup(starGroupUtils);

                if(GUILayout.Button("Clear Star Group"))
                    starGroupUtils.ClearStarGroup();
            }
        }
    }

    private void GetNearbyStars(StarGroupUtilities sgUtils) {
        StarData[] stars = FindObjectsOfType<StarData>();
        Vector3 pos = sgUtils.Focus.transform.position;
        sgUtils.ClearStarGroup();

        for(int i = 0; i < stars.Length; i++) {
            float distance = Vector3.Distance(pos, stars[i].transform.position);
            if(distance <= sgUtils.NearbyStarsDistance) {
                sgUtils.AddToStarGroup(stars[i]);
            }
        }
    }

    private void CreateStarGroup(StarGroupUtilities sgUtils) {
        Vector3 averagePosition = Vector3.zero;
        int numberOfStars = 0;

        if(copy == null)
            copy = new List<GameObject>();

        copy.Clear();

        for(int i = 0; i < sgUtils.StarGroup.Count; i++) {
            GameObject go = sgUtils.StarGroup[i].gameObject;

            if(!copy.Contains(go))
                copy.Add(go);

            averagePosition += go.transform.position;
            numberOfStars++;
        }

        averagePosition /= numberOfStars;
        GameObject barycenter = new GameObject(sgUtils.StarGroupBarycenterName);
        barycenter.transform.position = averagePosition;

        for(int i = 0; i < copy.Count; i++) {
            Instantiate(copy[i], copy[i].transform.position, copy[i].transform.rotation, barycenter.transform);
        }
    }    
}