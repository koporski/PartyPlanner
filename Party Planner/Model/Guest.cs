using Party_Planner.Enums;

namespace Party_Planner.Model
{
    public class Guest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public Response Response { get; set; }
        public bool Invited { get; set; }                
    }
}
