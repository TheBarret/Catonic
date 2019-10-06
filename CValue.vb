Public Class CValue
    Public Property Type As CTypes
    Public Property Value As String
    Public Property Reference As String
    Sub New(ref As String, value As String, type As CTypes)
        Me.Reference = ref
        Me.Value = value
        Me.Type = type
    End Sub
    Public ReadOnly Property Length As Integer
        Get
            Return Me.Value.Length
        End Get
    End Property
    Public Overrides Function ToString() As String
        Return String.Format("{0} = {1} as {2}", Me.Reference, Me.Value, Me.Type.ToString)
    End Function
End Class
