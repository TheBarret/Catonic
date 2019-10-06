Namespace Ciphers
    Public Class Rot47
        Implements IProvider
        Public Function Input(value As String) As String Implements IProvider.Input
            Return Me.Transform(value)
        End Function
        Public Function Output(value As String) As String Implements IProvider.Output
            Return Me.Transform(value)
        End Function
        Private Function Transform(value As String) As String
            Dim offset As Integer, index As Integer
            Dim result As Char() = New Char(value.Length - 1) {}
            For i As Integer = 0 To value.Length - 1
                index = Strings.AscW(value(i))
                If (index = 32) Then
                    result(i) = " "c
                    Continue For
                End If
                offset = index + 47
                If (offset > 126) Then
                    offset -= 94
                ElseIf (offset < 33) Then
                    offset += 94
                End If
                result(i) = Strings.ChrW(offset)
            Next
            Return New String(result)
        End Function
    End Class
End Namespace
