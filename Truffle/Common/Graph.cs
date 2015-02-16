using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schloss.Data.Neo4j;
using Newtonsoft;

// Wishlu
using System.Net;
using Newtonsoft.Json;
using Schloss.Data.Neo4j.Cypher;

namespace Truffle.Common
{
    public static class Graph
    {
        private static GraphClient client;
        
        public static GraphClient Instance
        {
            get 
            {
                if (client == null)
                    Initialize();

                return client; 
            }
        }

        static Graph()
        {
            Initialize();            
        }

        static void Initialize()
        {
            try
            {
                Logger.Log("Initializing graph database...");

                ServicePointManager.Expect100Continue = false;
                ServicePointManager.DefaultConnectionLimit = 200;

                HttpClientWrapper clienthttp = new HttpClientWrapper();

                //client = new GraphClient(new Uri("http://dev.wishlu.com:7474/db/data"), clienthttp);
                //client = new GraphClient(new Uri("http://54.86.249.215:7474/db/data"), clienthttp);
                client = new GraphClient(new Uri("http://10.0.0.30:7474/db/data"), clienthttp);
                client.Connect();

                Logger.Log("Connected to graph database!");                                
            }
            catch
            {
                client = null;                
            }
        }

        public static List<WishLuUser> GetUsers()
        {
            return Instance.Cypher
                .Match("(user:WishLuUser)")
                .Where((WishLuUser user) => user.ShipAddress1 != "")
                .Return(user => user.As<WishLuUser>())
                .Results.ToList();
        }

        public static WishLuUser GetUserById(Guid id)
        {
            return Instance.Cypher
                .Match("(user:WishLuUser)")
                .Where((WishLuUser user) => user.Id == id)
                .Return(user => user.As<WishLuUser>())
                .Results.Single();
        }
    }

    public class WishLuUser
    {
        [JsonProperty("Id")]
        public        Guid                   Id                    { get; set; }

        [JsonProperty("FirstName")]
        public String FirstName { get; set; }

        [JsonProperty("LastName")]
        public String LastName { get; set; }

        [JsonProperty("LoginId")]
        public String LoginId { get; set; }

        [JsonProperty("PasswordHash")]
        public Byte[] PasswordHash { get; set; }

        [JsonProperty("PasswordSalt")]
        public Byte[] PasswordSalt { get; set; }

        [JsonProperty("ConnectionIds")]
        public HashSet<string> ConnectionIds { get; set; }

        [JsonProperty("LanguageId")]
        public String LanguageId { get; set; }

        [JsonProperty("DateOfBirth")]
        public DateTimeOffset? DateOfBirth { get; set; }

        [JsonProperty("Address1")]
        public String Address1 { get; set; }

        [JsonProperty("Address2")]
        public String Address2 { get; set; }

        [JsonProperty("City")]
        public String City { get; set; }

        [JsonProperty("StateOrProvince")]
        public String StateOrProvince { get; set; }

        [JsonProperty("ZipOrPostalCode")]
        public String ZipOrPostalCode { get; set; }

        [JsonProperty("CountryId")]
        public String CountryId { get; set; }

        [JsonProperty("ShipAddress1")]
        public String ShipAddress1 { get; set; }

        [JsonProperty("ShipAddress2")]
        public String ShipAddress2 { get; set; }

        [JsonProperty("ShipCity")]
        public String ShipCity { get; set; }

        [JsonProperty("ShipStateOrProvince")]
        public String ShipStateOrProvince { get; set; }

        [JsonProperty("ShipZipOrPostalCode")]
        public String ShipZipOrPostalCode { get; set; }

        [JsonProperty("ShipCountryId")]
        public String ShipCountryId { get; set; }

        [JsonProperty("PhoneNumber")]
        public String PhoneNumber { get; set; }

        [JsonProperty("Email")]
        public String Email { get; set; }

        [JsonProperty("Headline")]
        public String Headline { get; set; }

        [JsonProperty("Website")]
        public String Website { get; set; }

        [JsonProperty("BlogURL")]
        public String BlogURL { get; set; }

        [JsonProperty("FacebookPageId")]
        public String FacebookPageId { get; set; }

        [JsonProperty("FacebookAccessToken")]
        public String FacebookAccessToken { get; set; }

        [JsonProperty("TwitterUserId")]
        public String TwitterUserId { get; set; }

        [JsonProperty("TwitterAccessToken")]
        public String TwitterAccessToken { get; set; }

        [JsonProperty("TwitterSecretToken")]
        public String TwitterSecretToken { get; set; }

        [JsonProperty("GooglePlusId")]
        public String GooglePlusId { get; set; }

        [JsonProperty("GooglePlusAccessToken")]
        public String GooglePlusAccessToken { get; set; }

        [JsonProperty("GooglePlusRefreshToken")]
        public String GooglePlusRefreshToken { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("IsAdminUser")]
        public bool IsAdminUser { get; set; }

        [JsonProperty("ImageFileName")]
        public String ImageFileName { get; set; }

        [JsonProperty("Gender")]
        public Char? Gender { get; set; }

        [JsonProperty("DeviceId")]
        public String DeviceId { get; set; }

        [JsonProperty("SessionId")]
        public Guid? SessionId { get; set; }

        [JsonProperty("FloatingWishLuId")]
        public Guid FloatingWishLuId { get; set; }

        [JsonProperty("BirthdayWishLuId")]
        public Guid BirthdayWishLuId { get; set; }

        [JsonProperty("SessionExpirationTime")]
        public DateTimeOffset? SessionExpirationTime { get; set; }

        [JsonProperty("LoginCount")]
        public int LoginCount { get; set; }

        [JsonProperty("TutorialMode")]
        public bool TutorialMode { get; set; }

        [JsonProperty("TutorialStep")]
        public int TutorialStep { get; set; }

        // Verification
        [JsonProperty("VerificationCode")]
        public Guid VerificationCode { get; set; }

        [JsonProperty("Verified")]
        public bool Verified { get; set; }

        // Password Reset
        [JsonProperty("InReset")]
        public bool InReset { get; set; }

        [JsonProperty("ResetCode")]
        public Guid ResetCode { get; set; }

        [JsonProperty("ResetExpiration")]
        public DateTimeOffset ResetExpiration { get; set; }
    }
}
