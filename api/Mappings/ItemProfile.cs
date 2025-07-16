using APIPractice.DTOs;
using APIPractice.Models;
using AutoMapper;

namespace APIPractice.Mappings
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<CreateItemDto, Item>();
        }
    }
}
