// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using ReflectionHomeWork;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Reflection");
        Console.WriteLine($"{Environment.OSVersion.Platform}");
        var test = new Class2String();
        var timestart = DateTime.Now;
        int iterations = 100000;
        string result = string.Empty;
        string resultJson = string.Empty;
        var f = F.Get(); ;
        for (var i = 0; i < iterations; i++)
        {
             result = test.Setialize(f);
        }
        var timestpo1 = DateTime.Now;
        Console.WriteLine(@"class F { int i1, i2, i3, i4, i5; string s1; public  int i6{get;set;} public static F Get() => new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 , i6=99, s1=""123,123,123""}; public override string ToString() { return $""i1 {i1} i2 {i2} i3 {i3} i4 {i4} i5 {i5} i6 {i6}""; } }");
        Console.WriteLine($"Custom serialize: Iterations: {iterations},Time (ms): {(timestpo1 - timestart).TotalMilliseconds}");
        Console.WriteLine($"result:{result}");
        var timestpo2 = DateTime.Now;
        Console.WriteLine($" Console.WriteLin,Time (ms): {(timestpo2 - timestpo1).TotalMilliseconds}");
        timestart = DateTime.Now;

        for (var i = 0; i < iterations; i++)
        {
            resultJson = JsonConvert.SerializeObject(f);
        }
        timestpo1 = DateTime.Now;

        Console.WriteLine($"Newton JSON serialize: Iterations: {iterations},Time (ms): {(timestpo1 - timestart).TotalMilliseconds}");
        Console.WriteLine($"result:{resultJson}");
        timestart = DateTime.Now;
        for (var i = 0; i < iterations; i++)
        {
             f=(F)test.Deserialize(result);
        }
        timestpo1 = DateTime.Now;

        Console.WriteLine($"Custom deserialize: Iterations: {iterations},Time (ms): {(timestpo1 - timestart).TotalMilliseconds}");
        Console.WriteLine($".ToString() => {f.ToString()}");
        timestart = DateTime.Now;
        for (var i = 0; i < iterations; i++)
        {
            f = JsonConvert.DeserializeObject<F>(resultJson);
        }
        timestpo1 = DateTime.Now;
        Console.WriteLine($"Newton JSON deserialize: Iterations: {iterations},Time (ms): {(timestpo1 - timestart).TotalMilliseconds}");
        Console.WriteLine($".ToString() => {f.ToString()}");

    }
}

class F { int i1, i2, i3, i4, i5; string s1; public int i6 { get; set; } public static F Get() => new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5, i6 = 99, s1 = "123,123,123" }; public override string ToString() { return $"i1 { i1} i2 { i2} i3 { i3} i4 { i4} i5 { i5} i6 { i6}  s1 {s1}"; } }