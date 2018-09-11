using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using MLAgents;

public class MazeAgent : Agent {

    static readonly int[,] MAP = {
        {0,0,0,1},
        {0,9,0,0},
        {0,0,0,2}};

    GameObject robo;
    int x; //x座標
    int y; //y座標

    //Stateの取得
    public override void CollectObservations()
    {
        AddVectorObs(this.x / 4f);
        AddVectorObs(this.y / 3f);
    }

    //フレーム毎に呼ばれる
    public override void AgentAction(float[] vectorAction, string textAction)
	{
        //Monitorの表示
        Monitor.verticalOffset = 80;
        Monitor.Log("Reward", "" +this.GetCumulativeReward(),
                this.gameObject.transform);

        //エピソード完了の判定=>宝物or罠にかかった場合終了
        if (MAP[this.y, this.x] ==1 || MAP[this.y, this.x] == 2){
            Done(); //エピソード完了 
            return;
        }

        //Actionの取得
        int action = (int)vectorAction [0];
        if (action < 0) return; //actionの値が0より下の場合ロボットの移動および報酬の指定をしない

        //ロボットの移動
        MoveRobo(action);

        //報酬の指定
        if (MAP [this.y, this.x] ==1){//宝箱に辿りついた場合
            AddReward(1.0f);
        }else if (MAP [this.y, this.x] == 2) {
            AddReward(-1.0f);
        }else {
            AddReward(-0.01f); //ステップ毎に-0.01の負の報酬
        }
    }

    //リセット時に呼ばれる
    public override void AgentReset()
    {
        robo = GameObject.Find("MazeAgent");
        SetRoboPosition (0,1);
    }

    void MoveRobo(int action)
    {
        //移動先の計算
        int dx = this.x;
        int dy = this.y;

        if (action == 0) {dy--;}//上に移動
        if (action == 1) {dy++;}//下に移動
        if (action == 2) {dx--;}//左に移動
        if (action == 3) {dx++;}//右に移動

        //画面外または壁のときは移動しない
        if (dx < 0 || 3 < dx || dy < 0 || 2 < dy || MAP[dy, dx] == 9) return;

        SetRoboPosition (dx,dy);
    }

    void SetRoboPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
        Vector3 pos = this.robo.transform.position; //gameObjectからポジションを取得
        pos.x = (240 - 960) / 2 + x * 240;
        pos.y = -(240 - 720) / 2 - y * 240;
        this.robo.transform.position = pos; //ポジションをgameObjectに適用
    }

    //GUIの処理
    void OnGUI()
    {
        GUI.skin.label.normal.textColor = Color.blue;
    }
}
