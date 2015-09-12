<%@ Page
    Title="Home Page"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="ViewSong.aspx.cs"
    Inherits="SongWebManager.ViewSong" %>

<!DOCTYPE html>
<html>
<head>
    <title>View/Edit Song</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.15/angular.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css"/>

    <script type="text/javascript">
        $(document).ready(function () {
           
        });

        function SaveSong() {
            var songContent = $("#newSongTextArea").val();
            var parameter = {"songContent": songContent }
            CallSaveSong(parameter);
            return false;
        }

        function UpdateSong(songId) {
            var textboxId = "#songContent" + songId;
            var songContent = $(textboxId).val();
            var parameter = { "songId": songId, "songContent": songContent }
            CallUpdateSong(parameter, textboxId);
            return false;
        }

        function CallSaveSong(parameter) {
            var pageUrl = "ViewSong.aspx/SaveSong";
            $.ajax({
                type: 'POST',
                url: pageUrl,
                data: JSON.stringify(parameter),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    onSuccess(data);
                },
                error: function (data, success, error) {
                    alert("Error: " + error);
                }
            });
        }

        function CallUpdateSong(parameter, textboxId) {
            var pageUrl = "ViewSong.aspx/UpdateSong";
            $.ajax({
                type: 'POST',
                url: pageUrl,
                data: JSON.stringify(parameter),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    //onSuccess(data);
                    if (data.d) {
                        $(textboxId).val(data.d);
                        alert("Saved");
                    }
                },
                error: function (data, success, error) {
                    alert("Error: " + error);
                }
            });
        }

        function onSuccess(data) {
            alert(data.d);
        }
    </script>
    <style type="text/css">
        body {
            vertical-align: middle; 
            margin: 20px 20px 20px 20px;
            font-size: 25px;
        }
         .songContentTextArea {
             height: 200px;
             width: 400px;
         }
    </style>




</head>
<body>
    <form runat="server">
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
        
        <button type="button" class="btn btn-primary btn-block" data-toggle="collapse" data-target="#newSongDiv">Cântare Nouă</button>
        <div id="newSongDiv"  class="collapse" style="margin: 0 0 50px 0" >
            <textarea class="form-control" id="newSongTextArea" rows="10">Titlul Cântârii&#13;&#10;&#13;&#10;Sfrofă(Înlocuiește)&#13;&#10;&#13;&#10;Refren(Înlocuiește)&#13;&#10;&#13;&#10;Sfrofă(Înlocuiește)&#13;&#10;&#13;&#10;Refren(Înlocuiește)&#13;&#10;&#13;&#10;.</textarea>
            <button type="button" class="btn btn-success btn-block" onclick="SaveSong()">Save</button>
        </div>
        <asp:Label runat="server" ID="Songs"></asp:Label>
    </form>
</body>

</html>

