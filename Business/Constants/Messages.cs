using Core.Entities.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi.";
        public static string ProductNameInvalid = "Ürün ismi geçersiz.";
        public static string ProductsListed = "Ürünler listelendi.";
        public static string MaintenanceTime = "Sistem bakımda.";
        public static string UnitPriceInvalid = "Ürün fiyatı geçersiz.";
        public static string ProductCountOfCategoryError = "Bir kategoride en fazla 50 ürün olabilir.";
        public static string ProductNameAlreadyExists = "Bu isimde bir ürün zaten mevcut.";
        public static string CategoryLimitExceeded = "Kategori limiti aşıldı.";
        public static string AuthorizationDenied = "Bu işlem için yetkiniz yok.";
        public static string UserRegistered = "Kullanıcı oluşturuldu.";
        public static string UserNotFound = "Kullanıcı bulunamadı.";
        public static string PasswordError = "Parola hatalı.";
        public static string SuccessfulLogin = "Giriş başarılı.";
        public static string UserAlreadyExists = "Bu mail adresiyle bir kullanıcı zaten kayıtlı.";
        public static string AccessTokenCreated = "Token oluşturuldu.";
        public static string ProductUpdated = "Ürün güncellendi.";
    }
} 
