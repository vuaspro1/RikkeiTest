using AutoMapper;
using RikkeiTest.Dto;
using RikkeiTest.Models;

namespace RikkeiTest.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserRoleDto, UserRole>();
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionDto, Permission>();
            CreateMap<RolePermission, RolePermissionDto>();
            CreateMap<RolePermissionDto, RolePermission>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}
