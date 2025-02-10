using AutoMapper;
using BackendShop.Core.Dto.Order;
using BackendShop.Data.Entities;
using BackendShop.Data.Enums;

namespace BackendShop.Core.MapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Mapping from Order entity to OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Items.Sum(item => item.Price * item.Quantity)))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address)); // Мапінг для адреси

            // Mapping from OrderDto to Order entity (for updating Order)
            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (OrderStatus)Enum.Parse(typeof(OrderStatus), src.Status)))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            // Mapping from OrderItem entity to OrderItemDto
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            // Mapping from CreateOrderDto to Order entity
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => OrderStatus.Pending)) // Default to Pending status
                .ForMember(dest => dest.DiscountId, opt => opt.MapFrom(src => src.DiscountId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address)); // Мапінг для адреси

            // Mapping from OrderItemDto to OrderItem entity
            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
        }
    }
}
