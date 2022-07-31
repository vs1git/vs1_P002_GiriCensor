namespace Vs1Plugin
{
    public interface SubComponentI
    {
        string componentType { get; }

        void Init();

        void Start();

        void Update();

        void FixedUpdate();

        void OnDestroy();
    }
}