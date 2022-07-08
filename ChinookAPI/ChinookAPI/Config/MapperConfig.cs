namespace ChinookAPI; 

public class MapperConfig : Profile {
    public MapperConfig() {
        CreateMap<ArtistCreateDto, Artist>().ReverseMap();
        CreateMap<ArtistReadDto, Artist>().ReverseMap();
        CreateMap<ArtistUpdateDto, Artist>().ReverseMap();
    }
}
