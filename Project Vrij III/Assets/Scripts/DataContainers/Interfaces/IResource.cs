using System;

namespace Game.Entities.Resources
{
    public interface IResource
    {
        int Current { get; }
        int Max { get; }

        bool IsEmpty { get; }
        bool IsFull { get; }

        event Action<int, int> Changed; // (current, max)
        event Action Emptied;
        event Action Filled;

        void Modify(int amount);
        void Set(int amount);
    }
}
