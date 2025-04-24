using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/", () =>
{
    // debug 用にUserをインスタンス化
    var testuser = User.CreateUser("クリエイト太郎");

    // ToString()の確認
    Console.WriteLine(testuser);
    Console.WriteLine(testuser.Id);

    // エンティティを可変性に対応させる
    // 変更前の名前を出力
    Console.WriteLine($"変更前の名前: {testuser.Name}");
    // 名前を変更
    testuser.ChangeName("チェンジ太郎");
    // 変更後の名前を出力
    Console.WriteLine($"変更後の名前: {testuser.Name}");
    return "ユーザー情報をコンソールに出力しました";
});


app.MapGet("/valueobject", () =>
{
    // 値オブジェクトは等価性によって識別される。
    var user1 = new UserName("クリエイト太郎");
    var user2 = new UserName("クリエイト太郎");

    // Equalsメソッドによる等価性比較
    Console.WriteLine(user1.Equals(user2)); // true

    // 演算子による等価性比較
    Console.WriteLine(user1 == user2); // true

    return "valueobjectのパスです。";
});


app.MapGet("/entity", () =>
{
    // エンティティは同一性によって識別される。
    var user1 = User.CreateUser("鈴木一郎");
    var user2 = User.CreateUser("鈴木一郎");
    var user3 = User.CreateUser("小林一郎");

    // Equalsメソッドによる同一性比較
    Console.WriteLine(user1.Equals(user2)); // IDが異なる為、false
    Console.WriteLine(user1.Equals(user3)); // IDが同じ為、true

    // 演算子による同一性比較
    Console.WriteLine(user1 == user2); // IDが異なる為、false
    Console.WriteLine(user1 == user3); // IDが同じ為、true

    return "entityのパスです。";
});

app.MapGet("/domainservice", () =>
{
    // domainService を用いた重複チェックの実装
    var user1 = User.CreateUser("鈴木一郎");

    var userService = new UserService();
    bool result = userService.Exists(user1);
    if (result)
    {
        throw new Exception($"{user1.Name}は重複しています。");
    }
    Console.WriteLine("データストア(リポジトリ)への問い合わせ実施後、データが永続化される。");

    return "domainserviceのパスです。";
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
