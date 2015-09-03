<%@ Page 
    Title="Home Page" 
    Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="ViewSong.aspx.cs" 
    Inherits="SongWebManager._Default" %>

<!DOCTYPE html>
<html>
<head>
    <title>Somee Try 1</title>
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.15/angular.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("div").hide();
        });

        function ShowSong(divId) {
            $("#" + divId).toggle(500);
        }

        function SaveSong(songId) {
            var pageUrl = "ViewSong.aspx/SaveSong";
            var songContent = $("#songContent"+songId).val();
            var parameter = { "songId": songId, "songContent": songContent}

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
                    return false;
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
<body>
    <form runat="server">
        
<%--        <asp:TextBox runat="server" ID="txtFirstName"></asp:TextBox>
        <asp:TextBox runat="server" ID="txtLastName"></asp:TextBox>
        <input type="button" onclick="jqueryAjaxCall()" value="Hello" />
        <label id="i" onclick="" >sadsa</label>--%>
       <asp:Label runat="server" ID="Songs"></asp:Label>
    </form>
</body>

</html>

