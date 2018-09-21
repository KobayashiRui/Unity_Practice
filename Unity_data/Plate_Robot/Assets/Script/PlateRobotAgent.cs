using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PlateRobotAgent : Agent {

    public GameObject body0;
    public GameObject body1;
    public GameObject body2;

    ConfigurableJoint jointbody1;
    ConfigurableJoint jointbody2;

//float currentPoslist = new List<float>(){0.0f,0.0f};
    //float[] currentPoslist = new float[]{0.0f, 0.0f};
    public float currentposbody1;
    public float currentposbody2;

    float targetPositionjointbody1;//body1の角度の目的角度
    float targetPositionjointbody2;//body2の角度の目的角度

    Vector3 body0_init;
    Vector3 body1_init;
    Vector3 body2_init;
    Quaternion rbody0_init;
    Quaternion rbody1_init;
    Quaternion rbody2_init;

    float beforeTime;

    bool isNewDecisionStep;
    int currentDecisionStep;
    int flag;
    int step;
    float old_posx;
    float old_pos2x;
    float old_pos3x;


    public override void InitializeAgent()
    {
        Debug.Log("InitializeAgent");
        jointbody1 = body1.GetComponent<ConfigurableJoint>();
        jointbody2 = body2.GetComponent<ConfigurableJoint>();
        body1_init = body1.transform.position;
        body2_init = body2.transform.position;
        body0_init = body0.transform.position;
        rbody1_init = body1.transform.rotation;
        rbody2_init = body2.transform.rotation;
        rbody0_init = body0.transform.rotation;
        targetPositionjointbody1 = 0.0f;
        targetPositionjointbody2 = 0.0f;
        currentposbody1 = 0.0f;
        currentposbody2 = 0.0f;
        beforeTime = Time.time;
        flag = 0;
        step = 0;
        old_posx = body0.transform.position.x;
        old_pos2x = body1.transform.position.x;
        old_pos3x = body2.transform.position.x;
    }

    //状態を取得する(Stateの取得時に呼ばれる)
    public override void CollectObservations()
    {
        AddVectorObs(body1.transform.rotation);
        AddVectorObs(jointbody1.targetRotation);
        AddVectorObs(jointbody2.targetRotation);
    }

    //ステップ時に呼ばれる// RequestDecision()が呼ばれたときのみ
    public override void AgentAction(float[] vectorAction, string textAction)
	{

        float reward;

        //Debug.Log(jointbody1.targetRotation);
        Quaternion rot1 = Quaternion.identity;
        Quaternion rot2 = Quaternion.identity;
        Vector3 x_axis = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 y_axis = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 z_axis = new Vector3(0.0f, 0.0f, 1.0f);
        rot1 = Quaternion.AngleAxis(vectorAction[0]*90.0f,x_axis) * rot1;
        rot1 = Quaternion.AngleAxis(vectorAction[1]*90.0f,y_axis) * rot1;
        rot1 = Quaternion.AngleAxis(vectorAction[2]*90.0f,y_axis) * rot1;
        rot2 = Quaternion.AngleAxis(vectorAction[3]*90.0f,x_axis) * rot2;
        rot2 = Quaternion.AngleAxis(vectorAction[4]*90.0f,y_axis) * rot2;
        rot2 = Quaternion.AngleAxis(vectorAction[5]*90.0f,z_axis) * rot2;


        jointbody1.targetRotation = rot1;
        jointbody2.targetRotation = rot2;

        reward = (body0.transform.position.x - old_posx) + (body1.transform.position.x - old_pos2x) + (body2.transform.position.x - old_pos3x);
        old_posx = body0.transform.position.x;
        old_pos2x = body1.transform.position.x;
        old_pos3x = body2.transform.position.x;
        Debug.Log(reward);
        AddReward(reward);
    }

    public void SetHinge(float targetPos,int i){
        //Debug.Log("targetpos");
        //Debug.Log(targetPos);
        if(i == 0){
        }else if(i == 1){
        Debug.Log("id");
        Debug.Log(i);
        }
    }
/*
    public void MoveJoint(){

        JointSpring hingeSpring2 = jointbody2.spring;
        hingeSpring2.targetPosition = currentposbody2;
        jointbody2.spring = hingeSpring2;

        JointSpring hingeSpring1 = jointbody1.spring;
        hingeSpring1.targetPosition = currentposbody1;//currentPoslist[0];
        jointbody1.spring = hingeSpring1;
    }

    public void MoveJoint_init(){

        JointSpring hingeSpring2 = jointbody2.spring;
        hingeSpring2.targetPosition = 0;
        jointbody2.spring = hingeSpring2;

        JointSpring hingeSpring1 = jointbody1.spring;
        hingeSpring1.targetPosition = 0;//currentPoslist[0];
        jointbody1.spring = hingeSpring1;
    }
*/

    //リセット時に呼ばれる
    public override void AgentReset()
    {
        Quaternion rot = Quaternion.identity;
        jointbody1.targetRotation= rot;
        jointbody2.targetRotation= rot;

        body0.transform.position = body0_init;
        body0.transform.rotation = rbody0_init;
        body1.transform.position = body1_init;
        body1.transform.rotation = rbody1_init;
        body2.transform.position = body2_init;
        body2.transform.rotation = rbody2_init;
        targetPositionjointbody1 = 0.0f;
        targetPositionjointbody2 = 0.0f;
        currentposbody1 = 0.0f;
        currentposbody2 = 0.0f;
        beforeTime = Time.time;
        flag = 0;
        step = 0;
        old_posx = body0.transform.position.x;
        old_pos2x = body1.transform.position.x;
        old_pos3x = body2.transform.position.x;
        Debug.Log("Reset");
    }
}
