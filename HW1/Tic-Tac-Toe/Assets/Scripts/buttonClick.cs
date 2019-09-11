using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonClick : MonoBehaviour
{
    public int[,] board;
    public bool begin;
    public bool turn=true;
    public int count = 0;
    public GameObject resetBtn;
    // Start is called before the first frame update
    void Awake(){
        Button btn;
        resetBtn = GameObject.Find("ResetButton");
        btn = resetBtn.transform.GetComponent<Button>();
        btn.onClick.AddListener(ResetButtonOnClick);
        hideResetButton();
        board = new int[3,3];
        cleanBoard();
    }
    void Start()
    {
         begin=true;
    }
    void setStatus(char c){
        switch(c){
            case 'A':
                board[0,0]=turn?1:2;
                break;
            case 'B':
                board[0,1]=turn?1:2;
                break;
            case 'C':
                board[0,2]=turn?1:2;
                break;
            case 'D':
                board[1,0]=turn?1:2;
                break;
            case 'E':
                board[1,1]=turn?1:2;
                break;
            case 'F':
                board[1,2]=turn?1:2;
                break;
            case 'G':
                board[2,0]=turn?1:2;
                break;
            case 'H':
                board[2,1]=turn?1:2;
                break;
            case 'I':
                board[2,2]=turn?1:2;
                break;
        }
        turn = !turn;
    }
    int getStatus(char c){
        switch(c){
            case 'A':
                return board[0,0];
            case 'B':
                return board[0,1];
            case 'C':
                return board[0,2];
            case 'D':
                return board[1,0];
            case 'E':
                return board[1,1];
            case 'F':
                return board[1,2];
            case 'G':
                return board[2,0];
            case 'H':
                return board[2,1];
            case 'I':
                return board[2,2];
                break;
        }
        return board[0,0];
    }
    // Update is called once per frame
    void Update()
    {
        GameObject obj;
        if (begin && checkFinish()) {
            begin=false;
            showResetButton();
        }
        if(begin==false) return;
        if(Input.GetMouseButtonDown(0)){
            Text text;
            //使用Ray Cast 射线碰撞来检测鼠标点了哪一个按钮
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
                {                  
                    string name = hit.collider.name;
                    char c = name[name.Length-1];
                    obj = GameObject.Find(name);
                    text = obj.transform.Find("Text").GetComponent<Text>();
                    if(getStatus(c)!=0) return;
                    if(turn){
                        text.text="X";
                    }
                    else{
                        text.text="O";
                    }
                    setStatus(c);
                    ++count;
                }
        }
    }
    bool checkFinish(){
        if(board[1,1]==0) return false;
        if((board[1,1]==board[0,0]&&board[1,1]==board[2,2]) ||
           (board[1,1]==board[0,2]&&board[2,0]==board[1,1]) ||
           (board[1,1]==board[0,1]&&board[2,1]==board[1,1]) ||
           (board[1,1]==board[1,0]&&board[1,2]==board[1,1])
        ){
            showResult(false,board[1,1]==1);return true;
        }
        else if(((board[0,0]==board[0,1]&&board[0,0]==board[0,2]) ||
            (board[0,0]==board[1,0]&&board[0,0]==board[2,0])) &&board[0,0]!=0
        ){
            showResult(false,board[0,0]==1);return true;
        }
        else if(((board[2,2]==board[2,0]&&board[2,2]==board[2,1]) ||
            (board[2,2]==board[1,2]&&board[2,2]==board[0,2])) && board[2,2]!=0
        ){
            showResult(false,board[2,2]==1);return true;
        }
        if(count==9) {showResult(true,true);return true;}
        return false;
    }
    void cleanBoard(){
        for(int i=0;i<3;++i)
            for(int j=0;j<3;++j)
                board[i,j] = 0;
        Button btn;
        foreach (Transform child in transform)
            {
                btn=child.GetComponent<Button>();
                btn.transform.Find("Text").GetComponent<Text>().text = "";
            }
    }
    void hideResetButton(){
        resetBtn.SetActive(false);
    }
    void showResetButton(){
        resetBtn.SetActive(true);
    }
    void showResult(bool isdraw,bool X){
        Text text = GameObject.Find("Result").GetComponent<Text>();
        if(isdraw){
            text.text ="A Draw";
        }
        else{
            if(X) text.text ="X Wins";
            else text.text = "O wins";
        }
    }
    void hiddleResult(){
        Text text =GameObject.Find("Result").GetComponent<Text>();
        text.text="";
    }
    void ResetButtonOnClick(){
        cleanBoard();
        hiddleResult();
        hideResetButton();
        begin = true;
        turn = true;
        count = 0;
    }
}
