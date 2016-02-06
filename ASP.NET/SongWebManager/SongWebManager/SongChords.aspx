<%@ Page
    Title=""
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="SongChords.aspx.cs"
    Inherits="SongWebManager.SongChords" %>

<!DOCTYPE html>
<html>
<head>
    <title>View/Edit Song</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.15/angular.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />

    <script type="text/javascript">
        $(document).ready(function () {

        });

        function SongEditPopup(songId) {
            var parameter = { "songId": songId }
            CallAjax(parameter);
            return false;
        }

        function CallAjax(parameter) {
            var pageUrl = "SongChords.aspx/EditSong";
            $.ajax({
                type: 'POST',
                url: pageUrl,
                data: JSON.stringify(parameter),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    //alert("Success");
                    onSuccess(data);
                },
                error: function (data, success, error) {
                    alert("Error: " + error);
                }
            });
            return false;
        }

        function onSuccess(data) {
            var arr = data.d.split("***");
            $("#SongIdTextArea").val(arr[0]);
            $("#EditSongTextArea").val(arr[1]);

            return false;
        }

    </script>
    <style type="text/css">
        .songContentTextArea {
            height: 200px;
            width: 400px;
        }

        body {
            vertical-align: middle;
            margin: 20px 20px 20px 20px;
            font-family: monospace;
        }

        .chordsClass {
            color: red;
            font-weight: bold;
        }

        .form-control {
            /*font-size: 40px;*/
        }
    </style>

</head>
<body style="">
    <form runat="server">

        <!-- Modal -->
        <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Song Id</h4>
                        <textarea class="form-control" id="SongIdTextArea" rows="1" runat="server">-1</textarea>
                    </div>
                    <div class="modal-body">
                        <p>Song Content</p>
                        <textarea class="form-control" rows="15" id="EditSongTextArea" runat="server">Nothing was loaded</textarea>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-success btn-block" id="SaveButton" onserverclick="SaveButton_ServerClick" runat="server">Save</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>

            </div>
        </div>
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red"></asp:Label>
        <asp:Label runat="server" ID="Songs"></asp:Label>

    </form>
</body>

</html>

