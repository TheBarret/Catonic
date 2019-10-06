Imports System.Text
Namespace Ciphers
    Public Class Bifid
        Implements IProvider
        Private Property Counter As Char = Strings.ChrW(0)
        Private Property Splitter As Char = Strings.ChrW(1)
        Private Property Delimiter As Char = Strings.ChrW(2)
        Public Function Input(value As String) As String Implements IProvider.Input
            Return String.Join(Me.Splitter, value.Select(Function(x) Me.Transform(x)))
        End Function
        Public Function Output(value As String) As String Implements IProvider.Output
            Dim x As Integer = 0
            Dim y As Integer = 0
            Dim index As Integer = 0
            Dim buffer As New StringBuilder
            For Each chr As Char In value.ToCharArray
                Select Case chr
                    Case Me.Counter
                        If (index = 0) Then
                            x += 1
                        ElseIf (index = 1) Then
                            y += 1
                        End If
                    Case Me.Delimiter
                        index += 1
                    Case Me.Splitter
                        If (x > 0 AndAlso y > 0) Then
                            buffer.Append(Me.Table(x - 1, y - 1))
                        End If
                        x = 0
                        y = 0
                        index = 0
                    Case Else
                        buffer.Append(chr)
                End Select
            Next
            If (x <> 0 AndAlso y <> 0) Then
                buffer.Append(Me.Table(x - 1, y - 1))
            End If
            Return buffer.ToString()
        End Function
        Private Function Transform(value As Char) As String
            For x As Integer = 1 To Me.Table.GetLength(0)
                For y As Integer = 1 To Me.Table.GetLength(1)
                    If (Me.Table(x - 1, y - 1) = value) Then
                        Return String.Concat(Me.Sequence(x), Me.Delimiter, Me.Sequence(y))
                    End If
                Next
            Next
            Return value
        End Function
        Private ReadOnly Property Sequence(n As Integer) As String
            Get
                Return New String(Me.Counter, n)
            End Get
        End Property
        Private ReadOnly Property Table As Char(,)
            Get
                Return {{"A"c, "B"c, "C"c, "D"c, "E"c},
                        {"F"c, "G"c, "H"c, "I"c, "J"c},
                        {"K"c, "L"c, "M"c, "N"c, "O"c},
                        {"P"c, "Q"c, "R"c, "S"c, "T"c},
                        {"U"c, "V"c, "W"c, "X"c, "Y"c},
                        {"Z"c, "a"c, "b"c, "c"c, "d"c},
                        {"e"c, "f"c, "g"c, "h"c, "i"c},
                        {"j"c, "k"c, "l"c, "m"c, "n"c},
                        {"o"c, "p"c, "q"c, "r"c, "s"c},
                        {"t"c, "u"c, "v"c, "w"c, "x"c},
                        {"y"c, "z"c, "0"c, "1"c, "2"c},
                        {"3"c, "4"c, "5"c, "6"c, "7"c},
                        {"8"c, "9"c, "."c, ","c, " "c}}
            End Get
        End Property
    End Class
End Namespace