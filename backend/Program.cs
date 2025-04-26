using TodoApi;
using TodoApi.Infrastructure.Repository;
using TodoApi.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// 開発環境のローカル設定ファイルを追加
builder.Configuration.AddJsonFile("appsettings.Development.local.json", optional: true, reloadOnChange: true);

// Supabaseクライアントの登録 (シングルトンで登録)
/*
    // 悪い例（シングルトンでない場合）
    app.MapGet("/api/data", () => {
        var client = new Supabase.Client(...); // 毎回新しい接続
        client.InitializeAsync().Wait();       // 毎回初期化
        var result = client.From(...).Get();   // 新しい接続で実行
        client.Dispose();                      // 接続を閉じる
    });

    // 良い例（シングルトンの場合）
    app.MapGet("/api/data", (Supabase.Client client) => {
        var result = client.From(...).Get();   // 既存の接続を再利用
    });
    シングルトンを使用しない場合、各リクエストで新しい接続を確立する必要があり、これは：
    パフォーマンスの低下
    リソースの無駄遣い
    接続数の制御が困難
    一貫性の保証が難しい
    といった問題を引き起こす可能性があります。
*/
builder.Services.AddSingleton<Supabase.Client>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var supabaseConfig = configuration.GetSection("Supabase");
    var url = supabaseConfig["Url"];
    var key = supabaseConfig["Key"];
    var options = new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = true
    };
    var client = new Supabase.Client(url, key, options);
    client.InitializeAsync().Wait();
    return client;
});

// UserRepositoryの登録（リクエストスコープでIUserRepositoryの実装としてUserRepositoryを提供）
builder.Services.AddScoped<IUserRepository, UserRepository>();

// UserServiceの登録（リクエストスコープでUserServiceを提供。コンストラクタでIUserRepositoryが自動注入される）
builder.Services.AddScoped<UserService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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
    // Console.WriteLine("domainserviceのパスに接続されました。");

    // var user = User.CreateUser("鈴木一郎");
    // var userService = new UserService(supabase);

    // // domainService を用いた重複チェックの実装
    // bool result = await userService.Exists(user);
    // if (result)
    // {
    //     throw new Exception($"{user.Name}は重複しています。");
    // }

    // // userRepository を用いらないユーザーの保存
    // // データモデルへの変換
    // var userModel = new UserModel
    // {
    //     Id = user.Id.ToString(),  // UserIdをそのまま使用
    //     Name = user.Name.ToString()  // UserNameをそのまま使用
    // };

    // // Supabaseにデータを保存
    // var response = await supabase.From<UserModel>().Insert(userModel);
    // Console.WriteLine($"インサート成功: {userModel.Id} - {userModel.Name}");

    // return Results.Ok(new
    // {
    //     message = "ユーザーの保存に成功しました",
    //     user_id = user.Id.ToString(),
    //     user_name = user.Name.ToString()
    // });
});


//引数にIUserRepositoryを渡す事で、UserRepositoryのインスタンスを注入する。
app.MapGet("/repository", async (IUserRepository userRepository) =>
{
    Console.WriteLine("repositoryのパスに接続されました。");

    var userService = new UserService(userRepository);
    var user = User.CreateUser("リポジトリ次郎");

    // domainService を用いた重複チェックの実装
    var result = await userService.Exists(user);
    if (result)
    {
        throw new Exception($"{user.Name}は重複しています。");
    }

    // userRepository を用いたユーザーの保存
    await userRepository.Save(user);

    return Results.Ok(new
    {
        message = "ユーザーの保存に成功しました",
        user_id = user.Id.ToString(),
        user_name = user.Name.ToString()
    });
});

app.Run();