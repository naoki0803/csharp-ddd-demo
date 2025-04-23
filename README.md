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
