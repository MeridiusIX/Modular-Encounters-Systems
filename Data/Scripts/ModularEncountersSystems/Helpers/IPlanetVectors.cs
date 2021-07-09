using ModularEncountersSystems.Entities;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace ModularEncountersSystems.Helpers {
    public interface IPlanetVectors {

        MyPlanet GetNearestPlanet(Vector3D coords);
        Vector3D GetSurfaceCoords(PlanetEntity planet, Vector3D coords);
        bool IsPositionUnderground(PlanetEntity planet, Vector3D coords);
        bool IsPositionUnderwater(PlanetEntity planet, Vector3D coords);
        bool AltitudeAtPosition(PlanetEntity planet, Vector3D coords);
        bool DistanceToPlanetCore(PlanetEntity planet, Vector3D coords);

    }

}
