using GalaSoft.MvvmLight.Views;

namespace Party_Planner.Interfaces
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
