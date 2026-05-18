using UnityEngine;

public class Test_LobbyUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_GameStart;
    [SerializeField] private DaniTechUIButton Button_GameReStart;
    [SerializeField] private DaniTechUIButton Button_GameQuit;

    

    private void OnEnable()
    {
        Button_GameStart.BindOnClickButtonEvent(OnClick_GameStart);
        Button_GameReStart.BindOnClickButtonEvent(OnClick_GameReStart);

        Button_GameQuit.BindOnClickButtonEvent(OnClick_GameQuit);
    }

    public void OnClick_GameStart()
    {
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.Lobby_UI);
    }

    public void OnClick_GameQuit()
    {
        DaniTechGameManager.Inst.SaveAndEndGame();
    }

    public void OnClick_GameReStart()
    {
        // TODO 세이브 로드 구현하면 할것..
    }
}
