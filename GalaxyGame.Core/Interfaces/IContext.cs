﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GalaxyGame.Core.Interfaces
{
    public interface IContext : IDisposable
    {
        void BeginTransaction();
        void Commit();
        IDbSet<T> DbSet<T>() where T : class, IEntity;
        void Save();
    }
}