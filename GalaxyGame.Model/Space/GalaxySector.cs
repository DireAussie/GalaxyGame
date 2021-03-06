using System;
using System.Collections.Generic;
using UnityEngine;

namespace GalaxyGame.Model.Space
{
    public class GalaxySector : Entity
    {
        public GalaxySector()
        {
            SolarSystems = new HashSet<SolarSystem>();
            SectorLinks = new HashSet<GalaxySectorLink>();
            Exploration = new Exploration();
        }
    
        public virtual string Name { get; set; }
        public virtual Galaxy Galaxy { get; set; }
        public virtual ICollection<SolarSystem> SolarSystems { get; set; }
        public virtual ICollection<GalaxySectorLink> SectorLinks { get; set; }
        public virtual Exploration Exploration { get; set; }
    }
}
