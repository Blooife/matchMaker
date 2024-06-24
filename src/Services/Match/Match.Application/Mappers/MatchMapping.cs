using Match.Application.DTOs.Match.Response;
using Match.Domain.Models;

namespace Match.Application.Mappers;

public class MatchMapping : AutoMapper.Profile
{
    public MatchMapping()
    {
        CreateMap<MatchEntity, MatchResponseDto>();
    }
}