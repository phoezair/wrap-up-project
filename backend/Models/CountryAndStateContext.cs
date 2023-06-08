using Microsoft.EntityFrameworkCore;

namespace CountryApi.Models;

public class CountryAndStateContext : DbContext
{
    public CountryAndStateContext(DbContextOptions<CountryAndStateContext> options) : base(options)
    {

    }

    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<State> States { get; set; } = null!;
}