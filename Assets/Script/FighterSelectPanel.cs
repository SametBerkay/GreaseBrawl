using UnityEngine;

public class FighterSelectPanel : MonoBehaviour
{
    public static FighterSelectPanel Instance;

    public FighterSelectUI[] fighterUIs;

    private void Awake()
    {
        Instance = this;
    }

    public void Setup(Fighter[] fighters)
    {
        for (int i = 0; i < fighterUIs.Length; i++)
        {
            fighterUIs[i].Setup(fighters[i]);
        }
    }

    public void SetActiveFighter(FighterSelectUI selected)
    {
        foreach (var f in fighterUIs)
            f.SetActive(false);

        selected.SetActive(true);
    }
}
