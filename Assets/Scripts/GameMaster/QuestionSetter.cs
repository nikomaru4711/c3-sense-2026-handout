using UnityEngine;

public class QuestionSetter : MonoBehaviour
{
    [SerializeField] private GameMaster gameMaster;

    private int difficulty;
    public int currentQuestionIndex = 0; // 現在の問題のインデックス
    public int numberOfQuestions; // 出題する問題の数
    public float timeLimitPerQuestion; // 1問あたりの制限時間（秒）
    public float timeRemaining; // 現在の問題の残り時間（秒）
    public float currentQuestionScore; // 現在の問題のスコア
    public KeyCode[] CorrectAnswers; // 現在の問題の正解のキーコード
    public int lives; // プレイヤーのライフ
    public int[] LivesByDifficulty = { 5, 3, 1 }; // 難易度ごとのライフ数

    private int[] NumOfQs = { 15, 20, 25 }; // 難易度ごとの出題数
    private float[] TimeLimits = { 10f, 7f, 5f }; // 難易度ごとの制限時間

    // アルファベットのキーコードのリスト
    public KeyCode[] AnswerKeys = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
        KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
        KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
        KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
        KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
        KeyCode.Z
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        difficulty = gameMaster.difficulty; // GameMasterから難易度を取得
        currentQuestionIndex = 0;
        if (difficulty >= 0 && difficulty < NumOfQs.Length)
        {
            numberOfQuestions = NumOfQs[difficulty]; // 難易度に応じた出題数を設定
            timeLimitPerQuestion = TimeLimits[difficulty]; // 難易度に応じた制限時間を設定
            timeRemaining = timeLimitPerQuestion; // 最初の問題の残り時間を設定
            lives = LivesByDifficulty[difficulty]; // 難易度に応じたライフ数を設定
            gameMaster.livesRemaining = lives; // GameMasterの残りライフを設定
            SetNextQuestion(); // 最初の問題を設定するメソッドを呼び出す
            gameMaster.isGameStarted = true; // ゲーム開始フラグを立てる
            Debug.Log($"ゲームが開始されました。難易度: {difficulty}, 出題数: {numberOfQuestions}, 制限時間: {timeLimitPerQuestion}秒, ライフ: {lives}");
        }
        else
        {
            Debug.LogError("無効な難易度が設定されています。");
            numberOfQuestions = 0;
            timeLimitPerQuestion = 0f;
            timeRemaining = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishQuestion()
    {
        if (currentQuestionIndex < numberOfQuestions)
        {
            // 次の問題を出題する処理をここに追加
            currentQuestionIndex++;
            timeRemaining = timeLimitPerQuestion; // 次の問題の残り時間をリセット
            SetNextQuestion(); // 次の問題を設定するメソッドを呼び出す
            Debug.Log($"次の問題が出題されました。現在の問題インデックス: {currentQuestionIndex}");
            Debug.Log($"現在のスコア: {gameMaster.score}");
            Debug.Log($"残りライフ: {gameMaster.livesRemaining}");
        }
        else
        {
            Debug.Log("すべての問題が出題されました。");
            // ゲーム終了の処理をここに追加
        }
    }

    public void UpdateScore(float timeRemaining, float score)
    {
        // スコアの更新処理
        gameMaster.score += (int)(score * timeRemaining);
        Debug.Log($"スコアが更新されました。現在のスコア: {gameMaster.score}");
    }

    public void CheckAnswer()
    {
        // プレイヤーの入力と正解を比較する処理
        foreach (KeyCode correctKey in CorrectAnswers)
        {
            if (!Input.GetKey(correctKey))
            {
                return;
            }
        }

        UpdateScore(timeRemaining, 100f);
        Debug.Log("正解です！");
        FinishQuestion(); // 次の問題を設定する
    }

    public void SetNextQuestion()
    {
        // 次の問題を設定する処理をここに追加
        CorrectAnswers = new KeyCode[difficulty + 1];
        CorrectAnswers[0] = AnswerKeys[Random.Range(0, AnswerKeys.Length)]; // ランダムに正解を設定
        if (difficulty >= 1)
        {
            CorrectAnswers[1] = AnswerKeys[Random.Range(0, AnswerKeys.Length)]; // 難易度が中級以上の場合、もう一つ正解を設定
            // 正解のキーが重複しないようにする
            while (CorrectAnswers[1] == CorrectAnswers[0])
            {
                CorrectAnswers[1] = AnswerKeys[Random.Range(0, AnswerKeys.Length)];
            }
            if (difficulty >= 2)
            {
                CorrectAnswers[2] = AnswerKeys[Random.Range(0, AnswerKeys.Length)]; // 難易度が上級の場合、さらにもう一つ正解を設定
                // 正解のキーが重複しないようにする
                while (CorrectAnswers[2] == CorrectAnswers[0] || CorrectAnswers[2] == CorrectAnswers[1])
                {
                    CorrectAnswers[2] = AnswerKeys[Random.Range(0, AnswerKeys.Length)];
                }
            }
        }

        // 次の問題の内容をコンソールに表示する
        Debug.Log($"次の問題が設定されました。正解のキーコード: {string.Join(", ", CorrectAnswers)}");
    }
}
