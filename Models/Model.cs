using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tessenger.Server.Models;

public class User

{

    [Key]
    [Column("id")]
    public long Id { get; set; }


    [Column("username")]
    [Required]
    [StringLength(150)]
    [DataType(DataType.Text)]
    public string Username { get; set; }

    [Column("password")]
    [Required]
    [StringLength(150)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}


public class CallRoom
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("adminusername")]
    public string AdminUsername { get; set; }

    [Column("roomid")]
    public string RoomID { get; set; }

    [Column("members")]
    public List<string> Members { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("createdtime")]
    public DateTime CreatedTime { get; set; }

    [Column("endtime")]
    public DateTime EndTime { get; set; }

    [Column("calltype")]
    public string CallType { get; set; }

}
public class Friend
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("username")]
    [Required]
    [StringLength(150)]
    [DataType(DataType.Text)]
    public string Username { get; set; }

    [Column("fusername")]
    public List<string> Fusername { get; set; }

    [Column("statusaccess")]

    public List<string> StatusAccess { get; set; }

    [Column("blocked")]
    public List<string> Blocked { get; set; }

}
public class Chat
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("username")]
    [Required]
    [StringLength(150)]
    [DataType(DataType.Text)]
    public string Username { get; set; }

    [Column("fusername")]
    [Required]
    [StringLength(150)]
    [DataType(DataType.Text)]
    public string Fusername { get; set; }

    [Column("sendusername")]
    [Required]
    [StringLength(150)]
    [DataType(DataType.Text)]
    public string Sendusername { get; set; }

    [Column("images")]
    [DataType(DataType.ImageUrl)]
    public List<string> Images { get; set; }

    [Column("documentfiles")]
    [DataType(DataType.Url)]
    public List<string> Documentfiles { get; set; }

    [Column("message")]
    [DataType(DataType.MultilineText)]
    [StringLength(int.MaxValue)]
    public string Message { get; set; }

    [Column("status")]
    [DataType(DataType.Text)]
    [StringLength(200)]
    public string Status { get; set; }

    [Column("time")]
    [DataType(dataType: DataType.DateTime)]
    public DateTime? Time { get; set; }

    [Column("reactemoji")]
    [DataType(dataType: DataType.Text)]
    public string ReactEmoji { get; set; }

    [Column("replymessid")]

    public string ReplyMessId { get; set; }

    [Column("voicerecoardfile")]
    public string VoicRecoardFile { get; set; }

}
public class Information
{
    [Key]
    [Column("username")]
    [DataType(DataType.Text)]
    [Required]
    public string Username { get; set; }
    [Column("name")]
    [DataType(DataType.Text)]

    public string Name { get; set; }
    [Column("highlighttext")]
    [DataType(DataType.MultilineText)]

    public string Highlighttext { get; set; }
    [Column("aboutme")]
    [DataType(DataType.MultilineText)]

    public string Aboutme { get; set; }
    [Column("contactnum")]
    [DataType(DataType.PhoneNumber)]

    public string Contactnum { get; set; }
    [Column("curredu")]
    [DataType(DataType.MultilineText)]

    public string CurrEdu { get; set; }
    [Column("address")]
    [DataType(DataType.MultilineText)]

    public string Address { get; set; }
    [Column("facebooklink")]
    [DataType(DataType.Text)]

    public string Facebooklink { get; set; }
    [Column("instragramlink")]
    [DataType(DataType.Text)]

    public string Instragramlink { get; set; }
    [Column("linkdinlink")]
    [DataType(DataType.Text)]

    public string Linkdinlink { get; set; }
    [Column("githublink")]
    [DataType(DataType.Text)]

    public string Githublink { get; set; }
    [Column("youtubelink")]
    [DataType(DataType.Text)]

    public string Youtubelink { get; set; }
    [Column("whatsappnum")]
    [DataType(DataType.PhoneNumber)]

    public string Whatsappnum { get; set; }
    [Column("tiktoklink")]
    [DataType(DataType.Text)]

    public string Tiktoklink { get; set; }
    [Column("redditlink")]
    [DataType(DataType.Text)]

    public string Redditlink { get; set; }
    [Column("snapchartlink")]
    [DataType(DataType.Text)]

    public string Snapchartlink { get; set; }
    [Column("twitterlink")]
    [DataType(DataType.Text)]

    public string Twitterlink { get; set; }
    [Column("pinterestlink")]
    [DataType(DataType.Text)]

    public string Pinterestlink { get; set; }
    [Column("websitelink")]
    [DataType(DataType.Url)]

    public string Websitelink { get; set; }
    [Column("website2link")]
    [DataType(DataType.Url)]

    public string Website2link { get; set; }
    [Column("website3link")]
    [DataType(DataType.Url)]

    public string Website3linlk { get; set; }
    [Column("nationality")]
    [DataType(DataType.Text)]

    public string Nationality { get; set; }
    [Column("isactive")]

    public bool Isactive { get; set; }
    [Column("profileimage")]
    [DataType(DataType.ImageUrl)]

    public string ProfileImage { get; set; }
    [Column("authentationemail")]
    [DataType(DataType.EmailAddress)]


    public string Authentation_Email { get; set; }
    [Column("authentationphonenumber")]
    [DataType(DataType.PhoneNumber)]

    public string Authentation_Phone_Number { get; set; }
    [Column("authentationauthenticatorapp")]
    [DataType(DataType.Text)]

    public string Authentation_Authenticator_App { get; set; }
    [Column("authentationsecurityquestions")]
    [DataType(DataType.Text)]

    public string Authentation_Security_Questions { get; set; }
    [Column("authentationsecuritykey")]
    [DataType(DataType.Text)]

    public string Authentation_Security_Key { get; set; }

    [Column("email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

}
public class Request
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("username")]
    public string Username { get; set; }
    [Column("getrequsteduser")]
    public List<string> GetRequsteduser { get; set; }
    [Column("sendrequsteduser")]
    public List<string> SendRequsteduser { get; set; }
    [Column("gettime")]
    public List<DateTime> GetTime { get; set; }
    [Column("sendtime")]
    public List<DateTime> SendTime { get; set; }
}
public class Group
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("username")]
    [DataType(DataType.Text)]
    [Required]
    public string Username { get; set; }

    [Column("members")]
    public List<string> Members { get; set; }

    [Column("admin")]
    public string Admin { get; set; }
    [Column("isadminfullaccess")]
    public bool isAdminFullAccess { get; set; }


}
public class GroupChat
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("groupusername")]
    [Required]
    [StringLength(150)]
    [DataType(DataType.Text)]
    public string GroupUsername { get; set; }

    [Column("sendusername")]
    [Required]
    [StringLength(150)]
    [DataType(DataType.Text)]
    public string Sendusername { get; set; }

    [Column("images")]
    [DataType(DataType.ImageUrl)]
    public List<string> Images { get; set; }

    [Column("documentfiles")]
    [DataType(DataType.Url)]
    public List<string> Documentfiles { get; set; }

    [Column("message")]
    [DataType(DataType.MultilineText)]
    [StringLength(int.MaxValue)]
    public string Message { get; set; }

    [Column("seensusername")]
    public List<string> SeensUsername { get; set; }

    [Column("status")]
    public string status { get; set; }

    [Column("time")]
    [DataType(dataType: DataType.DateTime)]
    public DateTime? Time { get; set; }

    [Column("reactemoji")]
    [DataType(dataType: DataType.Text)]
    public List<string> ReactEmoji { get; set; }

    [Column("reactemojiusername")]
    [DataType(dataType: DataType.Text)]
    public List<string> ReactEmojiUsername { get; set; }

    [Column("poloid")]
    public long PoloId { get; set; }

    [Column("replymessid")]
    public string ReplyMessId { get; set; }

    [Column("voicerecoardfile")]
    public string VoicRecoardFile { get; set; }
}


public class DocumentDn
{
    [Column("uniquename")]
    [DataType(DataType.Text)]
    public string Uniquename { get; set; }
    [Column("name")]
    [DataType(DataType.Text)]
    public string Name { get; set; }

    [Column("size")]
    [DataType(DataType.Text)]
    public string Size { get; set; }
    [Key]
    [Column("id")]
    public long Id { get; set; }

}



public class Polo
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("groupusername")]
    [DataType(DataType.Text)]
    [Required]
    public string GroupUsername { get; set; }

    [Column("senderusername")]
    [DataType(DataType.Text)]
    [Required]
    public string Sendername { get; set; }

    [Column("topics")]
    public List<string> Topics { get; set; }

    [Column("value")]
    public List<string> Values { get; set; }

    [Column("title")]
    [DataType(DataType.Text)]
    [Required]
    public string Title { get; set; }

    [Column("description")]
    [DataType(DataType.Text)]
    [Required]
    public string Description { get; set; }

    [Column("time")]
    [DataType(DataType.DateTime)]
    [Required]
    public DateTime Time { get; set; }

}


public class FriendListUpdateObject
{
    public string message { get; set; }

    public DateTime Time { get; set; }




}
public class GroupInformation
{
    [Key]
    [Column("id")]

    public int Id { get; set; }

    [Column("username")]
    [DataType(DataType.Text)]
    [Required]
    public string Username { get; set; }

    [Column("name")]
    public string Name { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("createDate")]
    [DataType(DataType.DateTime)]
    public DateTime CreateDate { get; set; }

    [Column("profileimage")]
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; }
}

public class GroupRequest
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("groupusername")]
    public string GroupUsername { get; set; }
    [Column("getrequstedgroupuser")]
    public List<string> GetRequstedGroupuser { get; set; }
    [Column("sendrequstedgroupuser")]
    public List<string> SendRequstedGroupuser { get; set; }
    [Column("gettime")]
    public List<DateTime> GetTime { get; set; }
    [Column("sendtime")]
    public List<DateTime> SendTime { get; set; }
}


public class GroupSendRequest
{


    public string GroupName { get; set; }
    public string GroupUsername { get; set; }
    public string ImageUrl { get; set; }

    public string Members { get; set; }
    public DateTime Time { get; set; }
    public string username { get; set; }


}
public class GroupGetRequest
{


    public string GroupName { get; set; }
    public string GroupUsername { get; set; }

    public string ImageUrl { get; set; }
    public DateTime Time { get; set; }
    public string username { get; set; }


}

public class UserRequest
{


    public string Name { get; set; }
    public string Username { get; set; }
    public string ImageUrl { get; set; }
    public DateTime Time { get; set; }



}



public class GroupAdminList
{
    public string GroupName { get; set; }
    public string GroupUsername { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
}

public partial class DocumentProperty 

{

    public string Uri { get; set; }

    public string Name { get; set; }

    public string Size { get; set; }

    public string Type { get; set; }

    public double UploadPreccess { get; set; }

}


public class ChatClientObject
{
    public long Id { get; set; }
    public string Senderusername { get; set; }
    public string SenderProfileImgae { get; set; }
    public string SenderName { get; set; }

    public List<string> Images { get; set; }

    public List<DocumentProperty> Documentfiles { get; set; }

    public string Message { get; set; }

    public string Status { get; set; }

    public DateTime? Time { get; set; }

    public string ReactEmoji { get; set; }

    public string VoicRecoardFile { get; set; }

    public bool ReplyHasFile { get; set; }
    public bool ReplyHasVoice { get; set; }
    public List<string> ReplyImages { get; set; }
    public string ReplyMessae { get; set; }
    public long ReplyId { get; set; }

}




