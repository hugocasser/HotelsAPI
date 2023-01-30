using HotelsWebAPI.Auth;
namespace HotelsWebAPI.APIs;

internal class HotelApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/hotels", Get)
            .Produces<List<Hotel>>(StatusCodes.Status200OK)
            .WithName("GetAllHotels")
            .WithTags("Getters");
        app.MapGet("/hotels/search/id/{id}", GetById)
            .Produces<Hotel>(StatusCodes.Status200OK)
            .WithName("GetAllHotel")
            .WithTags("Getters");
        app.MapGet("/hotels/search/name/{query}", GetByName)
            .Produces<List<Hotel>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("SearchHotels")
            .WithTags("Getters")
            .ExcludeFromDescription();
        app.MapGet("/hotels/search/location/{coordinate}", GetByLocation)
            .ExcludeFromDescription();
        app.MapPost("/hotels", InsertHotel)
            .Accepts<Hotel>("application/json")
            .Produces<List<Hotel>>(StatusCodes.Status201Created)
            .WithName("CreateHotel")
            .WithTags("Creators");
        app.MapPut("/hotels", UpdateHotel)
            .Accepts<Hotel>("application/json")
            .WithName("UpdateHotel")
            .WithTags("Updaters");
        ;
        app.MapDelete("/hotels/{id}", DeleteHotel)
            .WithName("DeleteHotel")
            .WithTags("Deleters");
    }

    [Authorize]
    private async Task<IResult> Get(IHotelRepository repository) =>
        Results.Extensions.Xml(await repository.GetHotelsAsync());

    [Authorize]
    private async Task<IResult> GetById(int id, IHotelRepository repository) =>
        await repository.GetHotelAsync(id) is Hotel hotel
            ? Results.Ok(hotel)
            : Results.NotFound();

    [Authorize]
    private async Task<IResult> GetByName(string query, IHotelRepository repository) =>
        await repository.GetHotelsAsync(query) is IEnumerable<Hotel> hotels
            ? Results.Ok(hotels)
            : Results.NotFound(Array.Empty<Hotel>());

    [Authorize]
    private async Task<IResult> GetByLocation(Coordinate coordinate, IHotelRepository repository) =>
        await repository.GetHotelsAsync(coordinate) is IEnumerable<Hotel> hotels
            ? Results.Ok(hotels)
            : Results.NotFound(Array.Empty<Hotel>());

    [Authorize]
    private async Task<IResult> InsertHotel([FromBody] Hotel hotel, IHotelRepository repository)
    {
        await repository.InsertHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.Created($"/hotels/{hotel.Id}", hotel);
    }

    [Authorize]
    private async Task<IResult> UpdateHotel([FromBody] Hotel hotel, IHotelRepository repository)
    {
        await repository.UpdateHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.NoContent();
    }
    [Authorize] 
    private async Task<IResult> DeleteHotel(int id, IHotelRepository repository)
    {
        await repository.DeleteHotelAsync(id);
        await repository.SaveAsync();
        return Results.NoContent();
    }
}  