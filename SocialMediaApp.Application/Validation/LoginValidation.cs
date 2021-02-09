using FluentValidation;
using SocialMediaApp.Application.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaApp.Application.Services.Validation
{
    public class LoginValidation:AbstractValidator<LoginDTO>
    {
         //Kullanıcılardan bilgi almak amacıyla sayfamıza yerleştirdiğimiz kontrollerin boş bırakılması ya da istenenden farklı türde veri girilmesi bizim için problem yaratabilir.
        //Örneğin bir metin kutusunun boş bırakılması veya e-posta adresinin kurallara uygun girilmemesi gibi durumlara karşı önlem almamız gerekir.
        //Böyle durumlarda doğrulama kontrollerini kullanmak (validation controls) işimizi oldukça kolaylaştırır.
        //Bu kontroller bağlı oldukları elemanı kontrol ederek, istenen şart sağlanmadığında sayfanın sunucuya gönderilmesini engeller ve oluşan hata ile ilgili kullanıcıya mesaj verirler.
        public LoginValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Enter a username");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter a username");
        }
    }
}
