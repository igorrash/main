using System;
using System.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using External;

namespace SongWebManager
{
    public partial class ViewSong : Page
    {
        private static DbConnect _dbConnect;
        private static DbConnect _dbHistConnect;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetDbHelperConnections();
                    ViewSongs();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
            }
        }

        [WebMethod]
        public static string UpdateSong(string songId, string songContent)
        {

            SetDbHelperConnections();
            var dt = _dbConnect.RunSql(string.Format(StaticSql.UpdateSongContent, songContent, songId));

            SetDbHelperHistConnections();
            var dtHist = _dbHistConnect.RunSql(string.Format(StaticSql.AddSongHist, songId, songContent));

            return songContent;
        }

        [WebMethod]
        public static string SaveSong(string songContent)
        {
            SetDbHelperConnections();

            var songName = Regex.Split(songContent, "\n\n");
            
            var dt = _dbConnect.RunSql(string.Format(StaticSql.AddSong, songName[0], songContent));

            return "success yeii";
        }


        public void ViewSongs()
        {
            var dt = _dbConnect.RunSql("Select songId, songName, songContent from SONGS order by songName");
            var songList = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                //assing divId
                var divId = "divContent" + row["songId"];

                //label for song title
                var songTitle = GenerateLabel(divId, row["songName"].ToString());

                //textarea for song content
                var songContent = GenerateTextBox("songContent" + row["songId"], row["songContent"].ToString());
                
                songList += songTitle + 
                            string.Format("<div id=\"{0}\" class=\"collapse\" >", divId)  + songContent
                            + GenerateButton("Save", "UpdateSong('" + row["songId"] + "')") + "</div>";
            }
            Songs.Text = songList;    
        }

        private static string FormatSongContent(DataRow row)
        {
            return   
                row["songContent"].ToString()
                    .Replace("\r\n", HtmlHelper.NewLine)
                    .Replace("\n", HtmlHelper.NewLine)
                    .Replace("\r", HtmlHelper.NewLine);
        }

        private string GenerateButton(string value, string onClick)
        {
            return string.Format("<button type=\"button\" class=\"btn btn-success btn-block\" onclick=\"{0}\">{1}</button>", onClick,
                value);
        }

        private string GenerateLabel(string id, string value, string onClick, bool newLine, string cursor = "auto")
        {
            var result = string.Format("<label style=\"{4};\" onclick=\"{0}\" id=\"{1}\" >{2}</label>{3}", onClick, id,
                value, newLine ? HtmlHelper.NewLine : string.Empty, "cursor:" + cursor);
            return result;
        }

        private string GenerateLabel(string divId, string value)
        {
            var result = string.Format("<label class=\"btn btn-primary btn-block\" data-toggle=\"collapse\" " +
                                       "data-target=\"#{0}\">{1}</label>", 
                                       divId, value);
            return result;
        }

        private string GenerateTextBox(string id, string value)
        {
            var result = string.Format("<textarea class=\"form-control\" rows=\"15\"  id=\"{0}\" >{1}</textarea>", id, value);
            return result;
        }

        private static void SetDbHelperConnections()
        {
            if (_dbConnect == null)
            {
                var database = ConfigurationManager.AppSettings["database"];
                var username = ConfigurationManager.AppSettings["username"];
                var password = ConfigurationManager.AppSettings["password"];

                _dbConnect = new DbConnect(database, username, password);
            }
        }

        private static void SetDbHelperHistConnections()
        {
            if (_dbHistConnect == null)
            {
                var database = ConfigurationManager.AppSettings["databaseHist"];
                var username = ConfigurationManager.AppSettings["usernameHist"];
                var password = ConfigurationManager.AppSettings["passwordHist"];

                _dbHistConnect = new DbConnect(database, username, password);
            }
        }

    }
}