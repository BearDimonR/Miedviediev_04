
using Miedviediev_04.Models;

namespace Miedviediev_04.Managers
{
    internal class UpdateManager<T>
    {
        private static readonly object Locker = new object();
        private static UpdateManager<T> _instance;

        internal static UpdateManager<T> Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (Locker)
                {
                    return _instance ?? (_instance = new UpdateManager<T>());
                }
            }
        }

        private IUpdaterOwner<T> _owner;
        public IUpdaterOwner<T> Owner => _owner;

        private UpdateManager()
        {
        }

        internal void Initialize(IUpdaterOwner<T> owner)
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