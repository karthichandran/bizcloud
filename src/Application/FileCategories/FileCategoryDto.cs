using AutoMapper;
using ReProServices.Application.Common.Mappings;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Application.FileCategories
{
    public class FileCategoryDto : IMapFrom<Domain.Entities.FileCategory>
    {
        [Key]
        public int FileCategoryId { get; set; }
        public string FileCategoryName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.FileCategory, FileCategoryDto>();
        }
    }
}
