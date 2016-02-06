<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SongWebManager.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Igor's Virtual Home</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.15/angular.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />
    <style>
        body {
            vertical-align: middle; 
            margin: 20px 20px 20px 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <a type="button" class="btn btn-primary btn-block btn-lg" href="ViewSong.aspx">ViewSong</a>
        <a type="button" class="btn btn-primary btn-block btn-lg" href="ViewSong.aspx">Sync</a>
<%--        <a type="button" class="btn btn-primary btn-block btn-lg" href="SongChords.aspx">Songs' Chords</a>--%>
    </form>
</body>
</html>
