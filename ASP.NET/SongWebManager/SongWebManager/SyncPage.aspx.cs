using System;
using System.Data;
//using DotNetOpenAuth.AspNet.Clients;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
using External;
//using Nemiro.OAuth.LoginForms;

namespace SongWebManager
{
    public partial class SyncPage : System.Web.UI.Page
    {
        private static DbConnect _dbConnect;
        private const string App_key = "06zohuge1wugnf1";
        private const string App_secret = "mpkckld77wvnrkd";

        protected void Page_Load(object sender, EventArgs e)
        {
            var config = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox) as
                    DropBoxConfiguration;

            string _AuthorizationCallbackUri = "http://localhost:56117/SyncPage.aspx";
            config.AuthorizationCallBack = new Uri(_AuthorizationCallbackUri);


            DropBoxRequestToken requestToken = DropBoxStorageProviderTools.GetDropBoxRequestToken(config, App_key,
                App_secret);

            var a = DropBoxStorageProviderTools.GetAccountInformation(requestToken);
            String AuthorizationUrl = DropBoxStorageProviderTools.GetDropBoxAuthorizationUrl(config, requestToken);
        }

        public void ViewSongs()
        {
            //var dt = _dbConnect.RunSql("Select songId, songName, songContent from SONGS order by songName");
            //var songList = string.Empty;
            //foreach (DataRow row in dt.Rows)
            //{
            //    //assing divId
            //    var divId = "divContent" + row["songId"];

            //    //label for song title
            //    var songTitle = GenerateLabel(divId, row["songName"].ToString());

            //    //textarea for song content
            //    var songContent = GenerateTextBox("songContent" + row["songId"], row["songContent"].ToString());

            //    songList += songTitle +
            //                string.Format("<div id=\"{0}\" class=\"collapse\" >", divId) + songContent
            //                + GenerateButton("Save", "UpdateSong('" + row["songId"] + "')") + "</div>";
            //}
        }
    }
}