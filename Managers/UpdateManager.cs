
namespace Miedviediev_04.Managers
{
    internal class UpdateManager
    {
        private static readonly object Locker = new object();
        private static UpdateManager _instance;

        internal static UpdateManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (Locker)
                {
                    return _instance ?? (_instance = new UpdateManager());
                }
            }
        }

        private IUpdaterOwner _owner;
        public IUpdaterOwner Owner => _owner;

        private UpdateManager()
        {
        }

        internal void Initialize(IUpdaterOwner owner)
        {
            _owner = owner;
        }

        internal void StartUpdate()
        {
            _owner.Updater.StartUpdate();
        }

        internal void StopUpdate()
        {
            _owner.Updater.StopUpdate();
        }

    }
}