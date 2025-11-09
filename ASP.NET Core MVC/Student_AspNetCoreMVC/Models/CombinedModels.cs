namespace Student_AspNetCoreMVC.Models
{
    public class CombinedModels
    {
        public Course? Course { get; set; } = null!;
        public IEnumerable<Teacher>? Teachers { get; set; } = null!;
    }
}
