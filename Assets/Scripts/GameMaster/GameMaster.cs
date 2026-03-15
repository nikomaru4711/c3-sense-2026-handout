using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private QuestionSetter questionSetter;

    [Header("ゲームの状態を管理する変数")]
    public int difficulty = -1; // -1: 未選択, 0: 初級, 1: 中級, 2: 上級
    public bool isGameStarted = false; // ゲームが開始されたかどうか

    public int livesRemaining; // プレイヤーの残りライフ
    public int score; // プレイヤーのスコア

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questionSetter.enabled = false; // ゲーム開始前はQuestionSetterを無効にする
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted)
        {
            questionSetter.timeRemaining -= Time.deltaTime; // 残り時間を減少させる
            if (questionSetter.timeRemaining <= 0f)
            {
                if (livesRemaining > 1)
                {
                    livesRemaining--; // ライフを減らす
                    Debug.Log($"時間切れです。残りライフ: {livesRemaining}");
                    questionSetter.FinishQuestion(); // 問題を終了するメソッドを呼び出す
                }
                else
                {
                    Debug.Log("ゲームオーバーです。");
                    isGameStarted = false; // ゲームを終了する
                    return;
                }
            }
            else
            {
                questionSetter.CheckAnswer(); // プレイヤーの入力をチェックするメソッドを呼び出す
            }
        }
    }

    public void StartGame()
    {
        if (difficulty == -1)
        {
            Debug.LogWarning("難易度が選択されていません。ゲームを開始できません。");
            return;
        }
        questionSetter.enabled = true; // QuestionSetterを有効にする
        Debug.Log("ゲームが開始されました。");
    }
}
