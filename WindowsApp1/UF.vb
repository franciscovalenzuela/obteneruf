Imports System.Net
Imports System.Net.NetworkInformation
Imports System.IO
Imports mshtml
Imports System.Text.RegularExpressions

Module UF
    Public Const useragent As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0"

    'Buscar uf en pagina web SII
    Public Function GetUfFecha(fecha As DateTime) As String
        Dim regex As New Regex("\.")
        Dim htmlcontent As String
        Dim uf As String
        Dim multiList As New List(Of List(Of String))

        multiList.Add(New List(Of String))
        For i As Integer = 1 To 13
            multiList.Add(New List(Of String))
        Next

        htmlcontent = RequestSiiPage(fecha.Year)
        Dim getindextabla = htmlcontent.IndexOf("'mes_all'>")
        Dim substring As String = htmlcontent.Substring(getindextabla, (htmlcontent.Length - getindextabla))

        FillListaUFDates(multiList, substring)
        uf = multiList(fecha.Month)((fecha.Day - 1))
        uf = uf.Replace(",", ".")
        uf = regex.Replace(uf, "", 1)
        Return uf
    End Function

    Private Function RequestSiiPage(year As Integer) As String
        Dim respuesta As WebResponse
        Dim request As HttpWebRequest
        Dim url As String = "http://www.sii.cl/valores_y_fechas/uf/uf" & year & ".htm "
        Dim data As Stream
        Dim reader As StreamReader
        Dim htmltexto As String

        request = CType(WebRequest.Create(url), HttpWebRequest)
        request.Referer = url
        request.UserAgent = useragent
        request.Method = "GET"
        respuesta = request.GetResponse
        data = respuesta.GetResponseStream
        reader = New StreamReader(data)

        htmltexto = reader.ReadToEnd()
        data.Close()
        reader.Close()
        Return htmltexto
    End Function

    Private Sub FillListaUFDates(ByVal mesdialist As List(Of List(Of String)), substring As String)
        Dim htmlDocument As IHTMLDocument2
        Dim elementos As IHTMLElementCollection
        Dim tabla As IHTMLTable
        Dim ct As Integer = 0
        htmlDocument = CType(New MSHTML.HTMLDocument(), IHTMLDocument2)
        htmlDocument.write(substring)

        elementos = htmlDocument.all.tags("TABLE")
        For Each element As IHTMLElement In elementos
            tabla = element
            For Each element1 As IHTMLElement In tabla.tBodies
                For Each elemnt As IHTMLElement In element1.children
                    For Each elemnt1 As IHTMLElement In elemnt.all
                        mesdialist(ct).Add(elemnt1.innerText)
                        ct += 1
                    Next
                    ct = 0
                Next
            Next
        Next
    End Sub
End Module
