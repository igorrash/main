using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Web.UI;
using External;

namespace SongWebManager
{
    public partial class SongChords : Page
    {
        private static DbConnect _dbConnect;
        private static DataTable _songs;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    SetDbHelperConnection();
                    ViewSongs();
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
            }
        }

        [WebMethod]
        public static string EditSong(string songId)
        {
            SetDbHelperConnection();
            if(_songs == null)
                _songs = _dbConnect.RunSql("Select songId, songName, songContent from SONGS where 1 = 1 order by songName ");

            var dbChordLine = from myRow in _songs.AsEnumerable()
                              where myRow.Field<int>("songId") == Convert.ToInt32(songId)
                              select myRow.Field<string>("songContent");
            var firstOrDefault = dbChordLine.FirstOrDefault();

            return songId + "***" + firstOrDefault;
        }

        public void SaveButton_ServerClick(object sender, EventArgs e)
        {
            SetDbHelperConnection();

            var songLines = Regex.Split(EditSongTextArea.Value, "\n").ToList().Select(x => x.Trim()).ToArray();

            var sql = string.Format(StaticSql.UpdateSongContent, EditSongTextArea.Value, SongIdTextArea.Value);

            var dt = _dbConnect.RunSql(sql);


        }

        public void ViewSongs()
        {
            _songs = _dbConnect.RunSql("Select songId, songName, songContent from SONGS where 1 = 1 order by songName ");
            var chords = _dbConnect.RunSql(string.Format("SELECT songId, rowNum, rowVal FROM SONGS_CHORDS"));

            var songList = string.Empty;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (DataRow row in _songs.Rows)
            {
                //assing divId
                var divId = "divContent" + row["songId"];

                //label for song title
                var songTitle = GenerateLabel(divId, row["songName"].ToString());

                //break song lines line by line
                var songLines = Regex.Split(row["songContent"].ToString(), "\n").ToList();
                songLines = songLines.Select(x => x.Trim()).ToList();


                var songContent = string.Empty;
                var rowCount = 0;
                foreach (var songLine in songLines)
                {
                    //Get the chord line for current song line
                    var chordLine = GetChordsLine(chords, row, rowCount);

                    //Attache chord line with song line
                    songContent += "<span class=\"chordsClass\">" + chordLine + "</span>" + HtmlHelper.NewLine +
                                   songLine + HtmlHelper.NewLine;

                    rowCount++;
                }


                //textarea for song content
                var songContentTextBox = GenerateTextBox("songContent" + row["songId"], songContent);

                songList += songTitle + string.Format("<div id=\"{0}\" class=\"collapse\" >", divId) +
                            GenerateButtons("Edit", "SongEditPopup('" + row["songId"] + "')") +
                            songContentTextBox + "</div>";
            }

            var a = sw.Elapsed.Milliseconds;
            Songs.Text = songList;
        }

        private static string GetChordsLine(DataTable chords, DataRow row, int rowCount)
        {
            var dbChordLine = from myRow in chords.AsEnumerable()
                where myRow.Field<int>("songId") == Convert.ToInt32(row["songId"]) &&
                      myRow.Field<int>("rowNum") == Convert.ToInt32(rowCount)
                select myRow.Field<string>("rowVal");
            var firstOrDefault = dbChordLine.FirstOrDefault();

            //Retrieve and format the chord line
            var formatedChordLine = (firstOrDefault == null)
                ? string.Empty
                : firstOrDefault.ToString(CultureInfo.InvariantCulture);


            var chordLine =
                (chords.Rows.Count > rowCount && chords.Rows.Count > 0 &&
                 !string.IsNullOrWhiteSpace(formatedChordLine))
                    ? formatedChordLine.ToString(CultureInfo.InvariantCulture)
                        .Replace("]", "&nbsp;")
                        .Replace("[", "&nbsp;")
                    : "{No Data}";
            return chordLine;
        }

        private string GenerateButtons(string value, string onClick)
        {
            //type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#myModal"
            return string.Format("<button  type=\"button\"  class=\"btn btn-warning btn-block\" " +
                                 "data-toggle=\"modal\" data-target=\"#myModal\" " +
                                 "onclick=\"{0}\">{1}</button>", onClick, value);
        }

        private string GenerateLabel(string divId, string value)
        {
            var result = string.Format("<label class=\"btn btn-primary btn-block\" data-toggle=\"collapse\" " +
                                       "data-target=\"#{0}\">{1}</label>",
                                       divId, value);
            return result;
        }

        private string GenerateTextBox(string id, string value)
        {//readonly=\"readonly\"
            var result = string.Format("<div>{1}</div>", id, value);
            return result;
        }

        private static void SetDbHelperConnection()
        {
            if (_dbConnect == null)
            {
                var database = ConfigurationManager.AppSettings["database"];
                var username = ConfigurationManager.AppSettings["username"];
                var password = ConfigurationManager.AppSettings["password"];

                _dbConnect = new DbConnect(database, username, password);
            }
        }
    }
}