using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//biblitoeka do zarządzania poprzez kod elemntami UI ctakich jak zdjęcia czy tekst
using UnityEngine.SceneManagement;//Biblioteka do zarządania scenami
public class LoadLevel : MonoBehaviour {

	public Image progressBar;//zmienna dla obrazka do przypisania w Unity aby wiedziało co modyfikować

	void Start () {
		StartCoroutine(LoadLevelAsync());// wywołanie korutyny na początku gry

	}
	IEnumerator LoadLevelAsync()//ienumerator do wprowadzenia opóżnienia, w tym przypadku będzie on stosowany aby ładował scene z grą ale przy okazji podawał wartość do loadingbara 
	{

		AsyncOperation operation = SceneManager.LoadSceneAsync("Main");//zmienna typu Async do asynchronicznego ładowania i przypisujemy do niej dany typ ładowania wymaganej sceny

		while(operation.isDone == false)//jeżeli operacja nie jest zakończona 
		{
			progressBar.fillAmount = operation.progress;//to ustawiamy ,,ładowanie" tego naszego obrazka
			yield return new WaitForEndOfFrame();//danie ,,oddechu" naszemu programowi
		}
	}
}
