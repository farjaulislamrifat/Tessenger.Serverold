using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Security.Cryptography;
using System.Text;
using Tessenger.Server.Controller;
using Tessenger.Server.Models;

namespace Tessenger.Server.Common
{
    public  class Common_Function
    {
        public readonly IConfiguration configuration;

        public Common_Function(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

       

        public static async Task<List<object>> ListRandomizeAsync(List<object> list)
        {

            int n = list.Count;
            Random random = new Random();
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                object temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }

            return list;

        }


        public  string Encrypt_Password(string Originalpass)
        {
            try
            {
                var key = configuration.GetValue<string>("encryptionKey");
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Convert.FromBase64String(key);
                    aes.IV = new byte[16];

                    using (MemoryStream memoryStream = new MemoryStream())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(Originalpass);
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

            }
            catch (Exception)
            {
                return null;
            }

        }

        public  string Decrypt_Password(string encrypted_password)
        {
            try
            {
                var key = configuration.GetValue<string>("encryptionKey");

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Convert.FromBase64String(key);
                    aes.IV = new byte[16];

                    using (MemoryStream memoryStream = new MemoryStream())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] cipherBytes = Convert.FromBase64String(encrypted_password);
                        cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static Information ArgInformation(string arg, Information information)
        {
            arg = arg.ToLower();
            Information result = new Information { Name = "", Email = "", Authentation_Email = "", Aboutme = "", Address = "", Authentation_Authenticator_App = "", Authentation_Phone_Number = "", Authentation_Security_Key = "", Authentation_Security_Questions = "", Contactnum = "", CurrEdu = "", Facebooklink = "", Githublink = "", Highlighttext = "", Instragramlink = "", Isactive = false, Linkdinlink = "", Nationality = "", Pinterestlink = "", ProfileImage = "", Redditlink = "", Snapchartlink = "", Tiktoklink = "", Twitterlink = "", Username = "", Website2link = "", Website3linlk = "", Websitelink = "", Whatsappnum = "", Youtubelink = "" };
            if (arg.Contains("username"))
            {
                result.Username = information.Username;
            }
            if (arg.Contains("email"))
            {
                result.Email = information.Email;
            }
            if (arg.Contains("authentationsecuritykey"))
            {
                result.Authentation_Security_Key = information.Authentation_Security_Key;
            }
            if (arg.Contains("authentationsecurityquestions"))
            {
                result.Authentation_Security_Questions = information.Authentation_Security_Questions;
            }
            if (arg.Contains("authentationauthenticatorapp"))
            {
                result.Authentation_Authenticator_App = information.Authentation_Authenticator_App;
            }
            if (arg.Contains("authentationphonenumber"))
            {
                result.Authentation_Phone_Number = information.Authentation_Phone_Number;
            }
            if (arg.Contains("authentationemail"))
            {
                result.Authentation_Email = information.Authentation_Email;
            }
            if (arg.Contains("profileimage"))
            {
                result.ProfileImage = information.ProfileImage;

            }
            if (arg.Contains("isactive"))
            {
                result.Isactive = information.Isactive;
            }
            if (arg.Contains("nationality"))
            {
                result.Nationality = information.Nationality;
            }
            if (arg.Contains("website3link"))
            {
                result.Website3linlk = information.Website3linlk;
            }
            if (arg.Contains("website2link"))
            {
                result.Website2link = information.Website2link;
            }
            if (arg.Contains("websitelink"))
            {
                result.Websitelink = information.Websitelink;
            }
            if (arg.Contains("pinterestlink"))
            {
                result.Pinterestlink = information.Pinterestlink;
            }
            if (arg.Contains("twitterlink"))
            {
                result.Twitterlink = information.Twitterlink;
            }
            if (arg.Contains("snapchartlink"))
            {
                result.Snapchartlink = information.Snapchartlink;
            }
            if (arg.Contains("redditlink"))
            {
                result.Redditlink = information.Redditlink;
            }
            if (arg.Contains("tiktoklink"))
            {
                result.Tiktoklink = information.Tiktoklink;
            }
            if (arg.Contains("whatsappnum"))
            {
                result.Whatsappnum = information.Whatsappnum;
            }
            if (arg.Contains("youtubelink"))
            {
                result.Youtubelink = information.Youtubelink;
            }
            if (arg.Contains("githublink"))
            {
                result.Githublink = information.Githublink;
            }
            if (arg.Contains("linkdinlink"))
            {
                result.Linkdinlink = information.Linkdinlink;
            }
            if (arg.Contains("instragramlink"))
            {
                result.Instragramlink = information.Instragramlink;
            }
            if (arg.Contains("facebooklink"))
            {
                result.Facebooklink = information.Facebooklink;
            }
            if (arg.Contains("address"))
            {
                result.Address = information.Address;
            }
            if (arg.Contains("curredu"))
            {
                result.CurrEdu = information.CurrEdu;
            }
            if (arg.Contains("name"))
            {
                result.Name = information.Name;
            }
            if (arg.Contains("highlighttext"))
            {
                result.Highlighttext = information.Highlighttext;
            }
            if (arg.Contains("aboutme"))
            {
                result.Aboutme = information.Aboutme;
            }
            if (arg.Contains("contactnum"))
            {
                result.Contactnum = information.Contactnum;
            }
            return result;
        }
        public static Group ArgGroup(string arg, Group group)
        {
            arg = arg.ToLower();
            Group result = new Group();
            if (arg.Contains("id"))
            {
                result.Id = group.Id;
            }
            if (arg.Contains("username"))
            {
                result.Username = group.Username;
            }
            if (arg.Contains("members"))
            {
                result.Members = group.Members;
            }
            if (arg.Contains("admin"))
            {
                result.Admin = group.Admin;
            }
            if (arg.Contains("isonlyadminfullsccess"))
            {
                result.isAdminFullAccess = group.isAdminFullAccess;
            }
            return result;
        }

        public static GroupInformation GroupInformationArg(string arg, GroupInformation groupInformation)
        {
            GroupInformation groupinformationnew = new GroupInformation();

            arg = arg.ToLower();

            if (arg.Contains("id"))
            {
                groupinformationnew.Id = groupInformation.Id;
            }

            if (arg.Contains("username"))
            {
                groupinformationnew.Username = groupInformation.Username;
            }
            if (arg.Contains("name"))
            {
                groupinformationnew.Name = groupInformation.Name;
            }

            if (arg.Contains("createDate"))
            {
                groupinformationnew.CreateDate = groupInformation.CreateDate;
            }
            if (arg.Contains("desctiption"))
            {
                groupinformationnew.Description = groupInformation.Description;
            }
            if (arg.Contains("imageurl"))
            {
                groupinformationnew.ImageUrl = groupInformation.ImageUrl;
            }

            return groupinformationnew;

        }
    }
}
