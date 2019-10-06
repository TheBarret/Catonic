Module Program

    Sub Main()


        Try

            Dim input As New Example With {.Message = "Hello, World!", .Bool = True, .IntNumber = 1000, .FloatNumber = Math.E}

            Serializer.Export("example.bin", input, Scrambler.Rot47)

            Dim output As New Example

            output = Serializer.Import(Of Example)("example.bin", output, Scrambler.Rot47)

            Console.WriteLine("Output: {0}", output.ToString)
        Catch ex As Exception
            Console.WriteLine("[exception] {0}", ex.Message)
        End Try


        Console.Read()
    End Sub

End Module
