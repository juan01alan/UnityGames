using UnityEngine;

[CreateAssetMenu(menuName = "Planet Data")]
public class PlanetData : ScriptableObject
{
    public string PlanetName;
    public enum PlanetType { Unknown, Jovian, Neptunian, Terrestrial }
    public float Mass;
    public MassMeasurementUnit MassMeasurementUnits;
    public float Radius;
    public RadiusMeasurementUnit RadiusMeasurementUnits;
    public float OrbitDistanceAU;
    public float OrbitPeriodYears;
    public float OrbitEccentricity; 
}