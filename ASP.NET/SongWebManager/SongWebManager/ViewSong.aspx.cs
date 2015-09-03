﻿using System;
using System.Configuration;
using System.Data;
using System.Web.Services;
using System.Web.UI;
using External;

namespace SongWebManager
{
    public partial class _Default : Page
    {
        private DbConnect _dbConnect;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_dbConnect == null)
            {
                var database = ConfigurationManager.AppSettings["database"];
                var username = ConfigurationManager.AppSettings["username"];
                var password = ConfigurationManager.AppSettings["password"];

                _dbConnect = new DbConnect(database, username, password);
            }

            ViewSongs();
        }

        [WebMethod]
        public static string SaveSong(string songId, string songContent)
        {
            Console.WriteLine("SongId:{0}SongContent:{1}", songId, songContent);

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
                var songTitle = GenerateLabel("songName" + row["songId"], row["songName"].ToString(),
                    "ShowSong('" + divId + "')", true, "pointer");

                //textarea for song content
                var songContent = GenerateTextBox("songContent" + row["songId"], row["songContent"].ToString());


                songList += songTitle + HtmlHelper.HorizontalLine + string.Format("<div id=\"{0}\">", divId) +
                            GenerateInput("button" + row["songId"], "save","SaveSong('" + row["songId"] + "')", InputTypeEnum.button) +
                            HtmlHelper.NewLine + songContent + "</div>";
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

        private string GenerateInput(string id, string value, string onClick, InputTypeEnum inputType)
        {
            return string.Format("<input type=\"{0}\" onclick=\"{1}\" value=\"{2}\" id=\"{3}\" />", inputType, onClick,
                value, id);
        }


        private string GenerateLabel(string id, string value, string onClick, bool newLine, string cursor = "auto")
        {
            var result = string.Format("<label style=\"{4};\" onclick=\"{0}\" id=\"{1}\" >{2}</label>{3}", onClick, id,
                value, newLine ? HtmlHelper.NewLine : string.Empty, "cursor:" + cursor);
            return result;
        }

        private string GenerateTextBox(string id, string value)
        {
            var result = string.Format("<textarea class=\"songContentTextArea\" id=\"{0}\" >{1}</textarea>", id, value);
            return result;
        }

    }
}