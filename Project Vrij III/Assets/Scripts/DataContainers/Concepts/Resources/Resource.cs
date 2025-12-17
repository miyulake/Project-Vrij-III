using System;

namespace Game.Entities.Resources
{
    public sealed class Resource : IResource
    {
        public int Current { get; private set; }
        public int Max { get; }

        public bool IsEmpty => Current == 0;
        public bool IsFull => Current == Max;

        public event Action<int, int> Changed;
        public event Action Emptied;
        public event Action Filled;

        public Resource(int max, int current)
        {
            if (max <= 0) 
                throw new ArgumentOutOfRangeException(nameof(max), "Max value of resource has to be positive!");

            Max = max;
            Current = Math.Clamp(current, 0, max);
        }

        public void Modify(int amount) => Set(Current + amount);

        public void Set(int amount)
        {
            var previous = Current;
            Current = Math.Clamp(amount, 0, Max);

            if (Current == previous) return;

            Changed?.Invoke(Current, Max);
            if (Current == 0 && previous > 0) Emptied?.Invoke();
            if (Current == Max && previous < Max) Filled?.Invoke();
        }
    }
}