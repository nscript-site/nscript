using System.Runtime.InteropServices;

[UnmanagedCallersOnly(EntryPoint = "add")]
public static int Add(int a, int b)
{
    return a + b;
}

[DllImport("pack/out/lib.dll")]
private static extern int add(int a, int b);

//@main
int result = add(2,4);
Console.WriteLine(result);
