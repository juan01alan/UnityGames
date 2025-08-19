using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarData : MonoBehaviour
{
    #region // Variables and Fields

    [SerializeField] private string starName = "Star Name";
    public string StarName { get { return starName; } }

    public enum SpectralClass { O, B, A, F, G, K, M, WhiteDwarf, BrownDrawf, RoguePlanet }
    [SerializeField] private SpectralClass spectralClass = SpectralClass.M;
    public SpectralClass Class { get { return spectralClass; } }

    [SerializeField] private Color color = Color.red;
    public Color Color { get { return color; } }

    [SerializeField] private float visualLightOutput;
    public float VisualLightOutput { get { return visualLightOutput; } }

    [SerializeField] private float bolometricOutput;
    public float BolometricOutput { get { return bolometricOutput; } }

    [SerializeField] private float mass;
    public float Mass { get { return mass; } }

    [SerializeField] private MassMeasurementUnit massMeasurementUnits;
    public MassMeasurementUnit MassMeasurementUnits { get { return massMeasurementUnits; } }

    [SerializeField] private float radius;
    public float Radius { get { return radius; } }

    [SerializeField] private RadiusMeasurementUnit radiusMeasurementUnits;
    public RadiusMeasurementUnit RadiusMeasurementUnits { get { return radiusMeasurementUnits; } }

    [SerializeField] private float temperatureK;
    public float TemperatureK { get { return temperatureK; } }
    
    [SerializeField] private List<PlanetData> planets = null;
    public List<PlanetData> Planets { get { return planets; } }

    [SerializeField] private float searchNearbyRange = 8f;
    public float SearchNearbyRange { get { return searchNearbyRange; } }

    #endregion


}