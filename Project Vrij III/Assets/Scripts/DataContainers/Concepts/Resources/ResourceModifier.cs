namespace Game.Entities.Resources
{
    public sealed class ResourceModifier
    {
        private readonly IResource m_Resource;
        private readonly float m_Rate;
        private float m_Accumulator;

        public ResourceModifier(IResource resource, float ratePerSecond)
        {
            m_Resource = resource;
            m_Rate = ratePerSecond;
        }

        public void Tick(float deltaTime)
        {
            m_Accumulator += m_Rate * deltaTime;
            var whole = (int)m_Accumulator;

            if (whole > 0)
            {
                m_Accumulator -= whole;
                m_Resource.Modify(whole);
            }
        }
    }
}