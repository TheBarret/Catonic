Module Program

    Sub Main()


        Try

            Dim input As New Example With {.Message = "Hello, World!", .Bool = True,
                                           .IntNumber = 1000, .FloatNumber = Math.E,
                                           .CollectionA = {"A", "B", "C"},
                                           .CollectionB = {1, 2, 3},
                                           .CollectionC = {1.0R, 2.0R, 3.0R},
                                           .CollectionD = {True, False},
                                           .CollectionE = {1, 2, 3}}

            Serializer.Export("example.bin", input)

            Dim output As Example = Serializer.Import(Of Example)("example.bin")

            Console.WriteLine("Output: {0}", output.ToString)
        Catch ex As Exception
            Console.WriteLine("[exception] {0}", ex.Message)
        End Try


        Console.Read()
    End Sub

End Module
