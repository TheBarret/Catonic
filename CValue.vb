Imports System.Text

Public Class CValue
    Public Property Type As CTypes
    Public Property Value As String
    Public Property Reference As String
    Sub New(ref As String, value As Object, type As CTypes)
        Me.Reference = ref
        Me.Type = type
        If (Not value.GetType.IsArray) Then
            Me.Value = value.ToString
        Else
            Me.Value = CValue.CollectArrayData(CType(value, IEnumerable))
        End If
    End Sub
    Public Shared Function CollectArrayData(collection As IEnumerable) As String
        Dim buffer As New List(Of String)
        For Each v In collection
            buffer.Add(v.ToString)
        Next
        Return String.Join(Strings.ChrW(0), buffer)
    End Function
    Public ReadOnly Property Length As Integer
        Get
            Return Me.Value.Length
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("{0} [{1}]", Me.Reference, Me.Type.ToString)
    End Function
End Class
