using Chatnik.ClientApplication.Core.Interfaces;

namespace Chatnik.ClientApplication.Core.Implementations
{
    public abstract class PresenterBase<T> : IPresenter
        where T : IView
    {
        protected T View { get; }
        
        public PresenterBase(T view)
        {
            View = view;
        }
        
        public abstract void OnFormLoaded();
    }
}