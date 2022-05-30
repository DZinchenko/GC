using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GC.Data.Objects;

namespace GC.Domain.Services.Repositories
{
    public interface IPathRepository
    {
        public Task<List<Path>> CreatePaths(List<Path> paths);
        public Task<List<PathPart>> CreatePathParts(List<PathPart> pathParts);
        public Task<Path> GetLastPathForDriver(int driverId);
        public Task<List<PathPart>> GetPathPartsForPath(int pathId);
    }
}
