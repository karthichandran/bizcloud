using System;
using AutoMapper;
using ReProServices.Application.Common.Mappings;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.CustomerPropertyFiles
{
    public class CustomerPropertyFileDto : IMapFrom<CustomerPropertyFile>
    {
        public int BlobID { get; set; }
        public Guid? OwnershipID { get; set; }
        public string FileName { get; set; }
        public byte[] FileBlob { get; set; }
        public DateTime? UploadTime { get; set; }

        public int? FileLength { get; set; }
        public string FileType { get; set; }
        public string PanID { get; set; }

        public int FileCategoryId { get; set; } = 4;
        public string GDfileID { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CustomerPropertyFile, CustomerPropertyFileDto>();
        }
    }
}
