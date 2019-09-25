Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim valor As String
        valor = UF.GetUfFecha(DateTimePicker1.Value)
        Label2.Text = valor
    End Sub
End Class
