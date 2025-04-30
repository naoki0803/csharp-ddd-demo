# DDD ボトムアップ本

## バージョン

-   .NET 7.0
-   Visual Studio 2022

## ドメイン

### ドメインとは

ドメインとは、ソフトウェアが対象とする業務領域のことです。DDD では、このドメインの知識をコードで表現することに重点を置きます。

このプロジェクトでは、以下のようなドメインのルールが存在します：

-   ユーザーは一意の ID を持つ必要がある
-   ユーザー名は 3 文字以上である必要がある
-   削除済みのユーザーは再度削除できない

### ドメインモデル

ドメインをソフトウェアとして表現するための設計モデル。ドメインオブジェクト（ドメインの概念をコードとして実装したもの）によって構成されます。

このプロジェクトのドメインモデルは以下の要素で構成されます：

1. **エンティティ**

    - ビジネスの実体を表現
    - 一意の識別子を持ち、同一性で区別される
    - 実装：User クラスなどのドメインオブジェクトとして実現

2. **値オブジェクト**

    - ビジネスの値の概念を表現
    - 属性の値で区別される
    - 実装：UserName, UserId クラスなどのドメインオブジェクトとして実現

3. **ドメインサービス**

    - 特定のエンティティに属さない操作を表現
    - 複数のオブジェクト間の調整を行う
    - 実装：UserService クラスなどのドメインオブジェクトとして実現

4. **リポジトリ**
    - エンティティの永続化を抽象化
    - ドメインとインフラの橋渡しを行う
    - 実装：IUserRepository インターフェースなどのドメインオブジェクトとして実現

### 実装例

以下は、このプロジェクトでのドメインモデルの実装例です：

```csharp
// エンティティの実装例
public class User  // エンティティとして実装
{
    // 値オブジェクトとして実装
    public UserId Id { get; private set; }     // 識別子
    public UserName Name { get; private set; } // 属性
    private bool isDeleted;                    // 状態

    // ファクトリメソッド - エンティティの生成ルール
    public static User CreateUser(string id, string name)

    // ドメインルール - エンティティ自身の振る舞い
    public void ChangeName(string name)

    // ドメインルール - エンティティの状態変更
    public void Delete()
}

// ドメインサービスの実装例
public class UserService
{
    // 複数のエンティティを跨ぐルール
    public bool Exists(User user)
}

// リポジトリの実装例
public interface IUserRepository
{
    // エンティティの永続化を抽象化
    void Save(User user)
    User Find(UserId id)
}
```

### 詳細な実装

各ドメインオブジェクトの具体的な実装の詳細は、それぞれ以下のセクションで説明します：

-   値オブジェクトの実装 → [値オブジェクト](#値オブジェクト)セクション
-   エンティティの実装 → [エンティティ](#エンティティ)セクション
-   ドメインサービスの実装 → [ドメインサービス](#ドメインサービス)セクション
-   リポジトリの実装 → [リポジトリ](#リポジトリ)セクション

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

エンティティは、ビジネスドメインにおける実体（実在するもの）を表現するオブジェクトです。以下の特徴を持ちます：

-   一意の識別子（ID）を持つ
-   ライフサイクルを通じて同一性が保持される
-   属性が変更されても同じエンティティとして識別される
-   ビジネスルールをカプセル化する

### 値オブジェクトとエンティティの違い

-   エンティティ: 同一性（ID）によって識別される
-   値オブジェクト: 属性の等価性によって識別される

例：

-   ユーザー（エンティティ）: ID が同じなら、名前が変わっても同一のユーザー
-   メールアドレス（値オブジェクト）: 文字列が変われば、異なるメールアドレスとして扱う

### エンティティの実装例

```csharp
// backend/Domain/Entities/User.cs
public class User : IEquatable<User>
{
    public UserId Id { get; private set; } = null!;
    public UserName Name { get; private set; } = null!;

    // コンストラクタをprivateにして、直接のインスタンス生成を禁止
    private User() { }

    // ファクトリメソッドによるインスタンス生成
    public static User CreateUser(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        var user = new User();
        user.Id = new UserId(Guid.NewGuid().ToString());
        user.Name = new UserName(name);
        return user;
    }

    // DBからの再構築用ファクトリメソッド
    public static User Reconstruct(string id, string name)
    {
        var user = new User();
        user.Id = new UserId(id);
        user.Name = new UserName(name);
        return user;
    }

    // ビジネスルールを含むメソッド
    public void ChangeName(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (name.Length < 3) throw new ArgumentException("ユーザー名は3文字以上です。", nameof(name));
        Name = new UserName(name);
    }

    // 同一性の実装
    public bool Equals(User? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
```

### エンティティの設計原則

1. **カプセル化の徹底**

    - プロパティは`private set`で保護
    - 状態変更は明示的なメソッドを通じて行う
    - 不変条件はコンストラクタとメソッドで保証

2. **ファクトリメソッドパターンの活用**

    ```csharp
    // 新規作成用のファクトリメソッド
    public static User CreateUser(string name)

    // 永続化データからの再構築用ファクトリメソッド
    public static User Reconstruct(string id, string name)
    ```

3. **値オブジェクトの活用**

    ```csharp
    public UserId Id { get; private set; }    // プリミティブ型ではなく値オブジェクトを使用
    public UserName Name { get; private set; } // 名前も値オブジェクトとして実装
    ```

4. **ドメインルールの実装**

    ```csharp
    public void ChangeName(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (name.Length < 3) throw new ArgumentException("ユーザー名は3文字以上です。");
        Name = new UserName(name);
    }
    ```

5. **同一性の実装**
    - `IEquatable<T>`インターフェースの実装
    - ID に基づく等価性の判定
    - `GetHashCode()`のオーバーライド

### エンティティの責務範囲

1. **含めるべき要素**

    - エンティティの属性（プロパティ）
    - ビジネスルール（バリデーション）
    - 状態変更のロジック
    - 同一性の実装

2. **含めるべきでない要素**
    - 他のエンティティとの相互作用（→ ドメインサービスの責務）
    - インフラストラクチャの関心事（→ リポジトリの責務）
    - ユースケース固有のロジック（→ アプリケーションサービスの責務）

### まとめ

-   エンティティは、ビジネスドメインの中核となる概念を表現する
-   同一性による識別と状態の可変性が特徴
-   適切なカプセル化とドメインルールの実装により、ドメインの整合性を保証
-   値オブジェクトと組み合わせることで、より表現力の高いドメインモデルを実現

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

## アプリケーションサービス

### アプリケーションサービスとは

アプリケーションサービスは、ユースケース単位でアプリケーションの振る舞いをまとめる層です。ドメイン層のビジネスロジックを組み合わせて、外部（API や UI など）からの要求に応じた処理を実現します。DDD においては、アプリケーションサービスがドメインモデルの操作を調整し、トランザクション管理や DTO 変換なども担います。

### アプリケーションサービスの責務

1. **ドメインロジックの調整**

    - ドメイン層のエンティティやドメインサービスを利用してユースケースを実装
    - ビジネスロジックは極力持たず、「調整役」として振る舞う
    - 外部インターフェース（API コントローラー等）から直接呼び出される

2. **コマンドパターンの活用**

    - 入力データのカプセル化
    - バリデーションの一元管理
    - 将来的な拡張性の確保

3. **DTO によるデータ変換**
    - ドメインオブジェクトの内部構造を外部に漏らさない
    - API レスポンスの形式を統一
    - ドメインモデルの変更の影響を局所化

### 実装例

#### コマンドクラス

```csharp
// backend/Applications/Users/Commands/UserUpdateCommand.cs
public class UserUpdateCommand
{
    public UserUpdateCommand(string id)
    {
        Id = id;
    }
    public string Id { get; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
```

#### DTO クラス

```csharp
// backend/Applications/Users/DTOs/UserData.cs
public class UserData(User source)
{
    public string Id { get; } = source.Id.Value;
    public string Name { get; } = source.Name.Value;
}
```

#### サービスクラス

```csharp
// backend/Applications/Users/Services/UserUpdateService.cs
public class UserUpdateService
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;

    public UserUpdateService(IUserRepository userRepository, UserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task<UserData?> Handle(UserUpdateCommand command)
    {
        var targetId = new UserId(command.Id);
        var user = await _userRepository.Find(targetId);
        if (user == null)
        {
            return null;
        }
        var name = command.Name;

        if (name != null)
        {
            var newUserName = new UserName(name);
            if (await _userService.Exists(newUserName))
            {
                throw new Exception("ユーザーが既に存在します。");
            }
            user.ChangeName(name);
        }

        await _userRepository.Save(user);

        var userData = new UserData(user);
        return userData;
    }
}
```

### パッケージ構成

アプリケーションサービスは以下のような構成で実装されています：

```
Applications/
  └── Users/
      ├── Commands/      # コマンドクラス
      │   ├── UserUpdateCommand.cs
      │   ├── UserRegisterCommand.cs
      │   └── UserDeleteCommand.cs
      ├── Services/      # アプリケーションサービス
      │   ├── UserUpdateService.cs
      │   ├── UserRegisterService.cs
      │   └── UserDeleteService.cs
      └── DTOs/         # データ転送オブジェクト
          └── UserData.cs
```

### 凝集度と結合度の最適化

#### 高凝集の実現

-   ユースケース単位でのサービスクラスの分割
    -   `UserRegisterService`: ユーザー登録処理
    -   `UserUpdateService`: ユーザー更新処理
    -   `UserDeleteService`: ユーザー削除処理
-   単一責任の原則に基づく実装

#### 低結合の実現

-   インターフェースと依存性注入の活用
-   DTO によるドメインモデルの隠蔽
-   コマンドパターンによる入力データの分離

### エラーハンドリングとバリデーション

```csharp
public async Task<UserData?> Handle(UserUpdateCommand command)
{
    // null チェックによる早期リターン
    var user = await _userRepository.Find(new UserId(command.Id));
    if (user == null) return null;

    // ドメインルールの検証
    if (command.Name != null)
    {
        var newUserName = new UserName(command.Name);
        if (await _userService.Exists(newUserName))
        {
            throw new Exception("ユーザーが既に存在します。");
        }
        user.ChangeName(command.Name);
    }

    // 永続化と戻り値の変換
    await _userRepository.Save(user);
    return new UserData(user);
}
```

### まとめ

-   アプリケーションサービスは「ユースケースの調整役」として、ドメイン層のロジックを組み合わせて外部要求に応える
-   コマンドパターンと DTO を活用し、入力データの管理と出力データの変換を適切に行う
-   高凝集・低結合な設計により、保守性と拡張性を確保
-   適切なエラーハンドリングとバリデーションにより、アプリケーションの堅牢性を担保
