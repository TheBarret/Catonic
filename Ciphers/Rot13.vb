Namespace Ciphers
    Public Class Rot13
        Implements IProvider
        Public Function Input(value As String) As String Implements IProvider.Input
            Return Me.Transform(value)
        End Function
        Public Function Output(value As String) As String Implements IProvider.Output
            Return Me.Transform(value)
        End Function
        Private Function Transform(value As String) As String
            Dim characters() As Char = value.ToCharArray()
            For i As Integer = 0 To characters.Length - 1
                Dim j As Integer = Strings.AscW(characters(i))
                If (j >= 97 AndAlso j <= 122) Then
                    If (j > 109) Then
                        j -= 13
                    Else
                        j += 13
                    End If
                ElseIf (j >= 65 AndAlso j <= 90) Then
                    If (j > 77) Then
                        j -= 13
                    Else
                        j += 13
                    End If
                End If
                characters(i) = Strings.ChrW(j)
            Next
            Return New String(characters)
        End Function
    End Class
End Namespace
