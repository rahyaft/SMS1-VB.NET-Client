Imports System.Net
Imports System.Text

Module Program

    Public Class SendModel
        Public Message As String
        Public Recipient As String
    End Class

    Public Class PatternSendModel
        Public Recipient As String
        Public PatternId As Integer
        Public Pairs As Dictionary(Of String, String)
    End Class

    Sub Main(args As String())

        ' SMS1 API base URL
        Dim apiBaseUrl = "https://SubDomain.Domain:Port/api/service/"

        ' The API name according to SMS1.ir
        ' Dim apiName = "send"
        Dim apiName = "patternSend"

        ' Token is received from SMS1.ir
        Dim token = "YOUT_TOKEN"

        ' Sample input model for API 'send'
        Dim sendModel As New SendModel()
        sendModel.Message = "سلام"
        sendModel.Recipient = "09120000000"

        ' Sample input model for API 'patternSend'
        Dim patternSendModel As New PatternSendModel()

        ' Pattern Id: received from SMS1.ir
        patternSendModel.PatternId = 7
        patternSendModel.Recipient = "09120000000"

        ' Variables that exist in the approved pattern of your Token
        patternSendModel.Pairs = New Dictionary(Of String, String) From
                                 {
                                    {"variable_1", "value_1"},
                                    {"variable_2", "value_2"}
                                 }

        ' In this example, we use the 'Newtonsoft.Json' library
        ' You may use any other third-party library, to convert an object into a JSON string
        ' For API 'send', you should use:
        ' Dim jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(sendModel)
        Dim jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(patternSendModel)

        ' Encoding the JSON string
        Dim data = Encoding.UTF8.GetBytes(jsonString)

        Dim statusCode = SendRequest(New Uri(apiBaseUrl + apiName), data, "application/json", token)

        ' HTTP status code
        Console.WriteLine(statusCode)
    End Sub

    Private Function SendRequest(uri As Uri, jsonDataBytes As Byte(), contentType As String, token As String) As Integer

        ' Creating the request
        Dim request As WebRequest

        ' Setting the URL
        request = WebRequest.Create(uri)

        request.ContentLength = jsonDataBytes.Length
        request.ContentType = contentType
        ' Setting the HTTP 'Authorization' header equal to the received Token from SMS1.ir
        request.Headers.Add("Authorization", "Bearer " + token)
        ' Setting the HTTP 'Accept' header to JSON
        request.Headers.Add("Accept", "application/json")
        ' Setting the HTTP method
        request.Method = "POST"

        ' Sending the request to server
        Using requestStream = request.GetRequestStream
            requestStream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
            requestStream.Close()

            ' Interpreting the response
            Try
                Dim response = DirectCast(request.GetResponse, HttpWebResponse)
                Return response.StatusCode
            Catch ex As WebException
                Dim response = DirectCast(ex.Response, HttpWebResponse)
                Return response.StatusCode
            End Try
        End Using
    End Function
End Module
