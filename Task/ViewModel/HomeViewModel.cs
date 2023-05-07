using Task.Models;

namespace Task.ViewModel
{
    public class HomeViewModel // For Model Binding process
    {
        public List<Slide> Slides { get; set; }
        public List<Service> Services { get; set; }     
    }
}
