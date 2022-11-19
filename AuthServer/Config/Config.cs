using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityServer4;

namespace AuthServer
{
    static public class Config
    {
        #region Scopes
        //API'larda kullanılacak izinleri tanımlar.
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("Garanti.Write","Garanti bankası yazma izni"),
                new ApiScope("Garanti.Read","Garanti bankası okuma izni"),
                new ApiScope("HalkBank.Write","HalkBank bankası yazma izni"),
                new ApiScope("HalkBank.Read","HalkBank bankası okuma izni"),
            };
        }
        #endregion
        #region Resources
        //API'lar tanımlanır.
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("Garanti"){
                    ApiSecrets = {
                        new Secret("garanti".Sha256())
                    },
                    Scopes = {
                        "Garanti.Write",
                        "Garanti.Read"
                    }
                },
                new ApiResource("HalkBank"){
                    ApiSecrets = {
                        new Secret("halkbank".Sha256())
                    },
                    Scopes = {
                        "HalkBank.Write",
                        "HalkBank.Read"
                    }
                }
            };
        }
        #endregion
        #region Clients
        //API'ları kullanacak client'lar tanımlanır.
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                        {//Muhasebe projesi için kullanıcak client 
                            ClientId = "GarantiBankasi",
                            ClientName = "GarantiBankasi",
                            RequireClientSecret = false,
                            AllowedGrantTypes = GrantTypes.Code,
                            AllowedScopes = { "Garanti.Write", "Garanti.Read" }
                        },
                new Client
                        { //Gencay Yıldız Örnek
                            ClientId = "HalkBankasi",
                            ClientName = "HalkBankasi",
                            ClientSecrets = { new Secret("halkbank".Sha256()) },
                            AllowedGrantTypes =  {GrantType.ClientCredentials},
                            AllowedScopes = { "HalkBank.Write", "HalkBank.Read" }
                            //AccessTokenLifetime =  2*60*60
                        },
                    new Client
                    {
                        ClientId = "OnlineBankamatik",
                        ClientName = "OnlineBankamatik",
                        ClientSecrets = { new Secret("onlinebankamatik".Sha256()) },
                        AllowedGrantTypes = IdentityServer4.Models.GrantTypes.Hybrid, //"code id_token'a karşılık gelen"
                        AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess,"Garanti.Write","Garanti.Read" },RedirectUris = { "https://localhost:4000/signin-oidc" },
                        RequirePkce = false,
                        AllowOfflineAccess = true,
                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                        RefreshTokenExpiration = TokenExpiration.Absolute,
                        AbsoluteRefreshTokenLifetime = 2 * 60 * 60 + (10 * 60),
                        RequireConsent = true
                    }
            };
        }
        #endregion
        public static IEnumerable<TestUser> GetTestUser()
        {
            return new List<TestUser>
            {
                new TestUser{SubjectId="test-user1",Username="test-user1",Password="12345",Claims={new Claim("name","test-user1"),new Claim("website","https:www.testuser1.com"),new Claim("gender","1")}},
                new TestUser{SubjectId="test-user2",Username="test-user2",Password="12345",Claims={new Claim("name","test-user2"),new Claim("website","https:www.testuser2.com"),new Claim("gender","0")}},

            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //üretilecek token içerisinde kesinlikle bir kullanıcı id/user id/subject id olmalıdır. OpenId kullanıcı id değerini ifade eder. Token'da "subid" olarka tutlacaktır
                new IdentityResources.Profile() //kullanıcı profil bilgilerini ve biryandan da kullanıcı için var olan tüm claimleri barındırır.
            };
        }

    }
}
