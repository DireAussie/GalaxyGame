//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GalaxyGame.EntityFrameworkModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class SpaceObject
    {
        public System.Guid Id { get; set; }
        public System.Guid SystemId { get; set; }
        public System.Guid SubSystemId { get; set; }
        public string Discriminator { get; set; }
    
        public virtual SolarSystem SolarSystem { get; set; }
        public virtual SubSystem SubSystem { get; set; }
    }
}