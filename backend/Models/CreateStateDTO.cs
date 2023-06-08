namespace CountryApi.Models;

public class CreateStateDTO
{
    public string Code { get; set; }
    public string Name { get; set; }
    public long CountryId { get; set; }
}
