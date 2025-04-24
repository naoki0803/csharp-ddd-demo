using TodoApi;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

var builder = WebApplication.CreateBuilder(args);

// 開発環境のローカル設定ファイルを追加
builder.Configuration.AddJsonFile("appsettings.Development.local.json", optional: true, reloadOnChange: true);

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

// 設定ファイルからSupabase情報を読み込む
var supabaseConfig = builder.Configuration.GetSection("Supabase");
var url = supabaseConfig["Url"];
var key = supabaseConfig["Key"];
var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};
var supabase = new Supabase.Client(url, key, options);
await supabase.InitializeAsync();

// Supabaseからusersテーブルの一覧を取得するエンドポイント
app.MapGet("/api/users", async () =>
{
    try
    {
        var result = await supabase.From<UserModel>().Get();
        // 取得したデータを単純な形式に変換
        var users = result.Models.Select(u => new
        {
            Id = u.Id,
            Name = u.Name
        }).ToList();

        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"エラー発生: {ex.Message}");
        return Results.Problem($"Supabaseからのデータ取得に失敗しました: {ex.Message}");
    }
});

// Supabaseにユーザーを追加するエンドポイント
app.MapGet("/api/users/insert", async () =>
{
    try
    {
        // Userエンティティを作成（ドメインモデル）
        var user = User.CreateUser("test次郎");

        // ドメインモデルからデータモデルへの変換
        var userModel = new UserModel
        {
            Id = user.Id.ToString(),  // UserIdをそのまま使用
            Name = user.Name.ToString()  // UserNameをそのまま使用
        };

        // Supabaseにデータを保存
        var response = await supabase.From<UserModel>().Insert(userModel);
        Console.WriteLine($"インサート成功: {userModel.Id} - {userModel.Name}");

        return Results.Ok(new
        {
            message = "ユーザーの保存に成功しました",
            user_id = user.Id.ToString(),
            user_name = user.Name.ToString()
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"エラー発生: {ex.Message}");
        return Results.Problem($"Supabaseへのデータ保存に失敗しました: {ex.Message}");
    }
});

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

app.MapGet("/domainservice", async () =>
{
    // domainService を用いた重複チェックの実装
    var user1 = User.CreateUser("鈴木一郎");
    var userService = new UserService(supabase);
    bool result = await userService.Exists(user1);
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

// Supabaseからデータを取得するためのデータモデル
namespace TodoApi
{
    [Table("users")]
    public class UserModel : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}