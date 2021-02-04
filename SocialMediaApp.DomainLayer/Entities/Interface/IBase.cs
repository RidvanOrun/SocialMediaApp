using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.DomainLayer.Entities.Interface
{
    public interface IBase<T> { T Id { get; set; } }
}
