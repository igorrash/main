<%@ Page
    Title="Home Page"
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="ViewSong.aspx.cs"
    Inherits="SongWebManager.ViewSong" %>

<!DOCTYPE html>
<html>
<head>
    <title>Somee Try 1</title>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.15/angular.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("div").hide();
        });

        function ShowSong(divId) {
            $("#" + divId).toggle(500);
        }

        function SaveSong() {
            var songContent = $("#newSongTextArea").val();
            var parameter = {"songContent": songContent }
            CallAjax(parameter);
            return false;
        }

        function UpdateSong(songId) {
            var songContent = $("#songContent" + songId).val();
            var parameter = { "songId": songId, "songContent": songContent }
            CallAjax(parameter);
            return false;
        }

        function CallAjax(parameter) {
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

        function onSuccess(data) {
            alert(data.d);
        }
    </script>
    <style type="text/css">
        .songContentTextArea {
            height: 200px;
            width: 400px;
        }
    </style>




</head>
<body style="vertical-align: middle; margin: 50px 50px 50px 50px;">
    <form runat="server">
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
       
        <label style="cursor: pointer; color:blue;" onclick="ShowSong('newSongDiv')">Cântare Nouă</label><br />
        <hr/>
        <div id="newSongDiv">
             <input type="button" onclick="SaveSong()" value="save"/><br/>
            <textarea class="songContentTextArea" id="newSongTextArea" >Titlul Cântârii&#13;&#10;&#13;&#10;Sfrofă(Înlocuiește)&#13;&#10;&#13;&#10;Refren(Înlocuiește)&#13;&#10;&#13;&#10;Sfrofă(Înlocuiește)&#13;&#10;&#13;&#10;Refren(Înlocuiește)&#13;&#10;&#13;&#10;.</textarea>
        </div>
        <asp:Label runat="server" ID="Songs"></asp:Label>
    </form>
</body>

</html>

