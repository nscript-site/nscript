
List<String> Test()
{
    List<String> list = new List<string>();
    list.Add("hello");
    list.Add("world!");
    return list;
}

//@main
var list = Test();
Console.WriteLine(list[0] + " " + list[1]);
