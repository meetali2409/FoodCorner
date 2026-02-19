using AutoMapper;
using FoodCorner.DTO;
using FoodCornerAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FoodItem, FoodDTO>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName));

        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryCreateDTO, Category>();

        CreateMap<FoodItemCreateDTO, FoodItem>();
        CreateMap<FoodItemUpdateDTO, FoodItem>();
        CreateMap<OrderCreateDTO, Order>()
         .ForMember(dest => dest.OrderItems,
             opt => opt.MapFrom(src => src.Items));

        CreateMap<OrderItemCreateDTO, OrderItem>();

        CreateMap<Order, OrderResponseDTO>()
            .ForMember(dest => dest.Items,
                opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<OrderItem, OrderItemResponseDTO>()
            .ForMember(dest => dest.FoodItemName,
                opt => opt.MapFrom(src => src.FoodItem.FoodItemName))
            .ForMember(dest => dest.TotalPrice,
                opt => opt.MapFrom(src => src.UnitPrice * src.Quantity));

    }
}
