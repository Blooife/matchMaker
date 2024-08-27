namespace Profile.Domain.Models;

public class Country : BaseModel<int>
{
    public string Name { get; set; }

    public List<City> Cities { get; set; } = new List<City>();
}