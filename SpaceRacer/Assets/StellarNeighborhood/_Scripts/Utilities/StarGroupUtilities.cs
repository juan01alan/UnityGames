using System.Collections.Generic;
using UnityEngine;

public class StarGroupUtilities : MonoBehaviour
{
    [SerializeField] private StarData focus = null;
    public StarData Focus { get { return focus; } }

    [SerializeField] private float nearbyStarsDistance = 8f;
    public float NearbyStarsDistance { get { return Mathf.Abs(nearbyStarsDistance); } }

    [SerializeField] private string starGroupBarycenterName = "New Barycenter";
    public string StarGroupBarycenterName { get { return starGroupBarycenterName; } }

    [SerializeField] private List<StarData> starGroup = null;
    public List<StarData> StarGroup { get { return starGroup; } set { starGroup = value; } }   

    public void ClearStarGroup() {
        if(starGroup == null)
            starGroup = new List<StarData>();

        starGroup.Clear();
    }

    public void AddToStarGroup(StarData star) {
        if(starGroup == null)
            starGroup = new List<StarData>();

        if(!starGroup.Contains(star))
            starGroup.Add(star);
    }
}