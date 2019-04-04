using System;

namespace Core
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(object id, Type entityType)
            : base($"Entity '{id}' of type '{entityType}' has not been found.")
        {
        }
    }
}
