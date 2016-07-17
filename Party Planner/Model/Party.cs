using System.Collections.ObjectModel;

namespace Party_Planner.Model
{
    public class Party
    {
        public string Name { get; set; }
        public ObservableCollection<Guest> GuestList { get; set; }
    }
}
