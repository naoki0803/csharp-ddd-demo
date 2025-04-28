# DDD ボトムアップ本

## バージョン

-   .NET 7.0
-   Visual Studio 2022

## ドメイン

### ドメインとは

### ドメイン / ドメインモデル /ドメインオブジェクトの違い

#### ドメイン

ドメインはビジネスの問題領域そのもの

-   ユーザーは一意の ID を持つ必要がある
-   ユーザー名は 3 文字以上である必要がある
-   削除済みのユーザーは再度削除できない

#### ドメインモデル

ドメインモデルはドメインの中でも、ビジネスの問題領域を表すもの

```csharp
// ドメインモデルの例：
public class User  // ユーザーという概念
{
    public UserId Id { get; private set; }  // IDという概念
    public UserName Name { get; private set; }  // 名前という概念
    private bool isDeleted;  // 削除状態という概念

    // ユーザー作成という操作の概念
    public static User CreateUser(string id, string name)

    // 名前変更という操作の概念
    public void ChangeName(string name)

    // 削除という操作の概念
    public void Delete()
}
```

#### ドメインオブジェクト

ドメインモデルを実際のコードとして実装したもの

```csharp
// 具体的な実装例：
public void ChangeName(string name)
{
    if (name == null) throw new ArgumentNullException(nameof(name));
    if (name.Length < 3) throw new ArgumentException("ユーザー名は3文字以上です。", nameof(name));
    Name = new UserName(name);
}

public void Delete()
{
    if (isDeleted) throw new InvalidOperationException("既に削除済みのユーザーです。");
    isDeleted = true;
}
```

## 値オブジェクト

### 値オブジェクトとは

### 値の性質

-   不変である
-   交換(再代入)が可能である
-   等価性によって比較される

> 変数に代入した値を再代入はできても、値そのものを根底から変更する事はできない。
> 値オブジェクトも変更は許容されず、再代入によって交換が可能になる。

### 値オブジェクトどうしの等価性の実装

```csharp
    var user1 = new UserName("鈴木太郎");
    var user2 = new UserName("鈴木太郎");

    Console.WriteLine(user1.Equals(user2));
    Console.WriteLine(user1 == user2);
```

以下実装をすることで、上記実行した際に、true となる。
※逆に記述が無いと、false となる。

-   IEquatable<T> を実装する
-   Equals メソッドをオーバーライドする
-   GetHashCode メソッドをオーバーライドする
-   == 演算子をオーバーロードする
-   != 演算子をオーバーロードする

```csharp
public class UserName : IEquatable<UserName>
{
    private readonly string value;

    public bool Equals(UserName other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(value, other.value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UserName)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return value != null ? value.GetHashCode() : 0;
        }
    }

    public static bool operator ==(UserName left, UserName right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(UserName left, UserName right)
    {
        return !(left == right);
    }
}

```

## エンティティ

### エンティティとは

### 値オブジェクトとエンティティの違い

-   エンティティ: 同一性によって識別される
-   値オブジェクト: 等価性によって識別される

> 人は誕生日(属性)が変わっても**同一人物**として識別される == 同一性が担保されている == エンティティ
> 名前は苗字が変わると**別名**として識別される == 同一性が担保されていない == 値オブジェクト

### エンティティの性質

-   可変である
-   同じ属性であっても区別される
-   同一性によって区別される

## ドメインサービス

### ドメインサービスとは

ドメインサービスは、エンティティや値オブジェクトに属さない、ドメインのビジネスロジックを表現するものです。以下のような特徴があります：

-   特定のエンティティや値オブジェクトに自然に属さないドメインロジックを担当する
-   複数のオブジェクト間の調整や相互作用に関連する処理を行う
-   ステートレス（状態を持たない）なオブジェクトとして実装される

典型的な例としては、重複チェックや認証プロセスなどが挙げられます。

```csharp
// ドメインサービスの例：
public class UserService
{
    // ユーザー重複チェックのドメインサービス
    public bool Exists(User user)
    {
        // 重複確認のコード
        // 本来はリポジトリを参照してuserの存在を確認する
        return user.Id != user.Id;
    }
}
```

### ドメインサービスを使用する状況

以下のような場合にドメインサービスを検討します：

1. 処理が複数のエンティティに関連する場合

    - 例：ユーザー名の重複チェック（他のユーザーの情報を参照する必要がある）

2. 処理がエンティティの責務を超える場合

    - 例：認証処理やアクセス権限の確認

3. ステートレスな操作が必要な場合
    - 例：計算処理やバリデーション

### ドメインサービスの注意点

-   ドメインサービスに過度に依存すると「ドメイン貧血症（Domain Anemia）」につながることがある
-   エンティティの責務を不必要にドメインサービスに移行するべきではない
-   ドメインサービスの名前は動詞ではなく名詞＋「Service」とする（例：UserService）

### 実装例

```csharp
// アプリケーション層での使用例：
app.MapGet("/domainservice", () =>
{
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
```

このように、ドメインサービスはドメインモデルの一部として、特定のエンティティに所属させるには不自然な操作をカプセル化します。ただし、可能な限りエンティティや値オブジェクトにビジネスロジックを配置し、ドメインサービスの使用は必要最小限に留めるべきです。

## リポジトリ

### リポジトリとは

リポジトリは、ドメイン層とインフラ層の橋渡しを担うデザインパターンです。エンティティや値オブジェクトなどのドメインオブジェクトを、永続化（データベースへの保存や取得）するためのインターフェースを提供します。DDD においては、リポジトリを介してドメイン層がインフラ層の実装詳細に依存しないように設計します。

-   ドメイン層ではリポジトリのインターフェース（例：`IUserRepository`）のみを参照し、具体的な実装（例：`UserRepository`）はインフラ層に隠蔽されます。
-   これにより、テストや実装の差し替えが容易になり、ドメインロジックの純粋性が保たれます。

### このプロジェクトでの実装例

#### インターフェース定義（ドメイン層）

```csharp
// backend/Domain/Repositories/IUserRepository.cs
public interface IUserRepository
{
    Task<User?> Find(UserName name);
    Task<User?> Find(UserId id);
    Task Save(User user);
}
```

#### 実装クラス（インフラ層）

```csharp
// backend/Infrastructure/Repositories/UserRepository.cs
public class UserRepository : IUserRepository
{
    private readonly Supabase.Client _supabase;
    public UserRepository(Supabase.Client supabase) { ... }
    public async Task Save(User user) { ... }
    public async Task<User?> Find(UserName name) { ... }
    public async Task<User?> Find(UserId id) { ... }
}
```

#### DI 登録と利用（Program.cs）

```csharp
// backend/Program.cs
builder.Services.AddScoped<IUserRepository, UserRepository>();

app.MapGet("/repository", async (IUserRepository userRepository) => {
    var userService = new UserService(userRepository);
    var user = User.CreateUser("リポジトリ次郎");
    var result = await userService.Exists(user);
    if (result) throw new Exception($"{user.Name}は重複しています。");
    await userRepository.Save(user);
    return Results.Ok(...);
});
```

#### アプリケーションサービスでの利用

```csharp
// backend/Applications/Services/UserApplicationService.cs
public class UserApplicationService
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;
    public UserApplicationService(IUserRepository userRepository, UserService userService) { ... }
    public async Task<UserData?> Register(string name) {
        var user = User.CreateUser(name);
        if (await _userService.Exists(user)) throw new Exception("ユーザーが既に存在します。");
        await _userRepository.Save(user);
        return new UserData(user);
    }
}
```

### まとめ

-   リポジトリは「ドメインオブジェクトの永続化・取得」を担う役割であり、DDD のレイヤー分離を実現する重要なパターンです。
-   このプロジェクトでは、インターフェースと実装を分離し、DI（依存性注入）を活用することで、柔軟かつテストしやすい設計となっています。
