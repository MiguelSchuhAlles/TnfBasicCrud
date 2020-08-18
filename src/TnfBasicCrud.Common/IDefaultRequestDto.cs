using System;
using Tnf.Dto;

namespace TnfBasicCrud.Common
{
    public interface IDefaultRequestDto : IRequestDto
    {
        Guid Id { get; set; }
    }
}
