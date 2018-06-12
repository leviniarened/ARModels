using UnityEngine;

public class ViewWidget : MonoBehaviour, IView {

	//void Start () {
		//Hide();
	//}
	
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
