namespace ChinookAPI; 

public class MapperConfiguration : Profile {
    public MapperConfiguration() {
        // Albums - READ
        CreateMap<Album, AlbumsReadDto>()
            .ForMember(x => x.Artist, y => y.MapFrom(x => x.Artist.Name))
            .ForMember(x => x.Genre, y => y.MapFrom(x => x.Tracks.First().Genre.Name))
            .ReverseMap();

        CreateMap<Album, AlbumReadDto>()
            .ForMember(x => x.Artist, y => y.MapFrom(x => x.Artist.Name))
            .ForMember(x => x.Genre, y => y.MapFrom(x => x.Tracks.First().Genre.Name))
            .ForMember(x => x.Price, y => y.MapFrom(
                x => $"${Math.Round(x.Tracks.Sum(x => x.UnitPrice), 2)}"
            ))
            .ForMember(x => x.Duration, y => y.MapFrom(
                x => Utilities.ConvertMillisecondsToTime(x.Tracks.Sum(x => x.Milliseconds))
            ))
            .ForMember(x => x.FileSize, y => y.MapFrom(
                x => $"{Utilities.ConvertBytesToMegabytes(x.Tracks.Sum(x => (long)x.Bytes))} MB"
            ))
            .ForMember(x => x.FileFormat, y => y.MapFrom(x => x.Tracks.First().MediaType.Name))
            .ForMember(x => x.Tracklist, y => y.MapFrom(x => x.Tracks.Select(
                x => $"{x.Name} ({Utilities.ConvertMillisecondsToTime(x.Milliseconds)})")
            ))
            .ReverseMap();

        // Artists - READ
        CreateMap<Artist, ArtistsReadDto>().ReverseMap();
        CreateMap<Artist, ArtistReadDto>()
            .ForMember(x => x.Albums, y => y.MapFrom(x => x.Albums.Select(x => x.Title)))
            .ReverseMap();

        // Employees - READ
        CreateMap<Employee, EmployeesReadDto>()
            .ForMember(x => x.Name, y => y.MapFrom(x => $"{x.FirstName} {x.LastName}"))
            .ReverseMap();

        // Genres - READ
        CreateMap<Genre, GenreReadDto>().ReverseMap();

        // MediaTypes - READ
        CreateMap<MediaType, MediaTypeReadDto>().ReverseMap();

        // Tracks - READ
        CreateMap<Track, TracksReadDto>()
            .ForMember(x => x.Artist, y => y.MapFrom(x => x.Album.Artist.Name))
            .ForMember(x => x.Genre, y => y.MapFrom(x => x.Genre.Name))
            .ReverseMap();

        CreateMap<Track, TrackReadDto>()
            .ForMember(x => x.Artist, y => y.MapFrom(x => x.Album.Artist.Name))
            .ForMember(x => x.Genre, y => y.MapFrom(x => x.Genre.Name))
            .ForMember(x => x.Price, y => y.MapFrom(x => $"${Math.Round(x.UnitPrice, 2)}"))
            .ForMember(x => x.Duration, y => y.MapFrom(x => Utilities.ConvertMillisecondsToTime(x.Milliseconds)))
            .ForMember(x => x.FileSize, y => y.MapFrom(x => $"{Utilities.ConvertBytesToMegabytes((long)x.Bytes)} MB"))
            .ForMember(x => x.FileFormat, y => y.MapFrom(x => x.MediaType.Name))
            .ReverseMap();

        // Playlists - READ
        CreateMap<Playlist, PlaylistReadDto>().ReverseMap();

        CreateMap<Playlist, PlaylistReadDto>()
            .ForMember(x => x.Tracklist, y => y.MapFrom(x => x.Tracks.Select(
                x => $"{x.Album.Artist.Name} - {x.Name}")
            ))
            .ReverseMap();

        // Customers - CREATE
        CreateMap<CustomerCreateDto, Customer>().ReverseMap();

        // Customers - READ
        CreateMap<Customer, CustomersReadDto>()
            .ForMember(x => x.Name, y => y.MapFrom(x => $"{x.FirstName} {x.LastName}"))
            .ForMember(x => x.Location, y => y.MapFrom(x => $"{x.Country}, {x.City}"))
            .ReverseMap();

        CreateMap<Customer, CustomerInvoicesReadDto>()
            .ForMember(x => x.Invoices, y => y.MapFrom(x => x.Invoices.Select(x => x)))
            .ReverseMap();

        // Customers - UPDATE
        CreateMap<CustomerUpdateDto, Customer>().ReverseMap();

        // Invoices - CREATE
        CreateMap<InvoiceCreateDto, Invoice>().ReverseMap();
        CreateMap<Customer, Invoice>()
            .ForMember(x => x.BillingAddress, y => y.MapFrom(x => x.Address))
            .ForMember(x => x.BillingCity, y => y.MapFrom(x => x.City))
            .ForMember(x => x.BillingState, y => y.MapFrom(x => x.State))
            .ForMember(x => x.BillingCountry, y => y.MapFrom(x => x.Country))
            .ForMember(x => x.BillingPostalCode, y => y.MapFrom(x => x.PostalCode))
            .ReverseMap();

        // Invoices - READ
        CreateMap<Invoice, InvoiceReadDto>()
            .ForMember(x => x.Date, y => y.MapFrom(x => x.InvoiceDate.ToString("yyyy-MM-dd")))
            .ForMember(x => x.TotalPrice, y => y.MapFrom(x => $"${x.Total}"))
            .ForMember(x => x.Orderlist, y => y.MapFrom(
                x => x.InvoiceLines.Select(x => $"[{x.Quantity}x] [${x.UnitPrice}] {x.Track.Name}")
             ))
            .ReverseMap();

        // InvoiceLines - CREATE
        CreateMap<InvoiceLineCreateDto, InvoiceLine>().ReverseMap();
    }

}
