namespace TodoApi;

/// <summary>
/// モデル番号を表すクラス
/// </summary>
public class ModelNumber
{
    private readonly string productCode;
    private readonly string branch;
    private readonly string lot;

    /// <summary>
    /// モデル番号を生成する
    /// </summary>
    /// <param name="productCode">商品コード</param>
    /// <param name="branch">枝番</param>
    /// <param name="lot">ロット番号</param>
    /// <exception cref="ArgumentNullException">引数がnullの場合</exception>

    public ModelNumber(string productCode, string branch, string lot)
    {
        /*nameof()について
        変数やパラメータの名前を文字列として取得する：コード内で使用している識別子（変数名、プロパティ名、パラメータ名など）をコンパイル時に文字列に変換します
        リファクタリングに強い：コード内でパラメータ名を変更した場合、nameof()を使った部分も自動的に更新されます
        タイプミスを防止：文字列リテラルで名前を指定する代わりにnameof()を使うことで、コンパイル時にタイプミスをチェックできます
        */
        if (productCode == null) throw new ArgumentNullException(nameof(productCode));
        if (branch == null) throw new ArgumentNullException(nameof(branch));
        if (lot == null) throw new ArgumentNullException(nameof(lot));
        this.productCode = productCode;
        this.branch = branch;
        this.lot = lot;
    }

    /*ToString()をオーバーライドする理由
        システム標準インターフェース: すべての.NETオブジェクトが持つ基本メソッドで、標準的な文字列表現を提供する
        自動的な呼び出し: デバッグ、ログ出力、Console.WriteLine()などで追加コードなしに使用される
        可読性の向上: オブジェクトの内容が「ABC123-01-X789」のように人間が理解しやすい形式で表示される
        値オブジェクトの本質: 値オブジェクトは値そのものを表すため、その文字列表現は重要な特性
        UI表示の一貫性: 画面表示やログなど、システム全体で一貫した表示形式を保証できる
        カスタムメソッド（getModelNumber()など）ではなくToString()を使うことで、
        標準的な方法でオブジェクトの文字列表現にアクセスでき、コードの一貫性と保守性が向上します。
    */
    public override string ToString()
    {
        return $"{productCode}-{branch}-{lot}";
    }
}